	// ActionScript file
	import Custom.Common.Com;
	
	import SubModule.Music.Music.DefaultPic;
	import SubModule.Music.Music.FileSet;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
	
	private var _ids:String;
	private var _opKind:String = "ADD";
	private var _saveCallBack:Function;
	private var _defaultPic:String="";
	private var __sequence:String = "";
	private var _fileSet:ArrayCollection = new ArrayCollection();
	private var _param:String;
	private var _state:String="0";  //只有STATE为-1才能修改，其余时候不能修改，STATE为-1时，要用户选择是否上传文件
	private var _subCode:String ;  //影片不是本站点发布页不能编辑
	private var _remote:RemoteObject;
	public function set ids(val:String):void
	{
		this._ids = val;
	}
	public function set opKind(val:String):void
	{
		this._opKind = val;
	}
	public function set param(val:String):void
	{
		this._param = val;
	}
	public function set saveCallBack(fun:Function):void
	{
		this._saveCallBack = fun;
	}
	private function init():void
	{
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
		this.btnPic.addEventListener(MouseEvent.CLICK,OnBtnPicClick);
		this.btnFileList.addEventListener(MouseEvent.CLICK,OnFileList);
		this.addEventListener(CloseEvent.CLOSE,OnClose);
	}
	
	public function initData():void
	{
		if (this._opKind == "ADD")
		{
			this.title = "增加音乐";
			var co:Com=new Com();
			this.__sequence=co.createSeq(this._param,"");
			
			this.height = 500;
			this.tbox_auditReason.visible = false;
			this.lb_auditResult.visible = false;
			this.chbox_reUpload.visible = false;
			this.tbox_title.enabled = true;
		}
		else
		{
			this.title = "修改音乐";
			this.btnFileList.enabled = this.chbox_reUpload.selected;
			this.chbox_reUpload.addEventListener(MouseEvent.CLICK, OnReUpload);
		}
		initKind();
	}
	private function OnReUpload(evt:MouseEvent):void
	{
		this.btnFileList.enabled = this.chbox_reUpload.selected;
	}
	private function initKind():void
	{
		_remote = new RemoteObject("fluorine");
		_remote.source = "ManagementService.FileT.Kind";
		_remote.showBusyCursor =true;
		_remote.GetKind(2);
		_remote.addEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote.addEventListener(FaultEvent.FAULT,OnGetKindFault);
	}
	private function OnGetKindResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetKindFault);
		var result:ArrayCollection = _remote.GetKind.lastResult as ArrayCollection;
		var obj:Object = new Object();
		obj.TEXT = "";
		obj.CODE = "";
		result.addItemAt(obj,0);
		this.cbox_publishType.dataProvider = result;
		this.cbox_publishType.labelField = "TEXT";
		
		if (this._opKind != "ADD")
		{
			GetInfo();
		}
	}
	private function OnGetKindFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetKindResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetKindFault);
		mx.controls.Alert.okLabel = "确定";
		Alert.show("网络或系统错误，获取音乐分类失败"+evt.fault.faultString,"错误",Alert.OK);
	}
	private function GetInfo():void
	{
		_remote = new RemoteObject("fluorine");
		_remote.source = "ManagementService.FileT.Music";
		_remote.showBusyCursor =true;
		_remote.GetMusicInfoById(this._ids);
		_remote.addEventListener(ResultEvent.RESULT,OnGetInfoResult);
		_remote.addEventListener(FaultEvent.FAULT,OnGetInfoFault);
	}
	
	private function OnGetInfoResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetInfoResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetInfoFault);
		var result:ArrayCollection = _remote.GetMusicInfoById.lastResult as ArrayCollection;
		if (result!=null && result.source.length>0)
		{
			this.tbox_title.text = result.source[0].TITLE;
			this.tbox_title.enabled = false;
			this.tbox_player.text = result.source[0].SINGER;
			this.tbox_director.text = result.source[0].AUTHOR;
			this.tbox_issueDate.text = result.source[0].ISSUEDATE;
			this.tbox_brief.text = result.source[0].BRIEF;
			this.tbox_lang.text = result.source[0].LANG;
			this._defaultPic = result.source[0].DEFAULTPIC;
			this.__sequence=result.source[0].SEQUENCE;	
		    var pSource:ArrayCollection = this.cbox_publishType.dataProvider as ArrayCollection;
		    for(var i:int =0; i<pSource.source.length;i++)
		    {
		    	if (pSource.source[i].CODE == result.source[0].PUBLISHTYPE)
		    	{
		    		this.cbox_publishType.selectedIndex = i;
		    		break;
		    	}
		    }
		    //如果STATE不为-1不允许修改，如果为-1则需要选择是否重传文件
		    if (result.source[0].STATE == "-1")
		    {		    	
				this.height = 580;
				this.tbox_auditReason.visible = true;
				this.lb_auditResult.visible = true;
		    }
		    else
		    {
		    	this.height = 500;
				this.tbox_auditReason.visible = false;
				this.lb_auditResult.visible = false;
		    }
		}
	}
	private function OnGetInfoFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetInfoResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetInfoFault);
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，初始化数据失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	private function OnSave(evt:MouseEvent):void
	{
		if (this.tbox_title.text == "")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写音乐名称。","提示",Alert.OK);
			return;
		}
		if (this.tbox_player.text == "")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写主演。","提示",Alert.OK);
			return;
		}
		if (this.tbox_director.text == "")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写。","提示",Alert.OK);
			return;
		}
		if (this.tbox_brief.text == "")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写介绍。","提示",Alert.OK);
			return;
		}
		if (this.cbox_publishType.selectedItem.CODE == "")
		{
			Alert.okLabel = "确定";
			Alert.show("请选择影片类型。","提示",Alert.OK);
			return;
		}
		if (this._defaultPic == "")
		{
			Alert.okLabel = "确定";
			Alert.show("请点击图片按钮上传图片。","提示",Alert.OK);
			return;
		}
		if(this._opKind == "ADD")
		{
			if (this._fileSet == null || this._fileSet.length == 0)
			{
				Alert.okLabel = "确定";
				Alert.show("请点击文件列表按钮输入需要上载的文件。","提示",Alert.OK);
				return;
			}
		}
		Alert.yesLabel = "是";
		Alert.noLabel = "否";
		Alert.show("数据保存后不能修改，传输任务即将开启，请确定你的影片文件是否正确，信息是否完整？","提示",Alert.YES|Alert.NO,this,Save);
	}	
	private function Save(evt:CloseEvent):void
	{
		if (Alert.YES == evt.detail)
		{
			_remote = new RemoteObject("fluorine");
			_remote.source = "ManagementService.FileT.Music";
			_remote.showBusyCursor =true;
			if (this._opKind == "ADD")
			{
				_remote.AddMusic(parentApplication._userInfo.trueName,
								 parentApplication._userInfo.subCode,
								 this.tbox_title.text,
								 this.cbox_publishType.selectedItem.CODE,
								 this.cbox_publishType.selectedItem.TEXT,
								 this.tbox_issueDate.text,
								 this.tbox_brief.text,
								 this.tbox_player.text,
								 this.tbox_director.text,
								 this.tbox_lang.text,
								 this._defaultPic,
								 this.__sequence,
								 this._fileSet,
								 parentApplication._userInfo.subServerIp);
			}
			else
			{
				var isReUpload:Boolean = false;
				if (this.chbox_reUpload.selected)
				{
					isReUpload = true; 
				}
				_remote.UpdateMusic(this._ids,
								 this.tbox_title.text,
								 this.cbox_publishType.selectedItem.CODE,
								 this.tbox_issueDate.text,
								 this.tbox_brief.text,
								 this.tbox_player.text,
								 this.tbox_director.text,
								 this.tbox_lang.text,
								 this._defaultPic,
								 this.__sequence,
								 isReUpload,
								 this._fileSet);
			}
			_remote.addEventListener(ResultEvent.RESULT,OnSaveResult);
			_remote.addEventListener(FaultEvent.FAULT,OnSaveFault);
		}
	}
	private function OnSaveResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		var result:String = "";
		if (this._opKind == "ADD")
		{
			result = _remote.AddMusic.lastResult as String;
		}
		else
		{
			result = _remote.UpdateMusic.lastResult as String;
		}
		if (result!="")
		{
			if (result.substr(0,2).toUpperCase()=="OK")
			{
				if (this._opKind == "ADD")
				{
					this._ids = result.substr(2);
				}
				if (this._saveCallBack!=null)
				{
					this._saveCallBack.call(this,
											this._ids,
											this.tbox_title.text,
											parentApplication._userInfo.trueName,
											parentApplication._userInfo.subCode,
											parentApplication._userInfo.subName,
											this.tbox_player.text,
											this.tbox_director.text,
											this.tbox_lang.text,
											this.cbox_publishType.selectedItem.CODE,
											this.cbox_publishType.selectedItem.TEXT,
											this._opKind);
				}
				PopUpManager.removePopUp(this);
			}
			else
			{
				Alert.okLabel = "确定";
				Alert.show(result,"错误",Alert.OK);
			}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("系统错误，保存失败。","错误",Alert.OK);
		}
	}
	private function OnSaveFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		Alert.okLabel = "确定";
			Alert.show("系统错误，保存失败。"+evt.fault.faultString,"错误",Alert.OK);
	}
	
	private function OnBtnPicClick(evt:MouseEvent):void
	{
		var pop:DefaultPic = DefaultPic(PopUpManager.createPopUp(this,DefaultPic,true));
		pop.picFileName = this._defaultPic;
		pop.sequence = this.__sequence;
		pop.type = this._param;
		pop.closeCallBack = defaultPicCloseCallBack;
		pop.loadFile();
		PopUpManager.centerPopUp(pop);
	}
	private function defaultPicCloseCallBack(picFileName:String):void
	{
		this._defaultPic=picFileName;
	}
	private function OnFileList(evt:MouseEvent):void
	{
		var pop:FileSet = FileSet(PopUpManager.createPopUp(this,FileSet,true));
		pop.files = this._fileSet;
		pop.callBack = fileCallBack;
		PopUpManager.centerPopUp(pop);
	}
	private function fileCallBack(files:ArrayCollection):void
	{
		this._fileSet = files;	
	}
	private function OnClose(evt:CloseEvent):void
	{
		mx.managers.PopUpManager.removePopUp(this);
	}