	// ActionScript file
	import Custom.Common.ShowProgress;
	
	import flash.events.MouseEvent;
	import flash.net.FileReference;
	
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
		
	private var file:FileReference;
	private var progress:ShowProgress;
	private var __picFileName:String="";
	private var __sequence:String="";
	private var __type:String="";
	private var __kindId:String="";
	private var __closeCallBack:Function;
	
	private var _remote:RemoteObject;
	
	public function set picFileName(val:String):void
	{
		this.__picFileName=val;
	}
	public function set sequence(val:String):void
	{
		this.__sequence=val;	
	}
	public function set type(val:String):void
	{
		this.__type = val;
	}
	public function set kindId(val:String):void
	{
		this.__kindId=val;
	}
	public function set closeCallBack(fun:Function):void
	{
		this.__closeCallBack=fun;
	}
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSelectFile.addEventListener(MouseEvent.CLICK,OnSelectFile);
		this.btnUpload.addEventListener(MouseEvent.CLICK,OnUpLoad);
		this.btnBack.addEventListener(MouseEvent.CLICK,OnBack);
	}
	public function loadFile():void
	{
		this.img.source = "";
		if (this.__picFileName != "")
		{			
			//this.img.source=parentApplication._userInfo.picUploadUrl+this.__type+"/"+this.__sequence+"/"+this.__picFileName;
			if (parentApplication._isDebug)
			{
				this.img.source = "../uploadFile/" + this.__type + "/" + this.__sequence + "/" + this.__picFileName;
			}
			else
			{
				this.img.source = "uploadFile/" + this.__type + "/" + this.__sequence + "/" + this.__picFileName;
			}		
		}
	}
	private function OnSelectFile(evt:MouseEvent):void
	{
		file=new FileReference();
		file.addEventListener(Event.SELECT, OnFileSelected);
		file.addEventListener(Event.COMPLETE, OnFileComplete);
		file.addEventListener(ProgressEvent.PROGRESS, OnFileProgress);
		file.addEventListener(HTTPStatusEvent.HTTP_STATUS,OnFileHttpStatus);
		file.addEventListener(IOErrorEvent.IO_ERROR,OnFileIoError);
		file.addEventListener(SecurityErrorEvent.SECURITY_ERROR,OnFileSecurityError);
		file.addEventListener(Event.CANCEL,OnFileCancel);
		file.addEventListener(Event.ACTIVATE,OnFileActive);
		file.addEventListener(Event.DEACTIVATE,OnFileDeActive);
		file.addEventListener(Event.OPEN,OnFileOpen);
		var imageTypes:FileFilter = new FileFilter("Images (*.jpg, *.jpeg, *.gif, *.png)", "*.jpg; *.jpeg; *.gif; *.png");
		//var allTypes:Array = new Array(textTypes,imageTypes);
		var allTypes:Array = new Array(imageTypes);
		file.browse(allTypes);
	}
	private function OnFileSelected(evt:Event):void
	{
		if (file.size>=1024*1024)
		{
			file.cancel();
			Alert.okLabel = "确定";
			Alert.show("上传图片文件不能大于1M,如果文件超过1M,请先压缩处理文件","警告",Alert.OK);
			return;
		}
		this.tbox_file.text=file.name;
	}
	private function OnUpLoad(evt:MouseEvent):void
	{
		if (file==null || file.name=="")
		{
			Alert.okLabel = "确定";
			mx.controls.Alert.show("请选择你要上传的文件","系统提示",Alert.OK);
			return;
		}
		UploadFileStart();
	}
	private function OnBack(evt:MouseEvent):void
	{
		if (this.__closeCallBack!=null)
		{
			this.__closeCallBack.call(this,this.__picFileName);
		}
		PopUpManager.removePopUp(this);
	}
	private function OnClose(evt:CloseEvent):void
	{
		if (this.__closeCallBack!=null)
		{
			this.__closeCallBack.call(this,this.__picFileName);
		}
		PopUpManager.removePopUp(this);
	}
	
	////////////////////////////////////////////////////////////////////
	private function UploadFileStart():void
	{	
		progress=ShowProgress(PopUpManager.createPopUp(this,ShowProgress,true));
		progress.task="正在上传:"+file.name.toString();
		progress.summary="已上传:0%";
		PopUpManager.centerPopUp(progress);
		progress.addEventListener(Event.CANCEL,OnCancel);
				
		var para:URLVariables=new URLVariables();
		para.type=this.__type;
		para.word="?:P)(OL>,ki8";
		para.sequence=this.__sequence;
		if (this.__picFileName=="")
		{
			para.saveFileName=file.name;
		}
		else
		{
			para.saveFileName=this.__picFileName;
		}
		
		var request:URLRequest;
		if (parentApplication._isDebug)
		{
			request = new URLRequest("../UploadFile.aspx");
		}
		else
		{
			request = new URLRequest("UploadFile.aspx");
		}
		request.data=para;
		request.method=URLRequestMethod.POST;
		file.upload(request,"aaa",true);
	}
	private function OnFileComplete(evt:Event):void
	{
		trace("COMPLETE");
		PopUpManager.removePopUp(progress);	
		if (this.__picFileName=="")
		{
			if (this.__kindId!="")
			{
				saveFileToDb();
			}
			this.__picFileName=file.name;
		}
		file=null;
		this.tbox_file.text="";
		loadFile();
	}
	private function OnFileProgress(evt:ProgressEvent):void
	{
		progress.max=evt.bytesTotal;
		progress.min=0;
		progress.current=evt.bytesLoaded;
		progress.summary="文件大小:"+evt.bytesTotal.toString()+"k,已上传:"+evt.bytesLoaded.toString()+"k,"+Math.round(100*evt.bytesLoaded/evt.bytesTotal).toString()+"%";
		
	}
	private function OnFileHttpStatus(evt:HTTPStatusEvent):void
	{
		trace("HTTPSTATIUS:"+evt.status.toString());	
	}
	private function OnFileIoError(evt:IOErrorEvent):void
	{
		trace("IOERROR");
		//removeFileEvent();
		PopUpManager.removePopUp(progress);
		file=null;
		this.tbox_file.text="";
		Alert.okLabel = "确定";
		Alert.show("上传文件失败","错误",Alert.OK);
	}
	private function OnFileSecurityError(evt:SecurityErrorEvent):void
	{
		trace("SECURITYERROR");
		//removeFileEvent();
		PopUpManager.removePopUp(progress);
		file=null;
		this.tbox_file.text="";
		Alert.okLabel = "确定";
		Alert.show("上传文件失败","错误",Alert.OK);
	}
	private function OnFileCancel(evt:Event):void
	{
		trace("CANCEL");
	}
	private function OnFileActive(evt:Event):void
	{
		trace("ACTIVE");
	}
	private function OnFileDeActive(evt:Event):void
	{
		trace("DEACTIVE");
	}
	private function OnFileOpen(evt:Event):void
	{
		trace("OPEN");
	}
	private function OnCancel(evt:Event):void
	{
		file.cancel();
		PopUpManager.removePopUp(progress);
	}
	
	private function saveFileToDb():void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Link.DefaultLink";
		_remote.showBusyCursor=true;
		_remote.PicSave(this.__kindId,this.__picFileName);
		_remote.addEventListener(ResultEvent.RESULT,OnSaveToDbResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSaveToDbFault);			
	}
	private function OnSaveToDbResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveToDbResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveToDbFault);
		var result:String=_remote.PicSave.lastResult as String;
		if (result!=null)
		{
			if (result.toUpperCase()!="OK")
			{
				Alert.okLabel = "确定";
				Alert.show(result,"错误",Alert.OK);
			}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("保存图片文件至数据库失败。","系统提示",Alert.OK);
		}
	}
	private function OnSaveToDbFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveToDbResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveToDbFault);
		Alert.okLabel = "确定";
		Alert.show("保存图片文件至数据库失败。","系统提示",Alert.OK);
	}