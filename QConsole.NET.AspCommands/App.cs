using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace QConsole.NET.AspCommands
{
    public class App : IQConsoleCommand
    {
        public string Name { get { return "app"; } }
        public string HelpText { get
    {
        return "Controls the web application. Available commands:\n recycle - recycles the application pool";
    } }
        public IEnumerable<string> Complete(string arguments)
        {
            if (string.IsNullOrEmpty(arguments) || "recycle".StartsWith(arguments))
            {
                return new List<string> {"recycle"};
            }

            return new string[0];
        }

        public ExecuteResponse Execute(string arguments)
        {
            if (arguments == "recycle")
            {
                Task.Factory.StartNew(() =>
                                          {
                                              Thread.Sleep(1000);
                                              //ApplicationPoolRecycle.RecycleCurrentApplicationPool();
                                              Environment.Exit(0);
                                          });


                return new ExecuteResponse() {Success = true, Result = "Recycling application pool..."};
            }

            return new ExecuteResponse() {Result = "Unknown command."};
        }

        public static class ApplicationPoolRecycle
        {
            /// <summary>Attempts to recycle current application pool</summary>
            /// <returns>Boolean indicating if application pool was successfully recycled</returns>
            public static bool RecycleCurrentApplicationPool()
            {
                try
                {
                    // Application hosted on IIS that supports App Pools, like 6.0 and 7.0
                    if (IsApplicationRunningOnAppPool())
                    {
                        // Get current application pool name
                        string appPoolId = GetCurrentApplicationPoolId();
                        // Recycle current application pool
                        RecycleApplicationPool(appPoolId);
                        return true;
                    }
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }

            private static bool IsApplicationRunningOnAppPool()
            {
                // Application is not hosted on IIS
                if (!AppDomain.CurrentDomain.FriendlyName.StartsWith("/LM/"))
                    return false;
                // Application hosted on IIS that doesn't support App Pools, like 5.1
                else if (!DirectoryEntry.Exists("IIS://Localhost/W3SVC/AppPools"))
                    return false;
                else
                    return true;
            }

            private static string GetCurrentApplicationPoolId()
            {
                string virtualDirPath = AppDomain.CurrentDomain.FriendlyName;
                virtualDirPath = virtualDirPath.Substring(4);
                int index = virtualDirPath.Length + 1;
                index = virtualDirPath.LastIndexOf("-", index - 1, index - 1);
                index = virtualDirPath.LastIndexOf("-", index - 1, index - 1);
                virtualDirPath = "IIS://localhost/" + virtualDirPath.Remove(index);
                DirectoryEntry virtualDirEntry = new DirectoryEntry(virtualDirPath);
                return virtualDirEntry.Properties["AppPoolId"].Value.ToString();
            }

            private static void RecycleApplicationPool(string appPoolId)
            {
                string appPoolPath = "IIS://localhost/W3SVC/AppPools/" + appPoolId;
                DirectoryEntry appPoolEntry = new DirectoryEntry(appPoolPath);
                appPoolEntry.Invoke("Recycle");
            }
        }
    }
}
