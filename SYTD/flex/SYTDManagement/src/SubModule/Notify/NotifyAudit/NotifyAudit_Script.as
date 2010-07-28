	// ActionScript file
	import Custom.Common.Com;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.events.DropdownEvent;
	import mx.events.MoveEvent;
	import mx.formatters.DateFormatter;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;

	private var __opKind:String="AUDIT";
	//private var __articleKind:String="";
	private var _param:String = "";
	private var _subCode:String = "";
	private var _subName:String = "";
	private var __notifyId:String = "";
	private var __saveCallBack:Function;
	private var __sequence:String = ""; 
	private var __defaultPic:String = "";
	private var _remote:RemoteObject;
	private var _remote1:RemoteObject;
	
	public function set opKind(val:String):void
	{
		this.__opKind=val;
	}
	public function set param(val:String):void
	{
		this._param = val;
	}
	public function set subCode(val:String):void
	{
		this._subCode = val;
	}
	public function set subName(val:String):void
	{
		this._subName = val;
	}
	public function set notifyId(val:String):void
	{
		this.__notifyId=val;
	}
	public function set saveCallBack(fun:Function):void
	{
		this.__saveCallBack=fun;
	}
	
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnPub.addEventListener(MouseEvent.CLICK,OnPub);
		this.btnNotPub.addEventListener(MouseEvent.CLICK,OnNotPub);
		this.addEventListener(MouseEvent.CLICK,OnClick);
		this.addEventListener(MoveEvent.MOVE,OnMove);
		this.cbox_start_hour.addEventListener(DropdownEvent.OPEN,OnDropOpen);
		this.cbox_start_hour.addEventListener(DropdownEvent.CLOSE,OnDropClose);
		this.cbox_start_minute.addEventListener(DropdownEvent.OPEN,OnDropOpen);
		this.cbox_start_minute.addEventListener(DropdownEvent.CLOSE,OnDropClose);
		this.cbox_end_hour.addEventListener(DropdownEvent.OPEN,OnDropOpen);
		this.cbox_end_hour.addEventListener(DropdownEvent.CLOSE,OnDropClose);
		this.cbox_end_minute.addEventListener(DropdownEvent.OPEN,OnDropOpen);
		this.cbox_end_minute.addEventListener(DropdownEvent.CLOSE,OnDropClose);
		this.df_start_day.addEventListener(DropdownEvent.OPEN,OnDropOpen);	
		this.df_start_day.addEventListener(DropdownEvent.CLOSE,OnDropClose);	
		this.df_end_day.addEventListener(DropdownEvent.OPEN,OnDropOpen);
		this.df_end_day.addEventListener(DropdownEvent.CLOSE,OnDropClose);
	}
	private function OnDropOpen(evt:DropdownEvent):void
	{		
		this.iFrame.hidden();	
	}
	private function OnDropClose(evt:DropdownEvent):void
	{
		this.iFrame.show();
	}
	public function initData():void
	{		
		var obj:Object;
		var HourSource:ArrayCollection = new ArrayCollection();
		for(var i:int=0;i<=23;i++)
		{
			var h:String = i.toString();
			if (h.length == 1)
			{
				h = "0" + h;
			}
			obj = new Object();
			obj.CODE = h;
			obj.TEXT = h;
			HourSource.addItem(obj);
		}
		this.cbox_start_hour.dataProvider = HourSource;
		this.cbox_start_hour.labelField = "TEXT";
		this.cbox_end_hour.dataProvider = HourSource;
		this.cbox_end_hour.labelField = "TEXT";
		
		var MinuteSource:ArrayCollection = new ArrayCollection();
		for(var j:int=0;j<=59;j++)
		{
			var m:String = j.toString();
			if (m.length == 1)
			{
				m = "0" + m;
			}
			obj = new Object();
			obj.CODE = m;
			obj.TEXT = m;
			MinuteSource.addItem(obj);
		}
		this.cbox_start_minute.dataProvider = MinuteSource;
		this.cbox_start_minute.labelField = "TEXT";
		this.cbox_end_minute.dataProvider = MinuteSource;
		this.cbox_end_minute.labelField = "TEXT";
				
		this.title = "审核通知公告";
	
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Notify.Notify";			
		_remote.showBusyCursor=true;
		_remote.GetNotifyInfo(this.__notifyId);
		_remote.addEventListener(ResultEvent.RESULT,OnGetNotifyResult);
		_remote.addEventListener(FaultEvent.FAULT,OnGetNotifyFault);
		
	}
	private function OnGetNotifyResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetNotifyResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetNotifyFault);
		var result:ArrayCollection=_remote.GetNotifyInfo.lastResult as ArrayCollection;
		if (result!=null && result.length>0)
		{
			this.tbox_title.text=result.source[0].NOTIFYTITLE;
			this.__defaultPic=result.source[0].DEFAULTPIC;
			this.__sequence=result.source[0].SEQUENCE;	
			var co:Com=new Com();
			this.__sequence=co.createSeq(this._param,result.source[0].SEQUENCE);
			//this.iFrame.source=this._param+"/"+this.__sequence;
			this.iFrame.type = this._param;
			this.iFrame.source = this.__sequence;
			this.iFrame.show();
			this.iFrame.value=result.source[0].NOTIFYCONTENT;
			
			var formatter:DateFormatter = new DateFormatter();
			formatter.formatString = "YYYY-MM-DD";
			this.df_start_day.text = formatter.format(result.source[0].PUBSTARTTIME);
			this.df_end_day.text = formatter.format(result.source[0].PUBENDTIME);
			
			var formatter1:DateFormatter = new DateFormatter();
			formatter1.formatString = "JJ";
			var pubStart_hour:String = formatter1.format(result.source[0].PUBSTARTTIME);
			var pubEnd_hour:String = formatter1.format(result.source[0].PUBENDTIME);
			
			var formatter2:DateFormatter = new DateFormatter();
			formatter2.formatString = "NN"
			var pubStart_minute:String = formatter2.format(result.source[0].PUBSTARTTIME);
			var pubEnd_minute:String = formatter2.format(result.source[0].PUBENDTIME);
			if (pubStart_hour.length == 1){ pubStart_hour = "0" + pubStart_hour; }
			if (pubEnd_hour.length == 1){ pubEnd_hour = "0"+pubEnd_hour; }
			if (pubStart_minute.length == 1){ pubStart_minute = "0" + pubStart_minute; }
			if (pubEnd_minute.length == 1){ pubEnd_minute = "0" + pubEnd_minute; }
			var i:int = 0;
			
			var start_hour_source:ArrayCollection = this.cbox_start_hour.dataProvider as ArrayCollection;
			for(i=0;i<start_hour_source.length;i++)
			{
				if (start_hour_source[i].CODE == pubStart_hour)
				{
					this.cbox_start_hour.selectedIndex = i;
					break;
				}
			}
			var end_hour_source:ArrayCollection = this.cbox_end_hour.dataProvider as ArrayCollection;
			for(i=0;i<end_hour_source.length;i++)
			{
				if (end_hour_source[i].CODE == pubEnd_hour)
				{
					this.cbox_end_hour.selectedIndex = i;
					break;
				}
			}
			var start_minute_source:ArrayCollection = this.cbox_start_minute.dataProvider as ArrayCollection;
			for(i=0;i<start_minute_source.length;i++)
			{
				if (start_minute_source[i].CODE == pubStart_minute)
				{
					this.cbox_start_minute.selectedIndex = i;
					break;
				}
			}
			var end_minute_source:ArrayCollection = this.cbox_end_minute.dataProvider as ArrayCollection;
			for(i=0;i<end_minute_source.length;i++)
			{
				if (end_minute_source[i].CODE == pubEnd_minute)
				{
					this.cbox_end_minute.selectedIndex = i;
					break;
				}
			}
		}
	}
	private function OnGetNotifyFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetNotifyResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetNotifyFault);
		this.iFrame.hidden();
		Alert.okLabel = "确定";
		Alert.show("初始化数据失败，请退出后重新进入。"+evt.fault.faultString,"系统提示",Alert.OK,this,AlertCloseHandle);
	}
	
	//发布
	private function OnPub(evt:MouseEvent):void
	{
		if (this.tbox_title.text=="")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请填写标题。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.tbox_title.setFocus();
			return;
		}
		if (this.df_start_day.text == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请选择发布开始日期。","系统提示",Alert.OK,this,AlertCloseHandle);
			return;
		}
		if (this.df_end_day.text == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请选择发布结束日期。","系统提示",Alert.OK,this,AlertCloseHandle);
			return;
		}
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Notify.Notify";
		_remote.showBusyCursor=true;
		var pubStartTime:String = this.df_start_day.text + " "+this.cbox_start_hour.text+":"+this.cbox_start_minute.text + ":00";
		var pubEndTime:String = this.df_end_day.text + " "+this.cbox_end_hour.text + ":" + this.cbox_end_minute.text + ":00";
		
		_remote.PubNotify(this.__notifyId,
						   this._subCode,
						   this.tbox_title.text,
						   this.iFrame.value,
						   pubStartTime,
						   pubEndTime,
						   parentApplication._userInfo.trueName,
						   this.__sequence);
						   
		_remote.addEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.addEventListener(FaultEvent.FAULT,OnPubFault);
		//Alert.show(notifyContent);
	}
	private function OnPubResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
		var result:String = _remote.PubNotify.lastResult as String;
		
		if (result!=null)
		{
			if (result.substr(0,2).toUpperCase()=="OK")
			{				
					var pubStartTime:String = this.df_start_day.text + " "+this.cbox_start_hour.text+":"+this.cbox_start_minute.text + ":00";
					var pubEndTime:String = this.df_end_day.text + " "+this.cbox_end_hour.text + ":" + this.cbox_end_minute.text + ":00";
		
					this.__saveCallBack.call(this,
											 this.__notifyId,
											 this._subCode,
											 this._subName,
											 this.tbox_title.text,
											 "1",
											 "已发布",
											 pubStartTime,
											 pubEndTime,
											 parentApplication._userInfo.trueName,
											 this.__sequence);
			
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
	private function OnPubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnPubFault);
		this.iFrame.hidden();
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，保存失败。"+evt.fault.faultString,"系统提示",Alert.OK,this,AlertCloseHandle);
	}
	
	//不发布
	private function OnNotPub(evt:MouseEvent):void
	{
		if (this.tbox_title.text=="")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请填写标题。","系统提示",Alert.OK,this,AlertCloseHandle);
			this.tbox_title.setFocus();
			return;
		}
		if (this.df_start_day.text == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请选择发布开始日期。","系统提示",Alert.OK,this,AlertCloseHandle);
			return;
		}
		if (this.df_end_day.text == "")
		{
			this.iFrame.hidden();
			Alert.okLabel = "确定";
			Alert.show("请选择发布结束日期。","系统提示",Alert.OK,this,AlertCloseHandle);
			return;
		}
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Notify.Notify";
		_remote.showBusyCursor=true;
		var pubStartTime:String = this.df_start_day.text + " "+this.cbox_start_hour.text+":"+this.cbox_start_minute.text + ":00";
		var pubEndTime:String = this.df_end_day.text + " "+this.cbox_end_hour.text + ":" + this.cbox_end_minute.text + ":00";
		
		_remote.NotPubNotify(this.__notifyId,
						   this._subCode,
						   this.tbox_title.text,
						   this.iFrame.value,
						   pubStartTime,
						   pubEndTime,
						   parentApplication._userInfo.trueName,
						   this.__sequence);
	
		_remote.addEventListener(ResultEvent.RESULT,OnNotPubResult);
		_remote.addEventListener(FaultEvent.FAULT,OnNotPubFault);
		//Alert.show(notifyContent);
	}
	private function OnNotPubResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnNotPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnNotPubFault);
		var result:String = _remote.NotPubNotify.lastResult as String;
		
		if (result!=null)
		{
			if (result.substr(0,2).toUpperCase()=="OK")
			{				
					var pubStartTime:String = this.df_start_day.text + " "+this.cbox_start_hour.text+":"+this.cbox_start_minute.text + ":00";
					var pubEndTime:String = this.df_end_day.text + " "+this.cbox_end_hour.text + ":" + this.cbox_end_minute.text + ":00";
		
					this.__saveCallBack.call(this,
											 this.__notifyId,
											 this._subCode,
											 this._subName,
											 this.tbox_title.text,
											 "-1",
											 "未发布",
											 pubStartTime,
											 pubEndTime,
											 parentApplication._userInfo.trueName,
											 this.__sequence);
				
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
	private function OnNotPubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnNotPubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnNotPubFault);
		this.iFrame.hidden();
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，保存失败。"+evt.fault.faultString,"系统提示",Alert.OK,this,AlertCloseHandle);
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
		