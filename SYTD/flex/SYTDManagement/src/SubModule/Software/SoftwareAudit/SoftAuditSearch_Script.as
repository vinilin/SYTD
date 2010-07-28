// ActionScript file
import flash.events.MouseEvent;

import mx.collections.ArrayCollection;
import mx.events.CloseEvent;
import mx.managers.PopUpManager;
import mx.rpc.events.FaultEvent;
import mx.rpc.events.ResultEvent;
import mx.rpc.remoting.mxml.RemoteObject;
	
	private var _searchCallBack:Function;
	private var _remote:RemoteObject;
	private var _remote1:RemoteObject;
	public function set searchCallBack(fun:Function):void
	{
		this._searchCallBack=fun;
	}
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSearch.addEventListener(MouseEvent.CLICK,OnSearch);
		initState();
		initUser();
		initKind();
	}
	private function initState():void
	{
		var state:ArrayCollection=new ArrayCollection();
		var obj:Object;
		
		obj=new Object();
		obj.text="";
		obj.code="";
		state.addItem(obj);
		
		obj=new Object();
		obj.text="待审核";
		obj.code="0";
		state.addItem(obj);
		
		obj=new Object();
		obj.text="已发布";
		obj.code="1";
		state.addItem(obj);
		
		obj=new Object();
		obj.text="未发布";
		obj.code="-1";
		state.addItem(obj);
		this.cbox_state.dataProvider=state;
		this.cbox_state.labelField="text";
	}
	 	
	private function initUser():void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Security.User";
		_remote.showBusyCursor=true;
		_remote.GetAllUserList();
		_remote.addEventListener(ResultEvent.RESULT,OnInitUserResult);
		_remote.addEventListener(FaultEvent.FAULT,OnInitUserFault);
	}
	private function OnInitUserResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnInitUserResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnInitUserFault);
		var result:ArrayCollection=_remote.GetAllUserList.lastResult as ArrayCollection;
		var obj:Object;
		if (result!=null)
		{
			obj=new Object();
			obj.USERNAME="";
			obj.TRUENAME="";
			result.addItemAt(obj,0);
			this.cbox_addMan.dataProvider=result;
			this.cbox_addMan.labelField="TRUENAME";
			
			this.cbox_auditMan.dataProvider=result;
			this.cbox_auditMan.labelField="TRUENAME";
		}
		else
		{
			result=new ArrayCollection();
			obj=new Object();
			obj.USERNAME="";
			obj.TRUENAME="";
			result.addItemAt(obj,0);
			this.cbox_addMan.dataProvider=result;
			this.cbox_addMan.labelField="TRUENAME";
			
			this.cbox_auditMan.dataProvider=result;
			this.cbox_auditMan.labelField="TRUENAME";
		}
	}
	private function OnInitUserFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnInitUserResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnInitUserFault);
		var result:ArrayCollection=new ArrayCollection();
		var obj:Object=new Object();
		obj.USERNAME="";
		obj.TRUENAME="";
		result.addItemAt(obj,0);
		this.cbox_addMan.dataProvider=result;
		this.cbox_addMan.labelField="TRUENAME";
		
		this.cbox_auditMan.dataProvider=result;
		this.cbox_auditMan.labelField="TRUENAME";
	}
	
	private function initKind():void
	{
		_remote1 = new RemoteObject("fluorine");
		_remote1.source = "ManagementService.Sys.SystemKind";
		_remote1.showBusyCursor =true;
		_remote1.GetAllKind("global","011");
		_remote1.addEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote1.addEventListener(FaultEvent.FAULT,OnGetKindFault);
	}
	private function OnGetKindResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetKindFault);
		var result:ArrayCollection = _remote1.GetAllKind.lastResult as ArrayCollection;
		var obj:Object = new Object();
		obj.TEXT = "";
		obj.CODE = "";
		result.addItemAt(obj,0);
		this.cbox_publishType.dataProvider = result;
		this.cbox_publishType.labelField = "TEXT";
		
	}
	private function OnGetKindFault(evt:FaultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote1.removeEventListener(FaultEvent.FAULT,OnGetKindFault);
		var result:ArrayCollection = new ArrayCollection();
		var obj:Object = new Object();
		obj.TEXT = "";
		obj.CODE = "";
		result.addItemAt(obj,0);
		this.cbox_publishType.dataProvider = result;
		this.cbox_publishType.labelField = "TEXT";
	}
	private function OnSearch(evt:MouseEvent):void
	{	
		if (_searchCallBack!=null)
		{
			this._searchCallBack.call(this,
									  this.tbox_title.text,
									  this.cbox_publishType.selectedItem.CODE,
									  this.cbox_addMan.selectedItem.TRUENAME,
									  this.df_addStartTime.text,
									  this.df_addEndTime.text,
									  this.cbox_auditMan.selectedItem.TRUENAME,
									  this.df_auditStartTime.text,
									  this.df_auditEndTime.text,
									  this.cbox_state.selectedItem.code);
		}
		PopUpManager.removePopUp(this);
	}
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}