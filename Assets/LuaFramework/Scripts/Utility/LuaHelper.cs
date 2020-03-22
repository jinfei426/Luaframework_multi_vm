﻿using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using LuaInterface;
using System;

namespace LuaFramework {
    public static class LuaHelper {

        /// <summary>
        /// getType
        /// </summary>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static System.Type GetType(string classname) {
            Assembly assb = Assembly.GetExecutingAssembly();  //.GetExecutingAssembly();
            System.Type t = null;
            t = assb.GetType(classname);
            if (t == null) {
                t = assb.GetType(classname);
            }
            return t;
        }

        //unity
        public static System.Type GetUniType(string classname)
        {
            Assembly assb = Assembly.GetAssembly(typeof(GameObject));
            if(assb != null)
            {
                return assb.GetType(classname);
            }
            return null;
        }

        public static UnityEngine.Object GetComponent(Transform trans, string compName)
        {
            Component co = trans.GetComponent(compName);
            return co;
        }

        /// <summary>
        /// 面板管理器
        /// </summary>
        public static PanelManager GetPanelManager(int appType) {

            return AppFacade.GetApp((FaceType)appType).GetManager<PanelManager>(ManagerName.Panel);
        }

        /// <summary>
        /// 资源管理器
        /// </summary>
        public static ResourceManager GetResManager(int appType) {
            return AppFacade.GetApp((FaceType)appType).GetManager<ResourceManager>(ManagerName.Resource);
        }

 
        /// <summary>
        /// pbc/pblua函数回调
        /// </summary>
        /// <param name="func"></param>
        public static void OnCallLuaFunc(LuaByteBuffer data, LuaFunction func) {
            if (func != null) func.Call(data);
            Debug.LogWarning("OnCallLuaFunc length:>>" + data.buffer.Length);
        }

        /// <summary>
        /// cjson函数回调
        /// </summary>
        /// <param name="data"></param>
        /// <param name="func"></param>
        public static void OnJsonCallFunc(string data, LuaFunction func) {
            Debug.LogWarning("OnJsonCallback data:>>" + data + " lenght:>>" + data.Length);
            if (func != null) func.Call(data);
        }
    }
}