using UnityEngine;

namespace LuaFramework
{
    public class AppConst { 
        //CS代码版本
        public static int Version = 1;    

        public const bool DebugMode = false;                       //调试模式-用于内部测试
        /// <summary>
        /// 如果想删掉框架自带的例子，那这个例子模式必须要
        /// 关闭，否则会出现一些错误。
        /// </summary>
        public const bool ExampleMode = true;                       //例子模式 

        /// <summary>
        /// 如果开启更新模式，前提必须启动框架自带服务器端。
        /// 否则就需要自己将StreamingAssets里面的所有内容
        /// 复制到自己的Webserver上面，并修改下面的WebUrl。
        /// </summary>
        /// 
        public const bool LuaByteMode = false;                       //Lua字节码模式-默认关闭  
        public static bool UpdateMode = true;                       //更新模式-默认关闭 
        public static bool LuaBundleMode = true;                    //Lua代码AssetBundle模式 打包Lua代码  

        public const int TimerInterval = 1;
        public const int GameFrameRate = 30;                        //游戏帧频
         
        private const string FrameDir = "LuaFramework";                 //框架根目录名字
        public const string AssetDir = "StreamingAssets";               //素材目录
        public static string WebUrl = "127.0.0.1"; 

        public const string AssetsIds = "AssetsId.txt";             //资源id, 游戏使用
        public const string AbAssetsIds = "AbAssetsId.txt";         //打包生成的实际的资源索引文件
        public const string PureResDir = "PureRes";                 //纯资源目录（不用打包ab的）
         

        public static string FrameworkRoot {
            get {
                if(!Application.isEditor)
                {
                    //只在本地用
                    throw new System.Exception("-----------FrameworkRoot---------- only use in editor");
                }
                return Application.dataPath + "/" + FrameDir;
            }
        }

        
        public static readonly string MainWorkName = "VmMain";    //主虚拟机的工作目录
        public static readonly string SubWorkName = "VmSub";   //次虚拟机的工作目录
        public static string MainResDirNm = "Vm1";         //主虚拟机打包目录名（在AssetDir 即StreamingAssets下） 
        public static string SubResDirNm = "vm2";       //次虚拟机打包目录名
        public static readonly string MainVmExNm = "_main.unity3d";   //主虚拟机打包后缀
        public static readonly string SubVmExNm = ".unity3d";        //次虚拟机打包后缀 
         
    }
}