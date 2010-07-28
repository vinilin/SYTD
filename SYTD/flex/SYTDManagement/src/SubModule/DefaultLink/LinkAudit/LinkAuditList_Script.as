// ActionScript file
	
	import Custom.Renderer.CenteredCheckBoxHeaderRenderer;
	import Custom.Renderer.CenteredCheckBoxItemRenderer;
	
	import SubModule.DefaultLink.LinkAudit.LinkAudit;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.collections.ListCollectionView;
	import mx.controls.Alert;
	import mx.controls.DataGrid;
	import mx.events.CloseEvent;
	import mx.events.ListEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
	
	[Bindable]private var _searchResult:ArrayCollection=new ArrayCollection();
	private var _pageIndex:int=0;
	private var _pageSize:int=100;	
	/////////////////////////////////
	public var selectAllFlag:Boolean;
	[Bindable]public var hr:ClassFactory;	
	/////////////////////////////////////////////
	
	private var _remote:RemoteObject;
	
	//-------------------------------------------
	//查询参数
	private var _search_condition_linkName:String = "";
	private var _search_condition_linkUrl:String = "";
	private var _search_condition_addMan:String = "";
	private var _search_condition_addStartTime:String = "";
	private var _search_condition_addEndTime:String = "";
	private var _search_condition_auditMan:String = "";
	private var _search_condition_auditStartTime:String = "";
	private var _search_condition_auditEndTime:String = "";
	private var _search_condition_state:String = "";
	//-------------------------------------------
	
	private function init():void
	{
		////////////////////////////////////////////
		hr = new ClassFactory(CenteredCheckBoxHeaderRenderer);
		hr.properties = {stateHost: this, stateProperty: "selectAllFlag"};
		//---------------------------------------
		pager.addEventListener(Event.CHANGE,OnPagerChange);
		dg.addEventListener(MouseEvent.CLICK,OnCheckBoxClickHandler);
		dg.addEventListener(ListEvent.ITEM_DOUBLE_CLICK,OnDgItemDbClick);
		dg.addEventListener(ListEvent.ITEM_CLICK,OnDgItemClick);
		//---------------------------------------
		this.btnBatchPub.addEventListener(MouseEvent.CLICK,OnBatchPub);
		this.btnNotBatchPub.addEventListener(MouseEvent.CLICK,OnNotBatchPub)
		this.btnAudit.addEventListener(MouseEvent.CLICK,OnAudit);
		this.btnDelete.addEventListener(MouseEvent.CLICK,OnDelete);
		this.btnSearch.addEventListener(MouseEvent.CLICK,OnSearch);
		/////////////////////////////////////////////
		//Search();
		Resize();
		Search();
	}
	public function Resize():void
	{
		var parentWidth:int=parentApplication.width;
		var parentHeight:int=parentApplication.height;
		var parentContentWidth:int=parentWidth-parentApplication.vbox_menu.width-10;
		var parentContentHeight:int=parentHeight-94;
		
		this.controlBar_Title.width=parentContentWidth;
		this.dg.width=parentContentWidth;
		this.ToolBar.width=parentContentWidth;
		this.HboxPager.width=parentContentWidth;
		this.dg.height=parentContentHeight-this.controlBar_Title.height-this.ToolBar.height-this.HboxPager.height-18;
	}
	
	
	public function Search():void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Link.DefaultLink";
		_remote.showBusyCursor=true;
		_remote.GetLinkList(this._search_condition_linkName,
							this._search_condition_linkUrl,
							this._search_condition_addMan,
							this._search_condition_addStartTime,
							this._search_condition_addEndTime,
							this._search_condition_auditMan,
							this._search_condition_auditStartTime,
							this._search_condition_auditEndTime,
							this._search_condition_state,
							this._pageSize,
							this._pageIndex);
		_remote.addEventListener(ResultEvent.RESULT,OnSearchResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSearchFault);					
	}
	private function OnSearchResult(evt:ResultEvent):void
	{
		_searchResult=_remote.GetLinkList.lastResult as ArrayCollection;
		_remote.removeEventListener(ResultEvent.RESULT,OnSearchResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSearchFault);
		_remote.disconnect();
		var itemTotal:int=0;
		if (_searchResult.length>0)
		{
			itemTotal=_searchResult.source[0].recordCount;
		}
		pager.itemTotal=itemTotal;
		pager.currentPage=this._pageIndex;
		pager.pageSize=this._pageSize;
		pager.show=true;
		pager.pagerBind();
	}
	private function OnSearchFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSearchResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSearchFault);
		_remote.disconnect();
		Alert.show("网络或系统错误，加载数据失败。"+evt.fault.faultString);
	}
	
	private function OnBatchPub(evt:MouseEvent):void
	{
		if (this.dg.selectedItems==null || this.dg.selectedItems.length==0)
		{
			Alert.show("请选择你要发布的项","系统提示")
			return;
		}
		Alert.show("你确定要发布选择的项","系统提示",3,this,AlertPubCloseHandler);
	}
	private function AlertPubCloseHandler(evt:CloseEvent):void
	{
		if (Alert.YES==evt.detail)
		{
			var selectSrc:Array = new Array();
			for (var i:int=0;i<this.dg.selectedItems.length;i++)
			{
				selectSrc.push(this.dg.selectedItems[i].ID);
			}
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Link.DefaultLink";
			_remote.showBusyCursor=true;
			_remote.BatchPub(selectSrc,parentApplication._userInfo.trueName);
			_remote.addEventListener(ResultEvent.RESULT,OnBatchPubResult);
			_remote.addEventListener(FaultEvent.FAULT,OnBatchPubFault);
		}
	}
	private function OnBatchPubResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnBatchPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnBatchPubFault);
		var result:String = _remote.BatchPub.lastResult as String;
		if (result != null && result == "OK")
		{
			for (var i:int=0;i<this.dg.selectedItems.length;i++)
			{
				var obj:Object = this.dg.selectedItems[i];
				obj.STATE = 1;
				obj.STATENAME = "已发布";
				obj.AUDITMAN = parentApplication._userInfo.trueName;
				obj.AUDITTIME = new Date();
				ListCollectionView(dg.dataProvider).itemUpdated(this.dg.selectedItems[i]);
			}
		}
		else
		{
			Alert.show("批量发布失败。","错误");
		}
	}
	private function OnBatchPubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnBatchPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnBatchPubFault);
		Alert.show("网络或系统错误，批量发布失败。"+evt.fault.faultString,"错误");
	}
	
	
	private function OnNotBatchPub(evt:MouseEvent):void
	{
		if (this.dg.selectedItems==null || this.dg.selectedItems.length==0)
		{
			Alert.show("请选择你要不发布的项","系统提示")
			return;
		}
		Alert.show("你确定要不发布选择的项","系统提示",3,this,AlertNotPubCloseHandler);
	}
	private function AlertNotPubCloseHandler(evt:CloseEvent):void
	{
		if (Alert.YES==evt.detail)
		{
			var selectSrc:Array = new Array();
			for (var i:int=0;i<this.dg.selectedItems.length;i++)
			{
				selectSrc.push(this.dg.selectedItems[i].ID);
			}
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Link.DefaultLink";
			_remote.showBusyCursor=true;
			_remote.BatchNotPub(selectSrc,parentApplication._userInfo.trueName);
			_remote.addEventListener(ResultEvent.RESULT,OnBatchNotPubResult);
			_remote.addEventListener(FaultEvent.FAULT,OnBatchNotPubFault);
		}
	}
	private function OnBatchNotPubResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnBatchNotPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnBatchNotPubFault);
		var result:String = _remote.BatchNotPub.lastResult as String;
		if (result != null && result == "OK")
		{
			for (var i:int=0;i<this.dg.selectedItems.length;i++)
			{
				var obj:Object = this.dg.selectedItems[i];
				obj.STATE = -1;
				obj.STATENAME = "未发布";
				obj.AUDITMAN = parentApplication._userInfo.trueName;
				obj.AUDITTIME = new Date();
				ListCollectionView(dg.dataProvider).itemUpdated(this.dg.selectedItems[i]);
			}
		}
		else
		{
			Alert.show("批量不发布失败。","错误");
		}
	}
	private function OnBatchNotPubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnBatchPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnBatchNotPubFault);
		Alert.show("网络或系统错误，批量不发布失败。"+evt.fault.faultString,"错误");
	}
	
	/////审核
	private function OnAudit(evt:MouseEvent):void
	{
		if (this.dg.selectedItems==null || this.dg.selectedItems.length==0)
		{
			mx.controls.Alert.show("请选择你要审核的项。");
			return;
		}
		else if(this.dg.selectedItems.length!=1)
		{
			mx.controls.Alert.show("你选择了多项数据，请选择你要审核的项或直接双击行进入审核。");
			return;
		}
		Audit();
	}
	private function OnDgItemDbClick(evt:ListEvent):void
	{
		Audit();
	}
	private function Audit():void
	{
		var pop:LinkAudit=LinkAudit(PopUpManager.createPopUp(this,LinkAudit,true));
		pop.linkId=this.dg.selectedItems[0].ID;
		pop.opKind="AUDIT";
		pop.saveCallBack=saveCallBack;
		this.callLater(pop.initData);
		PopUpManager.centerPopUp(pop);
	}
	
	//增加、修改后的回调函数
	private function saveCallBack(id:String,
								  linkName:String,
								  linkUrl:String,
								  listOrder:String,
								  stateName:String,
								  state:String,
								  auditMan:String,
								  defaultPic:String):void
	{		
			var obj:Object;
			obj=this.dg.selectedItems[0];
			obj.LINKNAME = linkName;
			obj.LINKURL = linkUrl;
			obj.STATE = state;
			obj.STATENAME = stateName;
			obj.AUDITMAN = auditMan;
			obj.AUDITTIME = new Date();
			obj.DEFAULTPIC = defaultPic;
			ListCollectionView(dg.dataProvider).itemUpdated(this.dg.selectedItems[0]);
	}
	
	/////删除
	private function OnDelete(evt:MouseEvent):void
	{
		if (this.dg.selectedItems==null || this.dg.selectedItems.length==0)
		{
			Alert.show("请选择你要删除的项","系统提示")
			return;
		}
		Alert.show("你确定要删除选择的项","系统提示",3,this,AlertDeleteCloseHandler);
		
	}
	private function AlertDeleteCloseHandler(evt:CloseEvent):void
	{
		if (Alert.YES==evt.detail)
		{
			var selectSrc:ArrayCollection = new ArrayCollection();
			for (var i:int=0;i<this.dg.selectedItems.length;i++)
			{
				var obj:Object = new Object();
				obj.ID = this.dg.selectedItems[i].ID;
				obj.LINKNAME = this.dg.selectedItems[i].LINKNAME;
				obj.LINKURL = this.dg.selectedItems[i].LINKURL;
				obj.DEFAULTPIC = this.dg.selectedItems[i].DEFAULTPIC;
				obj.SEQUENCE = this.dg.selectedItems[i].SEQUENCE;
				selectSrc.addItem(obj);
			}
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Link.DefaultLink";
			_remote.showBusyCursor=true;
			_remote.DeleteLink(selectSrc);
			_remote.addEventListener(ResultEvent.RESULT,OnDeleteResult);
			_remote.addEventListener(FaultEvent.FAULT,OnDeleteFault);
		}
	}
	private function OnDeleteResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnDeleteResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnDeleteFault);
		var result:ArrayCollection = _remote.DeleteLink.lastResult as ArrayCollection;
		if (result != null)
		{
			var message:String = "";
			for(var i:int=0;i<result.length;i++)
			{
				if (result.source[i].result == "OK")
				{
					for(var j:int=0;j<this._searchResult.length;j++)
					{
						if (result.source[i].ID == this._searchResult.source[j].ID)
						{
							this._searchResult.removeItemAt(j);
							this.pager.itemTotal --;
							break;
						}
					}
				}
				else
				{
					message += "删除“"+result.source[i].LINKNAME+"”失败，"+result.source[i].result + "\r\n";
				}
			}
			if (message!="")
			{
				Alert.show(message);
			}
			///如果删除后当前页为空了，则重新查询
			if (_searchResult.length==0)
			{
				Search();
			}
		}
		else
		{
			Alert.show("删除失败。","错误");
		}
	}
	private function OnDeleteFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnDeleteResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnDeleteFault);
		Alert.show("网络或系统错误，删除失败。"+evt.fault.faultString,"错误");
	}
	
	
	////查询
	private function OnSearch(evt:MouseEvent):void
	{
		var pop:LinkAuditSearch=LinkAuditSearch(PopUpManager.createPopUp(this,LinkAuditSearch,true));
		pop.searchCallBack=searchCallBack;
		PopUpManager.centerPopUp(pop);
	}
	private function searchCallBack(linkName:String,
									linkUrl:String,
									addMan:String,
									addStartTime:String,
									addEndTime:String,
									auditMan:String,
									auditStartTime:String,
									auditEndTime:String,
									state:String):void
	{
		this._search_condition_linkName = linkName;
		this._search_condition_linkUrl = linkUrl;
	    this._search_condition_addMan = addMan;
	    this._search_condition_addStartTime = addStartTime;
	    this._search_condition_addEndTime = addEndTime;
	    this._search_condition_auditMan = auditMan;
	    this._search_condition_auditStartTime = auditStartTime;
	    this._search_condition_auditEndTime = auditEndTime;
	    this._search_condition_state = state;
	    this._pageIndex=0;
	    Search();
	}
	
	/////////////////分页事件////////////////////////////////
	private function OnPagerChange(evt:Event):void
	{
		this._pageIndex=pager.currentPage;
		Search();
	}
	
	////////选择
	private function OnDgItemClick(evt:ListEvent):void
	{		
		var i:int=0;
		if (!(evt.itemRenderer is CenteredCheckBoxItemRenderer))
		{
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
		////////////////////////////////////////
		var allCheck:Boolean=true;
		for(i=0;i<_searchResult.length;i++)
		{
			if (!_searchResult.source[i].ISCHECKED)	
			{
				allCheck=false;
				break;
			}
		}
		selectAllFlag=allCheck;
		//////////////////////////////////////////
	}
	
	private function OnCheckBoxClickHandler(event:MouseEvent):void
	{
		var obj:Object;
		var _selectItems:Array=new Array();
		var tempDg:DataGrid=DataGrid(event.currentTarget);
    	if(event.target is CenteredCheckBoxHeaderRenderer)
    	{
        	//this._selectItems=new Array();
        	for each(obj in tempDg.dataProvider)
        	{
            	obj.ISCHECKED = CenteredCheckBoxHeaderRenderer(event.target).selected;
            	if (obj.ISCHECKED)
            	{
            		_selectItems.push(obj);
            	}
            	ListCollectionView(tempDg.dataProvider).itemUpdated(obj);
        	}
        	tempDg.selectedItems=_selectItems;
    	}  
    	else if(event.target is CenteredCheckBoxItemRenderer)
    	{
    		var allCheck:Boolean=true;
    		for each(obj in tempDg.dataProvider)
    		{
    			if (obj.ISCHECKED)
    			{
    				_selectItems.push(obj);
    			}
    			else
    			{
    				allCheck=false;
    			}
    		}
    		tempDg.selectedItems=_selectItems;
    		selectAllFlag=allCheck;
    	}	
	}