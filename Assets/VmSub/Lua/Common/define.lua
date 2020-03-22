
Util = LuaFramework.Util; 
 
LuaHelper = LuaFramework.LuaHelper; 


resMgr = LuaHelper.GetResManager(CurAppType);
panelMgr = LuaFramework.LuaHelper.GetPanelManager(CurAppType);


WWW = UnityEngine.WWW;
GameObject = UnityEngine.GameObject;


--LuaBehaviour调用
function NewMonoLuaObj(monoName, gameObject)
	local newObj = {gameObject = gameObject}
	local cls = rawget(_G, monoName)
	setmetatable(newObj, {__index = cls})
	return newObj
end
 