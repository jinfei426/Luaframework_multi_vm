TestView1 = {};

function TestView1:OnCreate(go)
	print("---子虚拟机 TestView1:OnCreate---");
	local root = go.transform;
	root:Find("btnTest"):GetComponent("Button").onClick:AddListener(function( ... )
		print("----子虚拟机-click test1");
	end);

	root:Find("btnQuit"):GetComponent("Button").onClick:AddListener(function( ... )
		print("----子虚拟机-退出子容器 "); 
		QuitVm();
	end);

	root:Find("btnSend"):GetComponent("Button").onClick:AddListener(function( ... )
		local msg = "你是谁";
		print("子容器发送： ", msg);
		EventManager.Inst:Send(EEvent.Channel, ChannelEvt.sub2main, msg);
	end);
end