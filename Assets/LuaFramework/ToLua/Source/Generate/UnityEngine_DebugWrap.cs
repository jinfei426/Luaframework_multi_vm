﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UnityEngine_DebugWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.Debug), typeof(System.Object));
		L.RegFunction("DrawLine", DrawLine);
		L.RegFunction("DrawRay", DrawRay);
		L.RegFunction("Break", Break);
		L.RegFunction("DebugBreak", DebugBreak);
		L.RegFunction("Log", Log);
		L.RegFunction("LogFormat", LogFormat);
		L.RegFunction("LogError", LogError);
		L.RegFunction("LogErrorFormat", LogErrorFormat);
		L.RegFunction("ClearDeveloperConsole", ClearDeveloperConsole);
		L.RegFunction("LogException", LogException);
		L.RegFunction("LogWarning", LogWarning);
		L.RegFunction("LogWarningFormat", LogWarningFormat);
		L.RegFunction("Assert", Assert);
		L.RegFunction("AssertFormat", AssertFormat);
		L.RegFunction("LogAssertion", LogAssertion);
		L.RegFunction("LogAssertionFormat", LogAssertionFormat);
		L.RegFunction("New", _CreateUnityEngine_Debug);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("unityLogger", get_unityLogger, null);
		L.RegVar("developerConsoleVisible", get_developerConsoleVisible, set_developerConsoleVisible);
		L.RegVar("isDebugBuild", get_isDebugBuild, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_Debug(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UnityEngine.Debug obj = new UnityEngine.Debug();
				ToLua.PushSealed(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UnityEngine.Debug.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DrawLine(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 1);
				UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 2);
				UnityEngine.Debug.DrawLine(arg0, arg1);
				return 0;
			}
			else if (count == 3)
			{
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 1);
				UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 2);
				UnityEngine.Color arg2 = ToLua.ToColor(L, 3);
				UnityEngine.Debug.DrawLine(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 4)
			{
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 1);
				UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 2);
				UnityEngine.Color arg2 = ToLua.ToColor(L, 3);
				float arg3 = (float)LuaDLL.luaL_checknumber(L, 4);
				UnityEngine.Debug.DrawLine(arg0, arg1, arg2, arg3);
				return 0;
			}
			else if (count == 5)
			{
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 1);
				UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 2);
				UnityEngine.Color arg2 = ToLua.ToColor(L, 3);
				float arg3 = (float)LuaDLL.luaL_checknumber(L, 4);
				bool arg4 = LuaDLL.luaL_checkboolean(L, 5);
				UnityEngine.Debug.DrawLine(arg0, arg1, arg2, arg3, arg4);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.DrawLine");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DrawRay(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 1);
				UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 2);
				UnityEngine.Debug.DrawRay(arg0, arg1);
				return 0;
			}
			else if (count == 3)
			{
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 1);
				UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 2);
				UnityEngine.Color arg2 = ToLua.ToColor(L, 3);
				UnityEngine.Debug.DrawRay(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 4)
			{
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 1);
				UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 2);
				UnityEngine.Color arg2 = ToLua.ToColor(L, 3);
				float arg3 = (float)LuaDLL.luaL_checknumber(L, 4);
				UnityEngine.Debug.DrawRay(arg0, arg1, arg2, arg3);
				return 0;
			}
			else if (count == 5)
			{
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 1);
				UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 2);
				UnityEngine.Color arg2 = ToLua.ToColor(L, 3);
				float arg3 = (float)LuaDLL.luaL_checknumber(L, 4);
				bool arg4 = LuaDLL.luaL_checkboolean(L, 5);
				UnityEngine.Debug.DrawRay(arg0, arg1, arg2, arg3, arg4);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.DrawRay");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Break(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UnityEngine.Debug.Break();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DebugBreak(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UnityEngine.Debug.DebugBreak();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Log(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Debug.Log(arg0);
				return 0;
			}
			else if (count == 2)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 2);
				UnityEngine.Debug.Log(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.Log");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogFormat(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (TypeChecker.CheckTypes<UnityEngine.Object, string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 3, count - 2))
			{
				UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				object[] arg2 = ToLua.ToParamsObject(L, 3, count - 2);
				UnityEngine.Debug.LogFormat(arg0, arg1, arg2);
				return 0;
			}
			else if (TypeChecker.CheckTypes<string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 2, count - 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				object[] arg1 = ToLua.ToParamsObject(L, 2, count - 1);
				UnityEngine.Debug.LogFormat(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.LogFormat");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogError(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Debug.LogError(arg0);
				return 0;
			}
			else if (count == 2)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 2);
				UnityEngine.Debug.LogError(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.LogError");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogErrorFormat(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (TypeChecker.CheckTypes<UnityEngine.Object, string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 3, count - 2))
			{
				UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				object[] arg2 = ToLua.ToParamsObject(L, 3, count - 2);
				UnityEngine.Debug.LogErrorFormat(arg0, arg1, arg2);
				return 0;
			}
			else if (TypeChecker.CheckTypes<string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 2, count - 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				object[] arg1 = ToLua.ToParamsObject(L, 2, count - 1);
				UnityEngine.Debug.LogErrorFormat(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.LogErrorFormat");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearDeveloperConsole(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UnityEngine.Debug.ClearDeveloperConsole();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogException(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				System.Exception arg0 = (System.Exception)ToLua.CheckObject<System.Exception>(L, 1);
				UnityEngine.Debug.LogException(arg0);
				return 0;
			}
			else if (count == 2)
			{
				System.Exception arg0 = (System.Exception)ToLua.CheckObject<System.Exception>(L, 1);
				UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 2);
				UnityEngine.Debug.LogException(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.LogException");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogWarning(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Debug.LogWarning(arg0);
				return 0;
			}
			else if (count == 2)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 2);
				UnityEngine.Debug.LogWarning(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.LogWarning");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogWarningFormat(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (TypeChecker.CheckTypes<UnityEngine.Object, string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 3, count - 2))
			{
				UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				object[] arg2 = ToLua.ToParamsObject(L, 3, count - 2);
				UnityEngine.Debug.LogWarningFormat(arg0, arg1, arg2);
				return 0;
			}
			else if (TypeChecker.CheckTypes<string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 2, count - 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				object[] arg1 = ToLua.ToParamsObject(L, 2, count - 1);
				UnityEngine.Debug.LogWarningFormat(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.LogWarningFormat");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Assert(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				bool arg0 = LuaDLL.luaL_checkboolean(L, 1);
				UnityEngine.Debug.Assert(arg0);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<string>(L, 2))
			{
				bool arg0 = LuaDLL.luaL_checkboolean(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				UnityEngine.Debug.Assert(arg0, arg1);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<UnityEngine.Object>(L, 2))
			{
				bool arg0 = LuaDLL.luaL_checkboolean(L, 1);
				UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
				UnityEngine.Debug.Assert(arg0, arg1);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<object>(L, 2))
			{
				bool arg0 = LuaDLL.luaL_checkboolean(L, 1);
				object arg1 = ToLua.ToVarObject(L, 2);
				UnityEngine.Debug.Assert(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<string, UnityEngine.Object>(L, 2))
			{
				bool arg0 = LuaDLL.luaL_checkboolean(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				UnityEngine.Object arg2 = (UnityEngine.Object)ToLua.ToObject(L, 3);
				UnityEngine.Debug.Assert(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<object, UnityEngine.Object>(L, 2))
			{
				bool arg0 = LuaDLL.luaL_checkboolean(L, 1);
				object arg1 = ToLua.ToVarObject(L, 2);
				UnityEngine.Object arg2 = (UnityEngine.Object)ToLua.ToObject(L, 3);
				UnityEngine.Debug.Assert(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.Assert");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AssertFormat(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (TypeChecker.CheckTypes<bool, UnityEngine.Object, string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 4, count - 3))
			{
				bool arg0 = LuaDLL.lua_toboolean(L, 1);
				UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
				string arg2 = ToLua.ToString(L, 3);
				object[] arg3 = ToLua.ToParamsObject(L, 4, count - 3);
				UnityEngine.Debug.AssertFormat(arg0, arg1, arg2, arg3);
				return 0;
			}
			else if (TypeChecker.CheckTypes<bool, string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 3, count - 2))
			{
				bool arg0 = LuaDLL.lua_toboolean(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				object[] arg2 = ToLua.ToParamsObject(L, 3, count - 2);
				UnityEngine.Debug.AssertFormat(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.AssertFormat");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogAssertion(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Debug.LogAssertion(arg0);
				return 0;
			}
			else if (count == 2)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 2);
				UnityEngine.Debug.LogAssertion(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.LogAssertion");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogAssertionFormat(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (TypeChecker.CheckTypes<UnityEngine.Object, string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 3, count - 2))
			{
				UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				object[] arg2 = ToLua.ToParamsObject(L, 3, count - 2);
				UnityEngine.Debug.LogAssertionFormat(arg0, arg1, arg2);
				return 0;
			}
			else if (TypeChecker.CheckTypes<string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 2, count - 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				object[] arg1 = ToLua.ToParamsObject(L, 2, count - 1);
				UnityEngine.Debug.LogAssertionFormat(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.Debug.LogAssertionFormat");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_unityLogger(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, UnityEngine.Debug.unityLogger);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_developerConsoleVisible(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, UnityEngine.Debug.developerConsoleVisible);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isDebugBuild(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, UnityEngine.Debug.isDebugBuild);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_developerConsoleVisible(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			UnityEngine.Debug.developerConsoleVisible = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

