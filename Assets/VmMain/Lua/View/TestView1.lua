TestView1 = {};

local id = 10;
function TestView1:OnCreate(go)
	print("---主虚拟机 TestView1:OnCreate---");
	local root = go.transform;
	root:Find("btnTest"):GetComponent("Button").onClick:AddListener(function( ... )
		print("-----主容器 click test1 oooo");
	end);

	root:Find("btnCreate"):GetComponent("Button").onClick:AddListener(function( ... ) 
		GlobalVar.i1 = id;
		id = id + 1;
		--GlobalVar.str1 等等按需要可加
		print("主容器启动子容器： ", LuaFramework.AppConst.SubResDirNm, "设置环境参数", GlobalVar.i1);
		Util.StartVm(LuaFramework.AppConst.SubResDirNm);		--可以指定其他名字
	end);

	root:Find("btnSend"):GetComponent("Button").onClick:AddListener(function( ... )
		local msg = "兄弟你好啊";
		print("主容器发送： ", msg);
		EventManager.Inst:Send(EEvent.Channel, ChannelEvt.main2sub, msg);
	end);

	
end