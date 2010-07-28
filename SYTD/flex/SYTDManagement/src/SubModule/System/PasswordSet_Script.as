	// ActionScript file
 	import flash.events.MouseEvent;
 	
 	import mx.controls.Alert;
 	import mx.rpc.events.FaultEvent;
 	import mx.rpc.events.ResultEvent;
 	import mx.rpc.remoting.mxml.RemoteObject;
	
	private var _remote:RemoteObject=new RemoteObject("fluorine");
	private function init():void
	{
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
		Resize();
	    this.tbox_userName.text=parentApplication._userInfo.userName;
	}
	
	public function Resize():void
	{
		var parentWidth:int=parentApplication.width;
		var parentHeight:int=parentApplication.height;
		var parentContentWidth:int=parentWidth-parentApplication.vbox_menu.width-10;
		var parentContentHeight:int=parentHeight-94;
		
		this.controlBar_Title.width=parentContentWidth;
		this.HBox_Content.width=parentContentWidth;
		this.HBox_Content.height=parentContentHeight-this.controlBar_Title.height-8;
	}
	
	private function OnSave(evt:MouseEvent):void
	{
		if (this.tbox_password.text=="")
		{
			Alert.okLabel = "确定";
			Alert.show("请提供现在的密码。","提示",Alert.OK);
			this.tbox_password.text = "";
			this.tbox_newPassword.text = "";
			this.tbox_qPassword.text = "";
			return;
		}
		if (this.tbox_newPassword.text=="")
		{
			Alert.okLabel = "确定";
			Alert.show("新密码不能为空。","提示",Alert.OK);
			this.tbox_password.text = "";
			this.tbox_newPassword.text = "";
			this.tbox_qPassword.text = "";
			return;
		}
		if (this.tbox_newPassword.text!=this.tbox_qPassword.text)
		{
			Alert.okLabel = "确定";
			Alert.show("新密码与密码确认不一致。","提示",Alert.OK);
			this.tbox_password.text = "";
			this.tbox_newPassword.text = "";
			this.tbox_qPassword.text = "";
			return;
		}
		_remote.source="ManagementService.Security.Security";
		_remote.showBusyCursor=true;
		_remote.UpdatePassword(this.tbox_userName.text,
		                       this.tbox_password.text,
		                       this.tbox_newPassword.text);
		_remote.addEventListener(ResultEvent.RESULT,OnUpdateSuccess);
		_remote.addEventListener(FaultEvent.FAULT,OnUpdateFault);
	}
	private function OnUpdateSuccess(evt:ResultEvent):void
	{
		var result:String=_remote.UpdatePassword.lastResult as String;
		if (result.toUpperCase() == "OK")
		{
			Alert.okLabel = "确定";
			Alert.show("密码已修改，请牢记。","提示",Alert.OK);
			this.tbox_password.text = "";
			this.tbox_newPassword.text = "";
			this.tbox_qPassword.text = "";
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show(result,"错误",Alert.OK);
			this.tbox_password.text = "";
			this.tbox_newPassword.text = "";
			this.tbox_qPassword.text = "";
		}
		_remote.removeEventListener(ResultEvent.RESULT,OnUpdateSuccess);
		_remote.removeEventListener(FaultEvent.FAULT,OnUpdateFault);
	}
	private function OnUpdateFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnUpdateSuccess);
		_remote.removeEventListener(FaultEvent.FAULT,OnUpdateFault);
		this.tbox_password.text = "";
		this.tbox_newPassword.text = "";
		this.tbox_qPassword.text = "";
		Alert.okLabel = "确定";
		Alert.show("系统错误，密码修改失败。","错误",Alert.OK);
	}