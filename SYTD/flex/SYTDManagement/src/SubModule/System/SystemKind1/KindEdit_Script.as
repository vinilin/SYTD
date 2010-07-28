	// ActionScript file
	import Custom.Common.Com;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
		
	private var _ids:String = "";
	private var _opKind:String = "ADD";
	private var _isExistIcon:String = "";
	private var _param:String = "";
	private var _kind:String = "";
	private var _pptr:String = "";
	private var _code:String = "";
	private var _listOrder:String = "";
	private var __defaultPic:String = "";
	private var __sequence:String = "";
	private var _subCode:String = "";
	private var _subName:String = "";
	private var _remote:RemoteObject;
	private var _remote1:RemoteObject;
	private var _saveCallBack:Function;
	public function set ids(val:String):void
	{
		this._ids = val;
	}
	public function set opKind(val:String):void
	{
		this._opKind = val;	
	}
	public function set isExistIcon(val:String):void
	{
		this._isExistIcon = val;
	}
	public function set param(val:String):void
	{
		this._param = val;
	}
	public function set kind(val:String):void
	{
		this._kind = val;
	}
	public function set pptr(val:String):void
	{
		this._pptr = val;
	}
	public function set code(val:String):void
	{
		this._code = val;
	}
	public function set listOrder(val:String):void
	{
		this._listOrder = val;
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
		this._saveCallBack = fun;
	}
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
		this.btnDefaultPic.addEventListener(MouseEvent.CLICK,OnDefaultPicClick);
	}
		
	public function initData():void
	{
		if (this._isExistIcon.toUpperCase()=="ICON")
		{
			this.btnDefaultPic.visible = true;
		}
		else
		{
			this.btnDefaultPic.visible = false;
		}
		switch(this._param.toUpperCase())
			{
				case "006":
				case "007":
				case "009":
					
					this.lb_linkFlag.visible = true;
					this.lb_linkUrl.visible = true;
					this.cbox_linkOrKind.visible = true;
					this.tbox_linkUrl.visible = true;
					
					break;
						
				default:	
					this.title = "增加类别";
					this.lb_linkFlag.visible = false;
					this.lb_linkUrl.visible = false;
					this.cbox_linkOrKind.visible = false;
					this.tbox_linkUrl.visible = false;
					break;
			}
		if (this._opKind == "ADD")
		{
			switch(this._param.toUpperCase())
			{
				case "009":
					this.title = "创建新专题";
					break;
						
				default:	
					this.title = "增加类别";
					break;
			}
			var co:Com =new Com();
			this.__sequence=co.createSeq(this._param,"");		
			
		}
		else
		{
			switch(this._param.toUpperCase())
			{
				case "009":
					this.title = "修改专题名称";
					break;
				
				default:
					this.title = "修改类别";
					break;
			}
			_remote = new RemoteObject("fluorine");
			_remote.source = "ManagementService.Sys.SystemKind";
			_remote.GetKindInfo(this._ids);
			_remote.addEventListener(ResultEvent.RESULT,OnGetInfoResult);
			_remote.addEventListener(FaultEvent.FAULT,OnGetInfoFault);
		}
	}
	
	private function OnGetInfoResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetInfoResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetInfoFault);
		var result:ArrayCollection = _remote.GetKindInfo.lastResult as ArrayCollection;
		if (result!=null && result.source.length>0)
		{
			this.tbox_text.text = result.source[0].TEXT;
			this.__defaultPic = result.source[0].DEFAULTPIC;
			this.__sequence = result.source[0].SEQUENCE;
			if (result.source[0].LINKORKIND == "0")
			{
				this.cbox_linkOrKind.selected = false;
			}
			else
			{
				this.cbox_linkOrKind.selected = true;
			}
			this.tbox_linkUrl.text = result.source[0].LINKURL;
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("加载信息失败。","错误",Alert.OK);
		}
	}
	
	private function OnGetInfoFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetInfoResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetInfoFault);
		Alert.okLabel = "确定";
		Alert.show("加载信息失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	
	private function OnSave(evt:MouseEvent):void
	{
		if (this.tbox_text.text=="")
		{
			Alert.okLabel = "确定";
			mx.controls.Alert.show("请填写类别名称。","错误",Alert.OK);
			return;
		}
		if (this._isExistIcon.toUpperCase() == "ICON" && this.__defaultPic == "")
		{
			Alert.okLabel = "确定";
			mx.controls.Alert.show("请先上传首页图片。","确定",Alert.OK);
			return;
		} 
		if (this.cbox_linkOrKind.selected && this.tbox_linkUrl.text == "")
		{
			Alert.okLabel = "确定";
			mx.controls.Alert.show("请填写链接地址。","确定",Alert.OK);
			return;
		}
		var linkOrKind:String = "0";
		if (this.cbox_linkOrKind.selected)
		{
			linkOrKind = "1";
		}
		_remote=new RemoteObject("fluorine");
		_remote.source = "ManagementService.Sys.SystemKind";
		_remote.showBusyCursor = true;
		if (this._opKind == "ADD")
		{
			_remote.AddKind(this._subCode,
							this._kind,
							this._pptr,
							this.tbox_text.text,
							"1",
							this.__defaultPic,
							this.__sequence,
							linkOrKind,
							this.tbox_linkUrl.text);
		}
		else
		{
			_remote.UpdateKind(this._ids,
							    this._subCode,
							    this._kind,
							    this._pptr,
							    this._code,
							    this.tbox_text.text,
							    this._listOrder,
							    "1",
								linkOrKind,
								this.tbox_linkUrl.text);
		}
		_remote.addEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSaveFault);
	}
	
	private function OnSaveResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		var result:ArrayCollection
		if (this._opKind=="ADD")
		{
			result = _remote.AddKind.lastResult as ArrayCollection;
		}
		else
		{
			result = _remote.UpdateKind.lastResult as ArrayCollection;
		}
		if (result!=null && result.source.length>0)
		{	
			var resultMessage:String=result.source[0].result;
			if (resultMessage == "OK")
			{
				if (this._opKind == "ADD")
				{
					this._ids = result.source[0].ID;
					this._code = result.source[0].CODE;
					this._listOrder = result.source[0].LISTORDER;
				}
				
				if (this._saveCallBack != null)
				{
					var linkOrKind:String = "0";
					if (this.cbox_linkOrKind.selected)
					{
						linkOrKind = "1";
					}
					this._saveCallBack.call(this,
											this._ids,
											this._subCode,
											this._subName,
											this._kind,
											this._pptr,
											this._code,
											this.tbox_text.text,
											this._listOrder,
											this.__defaultPic,
											this.__sequence,
											linkOrKind,
											this.tbox_linkUrl.text,
											this._opKind)
				}					
				PopUpManager.removePopUp(this);
			}
			else
			{
				Alert.show(resultMessage);
			}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("保存失败","错误",Alert.OK);
		}
	}
	
	private function OnSaveFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		Alert.okLabel = "确定";
		Alert.show("系统错误，保存失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	
	private function OnClose(evt:CloseEvent):void
	{
		mx.managers.PopUpManager.removePopUp(this);	
	}
	
	private function OnDefaultPicClick(evt:MouseEvent):void
	{
		var pop:DefaultPic = DefaultPic(PopUpManager.createPopUp(this,DefaultPic,true));
		pop.picFileName = this.__defaultPic;
		pop.kindId = this._ids;
		pop.sequence = this.__sequence;
		pop.type = this._param;
		pop.closeCallBack = defaultPicCloseCallBack;
		pop.loadFile();
		PopUpManager.centerPopUp(pop);
	}
	private function defaultPicCloseCallBack(picFileName:String):void
	{
		this.__defaultPic=picFileName;	
	}
	