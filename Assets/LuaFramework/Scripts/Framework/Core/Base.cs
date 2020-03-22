using UnityEngine;
using System.Collections;
using LuaFramework;
using System.Collections.Generic;

public class Base : MonoBehaviour {
    private AppFacade m_Facade;
    private LuaManager m_LuaMgr;
    private ResourceManager m_ResMgr;
    private TimerManager m_TimerMgr;
    private ThreadManager m_ThreadMgr;

     

    protected AppFacade Facade {
        get {
            if(m_Facade == null)
            {
                AppFacadeWrapper wrapper = gameObject.GetComponent<AppFacadeWrapper>();
                if (wrapper != null)
                {
                    m_Facade = wrapper.appFacade;
                }
            }
            return m_Facade;
        }
    }

    protected LuaManager LuaManager {
        get {
            if (m_LuaMgr == null) {
                m_LuaMgr = Facade.GetManager<LuaManager>(ManagerName.Lua);
            }
            return m_LuaMgr;
        }
    }

    protected ResourceManager ResManager {
        get {
            if (m_ResMgr == null) {
                m_ResMgr = Facade.GetManager<ResourceManager>(ManagerName.Resource);
            }
            return m_ResMgr;
        }
    }

    protected TimerManager TimerManager {
        get {
            if (m_TimerMgr == null) {
                m_TimerMgr = Facade.GetManager<TimerManager>(ManagerName.Timer);
            }
            return m_TimerMgr;
        }
    }

    protected ThreadManager ThreadManager {
        get {
            if (m_ThreadMgr == null) {
                m_ThreadMgr = Facade.GetManager<ThreadManager>(ManagerName.Thread);
            }
            return m_ThreadMgr;
        }
    }
}
