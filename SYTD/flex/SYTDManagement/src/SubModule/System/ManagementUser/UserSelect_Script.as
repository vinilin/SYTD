	// ActionScript file
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.collections.ListCollectionView;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.events.ListEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
	
	
	private var _selectCallBack:Function;
	[Bindable]private var _searchResult:ArrayCollection=new ArrayCollection();
	private var _pageIndex:int=0;
	private var _pageSize:int=100;	
	private var _funcCodes:ArrayCollection=new ArrayCollection();
	
	private var _remote:RemoteObject;
	private function init():void
	{
		//---------------------------------------
		dg.addEventListener(ListEvent.ITEM_CLICK,OnDgItemClick);
		dg.addEventListener(ListEvent.ITEM_DOUBLE_CLICK,OnOnDgItemDBClick);
		this.btnSelect.addEventListener(MouseEvent.CLICK,OnSelect);
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		//---------------------------------------
		Search();
	}
	
	public function set selectCallBack(fun:Function):void
	{
		this._selectCallBack = fun;
	}
	public function set funcCode(val:ArrayCollection):void
	{
		this._funcCodes = val;
	}
	
	private function Search():void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Security.ManagementUser";
		_remote.showBusyCursor=true;
		_remote.GetUserSelectList(this._funcCodes);
		_remote.addEventListener(ResultEvent.RESULT,OnSearchResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSearchFault);					
	}
	private function OnSearchResult(evt:ResultEvent):void
	{
		_searchResult=_remote.GetUserSelectList.lastResult as ArrayCollection;
		_remote.removeEventListener(ResultEvent.RESULT,OnSearchResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSearchFault);
		_remote.disconnect();
	}
	private function OnSearchFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSearchResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSearchFault);
		_remote.disconnect();
		Alert.okLabel = "确定";
		mx.controls.Alert.show("网络或系统错误，加载数据失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	
	private function OnSelect(evt:MouseEvent):void
	{
		if (this.dg.selectedItem==null)
		{
			Alert.okLabel = "确定";
			mx.controls.Alert.show("请选择用户（单击行选择）。","提示",Alert.OK);
			return;
		}
		if (this._selectCallBack!=null)
		{
			this._selectCallBack.call(this,this.dg.selectedItem.ID,this.dg.selectedItem.USERNAME);
		}
	    mx.managers.PopUpManager.removePopUp(this);
	}
	private function OnClose(evt:CloseEvent):void
	{
		mx.managers.PopUpManager.removePopUp(this);
	}
	
	////////选择
	private function OnDgItemClick(evt:ListEvent):void
	{		
		var i:int=0;
		
			for(i=0;i<_searchResult.length;i++)
			{
				_searchResult.source[i].ISCHECKED=0;
			}
			for(i=0;i<dg.selectedItems.length;i++)
			{
				this.dg.selectedItems[i].ISCHECKED=1;
				ListCollectionView(dg.dataProvider).itemUpdated(this.dg.selectedItems[i], "ISCHECKED");
			}
	}
	
	private function OnOnDgItemDBClick(evt:ListEvent):void
	{
		if (this._selectCallBack!=null)
		{
			this._selectCallBack.call(this,this.dg.selectedItem.ID,this.dg.selectedItem.USERNAME);
		}
	    mx.managers.PopUpManager.removePopUp(this);
	}
	