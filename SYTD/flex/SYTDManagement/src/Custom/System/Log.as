package Custom.System
{
	import mx.rpc.remoting.mxml.RemoteObject;
	
	public class Log
	{
		public function Log()
		{			
		}
		public static function WriteLog(module:String,logType:String,opDetail:String,opUser:String,dataId:String=null):void
		{
			var _remote:RemoteObject=new RemoteObject("fluorine");
			_remote.source="ManagementService.Com.Log";
			if (dataId!=null && dataId!="")
			{
				opDetail += "	数据ID"+dataId;
			}
			_remote.writeLog(module,logType,opDetail,opUser);
		}
	}
}