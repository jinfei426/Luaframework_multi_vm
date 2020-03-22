using System.Collections.Generic;
using UnityEngine;
 
public enum EEvent
{
    StartVm = 1,
    Exception = 2,
    Channel = 3,                //
    

    MAX = 100,
}


public delegate void EventHandler(params object[] args);

public class EventManager
{
    private static EventManager _inst;
    public static EventManager Inst
    {
        get
        {
            if(_inst == null)
            {
                _inst = new EventManager();
            }
            return _inst;
        }
    }

    private List<EventHandler>[] mHandlers = new List<EventHandler>[(int)EEvent.MAX + 1];
    private readonly int MaxId = (int)EEvent.MAX;

    public EventManager()
    {
        Application.logMessageReceived += _OnLogCallbackHandler;
    }
    
    //lua调用send， 工具生成的wrapper要求args至少要传一个参数
    public void Send(int eventId, params object[] args)
    {
        if (eventId < 0 || eventId > MaxId)
            return;

        List<EventHandler> handlers = mHandlers[eventId];
        if(handlers == null)
            return;

        bool someNull = false;
        for (int i = 0; i < handlers.Count ; ++i)
        {
            EventHandler h = handlers[i];
            if(h != null)
            {
                h(args);
            }
            else
            {
                someNull = true;
            }
        }

        if(someNull)
        {
            UnityEngine.Debug.LogWarning("-----------EventManager------send----empty " + eventId);
            for (int i=handlers.Count-1; i >= 0; --i)
            {
                handlers.RemoveAt(i);
            }
        }
    }

    public void Register(int eventId, EventHandler h)
    {
        if (eventId < 0 || eventId > MaxId || h == null)
            return;

        List<EventHandler> handlers = mHandlers[eventId];
        if (handlers == null)
        {
            handlers = new List<EventHandler>();
            mHandlers[eventId] = handlers;
        }

        if(!handlers.Contains(h))
        {
            handlers.Add(h);
        }
    }

    public void UnRegister(int eventId, EventHandler h)
    {
        if (eventId < 0 || eventId > MaxId)
            return;

        List<EventHandler> handlers = mHandlers[eventId];
        if (handlers == null)
            return;

        handlers.Remove(h);
    }

    private static void _OnLogCallbackHandler(string condition, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Exception:
                EventManager.Inst.Send((int)EEvent.Exception, stackTrace);
                break;
            case LogType.Error:
                //报错也异常退出
                EventManager.Inst.Send((int)EEvent.Exception, stackTrace, (int)type);
                break;
        }
    }
}

