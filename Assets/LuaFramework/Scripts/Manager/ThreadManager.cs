using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Net;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class ThreadEvent {
    public string Key;
    public List<object> evParams = new List<object>();
}

public class NotiData {
    public string evName;
    public object evParam;

    public NotiData(string name, object param) {
        this.evName = name;
        this.evParam = param;
    }
}

namespace LuaFramework {
    /// <summary>
    /// 当前线程管理器，同时只能做一个任务
    /// </summary>
    public class ThreadManager : Manager {
        private Thread thread;
        private Action<NotiData> func;
        private Stopwatch sw = new Stopwatch();
        private string currDownFile = string.Empty;
        private bool bOut = false;  //是否退出状态 
        private WebClient curClient = null;
        private int waitCnt = 0;

        object m_lockObject = new object();
        Queue<ThreadEvent> events = new Queue<ThreadEvent>();

        delegate void ThreadSyncEvent(NotiData data);
        private ThreadSyncEvent m_SyncEvent;
        AppFacade _AppFacade;
        UnityWebRequest wr = null;
        bool isLoading = false;
        float tick = 0.0f;
        float startTime = 0;
        Coroutine curCo = null;
        void Awake() {
            _AppFacade = Facade;
            m_SyncEvent = OnSyncEvent;
            thread = new Thread(OnUpdate);
        }

        // Use this for initialization
        void Start() {
            thread.Start();
            
        }

        /// <summary>
        /// 添加到事件队列
        /// </summary>
        public void AddEvent(ThreadEvent ev, Action<NotiData> func) { 
            lock (m_lockObject)
            {
                this.func = func;
                events.Enqueue(ev);
                if (thread == null)
                {
                    thread = new Thread(OnUpdate);
                    thread.Start();
                }
            } 
        }

        //取消当前任务
        public void RmEvent(ThreadEvent ev, Action<NotiData> func)
        {
            lock (m_lockObject)
            {
                if(this.func == func)
                {
                    //目前不支持多任务，默认把所有都取消
                    //TODO 支持多任务
                    this.func = null;
                    events.Clear();
                    if (curClient != null)
                    {
                        curClient.CancelAsync();
                        curClient = null;
                    }
                }
            } 
        } 

        /// <summary>
        /// 通知事件
        /// </summary>
        /// <param name="state"></param>
        private void OnSyncEvent(NotiData data) {
            if (this.func != null) func(data);  //回调逻辑层
                                                //  Facade.SendMessageCommand(data.evName, data.evParam); //通知View层
            _AppFacade.SendMessageCommand(data.evName, data.evParam);
        }

        // Update is called once per frame
        void OnUpdate() {
            while (true) { 
                if (events.Count > 0) {
                    waitCnt = 0;  //有工作，不用等
                    lock (m_lockObject)
                    {
                        if(events.Count > 0)
                        {
                            ThreadEvent e = events.Dequeue();
                            try
                            {
                                switch (e.Key)
                                {
                                    case NotiConst.UPDATE_EXTRACT: 
                                        OnExtractFile(e.evParams); 
                                        break;
                                    case NotiConst.UPDATE_DOWNLOAD:
                                        OnDownloadFile(e.evParams);
                                        break;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                UnityEngine.Debug.LogError(ex.Message);
                            }
                        }
                    }
                }
                Thread.Sleep(100);
           
                if(++waitCnt > 80)
                {
                    //8秒没有任务就退出
                    bool bQuit = false;
                    lock (m_lockObject)
                    {
                        bQuit = events.Count < 1;
                        if (bQuit) thread = null;
                    }
                    if(bQuit)
                    {
                        UnityEngine.Debug.Log("-----------quite thread------");
                        break;
                    }
                }
                if (bOut)
                    break;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        void OnDownloadFile(List<object> evParams) {
            string url = evParams[0].ToString();    
            currDownFile = evParams[1].ToString(); 

            using (WebClient client = new WebClient()) {
                sw.Start();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                client.DownloadFileAsync(new System.Uri(url), currDownFile); 
                curClient = client;
            }
        }

        public void Clear()
        {
            lock (m_lockObject)
            {
                events.Clear();
                if (curClient != null)
                {
                    curClient.CancelAsync();
                    curClient = null;
                }
            }

            if (curCo != null)
            {
                StopCoroutine(curCo);
                curCo = null;
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            if (bOut)
                return;

            //long 64位, 2的31次 = 2G
            long time = (long)(sw.Elapsed.TotalMilliseconds);
            long argData = (e.BytesReceived << 32) | time;
            //long d = data >> 32;
            //long d2 = data & 0xFFFFFFFF;
            NotiData data = new NotiData(NotiConst.UPDATE_PROGRESS, argData);
            if (m_SyncEvent != null) m_SyncEvent(data);//OnSyncEvent
            waitCnt = 0;

            if (e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive) {
                sw.Reset();

                data = new NotiData(NotiConst.UPDATE_DOWNLOAD, currDownFile);
                if (m_SyncEvent != null) m_SyncEvent(data);
            }
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        void OnExtractFile(List<object> evParams) {
            //UnityEngine.Debug.LogWarning("Thread evParams: >>" + evParams.Count);

            ///------------------通知更新面板解压完成--------------------
            NotiData data = new NotiData(NotiConst.UPDATE_DOWNLOAD, null);
            if (m_SyncEvent != null) m_SyncEvent(data);
        }

        /// <summary>
        /// 应用程序退出
        /// </summary>
        void OnDestroy() { 
            bOut = true;
            if (curCo != null)
            {
                StopCoroutine(curCo);
                curCo = null;
            }
        } 
    }
}