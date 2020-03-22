require "Common/define"
require "View/TestView1"

EEvent = {
    StartVm = 1,
    Exception = 2,
    Channel = 3, 
}

ChannelEvt = {
	main2sub = 1,	--主容器向子容器发送消息
	sub2main = 2,	--子容器向主容器发送消息
	--有需要可以继续加id	
};
local ChannelEvt = ChannelEvt;

-- 
local allUIs = {};
local function OnCreate(go, panelId, ui)
	print("--------子虚拟机---OnCreate ", go.name, panelId, ui);
	table.insert(allUIs, go);
	ui:OnCreate(go);
end

function Main()
	print("-----子容器启动-----当前环境参数：", GlobalVar.i1);
	local root = GameObject.Find("UIRoot/channel2");
	print("----channel2 ", root);
	panelMgr:CreatePanel(1, OnCreate, root.transform);

	EventManager.Inst:Register(EEvent.Channel, function(channelId, str)
		if channelId == ChannelEvt.main2sub then
			print("子容器收到主容器channel事件, 内容：", str);
		end
	end)
end

function QuitVm()
	--删掉所有UI
	for _1, go in pairs(allUIs) do
		GameObject.Destroy(go);
	end
	--退出虚拟机
	Util.QuitSubVm();
end
 