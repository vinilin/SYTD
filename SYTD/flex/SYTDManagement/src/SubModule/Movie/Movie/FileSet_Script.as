	// ActionScript file
	import Custom.Renderer.CenteredCheckBoxHeaderRenderer;
	import Custom.Renderer.CenteredCheckBoxItemRenderer;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.collections.ListCollectionView;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.events.ListEvent;
	import mx.managers.PopUpManager;
	
	
	[Bindable]private var _files:ArrayCollection=new ArrayCollection;
	private var _index:String = "";;
	private var _callBack:Function;
	public function set files(val:ArrayCollection):void
	{
		this._files = val;
		for(var i:int=0;i<this._files.length;i++)
		{
			this._files.source[i].INDEX = i;
		}
	} 	
	public function set callBack(fun:Function):void
	{
		this._callBack = fun;
	}
	/////////////////////////////////
	public var selectAllFlag:Boolean;	
	[Bindable]public var hr:ClassFactory;	
	/////////////////////////////////////////////
	
	private function init():void
	{
		////////////////////////////////////////////
		hr = new ClassFactory(CenteredCheckBoxHeaderRenderer);
		hr.properties = {stateHost: this, stateProperty: "selectAllFlag"};
		//---------------------------------------
		
		dg.addEventListener(MouseEvent.CLICK,OnCheckBoxClickHandler);
		dg.addEventListener(ListEvent.ITEM_CLICK,OnDgItemClick);
		
		this.addEventListener(CloseEvent.CLOSE,OnClose);
//		this.btnAdd.addEventListener(MouseEvent.CLICK,OnAdd);
		this.btnDelete.addEventListener(MouseEvent.CLICK,OnDelete);
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
	}
	
	private function OnAdd(evt:MouseEvent):void
	{
		this.tbox_fileName.text = "";
		this._index = "";
	}
	private function OnDelete(evt:MouseEvent):void
	{
		if (this.dg.selectedItems==null || this.dg.selectedItems.length==0)
		{
			Alert.okLabel = "确定";
			Alert.show("请选择你要删除的文件。","提示",Alert.OK);
			return;
		}
		Alert.yesLabel = "是";
		Alert.noLabel = "否";
		Alert.show("你确定要删除选中的文件？","提示",Alert.YES|Alert.NO,this,DeleteAlertClose);
	}
	private function DeleteAlertClose(evt:CloseEvent):void
	{
		if (Alert.YES == evt.detail)
		{
			for(var i:int=this.dg.selectedItems.length;i>0;i--)
			{
				for(var j:int=0;j<this._files.length;j++)
				{
					if (this._files.source[j].FILENAME == this.dg.selectedItems[i-1].FILENAME)
					
					{
						this._files.removeItemAt(j);
						this._index = "";
						break;
					}
				}
			}
		}
	}
	private function OnSave(evt:MouseEvent):void
	{
		if (this.tbox_fileName.text == "")
		{
			Alert.okLabel = "确定";
			Alert.show("请输入文件名","提示",Alert.OK);
			return;
		}
		var isExists:Boolean = false;
		for(var i:int=0;i<this._files.length;i++)
		{
			if (this._files.source[i].FILENAME == this.tbox_fileName.text)
			{
				isExists = true;
			}
		}
		if (isExists)
		{
			Alert.okLabel = "确定";
			Alert.show("该文件名已经存在。","提示",Alert.OK);
			return;
		}
		if (this._index == "")
		{
			var obj:Object = new Object();
			obj.FILENAME = this.tbox_fileName.text;
			this._files.addItem(obj);
		}
		else
		{
			this._files.source[_index].FILENAME = this.tbox_fileName.text;
		}
		this.tbox_fileName.text = "";
	}
	
	private function OnClose(evt:CloseEvent):void
	{
		if (this._callBack!=null)
		{
			this._callBack.call(this,this._files);
		}
		mx.managers.PopUpManager.removePopUp(this);
	}
	
	
	////////选择
	private function OnDgItemClick(evt:ListEvent):void
	{		
		var i:int=0;
		if (!(evt.itemRenderer is CenteredCheckBoxItemRenderer))
		{
			for(i=0;i<this._files.length;i++)
			{
				_files.source[i].ISCHECKED=0;
			}
			for(i=0;i<dg.selectedItems.length;i++)
			{
				this.dg.selectedItems[i].ISCHECKED=1;
				ListCollectionView(dg.dataProvider).itemUpdated(this.dg.selectedItems[i], "ISCHECKED");
			}
			this.tbox_fileName.text = this.dg.selectedItem.FILENAME;
			this._index = this.dg.selectedIndex as String;
		}
		////////////////////////////////////////
		var allCheck:Boolean=true;
		for(i=0;i<_files.length;i++)
		{
			if (!_files.source[i].ISCHECKED)	
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