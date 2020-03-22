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
local function OnCreate(go, panelId, ui)
	print("-----------OnCreate ", go.name, panelId, ui);
	ui:OnCreate(go);
end

function Main()
	local root = GameObject.Find("UIRoot/channel1");
	print("----VM1----: ", root);
	panelMgr:CreatePanel(1, OnCreate, root.transform);

	EventManager.Inst:Register(EEvent.Channel, function(channelId, str)
		if channelId == ChannelEvt.sub2main then
			print("主容器收到子容器channel事件, 内容：", str);
		end
	end)
end
 