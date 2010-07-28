	// ActionScript file
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
	
	private var _opKind:String = "ADD";
	private var _ids:String = "";
	private var _subCode:String = "";
	private var _subName:String = "";
	private var _param:String = "";
	private var _pptr:String = "1";
	private var _code:String = "";
	private var _text:String = "";
	private var _listOrder:String = "1";
	private var _saveCallBack:Function;
	
	private var _remote:RemoteObject = new RemoteObject("fluorine");
	
	public function set opKind(val:String):void
	{
		this._opKind = val;
	}
	public function set ids(val:String):void
	{
		this._ids = val;	
	}
	public function set subCode(val:String):void
	{
		this._subCode= val;
	}
	public function set subName(val:String):void
	{
		this._subName = val;
	}
	public function set param(val:String):void
	{
		this._param = val;
	}
	public function set pptr(val:String):void
	{
		this._pptr = val;
	}
	public function set code(val:String):void
	{
		this._code = val;	
	}
	public function set text(val:String):void
	{
		this._text = val;
		this.callLater(initData);
	}
	public function set listOrder(val:String):void
	{
		this._listOrder = val;
	}
	public function set saveCallBack(fun:Function):void
	{
		this._saveCallBack = fun;
	}
	
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
	}
	private function initData():void
	{
		this.tbox_text.text = this._text;
	}
	private function OnSave(evt:MouseEvent):void
	{
		if (this.tbox_text.text == "")
		{
			mx.controls.Alert.okLabel = "确定";
			Alert.show("请填写类别名称","提示",Alert.OK);
			return;
		}	
		_remote.source = "ManagementService.Sys.SystemKind";
		_remote.showBusyCursor = true;
		if (this._opKind == "ADD")
		{
			_remote.AddKind(this._subCode,
							this._param,
							this._pptr,
							this.tbox_text.text,
							"1",
							"",
							"");
		}
		else
		{
			_remote.UpdateKind(this._ids,
								this._subCode,
								this._param,
								this._pptr,
								this._code,
								this.tbox_text.text,
								this._listOrder,
								"1");
		}
		_remote.addEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSaveFault);
	}
	private function OnSaveResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		var result:ArrayCollection;
		if (this._opKind == "ADD")
		{
			result = _remote.AddKind.lastResult as ArrayCollection;
		}
		else
		{
			result = _remote.UpdateKind.lastResult as ArrayCollection;
		}
		if (result!=null && result.source.length>0)
		{ 
			if (result.source[0].result=="OK")
			{
				if (this._opKind=="ADD")
				{
					this._ids = result.source[0].ID;
					this._code = result.source[0].CODE;
					this._listOrder = result.source[0].LISTORDER;
				}
				if (this._saveCallBack!=null)
				{
					this._saveCallBack.call(this,
											this._subCode,
											this._subName,
											this._ids,
											this._pptr,
											this._code,
											this.tbox_text.text,
											this._listOrder,
											this._opKind);
				}
				PopUpManager.removePopUp(this);
			}
			else
			{
				Alert.okLabel = "确定";
				Alert.show(result.source[0].result,"错误",Alert.OK);
			}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("系统错误，保存失败。","错误",Alert.OK);
		}
	}
	private function OnSaveFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，保存失败"+evt.fault.faultString,"错误",Alert.OK);
	}
	
	private function OnClose(evt:CloseEvent):void
	{
		mx.managers.PopUpManager.removePopUp(this);
	}