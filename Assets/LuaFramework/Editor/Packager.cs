using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;

public class Packager {
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();
    static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();
    static List<string> gameAbFiles = new List<string>();

    static FaceType packType = FaceType.eCnt;

    //临时目录, 打包完会删除
    static string tmpAbsOutPutDir;
    static string LuaTempDir;

    //写死 Tolua的lua路径很稳定了， 基本不变
    static string AppDataPath
    {
        get { return Application.dataPath.ToLower(); }
    }
    static string toLuaDir = AppDataPath + "/LuaFramework/Tolua/Lua"; 
    static string outPutRootDir = Application.dataPath + "/" + AppConst.AssetDir;      //打包输出目录（默认是streamingAssetsPath)

    static string workDir;      //打包根目录
    static string abExtName;    //打包后缀
    static string luaDir;       //luaDir放在workDir里， 不变了
    static string pureDir;      //不打包目录
    static string abResOutDir;
     
    static List<string> needPackDirs = new List<string>();      //工作目录下 需要打包的的目录
    static Dictionary<string, string> typeDic = null;

    //打包报错
    static void LogPackErr(string log)
    {
        UnityEngine.Debug.LogError(log);
    }
      
    //确定打包要输出哪些目录，打包后缀
    static void MakeSurePaths(bool needCleanOldRes = true)
    {
        //lua临时文件目录，用完会删掉，唯一就行
        LuaTempDir = "Lua_uni_" + Util.GetTime() + "/";

        AppFacade ap = AppFacade.GetApp(packType);
        abExtName = ap.PackExtName;
        workDir = ap.WorkDir;
        luaDir = Application.dataPath + "/" + workDir + "/Lua";
        pureDir = Application.dataPath + "/" + workDir + "/" + AppConst.PureResDir;

        tmpAbsOutPutDir = outPutRootDir + abExtName; 
        abResOutDir = outPutRootDir + "/" + ap.RootResNm;

        //用小游戏标准目录打包。 小游戏和平台各自的目录结构是一样的，只是根目录名不同。 等路径！
        string toPackPath = ap.DataPath;
        if(needCleanOldRes)
        {
            if (Directory.Exists(toPackPath))
            {
                Directory.Delete(toPackPath, true);
            }
            if (Directory.Exists(abResOutDir))
            {
                Directory.Delete(abResOutDir, true);
            } 
            //AssetDatabase.Refresh();
        } 
         

        if (!Directory.Exists(outPutRootDir))
        {
            Directory.CreateDirectory(outPutRootDir); 
        } 

        if (Directory.Exists(tmpAbsOutPutDir))
        {
            Directory.Delete(tmpAbsOutPutDir, true);
        }
        Directory.CreateDirectory(tmpAbsOutPutDir);
        //AssetDatabase.Refresh();
    }

    private static BuildTarget GetCurBuildTarget()
    {
#if UNITY_IOS
            return BuildTarget.iOS;
#endif
#if UNITY_ANDROID
            return BuildTarget.Android;
#endif
        return BuildTarget.StandaloneWindows;
    }
     
    [MenuItem("LuaFramework/打包vm1资源", false, 101)]
    public static void BuildPlatRes()
    {
        AppConst.LuaBundleMode = true;
        BuildTarget tar = GetCurBuildTarget();
        packType = FaceType.vm1;
        BuildAssetResource(tar, AppConst.MainWorkName);
    }

    [MenuItem("LuaFramework/打包vm2资源", false, 102)]
    public static void BuildGameRes()
    {
        AppConst.LuaBundleMode = true;
        BuildTarget tar = GetCurBuildTarget();
        packType = FaceType.vm2;
        BuildAssetResource(tar, AppConst.SubWorkName);
    }

    public static void BuildAssetResource(BuildTarget target, string workDir) {

        string[] packItems = File.ReadAllLines(Application.dataPath + "/" + workDir + "/packCfg.txt");
        needPackDirs.AddRange(packItems);

        MakeSurePaths();

        maps.Clear();
        //Lua 
        HandleLuaBundle();

        //
        HandleExampleBundle();
        AssetDatabase.Refresh();
        BuildPipeline.BuildAssetBundles(tmpAbsOutPutDir, maps.ToArray(), BuildAssetBundleOptions.None, target);
        BuildGameAssetIds();
         
        if (Directory.Exists(pureDir))
        {
            //纯净的，不打包的资源
            FileTool ft = new FileTool();
            ft.CopyFilesToDirKeepSrcDirName(pureDir, tmpAbsOutPutDir); 
        }
         
        BuildFileIndex();
         
        string tmpDir = Application.dataPath + "/" + LuaTempDir;
        if (Directory.Exists(tmpDir)) Directory.Delete(tmpDir, true); 

        //
        Directory.Move(tmpAbsOutPutDir, abResOutDir); 
         
        // 
		AssetDatabase.Refresh(); 
    } 
    
    static void AddBuildMap(string bundleName, string pattern, string path) {
        string[] files = Directory.GetFiles(path, pattern);
        if (files.Length == 0) return;

        for (int i = 0; i < files.Length; i++) {
            files[i] = files[i].Replace('\\', '/');
        }
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = bundleName;
        build.assetNames = files;
        maps.Add(build);
    }

    //小游戏的ab
    static void AddGameBuildMap(string dirName, string abExtName, string pattern, string path, bool wholeFolder = true)
    {
        string[] files = Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
        if (files.Length == 0) return;
        for (int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Replace('\\', '/');
        }

        if (wholeFolder)
        {
            //对整个文件夹打成一个包
            string bundleName = dirName + abExtName;
            if (gameAbFiles.Contains(bundleName))
            {
                LogPackErr("-----------the same bundleName " + bundleName);
                return;
            }
            gameAbFiles.Add(bundleName);
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = bundleName;
            build.assetNames = files;
            maps.Add(build);
        }
        else
        {
            string metaExt = ".meta";
            //对文件夹下面的文件独自打包
            for (int i = 0; i < files.Length; ++i)
            {
                string filePath = files[i];
                string extName = Path.GetExtension(filePath);
                if (string.Equals(extName, metaExt))
                    continue;

                string[] abFiles = null;
                if (File.Exists(filePath + metaExt))
                {
                    abFiles = new string[] { filePath, filePath + metaExt };
                }
                else
                {
                    abFiles = new string[] { filePath };
                }

                //
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string bundleName = dirName + "_" + fileName + abExtName;
                if (gameAbFiles.Contains(bundleName))
                {
                    LogPackErr("-----------the same bundleName " + bundleName);
                    return;
                }
                gameAbFiles.Add(bundleName);
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = bundleName;
                build.assetNames = abFiles;
                maps.Add(build);
            }
        }
    }

    //生成小游戏资源id表
    static void BuildGameAssetIds()
    {
        string outDir = tmpAbsOutPutDir;
        string filePath = outDir + "/" + AppConst.AbAssetsIds;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        AssetDatabase.Refresh();

        //建个文件
        File.WriteAllText(filePath, "");
        string[] idItems = File.ReadAllLines(Application.dataPath + "/" + workDir + "/" + AppConst.AssetsIds);
        if (idItems.Length < 1) return;

        Dictionary<string, string> abPath2abName = new Dictionary<string, string>();    //简短路径

        string AssetBeginTag = "Assets:";
        string AssetEndTag = "Dependencies:";
        string gamePrefix = "- Assets/" + workDir + "/";
        int prefixLen = gamePrefix.Length;
        for (int i = 0; i < gameAbFiles.Count; ++i)
        {
            string abName = gameAbFiles[i];
            string manifest = outDir + "/" + abName + ".manifest";

            bool bInAssetsReg = false;
            string[] lines = File.ReadAllLines(manifest);
            foreach (var line in lines)
            {
                if (!bInAssetsReg)
                {
                    //开始
                    if (line.StartsWith(AssetBeginTag))
                    {
                        bInAssetsReg = true;
                    }
                    continue;
                }

                //结束
                if (bInAssetsReg && line.StartsWith(AssetEndTag))
                    break;

                if (!line.StartsWith(gamePrefix))
                    continue;

                //中间的资源区
                string abPath = line.Substring(prefixLen).Trim();
                string fullPath = line.Substring("- ".Length).Trim();
                if (abPath2abName.ContainsKey(abPath))
                {
                    //同一个abPath在不同ab包！！！ 不可能
                    LogPackErr(string.Format("不同ab包: {0} | {1} | {2}", abPath, abName, abPath2abName[abPath]));
                    return;
                }

                string abName0 = abName.Substring(0, abName.LastIndexOf('.'));
                abPath2abName[abPath] = abName0;
            }
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < idItems.Length; ++i)
        {
            string[] elements = idItems[i].Split(new char[] { '|', '.' });
            int id;
            if (!int.TryParse(elements[0].Trim(), out id))
            {
                UnityEngine.Debug.Log(string.Format("-------elements-第{0}行空白----------", i + 1));
                continue;
            }
            string path = elements[1].Trim().Replace("\\", "/");
            string suffix = elements[2].Trim();
            string key = path + "." + suffix;
            if (!abPath2abName.ContainsKey(key))
            {
                LogPackErr("can't find ab for " + key);
                return;
            }
            string assetType = ToAssetType(suffix);
            sb.AppendLine(string.Format("{0}|{1}|{2}|{3}", id, abPath2abName[key], "Assets/" + workDir + "/" + path + "." + suffix, assetType));
        }
        File.WriteAllText(filePath, sb.ToString());
        AssetDatabase.Refresh();
    }

    static string ToAssetType(string suffix)
    {
        if (typeDic == null)
        {
            typeDic = new Dictionary<string, string>();
            typeDic.Add("prefab", typeof(GameObject).Name);
            typeDic.Add("wav", typeof(AudioClip).Name);
            typeDic.Add("mp3", typeof(AudioClip).Name);
            typeDic.Add("mp4", typeof(UnityEngine.Video.VideoClip).Name);
        }
        string type = null;
        if (!typeDic.TryGetValue(suffix.ToLower().Trim(), out type))
        {
            return "to add";
        }
        return type;
    }

    /// <summary>
    /// 处理Lua代码包
    /// </summary>
    static void HandleLuaBundle() {
        string streamDir = Application.dataPath + "/" + LuaTempDir;
        if (!Directory.Exists(streamDir)) Directory.CreateDirectory(streamDir);
        string[] srcDirs = { luaDir + "/", toLuaDir + "/" };

        for (int i = 0; i < srcDirs.Length; i++)
        {
            if (AppConst.LuaByteMode)
            {
                string sourceDir = srcDirs[i];
                string[] files = Directory.GetFiles(sourceDir, "*.lua", SearchOption.AllDirectories);
                int len = sourceDir.Length;

                if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
                {
                    --len;
                }
                for (int j = 0; j < files.Length; j++)
                {
                    string str = files[j].Remove(0, len);
                    string dest = streamDir + str + ".bytes";
                    string dir = Path.GetDirectoryName(dest);
                    Directory.CreateDirectory(dir);
                    EncodeLuaFile(files[j], dest);
                }
            }
            else {

                //把源目录下的文件按照源目录 放入目标目录中  这里的目标目录是临时目录
                ToLuaMenu.CopyLuaBytesFiles(srcDirs[i], streamDir);
            }
        }
        //获取所有子目录
        string[] dirs = Directory.GetDirectories(streamDir, "*", SearchOption.AllDirectories);

        //下面即将大改！！！！！！！！！！！！！！！！！！！！
        //下面是把每个目录变成一个AB包，资源名就是资源名
        //改成一个AB包 资源名变成了目录名+资源名
        for (int i = 0; i < dirs.Length; i++)
        {
            string DirName = dirs[i].Replace(streamDir, string.Empty);
            string[] names = Directory.GetFiles(dirs[i]);

            for (int x = 0; x < names.Length; x++)
            {
                string FileName = names[x].Replace(dirs[i], string.Empty).Replace('\\', '/');
                string FinalName = (DirName + FileName).Replace('\\', '/');
                FinalName = FinalName.Replace('/', '_').ToLower();
                FileInfo file = new FileInfo(names[x]);
                file.MoveTo(streamDir + FinalName);
            }
        }
        AddBuildMap("lua" + abExtName, "*.bytes", "Assets/" + LuaTempDir);

        //-------------------------------处理非Lua文件----------------------------------
        /*
        string luaPath = tmpAbsOutPutDir + "/lua/";
        for (int i = 0; i < srcDirs.Length; i++) {
            paths.Clear(); files.Clear();
            string luaDataPath = srcDirs[i].ToLower();
            Recursive(luaDataPath);
            foreach (string f in files) {
                if (f.EndsWith(".meta") || f.EndsWith(".lua")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string path = Path.GetDirectoryName(luaPath + newfile);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                string destfile = path + "/" + Path.GetFileName(f);
                File.Copy(f, destfile, true);
            }
        }
         */
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 处理框架实例包
    /// </summary>
    static void HandleExampleBundle() {
        gameAbFiles.Clear();

        string workDirPath = "Assets/" + workDir + "/";
        foreach (string p in needPackDirs)
        {
            string path = p.Trim();
            if (string.IsNullOrEmpty(path))
                continue;

            bool abFolder = true;
            string abFilesTag = "Files:";
            if (path.StartsWith(abFilesTag))
            {
                //对文件夹里的每个文件独自打包
                abFolder = false;
                path = path.Substring(abFilesTag.Length);
            }
            string dirName = path.Substring(path.LastIndexOf("\\") + 1).Trim();
            string dirPath = workDirPath + path.Replace("\\", "/").Trim();
            string absPath = Application.dataPath + "/" + dirPath.Replace("Assets/", "");
            if (!Directory.Exists(absPath))
            {
                LogPackErr("--------HandleExampleBundle----no dir--- " + absPath);
                continue;
            }

            AddGameBuildMap(dirName, abExtName, "*.*", dirPath, abFolder);
        }
    }

    /// <summary>
    /// 处理Lua文件
    /// </summary>
    static void HandleLuaFile() {
        string luaPath = tmpAbsOutPutDir + "/lua/";

        //----------复制Lua文件----------------
        if (!Directory.Exists(luaPath)) {
            Directory.CreateDirectory(luaPath);
        }
        string[] luaPaths = { luaDir + "/", toLuaDir + "/" };

        for (int i = 0; i < luaPaths.Length; i++) {
            paths.Clear(); files.Clear();
            string luaDataPath = luaPaths[i].ToLower();
            Recursive(luaDataPath);
            int n = 0;
            foreach (string f in files) {
                if (f.EndsWith(".meta")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string newpath = luaPath + newfile;
                string path = Path.GetDirectoryName(newpath);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                if (File.Exists(newpath)) {
                    File.Delete(newpath);
                }
                if (AppConst.LuaByteMode) {
                    EncodeLuaFile(f, newpath);
                } else {
                    File.Copy(f, newpath, true);
                }
                UpdateProgress(n++, files.Count, newpath);
            }
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void BuildFileIndex(bool useTmp = true) {
        string resPath = tmpAbsOutPutDir;
        if(!useTmp)
        {
            resPath = outPutRootDir;
        }

        ///----------------------创建文件列表-----------------------
        string newFilePath = resPath + "/files.txt";
        if (File.Exists(newFilePath)) File.Delete(newFilePath);

        paths.Clear(); files.Clear();
        Recursive(resPath);

        string trimPath = resPath + "/";
        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++) {
            string file = files[i];
            string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store") || file.EndsWith(".manifest")) continue;

            string md5 = Util.md5file(file);
            FileInfo info = new FileInfo(file);
            string value = file.Replace(trimPath, string.Empty);
            sw.WriteLine(value + "|" + md5 + "|" + info.Length);
        }
        sw.Close(); fs.Close();
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path) {
        string svnTag = ".svn";
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names) {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs) {
            int svnIdx = dir.LastIndexOf(svnTag);
            if(svnIdx >= 0)
            {
                //.svn目录来的， 不理
                if (svnIdx + svnTag.Length == dir.Length)
                    continue;
            }
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }

    static void UpdateProgress(int progress, int progressMax, string desc) {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }

    public static void EncodeLuaFile(string srcFile, string outFile) {
        if (!srcFile.ToLower().EndsWith(".lua")) {
            File.Copy(srcFile, outFile, true);
            return;
        }
        bool isWin = true;
        string luaexe = string.Empty;
        string args = string.Empty;
        string exedir = string.Empty;
        string currDir = Directory.GetCurrentDirectory();
        if (Application.platform == RuntimePlatform.WindowsEditor) {
            isWin = true;
            luaexe = "luajit.exe";
            args = "-b -g " + srcFile + " " + outFile;
            exedir = AppDataPath.Replace("assets", "") + "LuaEncoder/luajit/";
        } else if (Application.platform == RuntimePlatform.OSXEditor) {
            isWin = false;
            luaexe = "./luajit";
            args = "-b -g " + srcFile + " " + outFile;
            exedir = AppDataPath.Replace("assets", "") + "LuaEncoder/luajit_mac/";
        }
        Directory.SetCurrentDirectory(exedir);
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = luaexe;
        info.Arguments = args;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = isWin;
        info.ErrorDialog = true;
        UnityEngine.Debug.Log(info.FileName + " " + info.Arguments);

        Process pro = Process.Start(info);
        pro.WaitForExit();
        Directory.SetCurrentDirectory(currDir);
    }
 
}