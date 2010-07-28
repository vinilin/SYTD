package Custom.System
{
	import flash.display.*;
	import flash.events.*;
	import flash.net.*;
	import flash.text.TextField;
	import flash.text.TextFormat;
	
	import mx.events.FlexEvent;
	import mx.preloaders.DownloadProgressBar;
	
	public class Loading extends DownloadProgressBar
	{
		private var logo:Loader;
		private var txt:TextField;
		private var _preloader:Sprite;
		public function Loading()
		{
			logo = new Loader();
			logo.load(new URLRequest("Images/loading.png"));
			addChild(logo);
			
			var style:TextFormat = new TextFormat(null,null,0x000000,null,null,null,null,null,"center");
			txt = new TextField();
			txt.defaultTextFormat = style;
			txt.width = 200;
			txt.selectable = false;
			txt.height = 20;
			addChild(txt);
			
			super();
		}
		override public function set preloader(value:Sprite):void{
			_preloader = value
			_preloader.addEventListener(ProgressEvent.PROGRESS,load_progress);
			_preloader.addEventListener(Event.COMPLETE,load_complete);
			_preloader.addEventListener(FlexEvent.INIT_PROGRESS,init_progress);
			_preloader.addEventListener(FlexEvent.INIT_COMPLETE,init_complete);
			
			stage.addEventListener(Event.RESIZE,resize)
			resize(null);
		}
		private function remove():void{
			_preloader.removeEventListener(ProgressEvent.PROGRESS,load_progress);
			_preloader.removeEventListener(Event.COMPLETE,load_complete);
			_preloader.removeEventListener(FlexEvent.INIT_PROGRESS,init_progress);
			_preloader.removeEventListener(FlexEvent.INIT_COMPLETE,init_complete);
			stage.removeEventListener(Event.RESIZE,resize)
		}
		private function resize(e:Event):void
		{
			logo.x = (stage.stageWidth - 382)/2;
			logo.y = (stage.stageHeight - 66)/2;
			txt.x = (stage.stageWidth - 200)/2;
			txt.y = logo.y + 40+5;
			
			graphics.clear();
			graphics.beginFill(0xFFFFFF);
			graphics.drawRect(0,0,stage.stageWidth,stage.stageHeight);
			graphics.endFill();
		}
		private function load_progress(e:ProgressEvent):void
		{
			txt.text = "正在加载..."+int(e.bytesLoaded/e.bytesTotal*100)+"%";
		}
		private function load_complete(e:Event):void
		{
			txt.text = "加载完毕!"
		}
		private function init_progress(e:FlexEvent):void
		{
			txt.text = "正在初始化..."
		}
		private function init_complete(e:FlexEvent):void
		{
			txt.text = "初始化完毕!"
			remove()
			dispatchEvent(new Event(Event.COMPLETE))
		}
	}
}