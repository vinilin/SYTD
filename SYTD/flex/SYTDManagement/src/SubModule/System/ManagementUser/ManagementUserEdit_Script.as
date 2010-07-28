	// ActionScript file
	import SubModule.System.ManagementUser.UserSelect;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.controls.Alert;
	import mx.events.CloseEvent;
	import mx.events.ListEvent;
	import mx.managers.PopUpManager;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;

	private var __opKind:String="ADD";
	private var __userId:String="";
	private var __userName:String="";
	private var __saveCallBack:Function;
	private var _remote:RemoteObject;
	private var _divisions:String="";
	private var _popedoms:ArrayCollection=new ArrayCollection();
	private var _funcCodes:ArrayCollection=new ArrayCollection();
	private var _param:String = "";
	private var _ModuleCode:String = "";
	[Bindable]private var _limitSource:XML=new XML();
	private function init():void
	{
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
		this.btnUserSelect.addEventListener(MouseEvent.CLICK,OnUserSelect);
	}
	public function set funcCodes(val:ArrayCollection):void
	{
		this._funcCodes = val;
	}
	public function set param(val:String):void
	{
		this._param = val;
	}
	public function set ModuleCode(val:String):void
	{
		this._ModuleCode = val;
	}
	public function set opKind(val:String):void
	{
		this.__opKind = val;
		if (val=="ADD")
		{
			this.title="增加用户";
			this.btnUserSelect.visible=true;
		}
		else
		{
			this.title="修改用户";
			this.btnUserSelect.visible=false;
			this.tbox_password.width=this.tbox_password.width-80;
			this.lb_passwordWarring.text="不修改则不填写";
			this.lb_passwordWarring.x=this.lb_passwordWarring.x-80;
		}
	}
	public function set userId(val:String):void
	{
		this.__userId=val;
	}
	public function set saveCallBack(fun:Function):void
	{
		this.__saveCallBack=fun;
	}
	public function initData():void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Sys.SubSection";			
		_remote.showBusyCursor=true;
		_remote.GetAllSubList();
		_remote.addEventListener(ResultEvent.RESULT,OnGetSubResult);
		_remote.addEventListener(FaultEvent.FAULT,OnGetSubFault);
	}
	private function OnGetSubResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetSubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetSubFault);
		var result:ArrayCollection=_remote.GetAllSubList.lastResult as ArrayCollection;
		this.cbox_subList.dataProvider=result;
		this.cbox_subList.labelField="SUBNAME";
		initUserInfo();
	}
	private function OnGetSubFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetSubResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetSubFault);
		Alert.okLabel = "确定";
		Alert.show("加载分站数据失败"+evt.fault.faultString,"错误",Alert.OK);
		mx.managers.PopUpManager.removePopUp(this);
	}
	private function initUserInfo():void
	{
		if (this.__opKind=="ADD")
		{
			initPopedom();
		}
		else if(this.__opKind=="UPDATE" || this.__opKind=="SELECT")
		{
			_remote=new RemoteObject("fluorine");
			_remote.source="ManagementService.Security.User";			
			_remote.showBusyCursor=true;
			_remote.GetUserInfoById(this.__userId);
			_remote.addEventListener(ResultEvent.RESULT,OnGetUserInfoResult);
			_remote.addEventListener(FaultEvent.FAULT,OnGetUserInfoFault);
		}
	}
	private function OnGetUserInfoResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetUserInfoResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetUserInfoFault);
		var result:ArrayCollection=_remote.GetUserInfoById.lastResult as ArrayCollection;
		if (result!=null && result.length>0)
		{
			this.tbox_userName.text=result.source[0].USERNAME;
			this.tbox_trueName.text=result.source[0].TRUENAME;
			this.tbox_remark.text=result.source[0].REMARK;
			this.__userName=result.source[0].USERNAME;
			var sub:ArrayCollection=this.cbox_subList.dataProvider as ArrayCollection;
			for(var i:int=0;i<sub.length;i++)
			{
				if (sub.source[i].SUBCODE==result.source[0].SUBCODE)
				{
					this.cbox_subList.selectedIndex=i;
					break;
				}
			}
			initPopedom();
		}
	}
	private function OnGetUserInfoFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetUserInfoResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetUserInfoFault);
		Alert.okLabel = "确定";
		Alert.show("初始化数据失败，请退出后重新进入。"+evt.fault.faultString,"系统提示",Alert.OK,this);
	}
	private function initPopedom():void
	{
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Security.ManagementUser";
		_remote.showBusyCursor=true;
		_remote.GetUserPopedomXML(this.__userName,this._param,this._ModuleCode);
		_remote.addEventListener(ResultEvent.RESULT,OnGetUserPopedomResult);
		_remote.addEventListener(FaultEvent.FAULT,OnGetUserPopedomFault);
	}
	private function OnGetUserPopedomResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetUserPopedomResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetUserPopedomFault);
		_limitSource=XML(_remote.GetUserPopedomXML.lastResult);
		this.tree_limit.callLater(expandTree);
	}
	private function expandTree():void 
 	{
        this.tree_limit.expandChildrenOf(_limitSource,true);
    }
	private function OnGetUserPopedomFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetUserPopedomResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetUserPopedomFault);
		Alert.okLabel = "确定";
		Alert.show("初始化用户权限失败"+evt.fault.faultString,"错误",Alert.OK);
	}
	private function OnSave(evt:MouseEvent):void
	{
		if (this.tbox_userName.text=="")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写用户名。","系统提示",Alert.OK,this);
			return;
		}
		if (this.tbox_password.text=="" && this.__opKind=="ADD")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写登录密码。","系统提示",Alert.OK,this);
			return;	
		}
		if (this.tbox_trueName.text=="")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写真实姓名。","系统提示",Alert.OK,this);
			return;
		}
		this._popedoms=GetLimitCheckNode(this._limitSource.children());
		if (this._popedoms==null || this._popedoms.length==0)
		{
			Alert.okLabel = "确定";
			Alert.show("请选择权限。","系统提示",Alert.OK,this);
			return;
		}
		_remote=new RemoteObject("fluorine");
		_remote.showBusyCursor=true;
		var roles:Array=new Array();		
		if (this.__opKind=="ADD")
		{
			_remote.source="ManagementService.Security.User";
			_remote.AddUser(this.tbox_userName.text,
							this.tbox_trueName.text,
							this.tbox_password.text,
							String(this.cbox_subList.selectedItem.SUBCODE),
							this.tbox_remark.text,
							parentApplication._userInfo.trueName,
							roles,this._popedoms);
		}
		else if(this.__opKind=="UPDATE" || this.__opKind=="SELECT")
		{
			_remote.source="ManagementService.Security.ManagementUser";
			_remote.UpdateUser(this.__userId,
							  this.__userName,
							  this.tbox_userName.text,
							  this.tbox_password.text,
							  this.tbox_trueName.text,
							  String(this.cbox_subList.selectedItem.SUBCODE),
							  this.tbox_remark.text,
							  parentApplication._userInfo.trueName,
							  roles,this._popedoms,this._funcCodes,this._param);	
		}
		_remote.addEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.addEventListener(FaultEvent.FAULT,OnSaveFault);
	}
	private function OnSaveResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		var result:String;
		if (this.__opKind=="ADD")
		{
			result=_remote.AddUser.lastResult as String;
		}
		else if(this.__opKind=="UPDATE" || this.__opKind=="SELECT")
		{
			result=_remote.UpdateUser.lastResult as String;
		}
		if (result!=null)
		{
			if (result.substr(0,2).toUpperCase()=="OK")
			{
				if (this.__opKind=="ADD")
				{
					this.__userId=result.substr(2);
				}
				
				_remote=new RemoteObject("fluorine");
				_remote.source="ManagementService.Security.ManagementUser";
				_remote.CheckOtherPopedom(this.tbox_userName.text,this._funcCodes);
				_remote.addEventListener(ResultEvent.RESULT,OnCheckPopedomResult);
				_remote.addEventListener(FaultEvent.FAULT,OnCheckPopedomFault);
			}
			else
			{
				Alert.okLabel = "确定";
				Alert.show(result,"系统提示",Alert.OK,this);
			}
		}
		else
		{
			Alert.okLabel = "确定";
			Alert.show("保存失败。","系统提示",Alert.OK,this);
		}
	}	
	private function OnSaveFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnSaveResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnSaveFault);
		Alert.okLabel = "确定";
		Alert.show("网络或系统错误，保存失败。"+evt.fault.faultString,"系统提示",Alert.OK,this);
	}
	private function OnCheckPopedomResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnCheckPopedomResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnCheckPopedomFault);
		var result:Boolean=_remote.CheckOtherPopedom.lastResult as Boolean;
		var otherPopedom:String="";
		var otherPopedomName:String="";
		if (result)
		{
			otherPopedom="1";
			otherPopedomName="有";
		}
		else
		{
			otherPopedom="0";
			otherPopedomName="无";
		}
		var userRoleName:String="";
		var tempStr:String = "";
		var i:int = 0;
		var j:int = 0;
		for(i=0;i<this._popedoms.length;i++)
		{
		    for(j=0;j<this._funcCodes.length;j++)
		    {
		    	if (this._funcCodes.source[j].KIND == "LANMU" && 
		    	    this._funcCodes.source[j].FUNCCODE == this._popedoms.source[i].FUNCCODE)
		    	{
		    		tempStr += " 栏目管理员";
		    		break;
		    	}
		    }
		    if (tempStr!="")
		    {
		    	break;
		    }
		}
		userRoleName += tempStr;
		tempStr = "";
		for(i=0;i<this._popedoms.length;i++)
		{
		    for(j=0;j<this._funcCodes.length;j++)
		    {
		    	if (this._funcCodes.source[j].KIND == "SHENHE" && 
		    	    this._funcCodes.source[j].FUNCCODE == this._popedoms.source[i].FUNCCODE)
		    	{
		    		tempStr += " 审核管理员";
		    		break;
		    	}
		    }
		    if (tempStr!="")
		    {
		    	break;
		    }
		}
		userRoleName += tempStr;
		tempStr = "";
		for(i=0;i<this._popedoms.length;i++)
		{
		    for(j=0;j<this._funcCodes.length;j++)
		    {
		    	if (this._funcCodes.source[j].KIND == "FENZHAN" && 
		    	    this._funcCodes.source[j].FUNCCODE == this._popedoms.source[i].FUNCCODE)
		    	{
		    		tempStr += " 分站管理员";
		    		break;
		    	}
		    }
		    if (tempStr!="")
		    {
		    	break;
		    }
		}
		userRoleName += tempStr;
		if (this.__saveCallBack!=null)
		{
			this.__saveCallBack.call(this,
									 this.__userId,
									 this.tbox_userName.text,
									 this.tbox_trueName.text,
									 this.cbox_subList.selectedItem.SUBCODE,
									 this.cbox_subList.selectedItem.SUBNAME,
									 this.tbox_remark.text,
									 userRoleName,
									 otherPopedom,otherPopedomName,
									 this.__opKind);
		}
		PopUpManager.removePopUp(this);									 
	}
	private function OnCheckPopedomFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnCheckPopedomResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnCheckPopedomFault);
		PopUpManager.removePopUp(this);
	}
	//获取权限列表选中节点
	private function GetLimitCheckNode(node:XMLList):ArrayCollection
    {
    	var result:ArrayCollection=new ArrayCollection();
    	var obj:Object;
    	for(var i:int=0; i < node.length(); i++) 
    	{
            var n:XML = node[i];   
            var chCount:int=n.children().length();
            if(n.children().length()>0) 
            {
            	if (String(n.@popedomDifference).toLowerCase()!="global")
            	{
            		if (String(n.@type)=="func")
            		{
            			_divisions=GetDivision(XMLList(n.children()[0]));
            		}
            	}
            	else
            	{
            		_divisions="global";
            	}
            	trace(String(n.@code)+"		"+_divisions);
                var tempResult:ArrayCollection=GetLimitCheckNode(n.children());
                if (tempResult.length>0)
                {   
                	obj=new Object();
                	obj.FUNCCODE=String(node[i].@code);
					if (String(node[i].@popedomDifference).toUpperCase()=="GLOBAL")
					{
						obj.SUBCODE="global";
					}
					else if(String(node[i].@popedomDifference).toUpperCase()=="GLOBALANDSUBSITE")
                	{
               			obj.SUBCODE=tempResult.source[0].SUBCODE;
                	}
                	else if(String(node[i].@popedomDifference).toUpperCase()=="SUBSITE")
                	{
                		obj.SUBCODE=tempResult.source[0].SUBCODE;	
                	}
                	result.addItem(obj);
                	for(var j:int=0;j<tempResult.length;j++)
                	{
                		//result.push(tempResult[j]);
                		obj=new Object();
                		obj.FUNCCODE=tempResult.source[j].FUNCCODE;
                		obj.SUBCODE=tempResult.source[j].SUBCODE;
                		result.addItem(obj);
                	}
                }
                tempResult=null;
             	
            }
            else
            {
            	if (n.@state=="checked" && n.@type=="func")
               	{
                	//result.push(String(node[i].@code));
                	obj=new Object();
                	obj.FUNCCODE=String(n.@code);
                	obj.SUBCODE=_divisions;	
                	result.addItem(obj);
                	/*if (String(n.@popedomDifference).toUpperCase()=="GLOBAL")
                	{
                		obj.FUNCCODE=String(n.@code);
                		obj.SUBCODE="global";	
                		result.addItem(obj);
                	}
                	else if(String(n.@popedomDifference).toUpperCase()=="GLOBALANDSUBSITE")
                	{
                		obj.FUNCCODE=String(n.@code);
                		obj.SUBCODE=divisions;
                		result.addItem(obj);
                	}
                	else if(String(n.@popedomDifference).toUpperCase()=="SUBSITE")
                	{
                		obj.FUNCCODE=String(n.@code);
                		obj.SUBCODE=divisions;
                		result.addItem(obj);	
                	}
                	*/
                	//trace(String(n.@code) + "	" + divisions + "	"+String(n.@popedomDifference));
               	}
            }
        } 
        return result;
    }
    
    private function GetDivision(node:XMLList):String
    {
    	var result:String="";
    	
    	for(var i:int=0; i < node.children().length(); i++) 
    	{
    		if (node.children()[i].@state=="checked")
    		{
    			result += node.children()[i].@code+"|";
    		}
    	}
    	if (result!="")
    	{//去掉最后一个“|”
    		result=result.substr(0,result.length-1);
    	}
    	return result;
    }
	
	//树形单击节点展开或收缩子节点
	private function On_TreeLimit_ItemClick(evt:ListEvent):void
	{
		var item:Object = Tree(evt.currentTarget).selectedItem;
		if (this.tree_limit.dataDescriptor.isBranch(item)) 
        { 
        	this.tree_limit.expandItem(item, !this.tree_limit.isItemOpen(item), true); 
        }
	}
	
	private function OnUserSelect(evt:MouseEvent):void
	{
		var pop:UserSelect=UserSelect(PopUpManager.createPopUp(this,UserSelect,true));
		pop.selectCallBack=SelectCallBack;
		PopUpManager.centerPopUp(pop);
	}
	public function SelectCallBack(Id:String,userName:String):void
	{	
		this.__userId=Id;
		this.__userName=userName;
		this.__opKind="SELECT";
		////////////////////////////////////////////////
		this.title="修改用户";
		this.tbox_password.width = 95;
		this.lb_passwordWarring.text="不修改则不填写";
		this.lb_passwordWarring.x = 174;
		////////////////////////////////////////////////
		initUserInfo();
	}
	
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}
	
	