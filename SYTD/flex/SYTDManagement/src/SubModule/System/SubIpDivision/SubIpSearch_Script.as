// ActionScript file
import flash.events.MouseEvent;

import mx.collections.ArrayCollection;
import mx.controls.Alert;
import mx.events.CloseEvent;
import mx.managers.PopUpManager;
import mx.rpc.events.FaultEvent;
import mx.rpc.events.ResultEvent;
import mx.rpc.remoting.mxml.RemoteObject;
	
	private var _searchCallBack:Function;
	private var _remote:RemoteObject;
	public function set searchCallBack(fun:Function):void
	{
		this._searchCallBack=fun;
	}
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSearch.addEventListener(MouseEvent.CLICK,OnSearch);
		initSub();
	}
	private function initSub():void
	{
		_remote=new RemoteObject("fluorine");
	 	_remote.source="SubSection";
	 	_remote.showBusyCursor=true;
	 	_remote.GetAllSubList();
	 	_remote.addEventListener(ResultEvent.RESULT,OnGetSubResult);
	 	_remote.addEventListener(FaultEvent.FAULT,OnGetSubFault);  
	}
	private function OnGetSubResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetSubResult);
	 	_remote.removeEventListener(FaultEvent.FAULT,OnGetSubFault); 
	 	var result:ArrayCollection=_remote.GetAllSubList.lastResult as ArrayCollection;
	 	if (result!=null || result.length>0)
	 	{
	 		var obj:Object=new Object();
	 		obj.subCode="";
	 		obj.subName="所有站点";
	 		result.addItemAt(obj,0);
	 		this.cbox_sub.dataProvider=result;
	 		this.cbox_sub.labelField="subName";
	 	}
	 	else
	 	{
	 		Alert.okLabel = "确定";
	 		Alert.show("没有站点数据，请先设置站点","系统提示",Alert.OK);
	 		PopUpManager.removePopUp(this);
	 	}
	}
	private function OnGetSubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetSubResult);
	 	_remote.removeEventListener(FaultEvent.FAULT,OnGetSubFault);  
	 	Alert.okLabel = "确定";
	 	Alert.show("站点数据加载失败，请重新进入。","错误",Alert.OK);
	 	PopUpManager.removePopUp(this);
	}
	private function OnSearch(evt:MouseEvent):void
	{
		if (this.tbox_IP.text!="")
		{
			_remote=new RemoteObject("fluorine");
			_remote.source="IP";
			_remote.showBusyCursor=true;
			_remote.IpCheck(this.tbox_IP.text)
			_remote.addEventListener(ResultEvent.RESULT,OnCheckResult);
			_remote.addEventListener(FaultEvent.FAULT,OnCheckFault);
		}
		else
		{
			Search();
		}
	}
	private function OnCheckResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnCheckResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnCheckFault);
		var result:Boolean=_remote.IpCheck.lastResult as Boolean;
		if (result)
		{
			Search();
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("你填写的IP不是有效IP。","错误",Alert.OK);
		}
	}
	private function OnCheckFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnCheckResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnCheckFault);
		Alert.okLabel = "确定";
		Alert.show("IP有效性检验失败。","错误",Alert.OK);
	}
	private function Search():void
	{		
		if (_searchCallBack!=null)
		{
			this._searchCallBack.call(this,this.cbox_sub.selectedItem.subCode,this.tbox_IP.text);
		}
		PopUpManager.removePopUp(this);
	}
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}