using System.Collections.Generic;

namespace QConsole.NET
{
    public interface IQConsoleCommand
    {
        string Name { get;  }
        string HelpText { get;  }
        IEnumerable<string> Complete(string arguments);
        ExecuteResponse Execute(string arguments);
    }
}