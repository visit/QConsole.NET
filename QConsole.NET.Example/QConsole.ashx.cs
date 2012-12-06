using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;

namespace QConsole.NET
{
    /// <summary>
    /// Summary description for QConsole
    /// </summary>
    public class QConsoleHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            NET.QConsole.Init();
            NET.QConsole.Authorize();
            context.Response.ContentType = "application/json";



            var command = context.Request["action"];
            switch (command)
            {
                case "commands":
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(CommandsResponse));
                    var commands = new CommandsDictionary<string,string>();
                    commands.Add("test","hej test");
                    var response = new CommandsResponse()
                                       {
                                           AutoComplete = "/QConsole.ashx?action=complete",
                                           Execute = "QConsole.ashx?action=execute",
                                           Commands = commands
                                       };

                    ser.WriteObject(context.Response.OutputStream,response);
                    break;
                case "complete":
                    var partial = context.Request.QueryString["command"];
                    string[] args = partial.Split(new[] { ' ' }, 2);

                    IEnumerable<string> completions = NET.QConsole.Complete(args[0], args.Count() > 1 ? args[1] : null);

                    DataContractJsonSerializer complSer = new DataContractJsonSerializer(typeof(IEnumerable<string>));

                    complSer.WriteObject(context.Response.OutputStream, completions);
                    break;
                case "execute":
                    var cmd = context.Request.QueryString["command"];
                    if (cmd != null)
                    {
                        args = cmd.Split(new [] {' '},2);
                        var result = NET.QConsole.Execute(args[0], args.Count() > 1 ? args[1] : null) ;
                        if (result == null)
                            throw new HttpException(400, "command not found");
                        else
                        {
                            DataContractJsonSerializer exSer = new DataContractJsonSerializer(typeof(ExecuteResponse));
                            
                            exSer.WriteObject(context.Response.OutputStream,result);
                            return;
                        }
                    }

                    throw new HttpException(400,"did not like input");
                    break;
                default:
                    throw new HttpException(400,"unknown action");
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}