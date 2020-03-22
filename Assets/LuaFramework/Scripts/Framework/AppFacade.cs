using UnityEngine;
using LuaFramework;

 
public enum FaceType
{
    vm1 = 0,    //虚拟机1(容器1)
    vm2 = 1,    //虚拟机2(容器2)
    eCnt,
}

public class AppFacade : Facade
{
    private static AppFacade[] _instances = new AppFacade[(int)FaceType.eCnt];
      

    private string m_RootResName = "";  //当前根资源目录名字
    public string RootResNm
    {
        get
        {
            return m_RootResName;
        }
    }

    //本实例的facetype
    public int InstType
    {
        get; set;
    }

    //本实例的所用后缀, unity打包出来的两个ab的名字如果相同的话，app同时加载这两个会冲突！
    //所以用后缀区分开来。 后面可以只用资源ID来在上层屏蔽后缀问题
    public string PackExtName
    {
        get
        {
            if (InstType == (int)FaceType.vm2)
            {
                return AppConst.SubVmExNm;
            }
            else
            {
                return AppConst.MainVmExNm;
            }
        }
    }

    public string WorkDir
    {
        get
        {
            if(InstType == (int)FaceType.vm2)
            {
                return AppConst.SubWorkName;
            }
            else
            {
                return AppConst.MainWorkName;
            }
        }
    }

    public static string GetDataRootPath()
    {
        if (Application.isMobilePlatform)
        {
            return Application.persistentDataPath + "/";
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            int i = Application.dataPath.LastIndexOf('/');
            return Application.dataPath.Substring(0, i + 1);
        }
        else
        {
            return Application.dataPath + "/" + AppConst.AssetDir + "/";
        }
    }

    public static string GetDataPathByGameName(string gameName)
    {
        string game = gameName.ToLower();
        return GetDataRootPath() + game + "/";
    }

    public static string GetUrlByGameName(string gameName)
    {
        string game = gameName.Trim();
        return AppConst.WebUrl + game + "/";
    }

    //本实例的数据包路径
    private string m_DataPath = null;
    public string DataPath
    {
        get
        {
            if(m_DataPath == null)
            {
                m_DataPath = GetDataPathByGameName(m_RootResName); 
            }
            
            return m_DataPath;
        }
    }

    //跟DataPath对应的文件服保存的app数据地址
    private string m_UrlPath = null;
    public string UrlPath
    {
        get
        {
            if(m_UrlPath == null)
            { 
                m_UrlPath = GetUrlByGameName(m_RootResName);
            }
            return m_UrlPath;
        }
    }

    //
    public string GetRelativePath()
    {
        return "file:///" + DataPath;
    }

    /// <summary>
    /// 应用程序内容路径
    /// </summary>
    public string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.dataPath + "/" + AppConst.AssetDir + "/";
                break;
        }
        return path + RootResNm + "/";
    }


    public AppFacade() : base()
    {
    }

    public static AppFacade GetApp(FaceType appType)
    {
        int idx = (int)appType;
        if (_instances[idx] == null)
        {
            AppFacade ap = new AppFacade();
            ap.InstType = (int)appType;
            if(appType == FaceType.vm2)
            {
                ap.m_RootResName = AppConst.SubResDirNm;
            }
            else
            {
                ap.m_RootResName = AppConst.MainResDirNm;
            }

            _instances[idx] = ap;
        }
        return _instances[idx];
    }

    override protected void InitFramework()
    {
        base.InitFramework(); 
    }

    /// <summary>
    /// 启动框架
    /// </summary>
    public void StartUp(string gameName = "") {

        if(!string.IsNullOrEmpty(gameName))
        {
            this.m_RootResName = gameName;
        }

        //加个AppFacadeWrapper, 给其他Manager找本Facade
        AppFacadeWrapper wp = AppGameManager.GetComponent<AppFacadeWrapper>();
        if(wp == null) wp = AppGameManager.AddComponent<AppFacadeWrapper>();
        wp.appFacade = this;

        new StartUpCommand().Execute(this);
    }

    public static void Clear(FaceType appType)
    {
        if(_instances[(int)appType] != null)
        {
            GetApp(appType).Clear();
            _instances[(int)appType] = null;
        }
    }
}

