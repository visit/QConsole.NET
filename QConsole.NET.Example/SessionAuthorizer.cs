using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QConsole.NET.Example
{
    public class SessionAuthorizer : IQConsoleAuthorization
    {
        public bool IsAuthorized()
        {
            return ((bool?) HttpContext.Current.Session["ConsoleAuthorized"]) == true;
        }
    }
}