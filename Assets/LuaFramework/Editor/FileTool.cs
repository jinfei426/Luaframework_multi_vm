using System.IO;
using System;

//见 http://www.cnblogs.com/jerry1999/p/4115867.html
public class FileTool
{

    public void CopyFilesToDirKeepSrcDirName(string srcPath, string destDir)
    {
        if (Directory.Exists(srcPath))
        {
            DirectoryInfo srcDirectory = new DirectoryInfo(srcPath);
            CopyDirectory(srcPath, destDir + @"\" + srcDirectory.Name);
        }
        else
        {
            CopyFile(srcPath, destDir);
        }
    }

    public void CopyFilesToDir(string srcPath, string destDir)
    {
        if (Directory.Exists(srcPath))
        {
            CopyDirectory(srcPath, destDir);
        }
        else
        {
            CopyFile(srcPath, destDir);
        }
    }

    private void CopyDirectory(string srcDir, string destDir)
    {
        DirectoryInfo srcDirectory = new DirectoryInfo(srcDir);
        DirectoryInfo destDirectory = new DirectoryInfo(destDir);
        
        if (destDirectory.FullName.StartsWith(srcDirectory.FullName, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new Exception("cannot copy parent to child directory.");
        }

        if (!srcDirectory.Exists)
        {
            return;
        }

        if (!destDirectory.Exists)
        {
            destDirectory.Create();
        }

        FileInfo[] files = srcDirectory.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            CopyFile(files[i].FullName, destDirectory.FullName);
        }

        DirectoryInfo[] dirs = srcDirectory.GetDirectories();
        for (int j = 0; j < dirs.Length; j++)
        {
            CopyDirectory(dirs[j].FullName, destDirectory.FullName + @"\" + dirs[j].Name);
        }
    }

    private void CopyFile(string srcFile, string destDir)
    { 
        DirectoryInfo destDirectory = new DirectoryInfo(destDir);
        string fileName = Path.GetFileName(srcFile);
        if (!File.Exists(srcFile))
        {
            return;
        }

        if (!destDirectory.Exists)
        {
            destDirectory.Create();
        }
        
        File.Copy(srcFile, destDirectory.FullName + @"\" + fileName, true);
    }
}
