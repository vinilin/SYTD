	// ActionScript file
	import Custom.Common.Com;
	
	import SubModule.Article.Article.DefaultPic;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.events.DropdownEvent;
	import mx.events.ListEvent;
	import mx.events.MoveEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;

	private var __opKind:String="UPDATE";
	//private var __articleKind:String="";
	private var _param:String = "";
	private var __articleId:String = "";
	private var __saveCallBack:Function;
	private var __sequence:String = ""; 
	private var __defaultPic:String = "";
	private var _subCode:String = "";
	private var _subName:String = "";
	
	private var _kindCode:String = "";
	private var _kindCode2:String = "";
	
	private var _remote:RemoteObject;
	private var _remote1:RemoteObject;
	private var _remote2:RemoteObject;
	
	
	public function set opKind(val:String):void
	{
		this.__opKind=val;
	}
	public function set param(val:String):void
	{
		this._param = val;
	}
	public function set articleId(val:String):void
	{
		this.__articleId=val;
	}
	public function set subCode(val:String):void
	{
		this._subCode = val;
	}
	public function set subName(val:String):void
	{
		this._subName = val;
	}
	public function set saveCallBack(fun:Function):void
	{
		this.__saveCallBack=fun;
	}
	
	private var _state:String  = "init";
	
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
		this.addEventListener(MouseEvent.CLICK,OnClick);
		this.addEventListener(MoveEvent.MOVE,OnMove);
		this.cbox_kind.addEventListener(DropdownEvent.OPEN,OnDropOpen);
		this.cbox_kind.addEventListener(DropdownEvent.CLOSE,OnDropClose);
		//this.cbox_kind.addEventListener(ListEvent.CHANGE,OnKindSelectChange);
		this.cbox_kind2.addEventListener(DropdownEvent.OPEN,OnDropOpen);
		this.cbox_kind2.addEventListener(DropdownEvent.CLOSE,OnDropClose);
	}
	private function OnDropOpen(evt:DropdownEvent):void
	{		
		this.iFrame.hidden();	
	}
	private function OnDropClose(evt:DropdownEvent):void
	{
		this.iFrame.show();
	}
	private function OnKindSelectChange(evt:ListEvent):void
	{
		if (this.cbox_kind.selectedItem.CODE != "")
		{
			initKind2();
		}
		else
		{
			var result:ArrayCollection = new ArrayCollection();
			var obj:Object = new Object();
			obj.CODE = "";
			obj.TEXT = "";
			result.addItem(obj);
			this.cbox_kind2.dataProvider = result;
			this.cbox_kind2.labelField = "TEXT";
		}
	}
	public function initData():void
	{	
		if (this.__opKind == "UPDATE")
		{
			initInfo();	
		}
		else
		{
			initInfo();
			//initKind();
		}
	}
	
	private function initKind():void
	{
		_remote1 = new RemoteObject("fluorine");
		_remote1.source = "ManagementService.Sys.SystemKind";
		_remote1.showBusyCursor = true;
		_remote1.GetAllKind(this._subCode,this._param,"1");
		_remote1.addEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote1.addEventListener(FaultEvent.FAULT,OnGetKindFault);
	}
	private function OnGetKindResult(evt:ResultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote1.removeEventListener(FaultEvent.FAULT,OnGetKindFault);
		var result:ArrayCollection = _remote1.GetAllKind.lastResult as ArrayCollection;
		if (result == null || (result != null && result.length != 1))
		{
			var obj:Object = new Object();
			obj.CODE = "";
			obj.TEXT = "";
			result.addItemAt(obj,0);
		}
		//this.cbox_kind.dataProvider = result;
		//this.cbox_kind.labelField = "TEXT";
		
		/* if (this._state == "init" && this._kindCode!="")
		{
			for(var i:int=0;i<result.length;i++)
			{
				if (this._kindCode == result.source[i].CODE)
				{
					this.cbox_kind.selectedIndex = i;
				}
			}
		} */
	}
	private function OnGetKindFault(evt:FaultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote1.removeEventListener(FaultEvent.FAULT,OnGetKindFault);		
		//Alert.show("加载类别失败。"+evt.fault.faultString,"系统提示",4,this,AlertCloseHandle);
		//this.iFrame.hidden();
	}
	private function initKind2():void
	{   
		_remote2 = new RemoteObject("fluorine");
		_remote2.source = "ManagementService.Sys.SystemKind";
		_remote2.showBusyCursor = true;
		if (this._state == "init")
		{
			_remote2.GetAllKind(this._subCode,this._param,this._kindCode);
		}
		else
		{
			_remote2.GetAllKind(this._subCode,this._param,this.cbox_kind.selectedItem.CODE);
		}
		_remote2.addEventListener(ResultEvent.RESULT,OnGetKind2Result);
		_remote2.addEventListener(FaultEvent.FAULT,OnGetKind2Fault);
		
	}
	private function OnGetKind2Result(evt:ResultEvent):void
	{
		_remote2.removeEventListener(ResultEvent.RESULT,OnGetKind2Result);
		_remote2.removeEventListener(FaultEvent.FAULT,OnGetKind2Fault);
		var result:ArrayCollection = _remote2.GetAllKind.lastResult as ArrayCollection;
		var obj:Object = new Object();
		obj.CODE = "";
		obj.TEXT = "";
		result.addItemAt(obj,0);
		
		this.cbox_kind2.dataProvider = result;
		this.cbox_kind2.labelField = "TEXT";
		
		if (this._state == "init" && this._kindCode2 != "")
		{
			for(var i:int=0;i<result.length;i++)
			{
				if (this._kindCode2 == result.source[i].CODE)
				{
					this.cbox_kind2.selectedIndex = i;
				}
			}
		}
		this._state = "";
	}
	private function OnGetKind2Fault(evt:FaultEvent):void
	{
		_remote2.removeEventListener(ResultEvent.RESULT,OnGetKind2Result);
		_remote2.removeEventListener(FaultEvent.FAULT,OnGetKind2Fault);
		var result:ArrayCollection = new ArrayCollection();
		var obj:Object = new Object();
		obj.CODE = "";
		obj.TEXT = "";
		result.addItem(obj);
		this.cbox_kind2.dataProvider = result;
		this.cbox_kind2.labelField = "TEXT";
		
		this._state = "";
	}
	public function initInfo():void
	{
		if (this.__opKind=="ADD")
		{
			this.title = "增加社区服务";
		}
		else if(this.__opKind=="UPDATE")
		{
			this.title = "修改社区服务";
		}
		if (this.__opKind=="ADD")
		{
			var co:Com=new Com();
			this.__sequence=co.createSeq(this._param,"");
			//this.iFrame.source=this._param+"/"+this.__sequence;
			this.iFrame.type = this._param;
			this.iFrame.source = this.__sequence;
			this.iFrame.show();
		}
		else if(this.__opKind=="UPDATE")
		{
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Article.Article";			
			_remote.showBusyCursor=true;
			_remote.GetArticleInfo(this.__articleId);
			_remote.addEventListener(ResultEvent.RESULT,OnGetArticleResult);
			_remote.addEventListener(FaultEvent.FAULT,OnGetArticleFault);
		}
	}
	private function OnGetArticleResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetArticleResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetArticleFault);
		var result:ArrayCollection=_remote.GetArticleInfo.lastResult as ArrayCollection;
		if (result!=null && result.length>0)
		{
			this.tbox_title.text=result.source[0].ARTICLETITLE;
			this.__defaultPic=result.source[0].DEFAULTPIC;
			this.__sequence=result.source[0].SEQUENCE;
			var co:Com=new Com();
			this.__sequence=co.createSeq(this._param,result.source[0].SEQUENCE);
			//this.iFrame.source=this._param+"/"+this.__sequence;
			this.iFrame.type = this._param;
			this.iFrame.source = this.__sequence;
			this.iFrame.show();
			this.iFrame.value=result.source[0].ARTICLECONTENT;
			this._kindCode = result.source[0].ARTICLEKIND;
			//this._kindCode2 = result.source[0].ARTICLEKIND;
			/*
			var kindSur:ArrayCollection = this.cbox_kind.dataProvider as ArrayCollection;
			if (kindSur != null)
			{
				for(var i:int=0;i<kindSur.length;i++)
				{
					if (kindSur.source[i].CODE == result.source[0].ARTICLEKIND)
					{
						this.cbox_kind.selectedIndex = i;
						break;
					}
				}
			}
			var kindSur2:ArrayCollection = this.cbox_kind2.dataProvider as ArrayCollection;
			if (kindSur2 != null)
			{
				for(var j:int=0;j<kindSur2.length;j++)
				{
					if (kindSur2.source[j].CODE == result.source[0].ARTICLEKIND2)
					{
						this.cbox_kind2.selectedIndex = j;
						break;
					}
				}
			}
			*/
		}
		//initKind();
		//initKind2();
	}
	private function OnGetArticleFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetArticleResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetArticleFault);
		this.iFrame.hidden();
		Alert.okLabel = "确定";
		Alert.show("初始化数据失败，请退出后重新进入。"+evt.fault.faultString,"系统提示",Alert.OK,this,AlertCloseHandle);
	}
	private function OnSave(evt:MouseEvent):void
	{
		if (this.tbox_title.text=="")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请填写标题。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.tbox_title.setFocus();
			return;
		}
		/*if (this.cbox_kind.selectedItem.CODE == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请选择类别,如果没有类别请先增加类别。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.cbox_kind.setFocus();
			return;
		} */
                /*
		if (this._param == "008" && this.cbox_kind2.selectedItem.CODE == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请选择子类别,如果没有子类别请先增加类别。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.cbox_kind2.setFocus();
			return;
		}
                */
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Article.Article";
		_remote.showBusyCursor=true;
		//var articleContent:String=this.iFrame.value;
		//var articleContent:String=ExternalInterface.call("GetEditValue");
		if (this.__opKind=="ADD")
		{
			_remote.AddArticle(this._subCode,
							this._param,
							"",//this.cbox_kind.selectedItem.CODE,
							"",//this.cbox_kind2.selectedItem.CODE,
							this.tbox_title.text,
							this.iFrame.value,
							parentApplication._userInfo.trueName,
							this.__sequence,
							this.__defaultPic,
							"");	
		}
		else if(this.__opKind=="UPDATE")
		{
			_remote.UpdateArticle(this.__articleId,
							   this._subCode,
							   this._param,	
							   "",//this.cbox_kind.selectedItem.CODE,
							   "",//this.cbox_kind2.selectedItem.CODE,
							   this.tbox_title.text,
							   this.iFrame.value,
							   parentApplication._userInfo.trueName,
							   this.__sequence,
							   this.__defaultPic,
							   "");
		}
		_remote.addEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSaveFault);
		//Alert.show(articleContent);
	}
	private function OnSaveResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		var result:String;
		if (this.__opKind=="ADD")
		{
			result=_remote.AddArticle.lastResult as String;
		}
		else if(this.__opKind=="UPDATE")
		{
			result=_remote.UpdateArticle.lastResult as String;
		}
		if (result!=null)
		{
			if (result.substr(0,2).toUpperCase()=="OK")
			{
				if (this.__opKind=="ADD")
				{
					this.__articleId=result.substr(2);
				}
				if (this.__saveCallBack!=null)
				{
					this.__saveCallBack.call(this,
											 this.__articleId,
											 this._subCode,
											 this._subName,
											 "",//this.cbox_kind.selectedItem.CODE,
											 "",//this.cbox_kind.selectedItem.TEXT,
											 "",//this.cbox_kind2.selectedItem.CODE,
											 "",//this.cbox_kind2.selectedItem.TEXT,
											 "待审核",
											 "0",
											 this.tbox_title.text,
											 parentApplication._userInfo.trueName,
											 "","",
											 this.__sequence,
											 "",
											 this.__opKind);
				}
				this.iFrame.remove();
				PopUpManager.removePopUp(this);
			}
			else
			{
				this.iFrame.hidden();
				Alert.okLabel = "确定";
				Alert.show(result,"系统提示",Alert.OK,this,AlertCloseHandle);
			}
		}
		else
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("保存失败。","系统提示",Alert.OK,this,AlertCloseHandle);
		}
	}
	private function OnSaveFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		this.iFrame.hidden();
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，保存失败。"+evt.fault.faultString,"错误",Alert.OK,this,AlertCloseHandle);
	}
	private function AlertCloseHandle(evt:CloseEvent):void
	{
		this.iFrame.show();
	}
	
	private function OnClose(evt:CloseEvent):void
	{
		this.iFrame.remove();
		if (this.__opKind=="ADD")
		{//删除生成的SEQUENCE目录
			var co:Com=new Com();
			co.deleteSeqDir(this._param,this.__sequence);
		}
		PopUpManager.removePopUp(this);
	}
	
	private function OnClick(evt:MouseEvent):void
	{
		if (this.iFrame.GetShowState())
		{
			this.move(this.x-1,this.y);
			this.move(this.x+1,this.y);
		}	
	}
	private function OnMove(evt:MoveEvent):void
	{
		this.iFrame.show();
		this.iFrame.moveIFrame();
	}
		
	
