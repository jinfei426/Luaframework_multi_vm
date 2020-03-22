using UnityEngine;
using System.Collections;
using System.IO;


namespace LuaFramework
{
    public class GameManager : Manager {
        public enum step
        {
            start = 1,
            step2,
            step3,
            step4,
            end, 
        } 

        /// <summary>
        /// 初始化游戏管理器
        /// </summary>
        void Awake() {
            DontDestroyOnLoad(gameObject);  //防止销毁自己

            EventManager.Inst.Send((int)EEvent.StartVm, Facade.RootResNm, (int)step.start);

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = AppConst.GameFrameRate;
        }
          
        private IEnumerator Start()
        {
            yield return StartCoroutine(OnUpdateResource());

            OnResourceInited();
        }


        //更新
        IEnumerator OnUpdateResource() {
            string dataPath = Facade.DataPath;  //数据目录
            string url = Facade.UrlPath;

            LoadWorker worker = gameObject.GetComponent<UpdateWorker>();
            if (worker == null) worker = gameObject.AddComponent<UpdateWorker>();
            yield return StartCoroutine(worker.StartWork(dataPath, url, ThreadManager));
        }


        /// <summary>
        /// 资源初始化结束
        /// </summary>
        public void OnResourceInited() { 
            ResManager.Initialize(AppConst.AssetDir, delegate() {
                Debug.Log("Initialize OK!!!"); 
                this.OnInitialize(); 
            });
        }

        void OnInitialize() {
            EventManager.Inst.Send((int)EEvent.StartVm, Facade.RootResNm, (int)step.end); 
            LuaManager.InitStart();
        }


        /// <summary>
        /// 析构函数
        /// </summary>
        void OnDestroy() {
            if (LuaManager != null) {
                LuaManager.Close();
            }
            Debug.Log("~GameManager was destroyed");
        }

        //显示切场UI
        public void ShowSwitchUI()
        { 
            LoadWorker worker = gameObject.GetComponent<SwitchWorker>();
            if (worker == null)
                gameObject.AddComponent<SwitchWorker>();
        }
    }
}