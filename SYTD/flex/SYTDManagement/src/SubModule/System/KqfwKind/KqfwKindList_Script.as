	// ActionScript file
	import Custom.Renderer.CenteredCheckBoxHeaderRenderer;
	import Custom.Renderer.CenteredCheckBoxItemRenderer;
	import Custom.System.Log;
	
	import SubModule.System.KqfwKind.KqfwKindEdit;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.collections.ListCollectionView;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.events.DragEvent;
	import mx.events.ListEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
	
	private var _subCode:String = "";
	private var _subName:String = "";
	private var _param:String = "008";
	
	[Bindable]private var _searchResult1:ArrayCollection=new ArrayCollection();	
	[Bindable]private var _searchResult2:ArrayCollection=new ArrayCollection();
	
	/////////////////////////////////
	public var selectAllFlag1:Boolean;
	[Bindable]public var hr1:ClassFactory;	
	/////////////////////////////////////////////
	
	/////////////////////////////////
	public var selectAllFlag2:Boolean;
	[Bindable]public var hr2:ClassFactory;	
	/////////////////////////////////////////////
	
	private var _remote1:RemoteObject = new RemoteObject("fluorine");
	private var _remote2:RemoteObject = new RemoteObject("fluorine");
	
	private function init():void
	{
		this._subCode = parentApplication._userInfo.subCode;
		this._subName = parentApplication._userInfo.subName;
		////////////////////////////////////////////
		hr1 = new ClassFactory(CenteredCheckBoxHeaderRenderer);
		hr1.properties = {stateHost: this, stateProperty: "selectAllFlag1"};
		//---------------------------------------
		////////////////////////////////////////////
		hr2 = new ClassFactory(CenteredCheckBoxHeaderRenderer);
		hr2.properties = {stateHost: this, stateProperty: "selectAllFlag2"};
		//---------------------------------------
		
		this.btnAdd1.addEventListener(MouseEvent.CLICK,OnBtnAdd1_Click);
		this.btnUpdate1.addEventListener(MouseEvent.CLICK,OnBtnUpdate1_Click);
		this.btnDelete1.addEventListener(MouseEvent.CLICK,OnBtnDelete1_Click);
		
		this.btnAdd2.addEventListener(MouseEvent.CLICK,OnBtnAdd2_Click);
		this.btnUpdate2.addEventListener(MouseEvent.CLICK,OnBtnUpdate2_Click);
		this.btnDelete2.addEventListener(MouseEvent.CLICK,OnBtnDelete2_Click);
		
		this.dg1.addEventListener(ListEvent.ITEM_CLICK,OnDg1_Click);
		this.dg2.addEventListener(ListEvent.ITEM_CLICK,OnDg2_Click);
		
		this.dg1.addEventListener(ListEvent.ITEM_DOUBLE_CLICK,OnDg1_DBClick);
		this.dg2.addEventListener(ListEvent.ITEM_DOUBLE_CLICK,OnDg2_DBClick);
		
		this.dg1.addEventListener(DragEvent.DRAG_COMPLETE,OnDragComplete1);
		this.dg2.addEventListener(DragEvent.DRAG_COMPLETE,OnDragComplete2);
		
		Resize();
		Search1();
	}
	
	public function Resize():void
	{
		var parentWidth:int=parentApplication.width;
		var parentHeight:int=parentApplication.height;
		var parentContentWidth:int=parentWidth - parentApplication.vbox_menu.width - 10;
		var parentContentHeight:int=parentHeight-94;
		
		this.hdBox.width = parentContentWidth;
		this.hdBox.height = parentContentHeight - this.controlBar_Title.height -10;
		this.callLater(Layout);
	}
	private function Layout():void
	{	
		this.dg1.height = this.Panel_left.height - this.ToolBar1.height - 10;
		this.dg2.height = this.Panel_right.height - this.ToolBar2.height - 10;
	}
	
	private function Search1():void
	{
		_remote1.source = "ManagementService.Sys.SystemKind";
		_remote1.showBusyCursor = true;
		_remote1.GetKindList(this._subCode,this._param,"1")
		_remote1.addEventListener(ResultEvent.RESULT,OnSearch1Result);
		_remote1.addEventListener(FaultEvent.FAULT,OnSearch1Fault);
	}
	private function OnSearch1Result(evt:ResultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnSearch1Result);
		_remote1.removeEventListener(FaultEvent.FAULT,OnSearch1Fault);
		this._searchResult1 = _remote1.GetKindList.lastResult as ArrayCollection;
	}
	private function OnSearch1Fault(evt:FaultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnSearch1Result);
		_remote1.removeEventListener(FaultEvent.FAULT,OnSearch1Fault);
		mx.controls.Alert.okLabel = "确定";
		Alert.show("网络或系统错误，获取分类失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	
	private function Search2(pptr:String):void
	{
		_remote2.source = "ManagementService.Sys.SystemKind";
		_remote2.showBusyCursor = true;
		_remote2.GetKindList(this._subCode,this._param,pptr)
		_remote2.addEventListener(ResultEvent.RESULT,OnSearch2Result);
		_remote2.addEventListener(FaultEvent.FAULT,OnSearch2Fault);
	}
	private function OnSearch2Result(evt:ResultEvent):void
	{
		_remote2.removeEventListener(ResultEvent.RESULT,OnSearch2Result);
		_remote2.removeEventListener(FaultEvent.FAULT,OnSearch2Fault);
		this._searchResult2 = _remote2.GetKindList.lastResult as ArrayCollection;
	}
	private function OnSearch2Fault(evt:FaultEvent):void
	{
		_remote2.removeEventListener(ResultEvent.RESULT,OnSearch2Result);
		_remote2.removeEventListener(FaultEvent.FAULT,OnSearch2Fault);
		mx.controls.Alert.okLabel = "确定";
		Alert.show("网络或系统错误，获取分类失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	
	private function OnBtnAdd1_Click(evt:MouseEvent):void
	{
		var pop:KqfwKindEdit = KqfwKindEdit(mx.managers.PopUpManager.createPopUp(this,KqfwKindEdit,true));
		pop.subCode = this._subCode;
		pop.subName = this._subName;
		pop.param = this._param;
		pop.pptr = "1";
		pop.opKind = "ADD";
		pop.saveCallBack = saveCallBack1;
		PopUpManager.centerPopUp(pop);
	}
	private function OnBtnUpdate1_Click(evt:MouseEvent):void
	{
		if (this.dg1.selectedItems==null || this.dg1.selectedItems.length == 0)
		{
			Alert.okLabel = "确定";
			Alert.show("请选择你要编辑的项，或双击要编辑的行直接进入编辑。","提示",Alert.OK);
			return;
		}		
		if (this.dg1.selectedItems.length >1)
		{
			Alert.okLabel = "确定";
			Alert.show("你选择了多行数据，请选择你要编辑的项，或双击要编辑的行直接进入编辑。","提示",Alert.OK);
			return;
		}
		Update1();
	}
	private function OnDg1_DBClick(evt:ListEvent):void
	{
		Update1();
	}
	private function Update1():void
	{
		var pop:KqfwKindEdit = KqfwKindEdit(PopUpManager.createPopUp(this,KqfwKindEdit,true));
		pop.ids = this.dg1.selectedItems[0].ID;
		pop.subCode = this._subCode;
		pop.subName = this._subName;
		pop.param = this._param;
		pop.pptr = this.dg1.selectedItems[0].PPTR;
		pop.code = this.dg1.selectedItems[0].CODE;
		pop.text = this.dg1.selectedItems[0].TEXT;
		pop.opKind = "UPDATE";
		pop.saveCallBack = saveCallBack1;
		PopUpManager.centerPopUp(pop);
	}
	private function saveCallBack1(subCode:String,subName:String,id:String,pptr:String,code:String,text:String,listOrder:String,opKind:String):void
	{
		var obj:Object;
		if (opKind=="ADD")
		{
			obj=new Object();
			for(var i:int=0;i<this._searchResult1.length;i++)
			{
				this._searchResult1.source[i].ISCHECKED=0;
			}
			obj.ISCHECKED=1;
			obj.ID=id;
			obj.SUBCODE = subCode;
			obj.SUBNAME = subName;
			obj.KIND = this._param;
			obj.PPTR = pptr;
			obj.CODE = code;
			obj.LISTORDER = listOrder;
			obj.TEXT=text;
			this._searchResult1.addItem(obj);
			//--------------------------------------
			selectAllFlag1=false;
			var _selectItems:Array=new Array();
			_selectItems.push(obj);
			this.dg1.selectedItems=_selectItems;
			//--------------------------------------
			this.dg1.verticalScrollPosition=0;
			Custom.System.Log.WriteLog("SystemKind","增加","增加类别" + text,parentApplication._userInfo.trueName,id);
		}
		else
		{
			obj=this.dg1.selectedItems[0];
			obj.SUBCODE = subCode;
			obj.SUBNAME = subName;
			obj.PPTR = pptr;
			obj.CODE = code;
			obj.TEXT = text;
			obj.LISTORDER = listOrder;
			ListCollectionView(dg1.dataProvider).itemUpdated(this.dg1.selectedItems[0]);
			Custom.System.Log.WriteLog("SystemKind","修改","修改类别" + text,parentApplication._userInfo.trueName,id);
		}
	} 
	private function OnBtnDelete1_Click(evt:MouseEvent):void
	{
		if (this.dg1.selectedItems == null || this.dg1.selectedItems.length == 0)
		{
			Alert.okLabel = "确定";
			Alert.show("请选择你要删除的项","提示",Alert.OK);
			return;
		}
		Alert.yesLabel = "是";
		Alert.noLabel = "否";
		Alert.show("你真的要删除选择的项？","提示",Alert.YES|Alert.NO,this,AlertDeleteHandler1);
	}
	private function AlertDeleteHandler1(evt:CloseEvent):void
	{
		if (Alert.YES == evt.detail)
		{
			this._searchResult2 = new ArrayCollection();
			var selectSrc:ArrayCollection=new ArrayCollection();
			for(var i:int=0;i<this.dg1.selectedItems.length;i++)
			{
				var obj:Object = new Object();
				obj.ID=this.dg1.selectedItems[i].ID;
				obj.SUBCODE = this.dg1.selectedItems[i].SUBCODE;
				obj.SUBNAME = this.dg1.selectedItems[i].SUBNAME;
				obj.KIND=this.dg1.selectedItems[i].KIND;
				obj.PPTR=this.dg1.selectedItems[i].PPTR;
				obj.CODE=this.dg1.selectedItems[i].CODE;
				obj.TEXT=this.dg1.selectedItems[i].TEXT;
				obj.DEFAULTPIC = this.dg1.selectedItems[i].DEFAULTPIC;
				obj.SEQUENCE = this.dg1.selectedItems[i].SEQUENCE;
				selectSrc.addItem(obj);
			}
			_remote1=new RemoteObject("fluorine");
			_remote1.source="ManagementService.Sys.SystemKind";
			_remote1.showBusyCursor=true;
			_remote1.DeleteKind(selectSrc,this._param);
			_remote1.addEventListener(ResultEvent.RESULT,OnDelete1Result);
			_remote1.addEventListener(FaultEvent.FAULT,OnDelete1Fault);
		}
	}
	private function OnDelete1Result(evt:ResultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnDelete1Result);
		_remote1.removeEventListener(FaultEvent.FAULT,OnDelete1Fault);
		var result:ArrayCollection=_remote1.DeleteKind.lastResult as ArrayCollection;
		if (result!=null)
		{
			var message:String="";
			for(var i:int=0;i<result.length;i++)
			{
				if (result.source[i].result=="0")
				{
					for(var j:int=0;j<this._searchResult1.length;j++)
					{
						if (this._searchResult1.source[j].ID==result.source[i].ID)
						{
							Custom.System.Log.WriteLog("SubSection","删除","删除类别"+this._searchResult1.source[j].TEXT,parentApplication._userInfo.trueName,this._searchResult1.source[j].ID);
							this._searchResult1.removeItemAt(j);
							break;
						}
					}
				}
				else
				{
					message += "删除"+result.source[i].TEXT+""+result.source[i].result+"\r\n";
				}
			}
			Sort1();
			if (message!="")
			{
				Alert.okLabel = "确定";
				Alert.show(message,"提示",Alert.OK);
			}	
			///如果删除后当前页为空了，则重新查询
			if (_searchResult1.length==0)
			{
				Search1();
			}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("删除失败。","错误",Alert.OK);
		}
	}
	private function OnDelete1Fault(evt:FaultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnDelete1Result);
		_remote1.removeEventListener(FaultEvent.FAULT,OnDelete1Fault);
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，删除失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	private function OnDg1_Click(evt:ListEvent):void
	{
		var i:int=0;
		if (!(evt.itemRenderer is CenteredCheckBoxItemRenderer))
		{
			for(i=0;i<_searchResult1.length;i++)
			{
				_searchResult1.source[i].ISCHECKED=0;
			}
			for(i=0;i<dg1.selectedItems.length;i++)
			{
				this.dg1.selectedItems[i].ISCHECKED=1;
				ListCollectionView(dg1.dataProvider).itemUpdated(this.dg1.selectedItems[i], "ISCHECKED");
			}
			Search2(this.dg1.selectedItems[0].CODE);
		}
		////////////////////////////////////////
		var allCheck:Boolean=true;
		for(i=0;i<_searchResult1.length;i++)
		{
			if (!_searchResult1.source[i].ISCHECKED)	
			{
				allCheck=false;
				break;
			}
		}
		selectAllFlag1=allCheck;
		//////////////////////////////////////////
	}
	
	private function OnCheckBoxClickHandler1(event:MouseEvent):void
	{
		var obj:Object;
		var _selectItems:Array=new Array();
		var tempDg:DataGrid=DataGrid(event.currentTarget);
    	if(event.target is CenteredCheckBoxHeaderRenderer)
    	{
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
    		selectAllFlag1=allCheck;
    	}	
	}
	private function OnDragComplete1(evt:DragEvent):void
	{
		Sort1();
	}
	private function Sort1():void
	{
		_remote1 = new RemoteObject("fluorine");
		_remote1.source = "ManagementService.Sys.SystemKind";
		_remote1.showBusyCursor = true;
		_remote1.KindSort(this._searchResult1,this._subCode);
		_remote1.addEventListener(ResultEvent.RESULT,OnSort1Success);
		_remote1.addEventListener(FaultEvent.FAULT,OnSort1Fault);
	}
	private function OnSort1Success(evt:ResultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnSort1Success);
		_remote1.removeEventListener(FaultEvent.FAULT,OnSort1Fault);
		var result:Boolean=_remote1.KindSort.lastResult as Boolean;
		if (result)
		{
			for(var i:int=0;i<_searchResult1.length;i++)
        	{            
            	_searchResult1.source[i].LISTORDER=String(i+1);
            	ListCollectionView(_searchResult1).itemUpdated(_searchResult1.source[i],"LISTORDER");
        	}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("排序保存失败。","系统提示",Alert.OK);
		}
	}
	private function OnSort1Fault(evt:FaultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnSort1Success);
		_remote1.removeEventListener(FaultEvent.FAULT,OnSort1Fault);
		Alert.okLabel = "确定";
		Alert.show("排序保存失败，"+evt.fault.faultString,"系统提示",Alert.OK);
	}
	
	private function OnBtnAdd2_Click(evt:MouseEvent):void
	{
		if (this.dg1.selectedItems == null || this.dg1.selectedItems.length == 0)
		{
			Alert.okLabel = "确定";
			Alert.show("请选择左边的一级类别。","提示",Alert.OK);
		}
		if (this.dg1.selectedItems.length >1)
		{
			Alert.okLabel = "确定";
			Alert.show("你选择了多行一级类别，请选择你要编辑的项的父类","提示",Alert.OK);
			return;
		}
		var pop:KqfwKindEdit = KqfwKindEdit(PopUpManager.createPopUp(this,KqfwKindEdit,true));
		pop.subCode = this._subCode;
		pop.subName = this._subName;
		pop.param = this._param;
		pop.pptr = this.dg1.selectedItems[0].CODE;
	    pop.opKind = "ADD";
	    pop.saveCallBack = saveCallBack2;
	    PopUpManager.centerPopUp(pop);
	}
	private function OnBtnUpdate2_Click(evt:MouseEvent):void
	{
		if (this.dg1.selectedItems == null || this.dg1.selectedItems.length == 0)
		{
			Alert.okLabel = "确定";
			Alert.show("你要编辑的项。","提示",Alert.OK);
		}
		if (this.dg1.selectedItems.length >1)
		{
			Alert.okLabel = "确定";
			Alert.show("你选择了多行数据，请选择你要编辑的项或双击行直接进入编辑。","提示",Alert.OK);
			return;
		}
		Update2();
	}
	private function OnDg2_DBClick(evt:ListEvent):void
	{
		Update2();
	}
	private function Update2():void
	{
		var pop:KqfwKindEdit = KqfwKindEdit(PopUpManager.createPopUp(this,KqfwKindEdit,true));
		pop.ids = this.dg2.selectedItems[0].ID;
		pop.subCode = this._subCode;
		pop.subName = this._subName;
		pop.param =this._param;
		pop.pptr = this.dg2.selectedItems[0].PPTR;
		pop.code = this.dg2.selectedItems[0].CODE;
		pop.text = this.dg2.selectedItems[0].TEXT;
		pop.listOrder = this.dg2.selectedItems[0].LISTORDER;
		pop.saveCallBack = saveCallBack2;
	}
	private function saveCallBack2(subCode:String,subName:String,id:String,pptr:String,code:String,text:String,listOrder:String,opKind:String):void
	{
		var obj:Object;
		if (opKind=="ADD")
		{
			obj=new Object();
			for(var i:int=0;i<this._searchResult2.length;i++)
			{
				this._searchResult2.source[i].ISCHECKED=0;
			}
			obj.ISCHECKED=1;
			obj.ID=id;
			obj.SUBCODE = subCode;
			obj.SUBNAME = subName;
			obj.KIND = this._param;
			obj.PPTR = pptr;
			obj.CODE = code;
			obj.LISTORDER = listOrder;
			obj.TEXT=text;
			this._searchResult2.addItem(obj);
			//--------------------------------------
			selectAllFlag2=false;
			var _selectItems:Array=new Array();
			_selectItems.push(obj);
			this.dg2.selectedItems=_selectItems;
			//--------------------------------------
			this.dg2.verticalScrollPosition=0;
			Custom.System.Log.WriteLog("SystemKind","增加","增加类别" + text,parentApplication._userInfo.trueName,id);
		}
		else
		{
			obj=this.dg2.selectedItems[0];
			obj.SUBCODE = subCode;
			obj.SUBNAME = subName;
			obj.PPTR = pptr;
			obj.CODE = code;
			obj.TEXT = text;
			obj.LISTORDER = listOrder;
			ListCollectionView(dg2.dataProvider).itemUpdated(this.dg2.selectedItems[0]);
			Custom.System.Log.WriteLog("SystemKind","修改","修改类别" + text,parentApplication._userInfo.trueName,id);
		}
	}
	private function OnBtnDelete2_Click(evt:MouseEvent):void
	{
		if (this.dg2.selectedItems == null || this.dg2.selectedItems.length == 0)
		{
			Alert.okLabel = "确定";
			Alert.show("请选择你要删除的项","提示",Alert.OK);
			return;
		}
		Alert.yesLabel = "是";
		Alert.noLabel = "否";
		Alert.show("你真的要删除选择的项？","提示",Alert.YES|Alert.NO,this,AlertDeleteHandler2);
	}
	private function AlertDeleteHandler2(evt:CloseEvent):void
	{
		if (Alert.YES == evt.detail)
		{
			var selectSrc:ArrayCollection=new ArrayCollection();
			for(var i:int=0;i<this.dg2.selectedItems.length;i++)
			{
				var obj:Object = new Object();
				obj.ID=this.dg2.selectedItems[i].ID;
				obj.SUBCODE = this.dg2.selectedItems[i].SUBCODE;
				obj.SUBNAME = this.dg2.selectedItems[i].SUBNAME;
				obj.KIND=this.dg1.selectedItems[i].KIND;
				obj.PPTR=this.dg2.selectedItems[i].PPTR;
				obj.CODE=this.dg2.selectedItems[i].CODE;
				obj.TEXT=this.dg2.selectedItems[i].TEXT;
				obj.DEFAULTPIC = this.dg2.selectedItems[i].DEFAULTPIC;
				obj.SEQUENCE = this.dg2.selectedItems[i].SEQUENCE;
				selectSrc.addItem(obj);
			}
			_remote2=new RemoteObject("fluorine");
			_remote2.source="ManagementService.Sys.SystemKind";
			_remote2.showBusyCursor=true;
			_remote2.DeleteKind(selectSrc,this._param);
			_remote2.addEventListener(ResultEvent.RESULT,OnDelete2Result);
			_remote2.addEventListener(FaultEvent.FAULT,OnDelete2Fault);
		}
	}
	private function OnDelete2Result(evt:ResultEvent):void
	{
		_remote2.removeEventListener(ResultEvent.RESULT,OnDelete2Result);
		_remote2.removeEventListener(FaultEvent.FAULT,OnDelete2Fault);
		var result:ArrayCollection=_remote2.DeleteKind.lastResult as ArrayCollection;
		if (result!=null)
		{
			var message:String="";
			for(var i:int=0;i<result.length;i++)
			{
				if (result.source[i].result=="0")
				{
					for(var j:int=0;j<this._searchResult2.length;j++)
					{
						if (this._searchResult2.source[j].ID==result.source[i].ID)
						{
							Custom.System.Log.WriteLog("SubSection","删除","删除类别"+this._searchResult2.source[j].TEXT,parentApplication._userInfo.trueName,this._searchResult2.source[j].ID);
							this._searchResult2.removeItemAt(j);
							break;
						}
					}
				}
				else
				{
					message += "删除"+result.source[i].TEXT+""+result.source[i].result+"\r\n";
				}
			}
			Sort2();
			if (message!="")
			{
				Alert.okLabel = "确定";
				Alert.show(message,"提示",Alert.OK);
			}	
			///如果删除后当前页为空了，则重新查询
			if (_searchResult2.length==0)
			{
				Search2(this.dg1.selectedItems[0].CODE);
			}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("删除失败。","错误",Alert.OK);
		}
	}
	private function OnDelete2Fault(evt:FaultEvent):void
	{
		_remote2.removeEventListener(ResultEvent.RESULT,OnDelete2Result);
		_remote2.removeEventListener(FaultEvent.FAULT,OnDelete2Fault);
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，删除失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	private function OnDg2_Click(evt:ListEvent):void
	{
		var i:int=0;
		if (!(evt.itemRenderer is CenteredCheckBoxItemRenderer))
		{
			for(i=0;i<_searchResult2.length;i++)
			{
				_searchResult2.source[i].ISCHECKED=0;
			}
			for(i=0;i<dg2.selectedItems.length;i++)
			{
				this.dg2.selectedItems[i].ISCHECKED=1;
				ListCollectionView(dg2.dataProvider).itemUpdated(this.dg2.selectedItems[i], "ISCHECKED");
			}
		}
		////////////////////////////////////////
		var allCheck:Boolean=true;
		for(i=0;i<_searchResult2.length;i++)
		{
			if (!_searchResult2.source[i].ISCHECKED)	
			{
				allCheck=false;
				break;
			}
		}
		selectAllFlag2=allCheck;
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
    		selectAllFlag2=allCheck;
    	}	
	}
	private function OnDragComplete2(evt:DragEvent):void
	{
		Sort2();
	}
	private function Sort2():void
	{
		_remote2 = new RemoteObject("fluorine");
		_remote2.source = "ManagementService.Sys.SystemKind";
		_remote2.showBusyCursor = true;
		_remote2.KindSort(this._searchResult2,this._subCode);
		_remote2.addEventListener(ResultEvent.RESULT,OnSort2Success);
		_remote2.addEventListener(FaultEvent.FAULT,OnSort2Fault);
	}
	private function OnSort2Success(evt:ResultEvent):void
	{
		_remote2.removeEventListener(ResultEvent.RESULT,OnSort2Success);
		_remote2.removeEventListener(FaultEvent.FAULT,OnSort2Fault);
		var result:Boolean=_remote2.KindSort.lastResult as Boolean;
		if (result)
		{
			for(var i:int=0;i<_searchResult2.length;i++)
        	{            
            	_searchResult2.source[i].LISTORDER=String(i+1);
            	ListCollectionView(_searchResult2).itemUpdated(_searchResult2.source[i],"LISTORDER");
        	}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("排序保存失败。","系统提示",Alert.OK);
		}
	}
	private function OnSort2Fault(evt:FaultEvent):void
	{
		_remote2.removeEventListener(ResultEvent.RESULT,OnSort2Success);
		_remote2.removeEventListener(FaultEvent.FAULT,OnSort2Fault);
		Alert.okLabel = "确定";
		Alert.show("排序保存失败，"+evt.fault.faultString,"系统提示",Alert.OK);
	}
	