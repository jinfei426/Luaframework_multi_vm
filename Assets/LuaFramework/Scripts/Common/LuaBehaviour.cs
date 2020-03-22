using UnityEngine;
using LuaInterface;

namespace LuaFramework
{
    public class LuaBehaviour : View {

        private LuaTable m_LuaChunk;
        private LuaFunction m_Start;
        private LuaFunction m_Update;
        private LuaFunction m_LateUpdate;
        private LuaFunction m_FixedUpdate;
        private LuaFunction m_OnEnable;
        private LuaFunction m_OnDisable;
        private LuaFunction m_OnTriggerEnter;
        private LuaFunction m_OnTriggerExit;
        private LuaFunction m_OnTriggerStay;
        private LuaFunction m_OnCollisionEnter;
        private LuaFunction m_OnCollisionExit;
        private LuaFunction m_OnCollisionStay;
        private LuaFunction m_OnDestroy; 

        void Awake() {
            LuaManager LuaMgr = LuaManager;
            if(LuaMgr==null) return;

            LuaFunction newObjFnc = LuaMgr.GetLuaState().GetFunction("NewMonoLuaObj");
            if (newObjFnc == null) return;

            //获取相关lua类型
            m_LuaChunk = newObjFnc.Invoke<string, GameObject, LuaTable>(name, gameObject);
            newObjFnc.Dispose();

            LuaFunction AwakeFunc = m_LuaChunk["Awake"] as LuaFunction;
            if(AwakeFunc != null)
            {
                AwakeFunc.Call(m_LuaChunk);
                AwakeFunc.Dispose();
            }

            Init();
        }

        void Init()
        {
            if (m_LuaChunk == null)
                return;

            m_Start = m_LuaChunk["Start"] as LuaFunction;
            m_Update = m_LuaChunk["Update"] as LuaFunction;
            m_LateUpdate = m_LuaChunk["LateUpdate"] as LuaFunction;
            m_FixedUpdate = m_LuaChunk["FixedUpdate"] as LuaFunction;
            m_OnEnable = m_LuaChunk["OnEnable"] as LuaFunction;
            m_OnDisable = m_LuaChunk["OnDisable"] as LuaFunction;
            m_OnTriggerEnter = m_LuaChunk[" OnTriggerEnter"] as LuaFunction;
            m_OnTriggerExit = m_LuaChunk["OnTriggerExit"] as LuaFunction;
            m_OnTriggerStay = m_LuaChunk["OnTriggerStay"] as LuaFunction;
            m_OnCollisionEnter = m_LuaChunk["OnCollisionEnter"] as LuaFunction;
            m_OnCollisionExit = m_LuaChunk["OnCollisionExit"] as LuaFunction;
            m_OnCollisionStay = m_LuaChunk["OnCollisionStay"] as LuaFunction;
            m_OnDestroy = m_LuaChunk["OnDestroy"] as LuaFunction;
        }

        //下面要不要添加几个脚本 防止调用一些没必要的函数
        //到时候在讨论
        void Start() {
            if (m_Start != null)
            {
                m_Start.Call(m_LuaChunk);
                m_Start.Dispose();
            }
        }

        void Update()
        {
            if (m_Update != null) m_Update.Call(m_LuaChunk);
        }

        void FixedUpdate()
        {
            if (m_FixedUpdate != null) m_FixedUpdate.Call(m_LuaChunk);
        }

        void LateUpdate()
        {
            if (m_LateUpdate != null) m_LateUpdate.Call(m_LuaChunk);
        }

        void OnTriggerEnter(Collider collider)
        {
            if (m_OnTriggerEnter != null) m_OnTriggerEnter.Call(m_LuaChunk, collider);
        }

        void OnTriggerExit(Collider collider)
        {
            if (m_OnTriggerExit != null) m_OnTriggerExit.Call(m_LuaChunk, collider);
        }

        void OnTriggerStay(Collider collider)
        {
            if (m_OnTriggerStay != null) m_OnTriggerStay.Call(m_LuaChunk, collider);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (m_OnCollisionEnter != null) m_OnCollisionEnter.Call(m_LuaChunk, collision);
        }

        void OnCollisionExit(Collision collision)
        {
            if (m_OnCollisionExit != null) m_OnCollisionExit.Call(m_LuaChunk, collision);
        }

        void OnCollisionStay(Collision collision)
        {
            if (m_OnCollisionStay != null) m_OnCollisionStay.Call(m_LuaChunk, collision);
        }

        void OnEnable()
        {
            if (m_OnEnable != null) m_OnEnable.Call(m_LuaChunk);
        }

        void OnDisable()
        {
            if (m_OnDisable != null) m_OnDisable.Call(m_LuaChunk);
        }

        void OnDestroy()
        {
            if (m_OnDestroy != null) m_OnDestroy.Call(m_LuaChunk);

            if(m_LuaChunk != null)
            {
                m_LuaChunk.Dispose();
                m_LuaChunk = null;
            }
        } 

        public LuaTable GetChunk()
        {
            return m_LuaChunk;
        }
        
    }
}