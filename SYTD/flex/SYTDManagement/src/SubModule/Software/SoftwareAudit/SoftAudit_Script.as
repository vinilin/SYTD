	// ActionScript file
	import mx.controls.*;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
	
	public var _ids:String;
	private var _remote:RemoteObject;
public var _saveCallBack:Function;
	
	private function OnOK():void
	{
		if(this.combAudit.selectedItem == null)
		{
			Alert.show("请选择审核结果","错误",Alert.OK);
			return;	
		}
		if(this.combAudit.selectedItem.data == "-1")
		{
			AuditSoft(-1);
		}
		else
		{
			AuditSoft(1);
		}
	}
	private function OnPlay():void
	{
		var url:String ="http://";
		url+=parentApplication._userInfo.subServerIp;
		url+="/predownload.aspx?ID="+_ids;
		ExternalInterface.call('window.open',url,'_blank');
	}
	private function AuditSoft(rst:int):void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.FileT.Software";
		
		_remote.AuditSoft(_ids, 
							rst,
							this.resonText.text,
							parentApplication._userInfo.trueName);
		_remote.addEventListener(FaultEvent.FAULT,OnAuditFault)
		_remote.addEventListener(ResultEvent.RESULT,OnAuditResult)
	}
	
	private function OnAuditFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnAuditResult);	
		_remote.removeEventListener(FaultEvent.FAULT,OnAuditFault);
		Alert.show(evt.fault.faultString,"错误",Alert.OK);
	}
	private function OnAuditResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnAuditResult);	
		_remote.removeEventListener(FaultEvent.FAULT,OnAuditFault);
		var result:String = "";
		result = _remote.AuditSoft.lastResult as String;
		if(result != "OK")
		{
			Alert.show(result,"错误",Alert.OK);
			return;
		}
		else
		{
			Alert.show("操作成功","提示",Alert.OK);
			this._saveCallBack.call();
		}
		mx.managers.PopUpManager.removePopUp(this);
	}
	
	private function OnCancel():void
	{
		mx.managers.PopUpManager.removePopUp(this);
	}
	
	private function OnClose():void
	{
		mx.managers.PopUpManager.removePopUp(this);
	}
	private function OnChanged():void
	{
		if(this.combAudit.selectedItem.data == "-1")
		{
			this.resonText.enabled =true;
		}
		else
		{
			this.resonText.enabled = false;
			this.resonText.text = "";
		}
	}