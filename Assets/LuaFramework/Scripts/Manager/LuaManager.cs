using UnityEngine;
using System.Collections;
using LuaInterface;

namespace LuaFramework {
    public class LuaManager : Manager {
        private LuaState lua;
        private LuaLoader loader;
        private LuaLooper loop = null;

        // Use this for initialization
        void Awake() {
            loader = new LuaLoader(Facade);
            lua = new LuaState();

            //editor下smallgame的lua路径
            string luaDir = Application.dataPath + "/" + Facade.WorkDir + "/Lua";
            lua.InitLuaPath(luaDir);

            //设置游戏类型
            lua.LuaPushInteger(Facade.InstType);
            lua.LuaSetGlobal("CurAppType");

            this.OpenLibs();
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, this);
        }
        public LuaState GetLuaState()
        {
            return lua;
        }
        public void InitStart() {
            InitLuaPath();
            InitLuaBundle();
            this.lua.Start();    //启动LUAVM
            this.StartMain();
            this.StartLooper();
        }

        void StartLooper() {
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson() {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        void StartMain() {
            lua.DoFile("Main.lua");

            LuaFunction main = lua.GetFunction("Main");
            main.Call();
            main.Dispose();
            main = null;    
        }
        
        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs() {
            lua.OpenLibs(LuaDLL.luaopen_pb);      
            lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);
            //lua.OpenLibs(LuaDLL.luaopen_socket_core); 

            this.OpenCJson();
        }

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        void InitLuaPath() {
            lua.AddSearchPath(Facade.DataPath + "lua");
        }

        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        void InitLuaBundle() {
            if (loader.beZip) {
				loader.AddBundle("lua" + Facade.PackExtName);
            }
        }

        public void DoFile(string filename) {
            lua.DoFile(filename);
        }

        // Update is called once per frame
        public object[] CallFunction(string funcName, params object[] args) {
            if (lua == null)
                return null;

            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) {
                return func.LazyCall(args);
            }
            return null;
        }

        public void LuaGC() {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close() {
            if(loop != null)
            {
                loop.Destroy();
                loop = null;
            }

            if(lua != null)
            {
                lua.Dispose();
                lua = null;
            }
            loader = null;
        }

#region luaide 调试库添加
        //如果项目中没有luasocket 请打开
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Socket_Core(System.IntPtr L)
        {
            return LuaDLL.luaopen_socket_core(L);
        }

        protected void OpenLuaSocket()
        {
            LuaConst.openLuaSocket = true;
            lua.BeginPreLoad();
            lua.RegFunction("socket.core", LuaOpen_Socket_Core);
            lua.EndPreLoad();
        }
#endregion
         
    }
}