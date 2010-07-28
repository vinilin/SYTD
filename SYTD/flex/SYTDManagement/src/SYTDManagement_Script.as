// ActionScript file
	import Custom.Common.Com;
	import Custom.System.UserInfo;
	
	import SubModule.Article.Article.ArticleList;
	import SubModule.Article.ArticleAudit.ArticleAuditList;
	import SubModule.Article_Kqfw.Article.KqfwArticleList;
	import SubModule.Article_Kqfw.ArticleAudit.KqfwArticleAuditList;
	import SubModule.DefaultLink.Link.LinkList;
	import SubModule.DefaultLink.LinkAudit.LinkAuditList;
	import SubModule.Notify.Notify.NotifyList;
	import SubModule.Notify.NotifyAudit.NotifyAuditList;
	import SubModule.System.FzLmManageUser.FzLmUserList;
	import SubModule.System.KQFWFZManageUser.KqfwFzUserList;
	import SubModule.System.KQFWLMManageUser.KqfwLmUserList;
	import SubModule.System.KQFWZLMManageUser.KqfwZLmUserList;
	import SubModule.System.KqfwKind.KqfwKindList;
	import SubModule.System.KqfwSHManageUser.KqfwSHUserList;
	import SubModule.System.PasswordSet;
	import SubModule.System.PublishType.PublishTypeList;
	import SubModule.System.QjLmManageUser.QjLmUserList;
	import SubModule.System.SubIpDivision.SubIpList;
	import SubModule.System.SubManageUser.UserList;
	import SubModule.System.SubSection.SubList;
	import SubModule.System.SystemKind1.KindList;
	import SubModule.System.SystemLog.LogList;
	import SubModule.System.SystemManageUser.SystemUserList;
	import SubModule.VidioNews.VidioNews.NewsList;
	import SubModule.VidioNews.VidioNewsAudit.NewsAuditList;
	
	import mx.collections.ArrayCollection;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.events.ListEvent;
	import mx.events.ModuleEvent;
	import mx.events.ResizeEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
	
	public var _isDebug:Boolean=false;
	public var _userInfo:UserInfo;
	public var _userPopedom:ArrayCollection=new ArrayCollection();
	private var login:Login;
	private var _timer:Timer;
	private var _LoginState:Boolean=false;
	
	private var _remote:RemoteObject;	
	private var _remote1:RemoteObject;
	
	private var _ModuleClass:String = "";
	private var _ModuleCode:String = "";
	private var _ModulePara:String = ""; 
			
	private function init():void
	{
		controls.visible=false;
	    this.img_top_1.visible=false;
		this.img_top_2.visible=false;
		this.vbox_middle.visible=false;
		if (_userInfo==null)
		{
			login=Login(PopUpManager.createPopUp(this,Login,true));
			PopUpManager.centerPopUp(login);
		}
		layoutDefine();	
		this.addEventListener(ResizeEvent.RESIZE,OnResize);
		this.lbLogout.addEventListener(MouseEvent.CLICK,OnLogout);
		_timer=new Timer(1000,0);
		_timer.addEventListener(TimerEvent.TIMER,OnTick);
	}
	private function OnTick(evt:TimerEvent):void
	{		
		var d:Date=new Date();
		lbWelCome.text=d.fullYear.toString()+ "年"+Custom.Common.Com.padLeft((d.getMonth()+1).toString(),2,"0")+"月"+Custom.Common.Com.padLeft((d.getDate()).toString(),2,"0")+"日 "+ Custom.Common.Com.padLeft(d.getHours().toString(),2,"0")+":"+Custom.Common.Com.padLeft(d.getMinutes().toString(),2,"0")+":"+Custom.Common.Com.padLeft(d.getSeconds().toString(),2,"0")+ "   欢迎你【"+_userInfo.trueName+"】"
	}
	public function LoginCallBack():void
	{
		_timer.start();
		controls.visible=true;
		
		this.img_top_1.visible=true;
		this.img_top_2.visible=true;
		this.vbox_middle.visible=true;
		createMenu();
	}
	
	private function createMenu():void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Security.Security";
   		_remote.showBusyCursor=true;   				
		_remote.GetUserMenu(this._userInfo.userName);
		_remote.addEventListener(ResultEvent.RESULT,OnGetMenuSuccess);
		_remote.addEventListener(FaultEvent.FAULT,OnGetMenuFault);
	}
	private function OnGetMenuSuccess(evt:ResultEvent):void
	{
	    var menuList:XML=XML(_remote.GetUserMenu.lastResult) ;
		tree_menu.labelField="@text";
		tree_menu.dataProvider=menuList.children(); 
		tree_menu.addEventListener(ListEvent.ITEM_CLICK,OnTreeItemCleckHandler);
		tree_menu.showRoot=true;
		vbox_content.addEventListener(ResizeEvent.RESIZE,OnContentResize);		
		_remote.removeEventListener(ResultEvent.RESULT,OnGetMenuSuccess);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetMenuFault);
	}	
	private function OnGetMenuFault(evt:FaultEvent):void
	{		
		_remote.removeEventListener(ResultEvent.RESULT,OnGetMenuSuccess);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetMenuFault);
		Alert.okLabel = "确定";
		Alert.show("菜单创建失败，请重新登录。"+evt.fault.faultString,"错误",Alert.OK);
		//Logout();		
	}
	
	private function OnTreeItemCleckHandler(evt:ListEvent):void
	{
		var item:Object = Tree(evt.currentTarget).selectedItem;
		if (tree_menu.dataDescriptor.isBranch(item)) 
        { 
        	tree_menu.expandItem(item, !tree_menu.isItemOpen(item), true); 
        }
        else
        {
        	/*
        	if(this._ModuleClass == "SubModule.Progress.ProgressList" )
        	{
        		if(this.MLoader.child != null)
        		{
		        	var pr:ProgressList = this.MLoader.child as ProgressList;
		        	pr.Stop();
	        	}
	        }
	        */
        	this.MLoader.unloadModule();
        	this.MLoader.url=item.@url;
        	this.MLoader.loadModule();         	       	
        	this._ModuleClass = item.@classPath;
        	this._ModuleCode = item.@code;
        	this._ModulePara = item.@param;
        	this.MLoader.addEventListener(ModuleEvent.READY,OnMLoaderUpdateComplete);
        }
	}
	
	private function OnMLoaderUpdateComplete(evt:ModuleEvent):void
	{
	}
	public function ModuleInitCompleteCall():void
	{
		switch(this._ModuleClass)
        	{
        		case "SubModule.System.PublishType.PublishTypeList":
        			var _category:String = "";
        			switch(this._ModulePara)
        			{
        				case "011":
        					_category = "1";
        					break;
        				case "012":
        					_category = "2";
        					break;
        				case "013":
        					_category = "3";
        					break;
        				case "014":
        					_category = "4";
        					break;
        				default:
        				 	_category = "1";
        				 	break;
        		 	}
        		 	(MLoader.child as SubModule.System.PublishType.PublishTypeList).category = _category;
        		    break;
        		
        		case "SubModule.Article.Article.ArticleList":
        		    (MLoader.child as SubModule.Article.Article.ArticleList).param = this._ModulePara;
        		    break;
        		    
        		case "SubModule.Article.ArticleAudit.ArticleAuditList":
        		    (MLoader.child as SubModule.Article.ArticleAudit.ArticleAuditList).param = this._ModulePara;
        		    break;
        		            
        		case "SubModule.System.SystemKind1.KindList":
        		    (MLoader.child as SubModule.System.SystemKind1.KindList).param = this._ModulePara;
        		     break;
        		
        		case "SubModule.Notify.Notify.NotifyList":
        			(MLoader.child as SubModule.Notify.Notify.NotifyList).param = this._ModulePara;
        			break;
        		case "SubModule.VidioNews.VidioNews.NewsList":
        		    (MLoader.child as SubModule.VidioNews.VidioNews.NewsList).param = this._ModulePara;
        		    break;
        		case "SubModule.VidioNews.VidioNewsAudit.NewsAuditList":
        		    (MLoader.child as SubModule.VidioNews.VidioNewsAudit.NewsAuditList).param = this._ModulePara;
        		    break;
        		default:
        			break;
        	}
	}
	 
	private function OnContentResize(evt:ResizeEvent):void
	{
		layoutDefine();
		MLoaderResize();
	}
	
	private function layoutDefine():void
	{
		vbox_middle.height=this.height-94;
		vbox_middle.width=this.width;
		HDBox.width=vbox_middle.width;
		HDBox.height=vbox_middle.height;
		
		
		vbox_menu.height=vbox_middle.height+1;
		vbox_content.height=vbox_middle.height;
		vbox_content.width=HDBox.width-vbox_menu.width-10;
		
		tree_menu.height=vbox_menu.height-33;
		
		MLoader.width=vbox_content.width;
		MLoader.height=vbox_content.height;
	}
	
	private function OnResize(evt:ResizeEvent):void
	{
		layoutDefine();
		MLoaderResize();
		if (login!=null)
		{
			PopUpManager.centerPopUp(login);
		}
	}
	
	private function MLoaderResize():void
	{
		switch(this._ModuleClass)
		{	
			case "SubModule.System.PublishType.PublishTypeList":
				(this.MLoader.child as SubModule.System.PublishType.PublishTypeList).Resize();
				break;
				
			case "SubModule.Article.Article.ArticleList":
			    (this.MLoader.child as SubModule.Article.Article.ArticleList).Resize();
			    break;
			    
			case "SubModule.Article.ArticleAudit.ArticleAuditList":
			    (this.MLoader.child as SubModule.Article.ArticleAudit.ArticleAuditList).Resize();
			    break;
			    
			case "SubModule.Article_Kqfw.Article.KqfwArticleList":
			    (this.MLoader.child as SubModule.Article_Kqfw.Article.KqfwArticleList).Resize();
			    break;
			    
			case "SubModule.Article_Kqfw.ArticleAudit.KqfwArticleAuditList":
			    (this.MLoader.child as SubModule.Article_Kqfw.ArticleAudit.KqfwArticleAuditList).Resize();
			    break;
			    
			case "SubModule.DefaultLink.Link.LinkList":
				(this.MLoader.child as SubModule.DefaultLink.Link.LinkList).Resize();
				break;
				
			case "SubModule.DefaultLink.LinkAudit.LinkAuditList":
				(this.MLoader.child as SubModule.DefaultLink.LinkAudit.LinkAuditList).Resize();
				break;	   
				 	
			case "SubModule.System.SubSection.SubList":
			    (this.MLoader.child as SubModule.System.SubSection.SubList).Resize();
			    break;
			    
			case "SubModule.System.SubIpDivision.SubIpList":
			    (this.MLoader.child as SubModule.System.SubIpDivision.SubIpList).Resize();
			    break;   
			     
		    case "SubModule.Notify.Notify.NotifyList":
    			(MLoader.child as SubModule.Notify.Notify.NotifyList).Resize();
    			break;
    			
    		 case "SubModule.Notify.NotifyAudit.NotifyAuditList":
    			(MLoader.child as SubModule.Notify.NotifyAudit.NotifyAuditList).Resize();
    			break;
    		
    		case "SubModule.System.SystemKind1.KindList":
    			(MLoader.child as SubModule.System.SystemKind1.KindList).Resize();
    			break;
    			
    		case "SubModule.System.SystemManageUser.SystemUserList":
    			(MLoader.child as SubModule.System.SystemManageUser.SystemUserList).Resize();
    			break;	
    			
    		case "SubModule.System.SubManageUser.UserList":
    			(MLoader.child as SubModule.System.SubManageUser.UserList).Resize();
    			break;	
    			
    		case "SubModule.System.QjLmManageUser.QjLmUserList":
    			(MLoader.child as SubModule.System.QjLmManageUser.QjLmUserList).Resize();
    			break;	
    			
    		case "SubModule.System.FzLmManageUser.FzLmUserList":
    			(MLoader.child as SubModule.System.FzLmManageUser.FzLmUserList).Resize();
    			break;	
    			
    		case "SubModule.System.KQFWLMManageUser.KqfwLmUserList":
    			(MLoader.child as SubModule.System.KQFWLMManageUser.KqfwLmUserList).Resize();
    			break;	
    			
    		case "SubModule.System.KQFWFZManageUser.KqfwFzUserList":
    			(MLoader.child as SubModule.System.KQFWFZManageUser.KqfwFzUserList).Resize();
    			break;	
    			
    		case "SubModule.System.KqfwSHManageUser.KqfwSHUserList":
    			(MLoader.child as SubModule.System.KqfwSHManageUser.KqfwSHUserList).Resize();
    			break;	
    			
    		case "SubModule.System.KQFWZLMManageUser.KqfwZLmUserList":
    			(MLoader.child as SubModule.System.KQFWZLMManageUser.KqfwZLmUserList).Resize();
    			break;	
    			
    		case "SubModule.System.SystemLog.LogList":
    			(MLoader.child as SubModule.System.SystemLog.LogList).Resize();
    			break;	
    			
    		case "SubModule.System.PasswordSet":
    			(MLoader.child as SubModule.System.PasswordSet).Resize();
    			break;	
    			
    		case "SubModule.System.KqfwKind.KqfwKindList":
    			(MLoader.child as SubModule.System.KqfwKind.KqfwKindList).Resize();	
    			break;
    			
    		case "SubModule.Article_Kqfw.Article.KqfwArticleList":
    			(MLoader.child as SubModule.Article_Kqfw.Article.KqfwArticleList).Resize();
    			break;
    			
    		case "SubModule.Article_Kqfw.ArticleAudit.KqfwArticleAuditList":
    			(MLoader.child as SubModule.Article_Kqfw.ArticleAudit.KqfwArticleAuditList).Resize();
    			break;
    			
			default:
				break;
		}
	}
	
	private function OnLogout(evt:MouseEvent):void
	{
		Alert.okLabel = "是";
		Alert.noLabel = "否";
		Alert.show("确定要注销？","系统提示",Alert.OK|Alert.NO,this,Logout_OK);
	}
	private function Logout_OK(evt:CloseEvent):void
	{
		if (Alert.OK == evt.detail)
		{
			Logout();
		}
	}
	private function Logout():void
	{
		this._timer.stop();
		this._userInfo=null;
		this._LoginState=false;
		init();
	}
	
	