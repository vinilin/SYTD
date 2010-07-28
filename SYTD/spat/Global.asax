<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        //在应用程序启动时运行的代码
        
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //在应用程序关闭时运行的代码

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        //在出现未处理的错误时运行的代码

    }

    void Session_Start(object sender, EventArgs e) 
    {
        //在新会话启动时运行的代码

    }

    void Session_End(object sender, EventArgs e) 
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。

    }
    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        String[] safeParameters = System.Configuration.ConfigurationSettings.AppSettings["safeParameters"].ToString().Split(',');
        for (int i = 0; i < safeParameters.Length; i++)
        {
            String parameterName = safeParameters[i].Split('-')[0];
            String parameterType = safeParameters[i].Split('-')[1];
            isValidParameter(parameterName, parameterType);
        }
    }
    public void isValidParameter(string parameterName, string parameterType)
    {
        string parameterValue = Request.QueryString[parameterName];
        if (parameterValue == null) return;
        if (parameterType.Equals("int32"))
        {
            if (!parameterCheck.isInt(parameterValue)) Response.Redirect("default.aspx");
        }
        else if (parameterType.Equals("USzip"))
        {
            if (!parameterCheck.isUSZip(parameterValue)) Response.Redirect("default.aspx");
        }
        else if (parameterType.Equals("email"))
        {
            if (!parameterCheck.isEmail(parameterValue)) Response.Redirect("default.aspx");
        }
        else if (parameterType.Equals("number"))
        {
            if (!parameterCheck.isNumber(parameterValue)) Response.Redirect("default.aspx");
        }
        else if (parameterType.Equals("type"))
        {
            if (!parameterCheck.isType(parameterValue)) Response.Redirect("default.aspx");
        }
        else if (parameterType.Equals("isSubCode"))
        {
            if (!parameterCheck.isSubCode(parameterValue)) Response.Redirect("default.aspx");
        }
    }   
</script>
