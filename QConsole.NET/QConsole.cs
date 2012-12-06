using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading;

namespace QConsole.NET
{
    public static class QConsole
    {
        private static readonly object AuthenticatorsInit = new object();

        public static void Init()
        {
            if (Authenticators == null)
            {
                lock (AuthenticatorsInit)
                {
                    if (Authenticators == null)
                    {
                        Thread.Sleep(500); //hack to wait for assemblies to load
                        Authenticators =
                            AppDomain.CurrentDomain.GetAssemblies()
                                     .SelectMany(ass => ass.GetTypes())
                                     .Where(t => (typeof (IQConsoleAuthorization)).IsAssignableFrom(t) && t.IsClass).Select(x => (IQConsoleAuthorization)Activator.CreateInstance(x)).ToList();
                    
                        Commands =
                            AppDomain.CurrentDomain.GetAssemblies()
                                     .SelectMany(ass => ass.GetTypes())
                                     .Where(t => (typeof(IQConsoleCommand)).IsAssignableFrom(t) && t.IsClass).Select(x => (IQConsoleCommand)Activator.CreateInstance(x)).ToList();
                    }
                }
            }
        }

        private static List<IQConsoleCommand> Commands { get; set; }

        private static List<IQConsoleAuthorization> Authenticators { get; set; }

        public static CommandsDictionary<string, string> ConsoleCommands
        {
            get
            {
                var commands = new CommandsDictionary<string, string>();
                Commands.ForEach(c => commands.Add(c.Name, c.HelpText));
                return commands;
            }
        }

        public static ExecuteResponse CommandNotFound = new ExecuteResponse();

        public static void Authorize()
        {
            bool isAuthed = Authenticators.All(x => x.IsAuthorized());
            if(!isAuthed)
                throw new AuthenticationException("Not authorized");
        }

        public static ExecuteResponse Execute(string command, string arguments)
        {
            var matchingCommand =
                Commands.SingleOrDefault(x => x.Name.Equals(command, StringComparison.InvariantCultureIgnoreCase));
            if (matchingCommand != null)
            {
                return matchingCommand.Execute(arguments);
            }
            else
            {
                return CommandNotFound;
            }
        }

        public static IEnumerable<string> Complete(string command, string arguments)
        {
            var matchingCommand =
                Commands.SingleOrDefault(x => x.Name.Equals(command, StringComparison.InvariantCultureIgnoreCase));
            if (matchingCommand == null)
            {
                return null;
            }

            return matchingCommand.Complete(arguments);
        }
    }
}