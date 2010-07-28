	// ActionScript file
	import Custom.Common.Com;
	
	import SubModule.DefaultLink.Link.DefaultPic;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;

	private var __opKind:String="ADD";
	private var _param:String = "DEFAULTLINK";
	private var __sequence:String = "";
	private var __linkId:String = "";
	private var __saveCallBack:Function;
	private var __defaultPic:String = "";
	[Bindable]private var _subs:ArrayCollection = new ArrayCollection();
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
	public function set sequence(val:String):void
	{
		this.__sequence = val;
	}
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnDefaultPic.addEventListener(MouseEvent.CLICK,OnDefaultPicClick);
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
	}
	public function initData():void
	{	
		if (this.__opKind=="ADD")
		{
			var co:Com=new Com();
			this.__sequence=co.createSeq(this._param,"");
			this.title = "增加首页链接";
		}
		else if(this.__opKind=="UPDATE")
		{
			this.title = "修改首页链接";
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Link.DefaultLink";			
			_remote.showBusyCursor=true;
			_remote.GetLinkInfo(this.__linkId);
			_remote.addEventListener(ResultEvent.RESULT,OnGetLinkResult);
			_remote.addEventListener(FaultEvent.FAULT,OnGetLinkFault);
		}
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
			this.__defaultPic=result.source[0].DEFAULTPIC;
			this.__sequence = result.source[0].SEQUENCE;
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("初始化数据失败，请退出后重新进入。","错误",Alert.OK);
		}
	}
	private function OnGetLinkFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetLinkResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetLinkFault);
		Alert.okLabel = "确定";
		Alert.show("初始化数据失败，请退出后重新进入。"+evt.fault.faultString,"系统提示",Alert.OK,this,AlertCloseHandle);
	}
	private function OnSave(evt:MouseEvent):void
	{
		if (this.tbox_linkName.text=="")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写链接名称。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.tbox_linkName.setFocus();
			return;
		}
		if (this.tbox_linkUrl.text == "")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写链接地址。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.tbox_linkUrl.setFocus();
			return;
		}
		if (this.__defaultPic == "")
		{
			Alert.okLabel = "确定";
			Alert.show("请先上传图片。","系统提示",Alert.OK,this,AlertCloseHandle);
			return;
		}
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Link.DefaultLink";
		_remote.showBusyCursor=true;
		if (this.__opKind=="ADD")
		{
			_remote.AddLink(this.tbox_linkName.text,
							this.tbox_linkUrl.text,
							this.__defaultPic,
							parentApplication._userInfo.trueName,
							this.__sequence);	
		}
		else if(this.__opKind=="UPDATE")
		{
			_remote.UpdateLink(this.__linkId,
							      this.tbox_linkName.text,
							      this.tbox_linkUrl.text,
							      this.__defaultPic,
							      parentApplication._userInfo.trueName);
		}
		_remote.addEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSaveFault);
		//Alert.show(articleContent);
	}
	private function OnSaveResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		//var result:String;
		if (this.__opKind=="ADD")
		{
			var addResult:ArrayCollection = _remote.AddLink.lastResult as ArrayCollection;
			if (addResult != null && addResult.length>0)
			{
				if (addResult.source[0].result == "OK")
				{
					this.__linkId = addResult.source[0].ID;
					if (this.__saveCallBack != null)
					{
						this.__saveCallBack.call(this,
											   this.__linkId,
											   this.tbox_linkName.text,
											   this.tbox_linkUrl.text,
											   addResult.source[0].LISTORDER,
											   "待审核",0,
											   parentApplication._userInfo.trueName,
											   "","",
											   this.__defaultPic,
											   this.__sequence,
											   this.__opKind);
					}	
					PopUpManager.removePopUp(this);
				}
				else
				{
					Alert.show(addResult.source[0].result);
				}
			}
			else
			{
				Alert.okLabel = "确定";
				Alert.show("保存失败。","错误",Alert.OK);
			}
		}
		else if(this.__opKind=="UPDATE")
		{
			var updateResult:String = _remote.UpdateLink.lastResult as String;
			if (updateResult == "OK")
			{
				if (this.__saveCallBack !=null)
				{
					this.__saveCallBack.call(this,
											 this.__linkId,
											 this.tbox_linkName.text,
											 this.tbox_linkUrl.text,
											 //addResult.source[0].LISTORDER,
											 "",//修改时LISTORDER 不改变
											 "待审核",0,
											 parentApplication._userInfo.trueName,
											 "","",
											 this.__defaultPic,
											 this.__sequence,
											 this.__opKind);
				}
				PopUpManager.removePopUp(this);
			}
			else
			{
				Alert.show(updateResult);
			}	
		}
	}
	private function OnSaveFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，保存失败。"+evt.fault.faultString,"系统提示",Alert.OK,this,AlertCloseHandle);
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
		var pop:DefaultPic = DefaultPic(PopUpManager.createPopUp(this,DefaultPic,true));
		pop.picFileName = this.__defaultPic;
		pop.linkId = this.__linkId;
		pop.sequence = this.__sequence;
		pop.closeCallBack = defaultPicCloseCallBack;
		pop.loadFile();
		PopUpManager.centerPopUp(pop);
	}
	private function defaultPicCloseCallBack(picFileName:String):void
	{
		this.__defaultPic=picFileName;	
	}