// ActionScript file
import flash.events.MouseEvent;

import mx.collections.ArrayCollection;
import mx.controls.Alert;
import mx.events.CloseEvent;
import mx.events.DropdownEvent;
import mx.managers.PopUpManager;
import mx.rpc.events.FaultEvent;
import mx.rpc.events.ResultEvent;
import mx.rpc.remoting.mxml.RemoteObject;

	private var __opKind:String="ADD";
	private var __ipId:String="";
	private var __saveCallBack:Function;
	private var _remote:RemoteObject;
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
	}
	public function set opKind(val:String):void
	{
		this.__opKind=val;
		if (val=="ADD")
		{
			this.title="增加站点IP段";
		}
		else
		{
			this.title="修改站点IP段";
		}
	}
	public function set ipId(val:String):void
	{
		this.__ipId=val;
	}
	public function set saveCallBack(fun:Function):void
	{
		this.__saveCallBack=fun;
	}
	/* private function popedomCheck():void
	{
		if (this.__opKind=="ADD")
		{
			if (parentApplication.CommonPopedomCheck("800005002")){this.btnSave.enabled=true;} else {this.btnSave.enabled=false;}
		}
		else if(this.__opKind=="UPDATE")
		{
			if (parentApplication.CommonPopedomCheck("800005003")){this.btnSave.enabled=true;} else {this.btnSave.enabled=false;}
		}
	} */
	public function initData():void
	{		
		//popedomCheck();
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
	 	if (result!=null || result.length>0)
	 	{
	 		this.cbox_sub.dataProvider=result;
	 		this.cbox_sub.labelField="SUBNAME";
	 		initInfo();
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
	private function initInfo():void
	{
		if (this.__opKind=="ADD")
		{
		}
		else if(this.__opKind=="UPDATE")
		{
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Sys.SubIp";			
			_remote.showBusyCursor=true;
			_remote.GetIpInfoById(this.__ipId);
			_remote.addEventListener(ResultEvent.RESULT,OnGetInfoResult);
			_remote.addEventListener(FaultEvent.FAULT,OnGetInfoFault);
		}		
	}
	private function OnGetInfoResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetInfoResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetInfoFault);
		var result:ArrayCollection=_remote.GetIpInfoById.lastResult as ArrayCollection;
		if (result!=null && result.length>0)
		{
			var subSource:ArrayCollection=ArrayCollection(this.cbox_sub.dataProvider);
			for(var i:int=0;i<subSource.length;i++)
			{
				if (subSource.source[i].SUBCODE==result.source[0].SUBCODE)
				{
				     this.cbox_sub.selectedIndex=i;
				     break;
				}
			}
			this.tbox_startIp.text=result.source[0].STARTIP;
			this.tbox_endIp.text=result.source[0].ENDIP;
		}
	}
	private function OnGetInfoFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetInfoResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetInfoFault);
		Alert.okLabel = "确定";
		Alert.show("初始化数据失败，请退出后重新进入。"+evt.fault.faultString,"系统提示",Alert.OK,this);
	}
	private function OnSave(evt:MouseEvent):void
	{
		if (this.cbox_sub.selectedItem==null || this.cbox_sub.selectedItem.SUBCODE=="")
		{
			Alert.okLabel = "确定";
			Alert.show("请选择站点。","系统提示",Alert.OK,this);
			return;
		}
		if (this.tbox_startIp.text=="")
		{
			Alert.okLabel = "确定";
			Alert.show("请开始IP地址。","系统提示",Alert.OK,this);
			return;	
		}
		if (this.tbox_endIp.text=="")
		{
			Alert.okLabel = "确定";
			Alert.show("请结束IP地址。","系统提示",Alert.OK,this);
			return;	
		}
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Sys.SubIp";
		_remote.showBusyCursor=true;
		if (this.__opKind=="ADD")
		{
			_remote.AddIp(this.cbox_sub.selectedItem.SUBCODE,
						  this.tbox_startIp.text,
						  this.tbox_endIp.text);	
		}
		else if(this.__opKind=="UPDATE")
		{
			_remote.UpdateIp(this.__ipId,
							 this.cbox_sub.selectedItem.SUBCODE,
						     this.tbox_startIp.text,
						     this.tbox_endIp.text);	
		}
		_remote.addEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSaveFault);
	}
	private function OnSaveResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		var result:String;
		if (this.__opKind=="ADD")
		{
			result=_remote.AddIp.lastResult as String;
		}
		else if(this.__opKind=="UPDATE")
		{
			result=_remote.UpdateIp.lastResult as String;
		}
		if (result!=null)
		{
			if (result.substr(0,2).toUpperCase()=="OK")
			{
				if (this.__opKind=="ADD")
				{
					this.__ipId=result.substr(2);
				}
				if (this.__saveCallBack!=null)
				{
					this.__saveCallBack.call(this,
											 this.__ipId,
											 this.cbox_sub.selectedItem.SUBCODE,
											 this.cbox_sub.selectedItem.SUBNAME,
											 this.tbox_startIp.text,
											 this.tbox_endIp.text,
											 this.__opKind);
				}
				PopUpManager.removePopUp(this);
			}
			else
			{
				Alert.okLabel = "确定";
				Alert.show(result,"系统提示",Alert.OK,this);
			}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("保存失败。","系统提示",Alert.OK,this);
		}
	}
	private function OnSaveFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，保存失败。"+evt.fault.faultString,"系统提示",Alert.OK,this);
	}
	
	private function OnBack(evt:MouseEvent):void
	{
		Alert.yesLabel = "是";
		Alert.noLabel = "否";
		Alert.show("确定要返回列表？","系统提示",Alert.YES|Alert.NO,this,AlertBackHandle);
	}
	private function AlertBackHandle(evt:CloseEvent):void
	{		
		if (Alert.YES==evt.detail)
		{
			PopUpManager.removePopUp(this);
		}
	}
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}
	
	