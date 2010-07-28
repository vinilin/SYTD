// ActionScript file
	
	import Custom.Renderer.CenteredCheckBoxHeaderRenderer;
	import Custom.Renderer.CenteredCheckBoxItemRenderer;
	import Custom.System.Log;
	
	import SubModule.System.SystemKind1.KindEdit;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.collections.ListCollectionView;
	import mx.controls.Alert;
	import mx.controls.DataGrid;
	import mx.events.CloseEvent;
	import mx.events.DragEvent;
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
	private var _remote1:RemoteObject;
	
	//-------------------------------------------
	//查询参数
	private var _param:String = "";
	private var _isExistIcon:String = "";
	private var _subCode:String = "global";
	private var _subName:String = "全局";
	public function set param(val:String):void
	{
		this._param = val;
	}
	//-------------------------------------------
	private function init():void
	{
		////////////////////////////////////////////
		hr = new ClassFactory(CenteredCheckBoxHeaderRenderer);
		hr.properties = {stateHost: this, stateProperty: "selectAllFlag"};
		//---------------------------------------
		this.dg.addEventListener(MouseEvent.CLICK,OnCheckBoxClickHandler);
		this.dg.addEventListener(ListEvent.ITEM_DOUBLE_CLICK,OnDgItemDbClick);
		this.dg.addEventListener(ListEvent.ITEM_CLICK,OnDgItemClick);
		this.dg.addEventListener(DragEvent.DRAG_COMPLETE,OnDragComplete);
		//---------------------------------------
		this.btnAdd.addEventListener(MouseEvent.CLICK,OnAdd);
		this.btnUpdate.addEventListener(MouseEvent.CLICK,OnUpdate);
		this.btnDelete.addEventListener(MouseEvent.CLICK,OnDelete);
		/////////////////////////////////////////////
			
		Resize();
		parentApplication.ModuleInitCompleteCall();
		initBaseInfo();	
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

		this.dg.height=parentContentHeight-this.controlBar_Title.height-this.ToolBar.height-18;
	}
	
	public function initBaseInfo():void
	{
		switch (this._param)
		{
			case "003":		
				this._isExistIcon = "ICON";
				this.lbTitle.text = "◎  企业文化 —— 类别管理";
				break;
				
			case "004":
				this.lbTitle.text = "◎  技术资讯 —— 类别管理";
				break;
				
			case "006":
				this.lbTitle.text = "◎  为您服务 —— 类别管理";
				this.dgColumn_linkOrKind.visible = true;
				this.dgColumn_link.visible = true;
				break;	
				
			case "007":
				this._isExistIcon = "ICON";
				this._subCode = parentApplication._userInfo.subCode;
				this._subName = parentApplication._userInfo.subName;
				this.lbTitle.text = "◎  专业服务 —— 类别管理";
				this.dgColumn_linkOrKind.visible = true;
				this.dgColumn_link.visible = true;
				break;	
				
			case "008":
				this.lbTitle.text = "◎  社区服务 —— 类别管理";
				break;
					
			case "009":
				this._isExistIcon = "ICON";
				this._subCode = "global";
				this.lbTitle.text = "◎  专题报道 —— 专题管理";
				this.KINDNAME_COLUMN.headerText = "专题名称";
				this.KINDCODE_COLUMN.headerText = "专题代码";
				this.dgColumn_linkOrKind.visible = true;
				this.dgColumn_link.visible = true;
				break;
				
			case "011":
				this.lbTitle.text = "◎  石油影院 —— 电影类别";
				break;
			
			case "012":
				this.lbTitle.text = "◎  音乐欣赏 —— 音乐类别";
				break;
				
			case "013":
				this.lbTitle.text = "◎  软件下载 —— 软件类别";
				break;
				
			case "014":
				this.lbTitle.text = "◎  动漫欣赏 —— 动漫类别";
				break;			
			default:
				break;	
		}
	}
	
	public function Search():void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Sys.SystemKind";
		_remote.showBusyCursor=true;
		_remote.GetKindList(this._subCode,this._param,"1");
		_remote.addEventListener(ResultEvent.RESULT,OnSearchResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSearchFault);					
	}
	private function OnSearchResult(evt:ResultEvent):void
	{
		_searchResult=_remote.GetKindList.lastResult as ArrayCollection;
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
		Alert.show("网络或系统错误，加载数据失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	
	/////修改
	private function OnUpdate(evt:MouseEvent):void
	{
		if (this.dg.selectedItems==null || this.dg.selectedItems.length==0)
		{
			Alert.okLabel = "确定";
			mx.controls.Alert.show("请选择你要编辑的项。","警告",Alert.OK);
			return;
		}
		else if(this.dg.selectedItems.length!=1)
		{
			Alert.okLabel = "确定";
			mx.controls.Alert.show("你选择了多项数据，请选择你要编辑的项或直接双击行进入编辑。","警告",Alert.OK);
			return;
		}
		Update();
	}
	private function OnDgItemDbClick(evt:ListEvent):void
	{
		Update();
	}
	private function Update():void
	{
		var pop:KindEdit = KindEdit(PopUpManager.createPopUp(this,KindEdit,true));
		pop.param = this._param;
		pop.ids = this.dg.selectedItems[0].ID;
		pop.kind = this.dg.selectedItems[0].KIND;
		pop.pptr = this.dg.selectedItems[0].PPTR;
		pop.code = this.dg.selectedItems[0].CODE;
		pop.listOrder = this.dg.selectedItems[0].LISTORDER;
		pop.isExistIcon =this._isExistIcon;
		pop.subCode = this._subCode;
		pop.subName = this._subName;
		pop.opKind = "UPDATE";
		pop.saveCallBack = saveCallBack;
		pop.initData();
		PopUpManager.centerPopUp(pop);
	}
	
	////增加
	private function OnAdd(evt:MouseEvent):void
	{
		var pop:KindEdit=KindEdit(PopUpManager.createPopUp(this,KindEdit,true));
		pop.param = this._param;
		pop.opKind = "ADD";
		pop.kind = this._param;
		pop.pptr = "1";
		pop.isExistIcon =this._isExistIcon;
		pop.subCode = this._subCode;
		pop.subName = this._subName;
		pop.saveCallBack=saveCallBack;
		pop.initData();
		PopUpManager.centerPopUp(pop);
	}
	
	//增加、修改后的回调函数
	private function saveCallBack(id:String,
								  subCode:String,
								  subName:String,	
								  kind:String,
								  pptr:String,
								  code:String,
								  text:String,
								  listOrder:String,
								  defaultPic:String,
								  sequence:String,
								  linkOrKind:String,
								  linkUrl:String,
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
			obj.ISCHECKED=1;
			obj.ID=id;
			obj.SUBCODE = subCode;
			obj.SUBNAME = subName;
			obj.KIND = kind;
			obj.PPTR = pptr;
			obj.CODE = code;
			obj.LISTORDER = listOrder;
			obj.TEXT=text;
			obj.DEFAULTPIC = defaultPic;
			obj.SEQUENCE = sequence;
			if (linkOrKind == "0")
			{
				obj.LINKORKIND_DESCRIPTION = "";
				obj.LINKORKIND = 0;
			}
			else
			{
				obj.LINKORKIND_DESCRIPTION = "链接";
				obj.LINKORKIND = 1;
			}
			obj.LINKURL = linkUrl;
			this._searchResult.addItem(obj);
			//--------------------------------------
			selectAllFlag=false;
			var _selectItems:Array=new Array();
			_selectItems.push(obj);
			this.dg.selectedItems=_selectItems;
			//--------------------------------------
			this.dg.verticalScrollPosition=0;
			Custom.System.Log.WriteLog("SystemKind","增加","增加类别" + text,parentApplication._userInfo.trueName,id);
		}
		else
		{
			obj=this.dg.selectedItems[0];
			obj.SUBCODE = subCode;
			obj.SUBNAME = subName;
			obj.KIND = kind;
			obj.PPTR = pptr;
			obj.CODE = code;
			obj.TEXT = text;
			obj.LISTORDER = listOrder;
			obj.DEFAULTPIC = defaultPic;
			obj.SEQUENCE = sequence;
			if (linkOrKind == "0")
			{
				obj.LINKORKIND_DESCRIPTION = "";
				obj.LINKORKIND = 0;
			}
			else
			{
				obj.LINKORKIND_DESCRIPTION = "链接";
				obj.LINKORKIND = 1;
			}
			obj.LINKURL = linkUrl;
			ListCollectionView(dg.dataProvider).itemUpdated(this.dg.selectedItems[0]);
			Custom.System.Log.WriteLog("SystemKind","修改","修改类别" + text,parentApplication._userInfo.trueName,id);
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
				obj.ID=this.dg.selectedItems[i].ID;
				obj.SUBCODE = this.dg.selectedItems[i].SUBCODE;
				obj.SUBNAME = this.dg.selectedItems[i].SUBNAME;
				obj.KIND=this.dg.selectedItems[i].KIND;
				obj.PPTR=this.dg.selectedItems[i].PPTR;
				obj.CODE=this.dg.selectedItems[i].CODE;
				obj.TEXT=this.dg.selectedItems[i].TEXT;
				obj.DEFAULTPIC = this.dg.selectedItems[i].DEFAULTPIC;
				obj.SEQUENCE = this.dg.selectedItems[i].SEQUENCE;
				selectSrc.addItem(obj);
			}
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Sys.SystemKind";
			_remote.showBusyCursor=true;
			_remote.DeleteKind(selectSrc,this._param);
			_remote.addEventListener(ResultEvent.RESULT,OnDeleteResult);
			_remote.addEventListener(FaultEvent.FAULT,OnDeleteFault);
		}
	}
	private function OnDeleteResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnDeleteResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnDeleteFault);
		var result:ArrayCollection=_remote.DeleteKind.lastResult as ArrayCollection;
		if (result!=null)
		{
			var message:String="";
			for(var i:int=0;i<result.length;i++)
			{
				if (result.source[i].result=="0")
				{
					for(var j:int=0;j<this._searchResult.length;j++)
					{
						if (this._searchResult.source[j].ID==result.source[i].ID)
						{
							Custom.System.Log.WriteLog("SubSection","删除","删除类别"+this._searchResult.source[j].TEXT,parentApplication._userInfo.trueName,this._searchResult.source[j].ID);
							this._searchResult.removeItemAt(j);
							break;
						}
					}
				}
				else
				{
					message += "删除"+result.source[i].TEXT+""+result.source[i].result+"\r\n";
				}
			}
			Sort();
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
		Alert.show("网络或系统错误，删除失败。"+evt.fault.faultString,"错误",Alert.OK);
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
	
	private function OnDragComplete(evt:DragEvent):void
	{
		Sort();
	}
	private function Sort():void
	{		
		_remote1 = new RemoteObject("fluorine");
		_remote1.source = "ManagementService.Sys.SystemKind";
		_remote1.showBusyCursor = true;
		_remote1.KindSort(this._searchResult,this._subCode);
		_remote1.addEventListener(ResultEvent.RESULT,OnSortSuccess);
		_remote1.addEventListener(FaultEvent.FAULT,OnSortFault);
	}
	private function OnSortSuccess(evt:ResultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnSortSuccess);
		_remote1.removeEventListener(FaultEvent.FAULT,OnSortFault);
		var result:Boolean=_remote1.KindSort.lastResult as Boolean;
		if (result)
		{
			for(var i:int=0;i<_searchResult.length;i++)
        	{            
            	_searchResult.source[i].LISTORDER=String(i+1);
            	ListCollectionView(_searchResult).itemUpdated(_searchResult.source[i],"LISTORDER");
        	}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("排序保存失败。","系统提示",Alert.OK);
		}
	}
	private function OnSortFault(evt:FaultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnSortSuccess);
		_remote1.removeEventListener(FaultEvent.FAULT,OnSortFault);
		Alert.okLabel = "确定";
		Alert.show("排序保存失败，"+evt.fault.faultString,"系统提示",Alert.OK);
	}
	
	
