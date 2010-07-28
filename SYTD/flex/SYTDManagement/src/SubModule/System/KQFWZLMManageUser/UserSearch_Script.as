	// ActionScript file
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.events.CloseEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
	
	private var _searchCallBack:Function;
	public function set searchCallBack(fun:Function):void
	{
		this._searchCallBack=fun;
	}
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSearch.addEventListener(MouseEvent.CLICK,OnSearch);
		initSub();
	}
	private function initSub():void
	{
		var result:ArrayCollection=new ArrayCollection();
		var obj:Object=new Object();
		obj.SUBCODE = parentApplication._userInfo.subCode;
		obj.SUBNAME = parentApplication._usernfo.subName;
		result.addItemAt(obj,0);
		this.cbox_subList.dataProvider=result;
		this.cbox_subList.labelField="SUBNAME";
	}
	private function OnSearch(evt:MouseEvent):void
	{
		if (_searchCallBack!=null)
		{
			this._searchCallBack.call(this,this.tbox_userName.text,
									  this.tbox_trueName.text,
									  this.cbox_subList.selectedItem.SUBCODE);
		}
		PopUpManager.removePopUp(this);
	}
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}