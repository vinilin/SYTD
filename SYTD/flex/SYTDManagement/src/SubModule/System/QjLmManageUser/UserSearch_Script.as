// ActionScript file
import flash.events.MouseEvent;

import mx.collections.ArrayCollection;
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
		_remote.source="ManagementService.Sys.SubSection";			
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
		var obj:Object=new Object();
		obj.SUBCODE="";
		obj.SUBNAME="";
		result.addItemAt(obj,0);
		this.cbox_subList.dataProvider=result;
		this.cbox_subList.labelField="SUBNAME";
	}
	private function OnGetSubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetSubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetSubFault);
		mx.controls.Alert.show("加载分站数据失败"+evt.fault.faultString);
		mx.managers.PopUpManager.removePopUp(this);
	}
	private function OnSearch(evt:MouseEvent):void
	{
		if (_searchCallBack!=null)
		{
			this._searchCallBack.call(this,this.tbox_userName.text,
									  this.tbox_trueName.text,
									  this.cbox_subList.selectedItem.SUBCODE);
		}
		PopUpManager.removePopUp(this);
	}
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}