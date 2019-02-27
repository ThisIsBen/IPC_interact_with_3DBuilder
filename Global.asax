<%@ Application Language="C#" %>
<%@ Import Namespace="IPCTest" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // 在應用程式啟動時執行的程式碼
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
        Application["visitors"]= 0;

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  在應用程式關閉時執行的程式碼

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // 在發生未處理的錯誤時執行的程式碼

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // 在新的工作階段啟動時執行的程式碼


        //when session in start application variable is increased by 1  
        Application.Lock();
        Application["visitors"] = (int)Application["visitors"] + 1;
        Application.UnLock(); 
    }

    void Session_End(object sender, EventArgs e) 
    {
        // 在工作階段結束時執行的程式碼
        // 注意: 只有在  Web.config 檔案中將 sessionstate 模式設定為 InProc 時，
        // 才會引起 Session_End 事件。如果將 session 模式設定為 StateServer 
        // 或 SQLServer，則不會引起該事件。


        //when session in end application variable is decrease by 1  
        Application.Lock();
        Application["visitors"] = (int)Application["visitors"] - 1;
        Application.UnLock();  

    }
       
</script>
