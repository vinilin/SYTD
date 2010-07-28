	// ActionScript file
	import Custom.Renderer.CenteredCheckBoxHeaderRenderer;
	import Custom.Renderer.CenteredCheckBoxItemRenderer;
	import Custom.System.Log;
	
	import SubModule.Movie.Movie.MovieEdit;
	import SubModule.Movie.Movie.MovieSearch;
	import SubModule.Movie.MovieAudit.MovieAudit;
	import SubModule.Movie.MovieAudit.MoviePublish;
	
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
	private var _param:String = "011";
	/////////////////////////////////
	public var selectAllFlag:Boolean;	
	[Bindable]public var hr:ClassFactory;	
	/////////////////////////////////////////////
	private var _remote:RemoteObject;
	
	private var _search_condition_title:String = "";
	private var _search_condition_publishType:String = "";
	private var _search_condition_addMan:String = "";
	private var _search_condition_addStartTime:String = "";
	private var _search_condition_addEndTime:String = "";
	private var _search_condition_auditMan:String = "";
	private var _search_condition_auditStartTime:String = "";
	private var _search_condition_auditEndTime:String = "";
	private var _search_condition_state:String = "";
	
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
		this.btnAccept.addEventListener(MouseEvent.CLICK,OnAccept);
		this.btnReject.addEventListener(MouseEvent.CLICK,OnPublish);
		/////////////////////////////////////////////
		Search();
		Resize();
	}
	
	public function OnPublish(evt:MouseEvent):void
	{
		var pop:MoviePublish= MoviePublish(PopUpManager.createPopUp(this,MoviePublish,true));
		pop.ids = this.dg.selectedItems[0].ID;
		pop._saveCallBack = Search;
		PopUpManager.centerPopUp(pop);
		return;
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
	
	private function Search():void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.FileT.Movie";
		_remote.showBusyCursor=true;
		_remote.GetList(this._search_condition_title,
						this._search_condition_publishType,
						this._search_condition_addMan,
						this._search_condition_addStartTime,
						this._search_condition_addEndTime,
						this._search_condition_auditMan,
						this._search_condition_auditStartTime,
						this._search_condition_auditEndTime,
						this._search_condition_state,
						this._pageSize,
						this._pageIndex);
		_remote.addEventListener(ResultEvent.RESULT,OnSearchResult)
		_remote.addEventListener(FaultEvent.FAULT,OnSearchFault);					
	}
	private function OnSearchResult(evt:ResultEvent):void
	{
		_searchResult=_remote.GetList.lastResult as ArrayCollection;
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
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，加载数据失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	
	private function OnDgItemDbClick(evt:ListEvent):void
	{
		Update();
	}
	private function Update():void
	{
		var pop:MovieEdit = MovieEdit(PopUpManager.createPopUp(this,MovieEdit,true));
		pop.ids = this.dg.selectedItems[0].ID;
		pop.opKind = "UPDATE";
		pop.param = this._param;
		pop.saveCallBack = saveCallBack;
		this.callLater(pop.initData);
		PopUpManager.centerPopUp(pop);
	}
	
	////增加
	private function OnAccept(evt:MouseEvent):void
	{
		
		var pop:MovieAudit = MovieAudit(PopUpManager.createPopUp(this,MovieAudit,true));
		pop._ids =  this.dg.selectedItems[0].ID;
		pop._saveCallBack = Search;
		PopUpManager.centerPopUp(pop);
	}
	
	//增加、修改后的回调函数
	private function saveCallBack(id:String,
								  title:String,
								  addMan:String,
								  subCode:String,
								  subName:String,
								  player:String,
								  director:String,
								  publishType:String,
								  publishTypeName:String,
								  opKind:String):void
	{		
		var obj:Object;
		if (opKind=="ADD")
		{
			obj=new Object();
			for(var i:int=0;i<_searchResult.length;i++)
			{
				_searchResult.source[i].ISCHECKED=0;
			}
			obj.ISCHECKED = 1;
			obj.ID = id;
			obj.TITLE = title;
			obj.ADDMAN = addMan;
			obj.ADDTIME = new Date();
			obj.SUBCODE = subCode;
			obj.SUBNAME = subName;
			obj.PLAYER = player;
			obj.DIRECTOR = director;
			obj.PUBLISHTYPE = publishType;
			obj.PUBLISHTYPENAME = publishTypeName;	
			this._searchResult.addItemAt(obj,0);
			//--------------------------------------
			selectAllFlag=false;
			var _selectItems:Array=new Array();
			_selectItems.push(obj);
			this.dg.selectedItems=_selectItems;
			//--------------------------------------
			this.pager.itemTotal++;
			this.dg.verticalScrollPosition=0;
			Custom.System.Log.WriteLog("Movie","增加","增加电影" + subName,parentApplication._userInfo.trueName,id);
		}
		else
		{
			obj=this.dg.selectedItems[0];
			obj.TITLE = title;
			obj.ADDMAN = addMan;
			obj.ADDTIME = new Date();
			obj.SUBCODE = subCode;
			obj.SUBNAME = subName;
			obj.PLAYER = player;
			obj.DIRECTOR = director;
			obj.PUBLISHTYPE = publishType;
			obj.PUBLISHTYPENAME = publishTypeName;
			obj.AUDITMAN = "";
			obj.AUDITTIME = "";	
			ListCollectionView(dg.dataProvider).itemUpdated(this.dg.selectedItems[0]);
			Custom.System.Log.WriteLog("Movie","修改","修改电影" + subName,parentApplication._userInfo.trueName,id);
		}
	}
	
	/////删除
	private function OnDelete(evt:MouseEvent):void
	{
		if (this.dg.selectedItems==null || this.dg.selectedItems.length==0)
		{
			Alert.okLabel = "确定";
			Alert.show("请选择你要删除的项","系统提示",Alert.OK)
			return;
		}
		Alert.yesLabel = "是";
		Alert.noLabel = "否";
		Alert.show("你确定要删除选择的项","系统提示",Alert.YES|Alert.NO,this,AlertDeleteCloseHandler);
		
	}
	private function AlertDeleteCloseHandler(evt:CloseEvent):void
	{
		if (Alert.YES==evt.detail)
		{
			var selectSrc:ArrayCollection=new ArrayCollection()
			for(var i:int=0;i<this.dg.selectedItems.length;i++)
			{
				var obj:Object=new Object();
				obj.ID = this.dg.selectedItems[i].ID;
				obj.TITLE = this.dg.selectedItems[i].TITLE;
				selectSrc.addItem(obj);
			}
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.FileT.Movie";
			_remote.showBusyCursor=true;
			_remote.DeleteMovie(selectSrc);
			_remote.addEventListener(ResultEvent.RESULT,OnDeleteResult);
			_remote.addEventListener(FaultEvent.FAULT,OnDeleteFault);
		}
	}
	private function OnDeleteResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnDeleteResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnDeleteFault);
		var result:ArrayCollection=_remote.DeleteMovie.lastResult as ArrayCollection;
		if (result!=null)
		{
			var message:String = "";
			for(var i:int=0;i<result.length;i++)
			{
				if (result.source[i].result == "0")
				{
					for(var j:int=0;j<this._searchResult.length;j++)
					{
						if (this._searchResult.source[j].ID==result.source[i].ID)
						{
							Custom.System.Log.WriteLog("Movie","删除","删除电影"+this._searchResult.source[j].TITLE,parentApplication._userInfo.trueName,this._searchResult.source[j].ID);
							this._searchResult.removeItemAt(j);
							this.pager.itemTotal--;
							break;
						}
					}
				}
				else
				{
					message += "删除电影"+result.source[i].TITLE + ","+result.source[i].result + "\r\n";
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
			Alert.okLabel = "确定";
			Alert.show("删除失败。","错误",Alert.OK);
		}
		
	}
	private function OnDeleteFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnDeleteResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnDeleteFault);
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，删除失败。"+evt.fault.faultDetail,"错误",Alert.OK);
	}
	////查询
	private function OnSearch(evt:MouseEvent):void
	{
		var pop:MovieSearch = MovieSearch(PopUpManager.createPopUp(this,MovieSearch,true));
		pop.searchCallBack=searchCallBack;
		PopUpManager.centerPopUp(pop);
	}
	private function searchCallBack(title:String,
									publishType:String,
									addMan:String,
									addStartTime:String,
									addEndTime:String,
									auditMan:String,
									auditStartTime:String,
									auditEndTime:String,
									state:String):void
	{
		this._search_condition_title = title;
	    this._search_condition_publishType = publishType;
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