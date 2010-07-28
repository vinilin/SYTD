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
    private var _category:String = "";
    private var _text:String = "";
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
	public function set text(val:String):void
	{
		this._text = val;
	}
	public function set category(val:String):void
	{
		this._category = val;
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
		
	public function initData():void
	{		
		if (this._opKind == "ADD")
		{
			this.title = "增加类别";
		}
		else
		{
			this.title = "修改类别";
			this.tbox_text.text = this._text;
		}
	}
	
	private function OnSave(evt:MouseEvent):void
	{
		if (this.tbox_text.text=="")
		{
			Alert.okLabel = "确定";
			mx.controls.Alert.show("请填写类别名称。","错误",Alert.OK);
			return;
		}
		_remote=new RemoteObject("fluorine");
		_remote.source = "ManagementService.Sys.PublishType";
		_remote.showBusyCursor = true;
		if (this._opKind == "ADD")
		{
			_remote.AddKind(this.tbox_text.text,this._category);
		}
		else
		{
			_remote.UpdateKind(this._ids,
							   this.tbox_text.text,
							   this._category);
		}
		_remote.addEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSaveFault);
	}
	
	private function OnSaveResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		var result:String = "";
		if (this._opKind=="ADD")
		{
			result = _remote.AddKind.lastResult as String;
		}
		else
		{
			result = _remote.UpdateKind.lastResult as String;
		}
		if (result!=null)
		{	
			
			if (result.substr(0,2) == "OK")
			{
				if (this._opKind == "ADD")
				{
					this._ids = result.substr(2);
				}
				
				if (this._saveCallBack != null)
				{
					this._saveCallBack.call(this,
											this._ids,
											this.tbox_text.text,
											this._opKind)
				}					
				PopUpManager.removePopUp(this);
			}
			else
			{
				Alert.show(result);
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
	
	