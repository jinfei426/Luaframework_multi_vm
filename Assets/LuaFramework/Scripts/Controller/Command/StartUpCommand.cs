using UnityEngine;
using System.Collections;
using LuaFramework;

public class StartUpCommand  {

    public void Execute(AppFacade app) {
        if (!Util.CheckEnvironment()) return;

        //-----------------关联命令-----------------------
        AppFacade facade = app; 

        //-----------------初始化管理器-----------------------
        facade.AddManager<LuaManager>(ManagerName.Lua);
        facade.AddManager<PanelManager>(ManagerName.Panel);
        facade.AddManager<TimerManager>(ManagerName.Timer); 
        facade.AddManager<ResourceManager>(ManagerName.Resource);
        facade.AddManager<ThreadManager>(ManagerName.Thread);
        facade.AddManager<GameManager>(ManagerName.Game);
    }
}