using UnityEngine;
using LuaInterface;

namespace LuaFramework
{
    public class PanelManager : Manager {
        private Transform parent;

        Transform Parent {
            get {
                if (parent == null) {
                    GameObject go = GameObject.FindWithTag("GuiCamera");
                    if (go != null) parent = go.transform;
                }
                return parent;
            }
        }

        /// <summary>
        /// 创建面板，请求资源管理器
        /// </summary>
        /// <param name="type"></param>
        public void CreatePanel(int Id, LuaFunction func = null, Transform root = null) {

            ResInfo info = ResManager.GetResInfo(Id);
            if(info==null)
            {
                Debug.LogError("加载Id="+Id+"  不存在该ID");
                return;
            }

            Transform attachNode = root == null ? Parent : root;
            string assetName = info.Name;

            ResManager.LoadResource(Id, delegate(UnityEngine.Object[] objs) {
                if (attachNode == null) return;
                if (objs.Length == 0) return;
                GameObject prefab = objs[0] as GameObject;
                if (prefab == null) return;

                GameObject go = Instantiate(prefab) as GameObject;
                //提取名字 XXXX/YYY/提panelName.unity3d  提panelName
                string luaPanelName = assetName;
                int startIdx = Mathf.Max(luaPanelName.LastIndexOf("/") + 1, 0);
                int endIdx = luaPanelName.LastIndexOf(".");
                endIdx = endIdx > 0 ? endIdx: luaPanelName.Length;
                luaPanelName = luaPanelName.Substring(startIdx, endIdx - startIdx);
           
                //
                go.name = luaPanelName.Trim();
                go.layer = LayerMask.NameToLayer("Default");
                go.transform.SetParent(attachNode);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.AddComponent<AppFacadeWrapper>().appFacade = Facade;
                LuaBehaviour lb = go.AddComponent<LuaBehaviour>();
                LuaTable tb = lb.GetChunk();

                if (func != null) func.Call(go, Id, tb);
            });
        }
    }
}