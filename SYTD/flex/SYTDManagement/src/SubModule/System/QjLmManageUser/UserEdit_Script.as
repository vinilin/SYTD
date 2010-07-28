	// ActionScript file
	import Custom.Renderer.CenteredCheckBoxHeaderRenderer;
	import Custom.Renderer.CenteredCheckBoxItemRenderer;
	
	import flash.events.MouseEvent;
	
	import mx.collections.ArrayCollection;
	import mx.collections.ListCollectionView;
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
	private var _ManagementCode:String = "015007";
	[Bindable]private var _roleSource:ArrayCollection = new ArrayCollection();
		
	/////////////////////////////////
	public var selectAllFlag:Boolean;
	[Bindable]public var hr:ClassFactory;	
	/////////////////////////////////////////////
	
	private function init():void
	{
		////////////////////////////////////////////
		hr = new ClassFactory(CenteredCheckBoxHeaderRenderer);
		hr.properties = {stateHost: this, stateProperty: "selectAllFlag"};
		//---------------------------------------
		this.dg.addEventListener(MouseEvent.CLICK,OnCheckBoxClickHandler);
		this.dg.addEventListener(ListEvent.ITEM_CLICK,OnDgItemClick);
		this.addEventListener(CloseEvent.CLOSE,OnClose);
		this.btnSave.addEventListener(MouseEvent.CLICK,OnSave);
		
	}
	public function set opKind(val:String):void
	{
		this.__opKind=val;
		if (val=="ADD")
		{
			this.title="增加栏目管理员和审核管理员管理（全局）";
		}
		else
		{
			this.title="修改栏目管理员和审核管理员管理（全局）";
			this.tbox_password.width=this.tbox_password.width-80;
			this.lb_passwordWarring.text="不修改则不填写";
			this.lb_passwordWarring.x=this.lb_passwordWarring.x-80;
			this.tbox_password1.width=this.tbox_password1.width-80;
			this.lb_passwordWarring1.text="不修改则不填写";
			this.lb_passwordWarring1.x=this.lb_passwordWarring1.x-80;
			
			this.tbox_userName.enabled = false;
		}
	}
	public function set userId(val:String):void
	{
		this.__userId=val;
	}
	public function set ManagementCode(val:String):void
	{
		this._ManagementCode = val;
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
			initUserRole();
		}
		else if(this.__opKind=="UPDATE")
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
			this.tbox_bindIp.text = result.source[0].BINDIP;
			if (result.source[0].ISBINDIP == "1")
			{
				this.rdo_bindip_yes.selected = true;
			}
			else
			{
				this.rdo_bindip_no.selected = true;
			}
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
		}
		initUserRole();
	}
	private function OnGetUserInfoFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetUserInfoResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetUserInfoFault);
		Alert.okLabel = "确定";
		Alert.show("初始化数据失败，请退出后重新进入。"+evt.fault.faultString,"系统提示",Alert.OK);
	}
	
	private function initUserRole():void
	{
		_remote = new RemoteObject("fluorine");
		_remote.source = "ManagementService.Security.User";
		var foreCloseRole:Array = new Array();
		foreCloseRole.push("001");
		foreCloseRole.push("019");
		foreCloseRole.push("041");
		_remote.GetAllRoleByUserNameAndPPtrRole(this.__userName,"000",foreCloseRole);
		_remote.addEventListener(ResultEvent.RESULT,OnGetUserRoleResult);
		_remote.addEventListener(FaultEvent.FAULT,OnGetUserRoleFault);
	}
	private function OnGetUserRoleResult(evt:ResultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetUserRoleResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetUserRoleFault);
		this._roleSource = _remote.GetAllRoleByUserNameAndPPtrRole.lastResult as ArrayCollection;
	}
	private function OnGetUserRoleFault(evt:FaultEvent):void
	{
		_remote.removeEventListener(ResultEvent.RESULT,OnGetUserRoleResult);
		_remote.removeEventListener(FaultEvent.FAULT,OnGetUserRoleFault);
		Alert.show("初始化数据失败，请退出后重新进入。"+evt.fault.faultString,"系统提示",Alert.OK);
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
		if (this.tbox_password.text!=this.tbox_password1.text && this.tbox_password!=null)
		{
			Alert.okLabel = "确定";
			Alert.show("密码与确认密码不一致。","系统提示",Alert.OK,this);
			return;
		}
		if (this.rdo_bindip_yes.selected && this.tbox_bindIp.text == "")
		{
			Alert.okLabel = "确定";
			Alert.show("请填写要绑定的IP地址。","系统提示",Alert.OK,this);
			return;
		}
		var roles:ArrayCollection=new ArrayCollection();
		var obj:Object ;
		for(var i:int=0;i<this._roleSource.length;i++)
		{
			if (this._roleSource.source[i].ISCHECKED == "1")
			{
				obj = new Object();
				obj.roleCode = this._roleSource.source[i].ROLECODE;
				obj.subCode = "global";
				obj.kindCode = "";
				roles.addItem(obj);
			}
		}
		if (roles == null || roles.length == 0)
		{
			Alert.okLabel = "确定";
			Alert.show("请选择用户角色。","系统提示",Alert.OK,this);
			return;
		}
		_remote=new RemoteObject("fluorine");
		_remote.source="ManagementService.Security.User";
		_remote.showBusyCursor=true;
		
		var isBindIp:String = "0";
		if (this.rdo_bindip_yes.selected)
		{
			isBindIp = "1";
		}
		if (this.__opKind=="ADD")
		{
			_remote.AddUser(this.tbox_userName.text,
							this.tbox_trueName.text,
							this.tbox_password.text,
							String(this.cbox_subList.selectedItem.SUBCODE),
							this.tbox_remark.text,
							parentApplication._userInfo.trueName,
							isBindIp,
							this.tbox_bindIp.text,
							roles,
							this._ManagementCode);
		}
		else if(this.__opKind=="UPDATE")
		{
			_remote.UpdateUser(this.__userId,
							  this.__userName,
							  this.tbox_userName.text,
							  this.tbox_password.text,
							  this.tbox_trueName.text,
							  String(this.cbox_subList.selectedItem.SUBCODE),
							  this.tbox_remark.text,
							  parentApplication._userInfo.trueName,
							  isBindIp,
							  this.tbox_bindIp.text,
							  roles);	
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
		else if(this.__opKind=="UPDATE")
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
				if (this.__saveCallBack!=null)
				{
					var isBindIp:String = "否";
					if (this.rdo_bindip_yes.selected)
					{
						isBindIp = "是";
					}
					this.__saveCallBack.call(this,
											 this.__userId,
											 this.tbox_userName.text,
											 this.tbox_trueName.text,
											 this.cbox_subList.selectedItem.SUBCODE,
											 this.cbox_subList.selectedItem.SUBNAME,
											 this.tbox_remark.text,
											 isBindIp,
											 this.tbox_bindIp.text,
											 this.__opKind);
				}
				PopUpManager.removePopUp(this);
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
	
	private function OnClose(evt:CloseEvent):void
	{
		PopUpManager.removePopUp(this);
	}
	
	////////选择
	private function OnDgItemClick(evt:ListEvent):void
	{		
		var i:int=0;
		if (!(evt.itemRenderer is CenteredCheckBoxItemRenderer))
		{
			for(i=0;i<this._roleSource.length;i++)
			{
				this._roleSource.source[i].ISCHECKED=0;
			}
			for(i=0;i<dg.selectedItems.length;i++)
			{
				this.dg.selectedItems[i].ISCHECKED=1;
				ListCollectionView(dg.dataProvider).itemUpdated(this.dg.selectedItems[i], "ISCHECKED");
			}
		}
		////////////////////////////////////////
		var allCheck:Boolean=true;
		for(i=0;i<this._roleSource.length;i++)
		{
			if (!this._roleSource.source[i].ISCHECKED)	
			{
				allCheck=false;
				break;
			}
		}
		selectAllFlag=allCheck;
		//////////////////////////////////////////
	}
	
	private function OnCheckBoxClickHandler(event:MouseEvent):void
	{
		var obj:Object;
		var _selectItems:Array=new Array();
		var tempDg:DataGrid=DataGrid(event.currentTarget);
    	if(event.target is CenteredCheckBoxHeaderRenderer)
    	{
        	//this._selectItems=new Array();
        	for each(obj in tempDg.dataProvider)
        	{
            	obj.ISCHECKED = CenteredCheckBoxHeaderRenderer(event.target).selected;
            	if (obj.ISCHECKED)
            	{
            		_selectItems.push(obj);
            	}
            	ListCollectionView(tempDg.dataProvider).itemUpdated(obj);
        	}
        	tempDg.selectedItems=_selectItems;
    	}  
    	else if(event.target is CenteredCheckBoxItemRenderer)
    	{
    		var allCheck:Boolean=true;
    		for each(obj in tempDg.dataProvider)
    		{
    			if (obj.ISCHECKED)
    			{
    				_selectItems.push(obj);
    			}
    			else
    			{
    				allCheck=false;
    			}
    		}
    		tempDg.selectedItems=_selectItems;
    		selectAllFlag=allCheck;
    	}	
	}