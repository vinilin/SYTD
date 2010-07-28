// ActionScript file
import SubModule.DefaultLink.Link.DefaultPic;

import flash.events.MouseEvent;

import mx.collections.ArrayCollection;
import mx.controls.Alert;
import mx.events.CloseEvent;
import mx.managers.PopUpManager;
import mx.rpc.events.FaultEvent;
import mx.rpc.events.ResultEvent;
import mx.rpc.remoting.mxml.RemoteObject;

	private var __opKind:String = "AUDIT";
	private var _param:String = "DEFAULTLINK";
	private var __sequence:String = "";
	private var __linkId:String = "";
	private var __saveCallBack:Function;
	private var __defaultPic:String = "";
	private var _remote:RemoteObject;
	private var _remote1:RemoteObject;
	
	public function set opKind(val:String):void
	{
		this.__opKind=val;
	}
	public function set linkId(val:String):void
	{
		this.__linkId=val;
	}
	public function set saveCallBack(fun:Function):void
	{
		this.__saveCallBack=fun;
	}
	
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnDefaultPic.addEventListener(MouseEvent.CLICK,OnDefaultPicClick);
		this.btnPub.addEventListener(MouseEvent.CLICK,OnPub);
		this.btnNotPub.addEventListener(MouseEvent.CLICK,OnNotPub);
	}
	public function initData():void
	{
		this.title = "审核首页链接";
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Link.DefaultLink";			
		_remote.showBusyCursor=true;
		_remote.GetLinkInfo(this.__linkId);
		_remote.addEventListener(ResultEvent.RESULT,OnGetLinkResult);
		_remote.addEventListener(FaultEvent.FAULT,OnGetLinkFault);
		
	}
	private function OnGetLinkResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetLinkResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetLinkFault);
		var result:ArrayCollection=_remote.GetLinkInfo.lastResult as ArrayCollection;
		if (result!=null && result.length>0)
		{
			this.tbox_linkName.text = result.source[0].LINKNAME;
			this.tbox_linkUrl.text = result.source[0].LINKURL;
			this.__defaultPic = result.source[0].DEFAULTPIC;
			this.__sequence = result.source[0].SEQUENCE;
		}
	}
	private function OnGetLinkFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetLinkResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetLinkFault);

		Alert.show("初始化数据失败，请退出后重新进入。"+evt.fault.faultString,"系统提示",4,this,AlertCloseHandle);
	}
	private function OnPub(evt:MouseEvent):void
	{
		if (this.tbox_linkName.text == "")
		{
			Alert.show("请填写链接名称。","系统提示",4,this,AlertCloseHandle);
			this.tbox_linkName.setFocus();
			return;
		}
		if (this.tbox_linkUrl.text == "")
		{
			Alert.show("请填写链接地址。","系统提示",4,this,AlertCloseHandle);
			this.tbox_linkUrl.setFocus();
			return;
		}
		if (this.__defaultPic == "")
		{
			Alert.show("请先上传图片。","系统提示",4,this,AlertCloseHandle);
			return;
		}
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Link.DefaultLink";
		_remote.showBusyCursor=true;
		_remote.PubLink(this.__linkId,
						this.tbox_linkName.text,
						this.tbox_linkUrl.text,
						this.__defaultPic,
						parentApplication._userInfo.trueName);
		_remote.addEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.addEventListener(FaultEvent.FAULT,OnPubFault);
	}
	private function OnPubResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
		var result:String;
		result=_remote.PubLink.lastResult as String;
		if (result!=null)
		{
			if (result.substr(0,2).toUpperCase() == "OK")
			{	
				if (this.__saveCallBack!=null)
				{
					this.__saveCallBack.call(this,
											 this.__linkId,
											 this.tbox_linkName.text,
											 this.tbox_linkUrl.text,
											 "",
											 "已发布","1",
											 parentApplication._userInfo.trueName,
											 this.__defaultPic);
				}
				PopUpManager.removePopUp(this);
			}
			else
			{
				Alert.show(result,"系统提示",4,this,AlertCloseHandle);
			}
		}
		else
		{
			Alert.show("保存失败。","系统提示",4,this,AlertCloseHandle);
		}
	}
	private function OnPubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);

		Alert.show("网络或系统错误，保存失败。"+evt.fault.faultString,"系统提示",4,this,AlertCloseHandle);
	}
	
	private function OnNotPub(evt:MouseEvent):void
	{
		if (this.tbox_linkName.text=="")
		{
			Alert.show("请填写链接名称。","系统提示",4,this,AlertCloseHandle);
			this.tbox_linkName.setFocus();
			return;
		}
		if (this.tbox_linkUrl.text == "")
		{
			Alert.show("请填写链接地址。","系统提示",4,this,AlertCloseHandle);
			this.tbox_linkUrl.setFocus();
			return;
		}
		if (this.__defaultPic == "")
		{
			Alert.show("请先上传图片。","系统提示",4,this,AlertCloseHandle);
			return;
		}
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Link.DefaultLink";
		_remote.showBusyCursor=true;
		_remote.NotPubLink(this.__linkId,
						   this.tbox_linkName.text,
						   this.tbox_linkUrl.text,
						   this.__defaultPic,
						   parentApplication._userInfo.trueName);
		_remote.addEventListener(ResultEvent.RESULT,OnNotPubResult);
		_remote.addEventListener(FaultEvent.FAULT,OnNotPubFault);
	}
	
	private function OnNotPubResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
		var result:String;
		result=_remote.NotPubLink.lastResult as String;
		if (result!=null)
		{
			if (result.substr(0,2).toUpperCase() == "OK")
			{	
				if (this.__saveCallBack!=null)
				{
					this.__saveCallBack.call(this,
											 this.__linkId,
											 this.tbox_linkName.text,
											 this.tbox_linkUrl.text,
											 "",
											 "未发布","-1",
											 parentApplication._userInfo.trueName,
											 this.__defaultPic);
				}
				PopUpManager.removePopUp(this);
			}
			else
			{
				Alert.show(result,"系统提示",4,this,AlertCloseHandle);
			}
		}
		else
		{
			Alert.show("审核失败。","系统提示",4,this,AlertCloseHandle);
		}
	}
	
	private function OnNotPubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
		Alert.show("网络或系统错误，审核失败。"+evt.fault.faultString,"系统提示",4,this,AlertCloseHandle);
	}
	
	private function AlertCloseHandle(evt:CloseEvent):void
	{
		//this.iFrame.show();
	}
	
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}
	
	private function OnDefaultPicClick(evt:MouseEvent):void
	{
		var pop:DefaultPic=DefaultPic(PopUpManager.createPopUp(this,DefaultPic,true));
		pop.picFileName = this.__defaultPic;
		pop.sequence = this.__sequence;
		pop.linkId = this.__linkId;
		pop.closeCallBack = defaultPicCloseCallBack;
		pop.loadFile();
		PopUpManager.centerPopUp(pop);
	}
	private function defaultPicCloseCallBack(picFileName:String):void
	{
		this.__defaultPic=picFileName;	
	}