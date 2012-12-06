using System.Runtime.Serialization;

namespace QConsole.NET
{
    [DataContract]
    public class CommandsResponse
    {
        [DataMember(Name = "autocomplete")]
        public string AutoComplete { get; set; }
        [DataMember(Name = "execute")]
        public string Execute { get; set; }
        [DataMember(Name = "commands")]
        public CommandsDictionary<string, string> Commands { get; set; }
    }
}