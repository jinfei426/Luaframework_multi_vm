/* 
    LuaFramework Code By Jarjin lee
*/

using System;
using System.Collections.Generic;
using UnityEngine;
 
public class Facade { 
    GameObject m_GameManager;
    Dictionary<string, object> m_Managers = new Dictionary<string, object>();

    protected GameObject AppGameManager {
        get {
            if (m_GameManager == null) {
                m_GameManager = new GameObject("AppGameManager");
            }
            return m_GameManager;
        }
    }

    protected Facade() {
        InitFramework();
    }
    protected virtual void InitFramework() {
        
    }

    public virtual void RegisterCommand(string commandName, Type commandType) { 
    }

    public virtual void RemoveCommand(string commandName) { 
    }

    public virtual bool HasCommand(string commandName) {
        return false;
    }

    public void RegisterMultiCommand(Type commandType, params string[] commandNames) {
        int count = commandNames.Length;
        for (int i = 0; i < count; i++) {
            RegisterCommand(commandNames[i], commandType);
        }
    }

    public void RemoveMultiCommand(params string[] commandName) {
        int count = commandName.Length;
        for (int i = 0; i < count; i++) {
            RemoveCommand(commandName[i]);
        }
    }

    public void SendMessageCommand(string message, object body = null) { 
    }

    /// <summary>
    /// 添加管理器
    /// </summary>
    public void AddManager(string typeName, object obj) {
        if (!m_Managers.ContainsKey(typeName)) {
            m_Managers.Add(typeName, obj);
        }
    }

    /// <summary>
    /// 添加Unity对象
    /// </summary>
    public T AddManager<T>(string typeName) where T : Component {
        object result = null;
        m_Managers.TryGetValue(typeName, out result);
        if (result != null) {
            return (T)result;
        }
        Component c = AppGameManager.AddComponent<T>();
        m_Managers.Add(typeName, c);
        return default(T);
    }

    /// <summary>
    /// 获取系统管理器
    /// </summary>
    public T GetManager<T>(string typeName) where T : class {
        if (!m_Managers.ContainsKey(typeName)) {
            return default(T);
        }
        object manager = null;
        m_Managers.TryGetValue(typeName, out manager);
        return (T)manager;
    }

    /// <summary>
    /// 删除管理器
    /// </summary>
    public void RemoveManager(string typeName) {
        if (!m_Managers.ContainsKey(typeName)) {
            return;
        }
        object manager = null;
        m_Managers.TryGetValue(typeName, out manager);
        Type type = manager.GetType();
        if (type.IsSubclassOf(typeof(MonoBehaviour))) {
            GameObject.Destroy((Component)manager);
        }
        m_Managers.Remove(typeName);
    }

    //清掉数据、缓存之类, 删掉监听
    public void Clear()
    {
        //禁掉协程先
        m_GameManager.SetActive(false);
        
        GameObject.Destroy(m_GameManager);
    }
}
