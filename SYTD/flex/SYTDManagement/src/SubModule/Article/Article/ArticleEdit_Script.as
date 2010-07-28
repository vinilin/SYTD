	// ActionScript file
	import Custom.Common.Com;
	
	import SubModule.Article.Article.DefaultPic;
	
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.events.DropdownEvent;
	import mx.events.MoveEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;

	private var __opKind:String="ADD";
	//private var __articleKind:String="";
	private var _param:String = "";
	private var _isExistIcon:Boolean = false;
	private var _isExistKind:Boolean = false;
	private var __articleId:String = "";
	private var __saveCallBack:Function;
	private var __sequence:String = ""; 
	private var __defaultPic:String = "";
	private var _subCode:String = "";
	private var _subName:String = "";
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
	public function set isExistIcon(val:Boolean):void
	{
		this._isExistIcon = val;
	}
	public function set isExistKind(val:Boolean):void
	{
		this._isExistKind = val;
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
	
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnDefaultPic.addEventListener(MouseEvent.CLICK,OnDefaultPicClick);
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
		this.addEventListener(MouseEvent.CLICK,OnClick);
		this.addEventListener(MoveEvent.MOVE,OnMove);
		this.cbox_kind.addEventListener(DropdownEvent.OPEN,OnDropOpen);
		this.cbox_kind.addEventListener(DropdownEvent.CLOSE,OnDropClose);
		this.ch_URL.addEventListener(Event.CHANGE,OnChURlChange);
	}
	private function OnDropOpen(evt:DropdownEvent):void
	{		
		this.iFrame.hidden();	
	}
	private function OnDropClose(evt:DropdownEvent):void
	{
		if (!this.ch_URL.selected)
		{
			this.iFrame.show();
		}
	}
	public function initData():void
	{
		if (this._isExistIcon)
		{
			this.btnDefaultPic.visible = true;
		}
		else
		{
			this.btnDefaultPic.visible = false;
		}
		initKind();
	}
	
	private function initKind():void
	{
		if (this._isExistKind)
		{	
			_remote1 = new RemoteObject("fluorine");
			_remote1.source = "ManagementService.Sys.SystemKind";
			_remote1.showBusyCursor = true;
			_remote1.GetAllKind(this._subCode,this._param,"1");
			_remote1.addEventListener(ResultEvent.RESULT,OnGetKindResult);
			_remote1.addEventListener(FaultEvent.FAULT,OnGetKindFault);
		}
		else
		{			
			var result:ArrayCollection = new ArrayCollection();
			var obj:Object = new Object();
			obj.CODE = "";
			obj.TEXT = "";
			result.addItem(obj);
			this.cbox_kind.dataProvider = result;
			this.cbox_kind.labelField = "TEXT";
			initInfo();
		}
	}
	private function OnGetKindResult(evt:ResultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote1.removeEventListener(FaultEvent.FAULT,OnGetKindFault);
		var result:ArrayCollection = _remote1.GetAllKind.lastResult as ArrayCollection;
		
		var obj:Object = new Object();
		obj.CODE = "";
		obj.TEXT = "";
		result.addItemAt(obj,0);
		this.cbox_kind.dataProvider = result;
		this.cbox_kind.labelField = "TEXT";
		initInfo();
	}
	private function OnGetKindFault(evt:FaultEvent):void
	{
		_remote1.removeEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote1.removeEventListener(FaultEvent.FAULT,OnGetKindFault);		
		//Alert.show("加载类别失败。"+evt.fault.faultString,"系统提示",4,this,AlertCloseHandle);
		//this.iFrame.hidden();
	}
	public function initInfo():void
	{		
		
		this.lb_source.visible = false;
		this.tbox_source.visible = false;
		this.lbKind2.visible = false;
		this.cbox_kind2.visible = false;
		this.cbox_kind.enabled = false;
		if (this.__opKind=="ADD")
		{
			switch(this._param.toUpperCase())
			{
				case "001":
					this.title = "增加石油要闻";
					this.lb_source.visible = true;
					this.tbox_source.visible = true;
					break;
					
				case "002":
					this.title = "增加图片新闻";
					this.lb_source.visible = true;
					this.tbox_source.visible = true;
					break;
					
				case "003":
					this.title = "增加企业文化";
					this.cbox_kind.enabled = true;
					break;
					
				case "004":
					this.title = "增加技术资讯";
					this.cbox_kind.enabled = true;
					break;
					
				case "006":
					this.title = "增加为您服务";
					this.cbox_kind.enabled = true;
					this.ch_URL.visible = true;
					this.lb_Url.visible = true;
					this.tbox_Url.visible = true;
					break;
					
				case "007":
					this.title = "增加专业服务";
					this.cbox_kind.enabled = true;
					this.ch_URL.visible = true;
					this.lb_Url.visible = true;
					this.tbox_Url.visible = true;
					break;
					
				case "008":
					this.title = "增加矿区服务";
					this.cbox_kind.enabled = true;
					this.lbKind2.visible = true;
					this.cbox_kind2.visible = true;
					break;
						
				case "009":
					this.title = "增加专题报道";
					this.cbox_kind.enabled = true;
					break;
					
				default:
					break;
			}
			
		}
		else if(this.__opKind=="UPDATE")
		{
			switch(this._param.toUpperCase())
			{
				case "001":
					this.title = "修改石油要闻";
					this.lb_source.visible = true;
					this.tbox_source.visible = true;
					break;
					
				case "002":
					this.title = "修改图片新闻";
					this.lb_source.visible = true;
					this.tbox_source.visible = true;
					break;
					
				case "003":
					this.title = "修改企业文化";
					this.cbox_kind.enabled = true;
					break;
					
				case "004":
					this.title = "修改技术资讯";
					this.cbox_kind.enabled = true;
					break;
					
				case "006":
					this.title = "修改为您服务";
					this.cbox_kind.enabled = true;
					 this.ch_URL.visible = true;
					this.lb_Url.visible = true;
					this.tbox_Url.visible = true;
					break;
					
				case "007":
					this.title = "修改专业服务";
					this.cbox_kind.enabled = true;
					this.ch_URL.visible = true;
					this.lb_Url.visible = true;
					this.tbox_Url.visible = true;
					break;
				
				case "008":
					this.title = "修改矿区服务";
					this.cbox_kind.enabled = true;
					this.lbKind2.visible = true;
					this.cbox_kind2.visible = true;
					break;
							
				case "009":
					this.title = "修改专题报道";
					this.cbox_kind.enabled = true;
					break;
					
				default:
					break;
			}
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
			this.tbox_source.text = result.source[0].SOURCE;
			var co:Com=new Com();
			this.__sequence=co.createSeq(this._param,result.source[0].SEQUENCE);
			//this.iFrame.source=this._param+"/"+this.__sequence;
			this.iFrame.type = this._param;
			this.iFrame.source = this.__sequence;
			if (result.source[0].DEFAULTPIC == "999999999")
			{
				this.iFrame.hidden();
				this.tbox_Url.text = result.source[0].ARTICLECONTENT;
				this.iFrame.value=result.source[0].ARTICLECONTENT;
				this.ch_URL.selected = true;
				this.lb_Url.visible = true;
				this.tbox_Url.visible = true;		
			}
			else
			{	
				this.iFrame.show();
				this.tbox_Url.text = result.source[0].ARTICLECONTENT;
				this.iFrame.value=result.source[0].ARTICLECONTENT;
				this.ch_URL.selected = false;
				this.lb_Url.visible = false;
				this.tbox_Url.visible = false;
			}
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
		}
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
		if (this._isExistKind && this.cbox_kind.selectedItem.CODE == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请选择类别,如果没有类别请先增加类别。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.cbox_kind.setFocus();
			return;
		} 
		/*if (this._param == "008" && this.cbox_kind2.selectedItem.CODE == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请选择子类别,如果没有子类别请先增加类别。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.cbox_kind2.setFocus();
			return;
		}*/
		if (this.ch_URL.selected && tbox_Url.text == "")
		{
			Alert.show("请填写外部URL","系统提示");
			return;
		}
		if ((this._param == "001" || this._param == "002") && this.tbox_source.text == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请填写新闻来源。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.tbox_source.setFocus();
			return;
		}
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Article.Article";
		_remote.showBusyCursor=true;
		//var articleContent:String=this.iFrame.value;
		//var articleContent:String=ExternalInterface.call("GetEditValue");
		var content:String;
		if (this.ch_URL.selected)
		{
			content = this.tbox_Url.text;
			this.__defaultPic = "999999999";	
		}
		else
		{
			if (this.__defaultPic == "999999999")
			{
				this.__defaultPic = "";
			}
			content = this.iFrame.value;
		}
		if (this.__opKind=="ADD")
		{
			_remote.AddArticle(this._subCode,
							this._param,
							this.cbox_kind.selectedItem.CODE,
							"", //kindCode2
							this.tbox_title.text,
							content,
							parentApplication._userInfo.trueName,
							this.__sequence,
							this.__defaultPic,
							this.tbox_source.text);	
		}
		else if(this.__opKind=="UPDATE")
		{
			_remote.UpdateArticle(this.__articleId,
							   this._subCode,
							   this._param,	
							   this.cbox_kind.selectedItem.CODE,
							   "", //kindCode2
							   this.tbox_title.text,
							   content,
							   parentApplication._userInfo.trueName,
							   this.__sequence,
							   this.__defaultPic,
							   this.tbox_source.text);
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
											 this.cbox_kind.selectedItem.CODE,
											 this.cbox_kind.selectedItem.TEXT,
											 "待审核",
											 "0",
											 this.tbox_title.text,
											 parentApplication._userInfo.trueName,
											 "","",
											 this.__sequence,
											 this.tbox_source.text,
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
		if (!this.ch_URL.selected)
		{
			this.iFrame.show();
		}
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
		
	private function OnDefaultPicClick(evt:MouseEvent):void
	{
		this.iFrame.hidden();
		var pop:DefaultPic=DefaultPic(PopUpManager.createPopUp(this,DefaultPic,true));
		pop.sequence=this.__sequence;
		pop.picFileName=this.__defaultPic;
		pop.type=this._param;
		pop.articleId=this.__articleId;
		pop.closeCallBack=defaultPicCloseCallBack;
		pop.loadFile();
		PopUpManager.centerPopUp(pop);
	}
	private function defaultPicCloseCallBack(picFileName:String):void
	{
		this.iFrame.show();
		this.__defaultPic=picFileName;	
	}
	
	private function OnChURlChange(evt:Event):void
	{
		if (this.ch_URL.selected)
		{
			this.iFrame.hidden();
			this.lb_Url.visible = true;
			this.tbox_Url.visible = true;	
		}
		else
		{
			this.iFrame.show();
			this.lb_Url.visible = false;
			this.tbox_Url.visible = false;
		}			
	}