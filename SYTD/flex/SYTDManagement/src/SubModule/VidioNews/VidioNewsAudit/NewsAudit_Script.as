// ActionScript file
import Custom.Common.Com;

import SubModule.VidioNews.VidioNewsAudit.DefaultPic;

import flash.events.MouseEvent;

import mx.collections.ArrayCollection;
import mx.controls.Alert;
import mx.events.CloseEvent;
import mx.events.DropdownEvent;
import mx.events.MoveEvent;
import mx.managers.PopUpManager;
import mx.rpc.events.FaultEvent;
import mx.rpc.events.ResultEvent;
import mx.rpc.remoting.mxml.RemoteObject;

	private var __opKind:String="ADD";
	//private var __articleKind:String="";
	private var _param:String = "";
	private var _isExistIcon:Boolean = false;
	private var _isExistKind:Boolean = false;
	private var __articleId:String = "";
	private var __saveCallBack:Function;
	private var __sequence:String = ""; 
	private var __defaultPic:String = "";
	private var _subCode:String = "global";
	private var _subName:String = "全局";
	private var _remote:RemoteObject;
	private var _remote1:RemoteObject;
	
	public function set opKind(val:String):void
	{
		this.__opKind=val;
	}
	public function set param(val:String):void
	{
		this._param = val;
	}
	public function set isExistKind(val:Boolean):void
	{
		this._isExistKind = val;
	}
	public function set isExistIcon(val:Boolean):void
	{
		this._isExistIcon = val;
	}
	public function set articleId(val:String):void
	{
		this.__articleId=val;
	}
	public function set subCode(val:String):void
	{
		this._subCode = val;
	}
	public function set subName(val:String):void
	{
		this._subName = val;
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
		this.addEventListener(MouseEvent.CLICK,OnClick);
		this.addEventListener(MoveEvent.MOVE,OnMove);
		this.cbox_kind.addEventListener(DropdownEvent.OPEN,OnDropOpen);
		this.cbox_kind.addEventListener(DropdownEvent.CLOSE,OnDropClose);
	}
	private function OnDropOpen(evt:DropdownEvent):void
	{		
		this.iFrame.hidden();	
	}
	private function OnDropClose(evt:DropdownEvent):void
	{
		this.iFrame.show();
	}
	public function initData():void
	{
		if (this._isExistIcon)
		{
			this.btnDefaultPic.visible = true;
		}
		else
		{
			this.btnDefaultPic.visible = false;
		}
		initKind();
	}
	private function initKind():void
	{
		if (this._isExistKind)
		{	
			_remote1 = new RemoteObject("fluorine");
			_remote1.source = "ManagementService.Sys.SystemKind";
			_remote1.showBusyCursor = true;
			_remote1.GetAllKind(this._subCode,this._param,"1");
			_remote1.addEventListener(ResultEvent.RESULT,OnGetKindResult);
			_remote1.addEventListener(FaultEvent.FAULT,OnGetKindFault);
		}
		else
		{			
			var result:ArrayCollection = new ArrayCollection();
			var obj:Object = new Object();
			obj.CODE = "";
			obj.TEXT = "";
			result.addItem(obj);
			this.cbox_kind.dataProvider = result;
			this.cbox_kind.labelField = "TEXT";
			initInfo();
		}
	}
	private function OnGetKindResult(evt:ResultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote1.removeEventListener(FaultEvent.FAULT,OnGetKindFault);
		var result:ArrayCollection = _remote1.GetAllKind.lastResult as ArrayCollection;
		
		var obj:Object = new Object();
		obj.CODE = "";
		obj.TEXT = "";
		result.addItemAt(obj,0);
		this.cbox_kind.dataProvider = result;
		this.cbox_kind.labelField = "TEXT";
		
		initInfo();
	}
	private function OnGetKindFault(evt:FaultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote1.removeEventListener(FaultEvent.FAULT,OnGetKindFault);	
	}
	public function initInfo():void
	{		
		this.lb_source.visible = false;
		this.tbox_source.visible = false;
		this.cbox_kind.enabled = false;

		this.lb_source.visible = true;
		this.tbox_source.visible = true;
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.VidioNews.VidioNews";			
		_remote.showBusyCursor=true;
		_remote.GetVidioNewsInfo(this.__articleId);
		_remote.addEventListener(ResultEvent.RESULT,OnGetArticleResult);
		_remote.addEventListener(FaultEvent.FAULT,OnGetArticleFault);
		
	}
	private function OnGetArticleResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetArticleResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetArticleFault);
		var result:ArrayCollection=_remote.GetVidioNewsInfo.lastResult as ArrayCollection;
		if (result!=null && result.length>0)
		{
			this.tbox_title.text=result.source[0].ARTICLETITLE;
			this.__defaultPic=result.source[0].DEFAULTVIDIO;
			this.__sequence=result.source[0].SEQUENCE;	
			this.tbox_source.text = result.source[0].SOURCE;
			var co:Com=new Com();
			this.__sequence=co.createSeq(this._param,result.source[0].SEQUENCE);
			//this.iFrame.source=this._param+"/"+this.__sequence;
			this.iFrame.type = this._param;
			this.iFrame.source = this.__sequence;
			this.iFrame.show();
			this.iFrame.value=result.source[0].ARTICLECONTENT;
			var kindSur:ArrayCollection = this.cbox_kind.dataProvider as ArrayCollection;
			if (kindSur != null)
			{
				for(var i:int=0;i<kindSur.length;i++)
				{
					if (kindSur.source[i].CODE == result.source[0].ARTICLEKIND)
					{
						this.cbox_kind.selectedIndex = i;
						break;
					}
				}
			}
		}
	}
	private function OnGetArticleFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetArticleResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetArticleFault);
		this.iFrame.hidden();
		Alert.okLabel = "确定";
		Alert.show("初始化数据失败，请退出后重新进入。"+evt.fault.faultString,"系统提示",Alert.OK,this,AlertCloseHandle);
	}
	private function OnPub(evt:MouseEvent):void
	{
		if (this.tbox_title.text=="")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请填写标题。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.tbox_title.setFocus();
			return;
		}
		if (this._isExistKind && this.cbox_kind.selectedItem.CODE == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请选择类别,如果没有类别请先增加类别。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.cbox_kind.setFocus();
			return;
		} 
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.VidioNews.VidioNews";
		_remote.showBusyCursor=true;
		_remote.PubVidio(this.__articleId,
						   this._subCode,
						   this._param,	
						   this.cbox_kind.selectedItem.CODE,
						   "", //kindCode2
						   this.tbox_title.text,
						   this.iFrame.value,
						   parentApplication._userInfo.trueName,
						   this.__sequence,
						   this.__defaultPic);
		_remote.addEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.addEventListener(FaultEvent.FAULT,OnPubFault);
	}
	private function OnPubResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
		var result:String;
		result=_remote.PubVidio.lastResult as String;
		if (result!=null)
		{
			if (result.substr(0,2).toUpperCase()=="OK")
			{	
				if (this.__saveCallBack!=null)
				{
					this.__saveCallBack.call(this,
											 this.__articleId,
											 this._subCode,
											 this._subName,
											 this.cbox_kind.selectedItem.CODE,
											 this.cbox_kind.selectedItem.TEXT,
											 "已发布",
											 "1",
											 this.tbox_title.text,
											 parentApplication._userInfo.trueName,
											 this.__sequence,
											 this.__opKind);
				}
				this.iFrame.remove();
				PopUpManager.removePopUp(this);
			}
			else
			{
				this.iFrame.hidden();
				Alert.okLabel = "确定";
				Alert.show(result,"系统提示",Alert.OK,this,AlertCloseHandle);
			}
		}
		else
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("保存失败。","系统提示",Alert.OK,this,AlertCloseHandle);
		}
	}
	private function OnPubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
		this.iFrame.hidden();
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，保存失败。"+evt.fault.faultString,"系统提示",Alert.OK,this,AlertCloseHandle);
	}
	
	private function OnNotPub(evt:MouseEvent):void
	{
		if (this.tbox_title.text=="")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请填写标题。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.tbox_title.setFocus();
			return;
		}
		if (this._isExistKind && this.cbox_kind.selectedItem.CODE == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请选择类别,如果没有类别请先增加类别。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.cbox_kind.setFocus();
			return;
		} 
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Article.Article";
		_remote.showBusyCursor=true;
		_remote.NotPubVidio(this.__articleId,
						   this._subCode,
						   this._param,	
						   this.cbox_kind.selectedItem.CODE,
						   "", //kindCode2
						   this.tbox_title.text,
						   this.iFrame.value,
						   parentApplication._userInfo.trueName,
						   this.__sequence,
						   this.__defaultPic);
		_remote.addEventListener(ResultEvent.RESULT,OnNotPubResult);
		_remote.addEventListener(FaultEvent.FAULT,OnNotPubFault);
	}
	
	private function OnNotPubResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
		var result:String;
		result=_remote.NotPubVidio.lastResult as String;
		if (result!=null)
		{
			if (result.substr(0,2).toUpperCase()=="OK")
			{	
				if (this.__saveCallBack!=null)
				{
					this.__saveCallBack.call(this,
											 this.__articleId,
											 this._subCode,
											 this._subName,
											 this.cbox_kind.selectedItem.CODE,
											 this.cbox_kind.selectedItem.TEXT,
											 "未发布",
											 "-1",
											 this.tbox_title.text,
											 parentApplication._userInfo.trueName,
											 this.__sequence,
											 this.__opKind);
				}
				this.iFrame.remove();
				PopUpManager.removePopUp(this);
			}
			else
			{
				this.iFrame.hidden();
				Alert.okLabel = "确定";
				Alert.show(result,"系统提示",Alert.OK,this,AlertCloseHandle);
			}
		}
		else
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("审核失败。","系统提示",Alert.OK,this,AlertCloseHandle);
		}
	}
	
	private function OnNotPubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
		this.iFrame.hidden();
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，审核失败。"+evt.fault.faultString,"系统提示",Alert.OK,this,AlertCloseHandle);
	}
	
	private function AlertCloseHandle(evt:CloseEvent):void
	{
		this.iFrame.show();
	}
	
	private function OnClose(evt:CloseEvent):void
	{
		this.iFrame.remove();
		PopUpManager.removePopUp(this);
	}
	
	private function OnClick(evt:MouseEvent):void
	{
		if (this.iFrame.GetShowState())
		{
			this.move(this.x-1,this.y);
			this.move(this.x+1,this.y);
		}	
	}
	private function OnMove(evt:MoveEvent):void
	{
		this.iFrame.show();
		this.iFrame.moveIFrame();
	}
		
	private function OnDefaultPicClick(evt:MouseEvent):void
	{
		this.iFrame.hidden();
		var pop:DefaultPic=DefaultPic(PopUpManager.createPopUp(this,DefaultPic,true));
		pop.sequence=this.__sequence;
		pop.picFileName=this.__defaultPic;
		pop.type=this._param;
		pop.articleId=this.__articleId;
		pop.closeCallBack=defaultPicCloseCallBack;
		pop.loadFile();
		PopUpManager.centerPopUp(pop);
	}
	private function defaultPicCloseCallBack(picFileName:String):void
	{
		this.iFrame.show();
		this.__defaultPic=picFileName;	
	}