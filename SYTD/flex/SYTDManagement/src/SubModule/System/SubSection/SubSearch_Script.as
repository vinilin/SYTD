// ActionScript file
import flash.events.MouseEvent;

import mx.events.CloseEvent;
import mx.managers.PopUpManager;
	
	private var _searchCallBack:Function;
	public function set searchCallBack(fun:Function):void
	{
		this._searchCallBack=fun;
	}
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSearch.addEventListener(MouseEvent.CLICK,OnSearch);
	}
	private function OnSearch(evt:MouseEvent):void
	{
		if (_searchCallBack!=null)
		{
			this._searchCallBack.call(this,this.tbox_subCode.text,this.tbox_subName.text);
		}
		PopUpManager.removePopUp(this);
	}
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}