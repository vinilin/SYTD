// ActionScript file
import mx.collections.ArrayCollection;
import mx.controls.Alert;
import mx.managers.PopUpManager;
import mx.rpc.events.FaultEvent;
import mx.rpc.events.ResultEvent;
import mx.rpc.remoting.mxml.RemoteObject;


[Bindable]private var _subSections:ArrayCollection=new ArrayCollection();
[Bindable]private var _publishSections:ArrayCollection=new ArrayCollection();
public var ids:String;
private var _remote:RemoteObject;
public var _saveCallBack:Function;

public function init():void
{
	//var subSections:Array = GetSubSections();
	CreateUI();
	
}
public function CreateUI():void
{
	_remote = new RemoteObject("fluorine");
	_remote.source = "ManagementService.FileT.Publish";
	_remote.showBusyCursor = true;
	_remote.GetSubSections();
	_remote.addEventListener(FaultEvent.FAULT,OnGetSecFault);	
	_remote.addEventListener(ResultEvent.RESULT, OnGetSecResult);
}
public function OnGetSecFault(evt:FaultEvent):void
{
	_remote.removeEventListener(ResultEvent.RESULT,OnGetSecResult);
	_remote.removeEventListener(FaultEvent.FAULT,OnGetSecFault);
	Alert.show(evt.fault.faultString,"错误",Alert.OK);
}
public function OnAdd():void
{
	if(this.srcList.selectedItem == null)
	{
		Alert.show("请选择要发布的站点","错误",Alert.OK);
		return;
	}
	_publishSections.addItem(this.srcList.selectedItem);
	this.dstList.dataProvider = _publishSections;
	_subSections.removeItemAt(this.srcList.selectedIndex);
	this.srcList.dataProvider = _subSections;
}
public function OnDstFocus():void
{
	focusManager.setFocus(dstList);
}
public function OnSrcFocus():void
{
	focusManager.setFocus(srcList);
}
public function OnRemove():void
{
	if(this.dstList.selectedItem == null)
	{
		Alert.show("请选择要移除的站点","错误",Alert.OK);
		return;
	}
	_subSections.addItem(this.dstList.selectedItem);
	this.srcList.dataProvider = _subSections;
	_publishSections.removeItemAt(this.dstList.selectedIndex);
	this.dstList.dataProvider = _publishSections;
	
}

public function OnGetSecResult(evt:ResultEvent):void
{
	_subSections=_remote.GetSubSections.lastResult as ArrayCollection;
	if(_subSections.length == 0)
	{
		Alert.show(evt.result.toString(),"错误",Alert.OK);
		return;
	}
	_remote.removeEventListener(ResultEvent.RESULT,OnGetSecResult);
	_remote.removeEventListener(FaultEvent.FAULT,OnGetSecFault);
	// 获取
	for(var i:int; i < _subSections.length; ++i)	
	{
		this.srcList.dataProvider = _subSections.source;
	}
	
}
public function OnOK():void
{
	var dst:Array = new Array();
	for(var i:int = 0; i < _publishSections.length; ++i)
	{
		dst.push(_publishSections.source[i].SUBCODE);
	}
	_remote=new RemoteObject("fluorine");
	_remote.source="ManagementService.FileT.Music";
	_remote.PublishMusic(ids,dst);
	_remote.addEventListener(FaultEvent.FAULT,OnPubFault);
	_remote.addEventListener(ResultEvent.RESULT,OnPubResult);
}
private function OnCancel():void
{
	mx.managers.PopUpManager.removePopUp(this);
}
public function OnPubFault(evt:FaultEvent):void
{
	_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
	_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
	Alert.show(evt.fault.faultString,"错误",Alert.OK);
}
public function OnPubResult(evt:ResultEvent):void
{
	_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
	_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
	var result:String = "";
	result = _remote.PublishMusic.lastResult as String;
	if(result != "OK")
	{
		Alert.show(result,"错误",Alert.OK);
		return;
	}
	else
	{
		Alert.show("操作成功","提示",Alert.OK);
		this._saveCallBack.call();
	}
	mx.managers.PopUpManager.removePopUp(this);
	
}
public function OnClose():void
{
	mx.managers.PopUpManager.removePopUp(this);
}