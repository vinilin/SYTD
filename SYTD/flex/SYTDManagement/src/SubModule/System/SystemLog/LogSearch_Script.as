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
	private var _remote1:RemoteObject;
	
	public function set searchCallBack(fun:Function):void
	{
		this._searchCallBack=fun;
	}
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSearch.addEventListener(MouseEvent.CLICK,OnSearch);
		initModule();
		initLogType();
	}
	private function initModule():void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Sys.SystemLog";			
		_remote.showBusyCursor=true;
		_remote.GetLogModule();
		_remote.addEventListener(ResultEvent.RESULT,OnGetModuleResult);
		_remote.addEventListener(FaultEvent.FAULT,OnGetModuleFault);
	}
	private function OnGetModuleResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetModuleResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetModuleFault);
		var result:ArrayCollection=_remote.GetLogModule.lastResult as ArrayCollection;
		var obj:Object=new Object();
		obj.MODULE="";
		result.addItemAt(obj,0);
		this.cbox_module.dataProvider=result;
		this.cbox_module.labelField="MODULE";
	}
	private function OnGetModuleFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetModuleResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetModuleFault);
		mx.controls.Alert.show("加载分站数据失败"+evt.fault.faultString);
		mx.managers.PopUpManager.removePopUp(this);
	}
	private function initLogType():void
	{
		_remote1=new RemoteObject("fluorine");
		_remote1.source="ManagementService.Sys.SystemLog";			
		_remote1.showBusyCursor=true;
		_remote1.GetLogType();
		_remote1.addEventListener(ResultEvent.RESULT,OnGetLogTypeResult);
		_remote1.addEventListener(FaultEvent.FAULT,OnGetLogTypeFault);
	}
	private function OnGetLogTypeResult(evt:ResultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnGetLogTypeResult);
		_remote1.removeEventListener(FaultEvent.FAULT,OnGetLogTypeFault);
		var result:ArrayCollection=_remote1.GetLogType.lastResult as ArrayCollection;
		var obj:Object=new Object();
		obj.LOGTYPE="";
		result.addItemAt(obj,0);
		this.cbox_logType.dataProvider=result;		
		this.cbox_logType.labelField="LOGTYPE";
	}
	private function OnGetLogTypeFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetLogTypeResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetLogTypeFault);
		mx.controls.Alert.show("加载分站数据失败"+evt.fault.faultString);
		mx.managers.PopUpManager.removePopUp(this);
	}
	private function OnSearch(evt:MouseEvent):void
	{
		if (_searchCallBack!=null)
		{
			var startDate:String="";
			var endDate:String="";
			if (this.df_startDate.text!="")
			{
				startDate=this.df_startDate.text+" 00:00:00";
			}
			if (this.df_endDate.text!="")
			{
				endDate=this.df_endDate.text+" 23:59:59";	
			}
			this._searchCallBack.call(this,this.cbox_module.selectedItem.MODULE,
									  this.cbox_logType.selectedItem.LOGTYPE,
									  startDate,
									  endDate);
		}
		PopUpManager.removePopUp(this);
	}
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}