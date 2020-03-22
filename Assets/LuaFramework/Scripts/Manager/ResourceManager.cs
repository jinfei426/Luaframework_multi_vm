using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using LuaInterface;
using UniObject = UnityEngine.Object;
using System.Reflection;

public class AssetBundleInfo {
    public AssetBundle m_AssetBundle;
    public int m_ReferencedCount;

    public AssetBundleInfo(AssetBundle assetBundle) {
        m_AssetBundle = assetBundle;
        m_ReferencedCount = 0;
    }
}

public class ResInfo
{
    public string ABN;
    public string Name;
    public string Type;
}

namespace LuaFramework
{

    public class ResourceManager : Manager
    {
        string m_BaseDownloadingURL = "";
        string[] m_AllManifest = null;
        AssetBundleManifest m_AssetBundleManifest = null;
        Dictionary<string, string> m_AbName2Manifest = new Dictionary<string, string>();
        Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
        Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();
        Dictionary<string, List<LoadAssetRequest>> m_LoadRequests = new Dictionary<string, List<LoadAssetRequest>>();
        Dictionary<string, Type> m_Types;
        Dictionary<int, ResInfo> ResLoadDict = new Dictionary<int, ResInfo>();
        List<LoadAssetRequest> m_reqQue = new List<LoadAssetRequest>();
        class LoadAssetRequest
        {
            public Type assetType;
            public string[] assetNames;
            public LuaFunction luaFunc;
            public Action<UniObject[]> sharpFunc;
            public string abName;
            public int seq;
        }
        int curSeq = 0;


        // Load AssetBundleManifest.
        public void Initialize(string manifestName, Action initOK)
        {
            StartCoroutine(CheckSeqLoad());

            float t1 = Time.realtimeSinceStartup;
            m_BaseDownloadingURL = Facade.GetRelativePath();
            LoadAsset(typeof(AssetBundleManifest), manifestName, new string[] { "AssetBundleManifest" }, delegate (UniObject[] objs)
            {
                if (objs.Length > 0)
                {
                    m_AssetBundleManifest = objs[0] as AssetBundleManifest;
                    float t2 = Time.realtimeSinceStartup;
                    m_AllManifest = m_AssetBundleManifest.GetAllAssetBundles();
                    Debug.LogFormat("GetAllAssetBundles {0}, {1}", t2 - t1, Time.realtimeSinceStartup - t2);

                    for (int i = 0; i < m_AllManifest.Length; i++)
                    {
                        int index = m_AllManifest[i].LastIndexOf('/');
                        string abName = m_AllManifest[i].Remove(0, index + 1);
                        if (string.IsNullOrEmpty(abName)) continue;

                        if (!m_AbName2Manifest.ContainsKey(abName))
                        {
                            m_AbName2Manifest.Add(abName, m_AllManifest[i]);
                        }
                        else
                        {
                            Debug.LogError("Initialize same AbName: " + abName);
                        }
                    }
                }

                ResIndexFileInit(initOK);
            });
        }

        /// <summary>
        /// 资源索引文件初始化
        /// </summary>
        void ResIndexFileInit(Action initOK)
        {
            string abAssetsIds = Facade.DataPath + "/" + AppConst.AbAssetsIds;
            string[] linedata = File.ReadAllLines(abAssetsIds);
            for (int i = 0; i < linedata.Length; ++i)
            {
                string str = linedata[i];
                string[] data = str.Split('|');
                if (data == null || data.Length != 4)
                {
                    Debug.LogError("ResIndex error str=" + str);
                    return;
                }
                int Id = int.Parse(data[0]);
                ResInfo info = new ResInfo();
                info.ABN = data[1] + ".unity3d";
                info.Name = data[2];
                info.Type = data[3];
                if (ResLoadDict.ContainsKey(Id))
                {
                    Debug.LogError("ResIndex Id repeat, Id=" + Id + "   linedata=" + linedata);
                    return;
                }
                ResLoadDict.Add(Id, info);
            }
            if (initOK != null) initOK();
        }


        public void LoadResource(int Id, Action<UniObject[]> func)
        {
            ResInfo info;
            if (!ResLoadDict.TryGetValue(Id, out info))
            {
                Debug.LogError("load resource error, Id=" + Id);
                return;
            }

            Type t = GetUniObjectType(info.Type);
            LoadAsset(t, info.ABN, new string[] { info.Name }, func);
        }

        /// <summary>
        /// 这个函数是PanelManager管理器 需要对应的资源信息
        /// </summary>
        /// <returns></returns>
        public ResInfo GetResInfo(int Id)
        {
            ResInfo info = null;
            ResLoadDict.TryGetValue(Id, out info);
            return info;
        }

        Type GetUniObjectType(string typeName)
        {
            Type t;
            if (m_Types == null)
            {
                m_Types = new Dictionary<string, Type>();
                //sealed class
                m_Types.Add(typeof(AudioClip).Name, typeof(AudioClip));
                m_Types.Add(typeof(UnityEngine.Video.VideoClip).Name, typeof(UnityEngine.Video.VideoClip));
            }
            if (!m_Types.TryGetValue(typeName, out t))
            {
                const string AssemblyPrefix = "UnityEngine.";
                Assembly unityAssembly = Assembly.GetAssembly(typeof(GameObject));
                t = unityAssembly.GetType(typeName);
                if (t == null && typeName.IndexOf(AssemblyPrefix) < 0)
                {
                    t = unityAssembly.GetType(AssemblyPrefix + typeName);
                }

                if (t == null)
                {
                    Debug.LogFormat("GetUniObjectType not type name: {0}", typeName);
                    return t;
                }
                m_Types.Add(typeName, t);
            }
            return t;
        }

        //---------------------------------------------------------------------
        string GetRealAssetPath(string abName)
        {
            if (abName.Equals(AppConst.AssetDir))
            {
                return abName + Facade.PackExtName;
            }
            abName = abName.ToLower();
            if (!abName.EndsWith(Facade.PackExtName))
            {
                abName += Facade.PackExtName;
            }
            if (abName.Contains("/"))
            {
                return abName;
            }

            string path = null;
            if (!m_AbName2Manifest.TryGetValue(abName, out path))
            {
                Debug.LogError("GetRealAssetPath Error:>>" + abName);
            }
            return path;
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        void LoadAsset(Type assetType, string abName, string[] assetNames, Action<UniObject[]> action = null, LuaFunction func = null)
        {
            abName = GetRealAssetPath(abName);

            LoadAssetRequest request = new LoadAssetRequest();
            request.assetType = assetType;
            request.assetNames = assetNames;
            request.luaFunc = func;
            request.sharpFunc = action;
            request.abName = abName;
            request.seq = curSeq++;
            m_reqQue.Add(request);

            List<LoadAssetRequest> requests = null;
            if (!m_LoadRequests.TryGetValue(abName, out requests))
            {
                //
                requests = new List<LoadAssetRequest>();
                requests.Add(request);
                m_LoadRequests.Add(abName, requests);
            }
            else
            {
                requests.Add(request);
            }
        }

        //检测单加载队列, 有序化加载, 减少并发压力
        IEnumerator CheckSeqLoad()
        {
            WaitForSeconds w = new WaitForSeconds(0.05f);
            while (true)
            {
                if (m_reqQue.Count > 0)
                {
                    LoadAssetRequest req = m_reqQue[0];
                    yield return StartCoroutine(OnLoadAsset(req.assetType, req.abName));
                    yield return null;
                }
                yield return w;
            }
        }

        IEnumerator OnLoadAsset(Type assetType, string abName)
        {
            AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null)
            {
                yield return StartCoroutine(OnLoadAssetBundle(abName, assetType));

                bundleInfo = GetLoadedAssetBundle(abName);
                if (bundleInfo == null)
                {
                    m_LoadRequests.Remove(abName);
                    Debug.LogError("OnLoadAsset--->>>" + abName + "=========reference=" + assetType.ToString());
                    yield break;
                }
            }
            List<LoadAssetRequest> list = null;
            if (!m_LoadRequests.TryGetValue(abName, out list))
            {
                m_LoadRequests.Remove(abName);
                yield break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                string[] assetNames = list[i].assetNames;
                List<UniObject> result = new List<UniObject>();

                AssetBundle ab = bundleInfo.m_AssetBundle;
                for (int j = 0; j < assetNames.Length; j++)
                {
                    string assetPath = assetNames[j];
                    AssetBundleRequest request = ab.LoadAssetAsync(assetPath, list[i].assetType);
                    yield return request;
                    result.Add(request.asset);
                }
                if (list[i].sharpFunc != null)
                {
                    list[i].sharpFunc(result.ToArray());
                    list[i].sharpFunc = null;
                }
                if (list[i].luaFunc != null)
                {
                    list[i].luaFunc.Call((object)result.ToArray());
                    list[i].luaFunc.Dispose();
                    list[i].luaFunc = null;
                }
                bundleInfo.m_ReferencedCount++;
                m_reqQue.Remove(list[i]);
            }
            m_LoadRequests.Remove(abName);
        }

        IEnumerator OnLoadAssetBundle(string abName, Type assetType)
        {
            string url = m_BaseDownloadingURL + abName;

            WWW download = null;
            if (assetType == typeof(AssetBundleManifest))
                download = new WWW(url);
            else
            {
                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(abName);
                if (dependencies.Length > 0)
                {
                    if (!m_Dependencies.ContainsKey(abName))
                        m_Dependencies.Add(abName, dependencies);

                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        string depName = dependencies[i];
                        AssetBundleInfo bundleInfo = null;
                        if (m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo))
                        {
                            bundleInfo.m_ReferencedCount++;
                        }
                        else
                        {
                            yield return StartCoroutine(OnLoadAssetBundle(depName, assetType));
                        }
                    }
                }

                download = new WWW(url);
            }
            yield return download;

            if (!m_LoadedAssetBundles.ContainsKey(abName))
            {
                AssetBundle assetObj = AssetBundle.LoadFromMemory(download.bytes);
                if (assetObj != null)
                {
                    m_LoadedAssetBundles.Add(abName, new AssetBundleInfo(assetObj));
                }
            }
            download.Dispose();
        }

        AssetBundleInfo GetLoadedAssetBundle(string abName)
        {
            AssetBundleInfo bundle = null;
            m_LoadedAssetBundles.TryGetValue(abName, out bundle);
            if (bundle == null) return null;

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return bundle;


            // Make sure all dependencies are loaded
            for (int i = 0; i < dependencies.Length; ++i)
            {
                AssetBundleInfo dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependencies[i], out dependentBundle);
                if (dependentBundle == null) return null;
            }
            return bundle;
        }

        /// <summary>
        /// 此函数交给外部卸载专用，自己调整是否需要彻底清除AB
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="isThorough"></param>
        public void UnloadAssetBundle(string abName, bool isThorough = false)
        {
            abName = GetRealAssetPath(abName);
            UnloadAssetBundleInternal(abName, isThorough);
            UnloadDependencies(abName, isThorough);
        }

        void UnloadDependencies(string abName, bool isThorough)
        {
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return;

            // Loop dependencies.
            for (int i = 0; i < dependencies.Length; ++i)
            {
                UnloadAssetBundleInternal(dependencies[i], isThorough);
            }
            m_Dependencies.Remove(abName);
        }

        void UnloadAssetBundleInternal(string abName, bool isThorough)
        {
            AssetBundleInfo bundle = GetLoadedAssetBundle(abName);
            if (bundle == null) return;

            if (--bundle.m_ReferencedCount <= 0)
            {
                if (m_LoadRequests.ContainsKey(abName))
                {
                    return;     //如果当前AB处于Async Loading过程中，卸载会崩溃，只减去引用计数即可
                }
                bundle.m_AssetBundle.Unload(isThorough);
                m_LoadedAssetBundles.Remove(abName);
            }
        }

        //TODO 清掉所有ab 注意异步问题
        public void UnloadAllAb()
        {
            foreach (var en in m_LoadedAssetBundles)
            {
                en.Value.m_AssetBundle.Unload(true);
            }
            m_LoadedAssetBundles.Clear();
        }

        void OnDestroy()
        {
            UnloadAllAb();
        }
    }
}