package Custom.Common
{
	import mx.formatters.DateFormatter;
	import mx.rpc.remoting.mxml.RemoteObject;
	public class Com
	{
		private var _remote:RemoteObject;
		public function createSeq(type:String,sequence:String):String
		{
			var seq:String="";
			if (sequence=="")
			{
				var formatter:DateFormatter=new DateFormatter();
				formatter.formatString="YYYYMMDDJJNNSS";
				var d:Date=new Date();
				seq=formatter.format(d);
				seq += randRange(100,999).toString();				
			}
			else
			{
				seq=sequence;
			}
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Com.Com";
			_remote.CreateDir(type,seq);
			return seq;
		}
		
		public function deleteSeqDir(type:String,Sequence:String):void
		{
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Com.Com";
			_remote.DeleteDir(type,Sequence);
		}
		
		public static function randRange(min:Number, max:Number):Number 
		{
    		var randomNum:Number = Math.floor(Math.random() * (max - min + 1)) + min;
    		return randomNum;
		}
		
		public static function padLeft(str:String,len:int,char:String):String
		{
			var strLen:int=str.length;
			for(var i:int=0;i<len-strLen;i++)
			{
				str = char + str;	
			}
			return str;
		}
	}
}