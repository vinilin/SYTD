// ActionScript file
import flash.events.MouseEvent;

import mx.collections.ArrayCollection;
import mx.controls.Alert;
import mx.events.CloseEvent;
import mx.managers.PopUpManager;
import mx.rpc.events.FaultEvent;
import mx.rpc.events.ResultEvent;
import mx.rpc.remoting.mxml.RemoteObject;

	private var __opKind:String="ADD";
	private var __subId:String="";
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
			this.title="增加站点";
		}
		else
		{
			this.title="修改站点";
		}
	}
	public function set subId(val:String):void
	{
		this.__subId=val;
	}
	public function set saveCallBack(fun:Function):void
	{
		this.__saveCallBack=fun;
	}
	public function initData():void
	{
		if (this.__opKind=="ADD")
		{
		}
		else if(this.__opKind=="UPDATE")
		{
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Sys.SubSection";			
			_remote.showBusyCursor=true;
			_remote.GetSubInfoById(this.__subId);
			_remote.addEventListener(ResultEvent.RESULT,OnGetInfoResult);
			_remote.addEventListener(FaultEvent.FAULT,OnGetInfoFault);
		}		
	}
	private function OnGetInfoResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetInfoResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetInfoFault);
		var result:ArrayCollection=_remote.GetSubInfoById.lastResult as ArrayCollection;
		if (result!=null && result.length>0)
		{
			this.tbox_subCode.text=result.source[0].SUBCODE;
			this.tbox_subName.text=result.source[0].SUBNAME;
			this.tbox_serverIp.text=result.source[0].SERVERIP;
			if (result.source[0].ISCENTER)
			{
				this.rdo_isCenter_yes.selected=true;
			}
			else
			{
				this.rdo_isCenter_no.selected=true;
			}
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
		if (this.tbox_subCode.text=="")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写站点编号。","系统提示",Alert.OK,this);
			return;
		}
		if (this.tbox_subName.text=="")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写站点名称。","系统提示",Alert.OK,this);
			return;	
		}
		if (this.tbox_serverIp.text=="")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写服务器IP。","系统提示",Alert.OK,this);
			return;
		}
		if (this.tbox_subCode.text.toUpperCase()=="GLOBAL")
		{
			Alert.okLabel = "确定";
			Alert.show("站点编号不能为“global”。","系统提示",Alert.OK,this);
			return;
		}
		if (this.tbox_subName.text.toUpperCase().indexOf("全局")>=0)
		{
			Alert.okLabel = "确定";
			Alert.show("站点名称不能包含“全局”二字。","系统提示",Alert.OK,this);
			return;
		}
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Sys.SubSection";
		_remote.showBusyCursor=true;
		var isCenter:String="0";
		if (this.rdo_isCenter_yes.selected){isCenter="1";}
		if (this.__opKind=="ADD")
		{
			_remote.AddSub(this.tbox_subCode.text,
							this.tbox_subName.text,
							this.tbox_serverIp.text,
							isCenter);	
		}
		else if(this.__opKind=="UPDATE")
		{
			_remote.UpdateSub(this.__subId,
							  this.tbox_subCode.text,
							  this.tbox_subName.text,
							  this.tbox_serverIp.text,
							  isCenter);	
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
			result=_remote.AddSub.lastResult as String;
		}
		else if(this.__opKind=="UPDATE")
		{
			result=_remote.UpdateSub.lastResult as String;
		}
		if (result!=null)
		{
			if (result.substr(0,2).toUpperCase()=="OK")
			{
				if (this.__opKind=="ADD")
				{
					this.__subId=result.substr(2);
				}
				if (this.__saveCallBack!=null)
				{
					var isCenterName:String="否";
					var isCenter:String="0";
					if (this.rdo_isCenter_yes.selected){isCenterName="是";isCenter="1";}
					this.__saveCallBack.call(this,
											 this.__subId,
											 this.tbox_subCode.text,
											 this.tbox_subName.text,
											 this.tbox_serverIp.text,
											 isCenter,
											 isCenterName,
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
	
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}
	
	