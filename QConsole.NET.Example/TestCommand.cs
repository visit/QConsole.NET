using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QConsole.NET.Example
{
    public class TestCommand : IQConsoleCommand
    {
        public string Name { get { return "Test"; } }
        public string HelpText { get { return "Help text"; } }

        private List<string> Completes;

        public TestCommand()
        {
            Completes = new List<string>() { "Agent", "Alster", "Baron", "Bose","Burnelli", "Columbia" };
        }

        public IEnumerable<string> Complete(string arguments)
        {
            if (string.IsNullOrWhiteSpace(arguments))
                return Completes;

            return Completes.Where(x => x.StartsWith(arguments,StringComparison.InvariantCultureIgnoreCase));
        }

        public ExecuteResponse Execute(string arguments)
        {
            return new ExecuteResponse
                       {
                           Callback = "alert(\"hello\");",
                           Result = "test result",
                           Success = true
                       };
        }
    }
}