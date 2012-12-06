using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.NET
{
    [DataContract]
    public class ExecuteResponse
    {
        [DataMember(Name = "result")]
        public string Result { get; set; }
        [DataMember(Name = "success")]
        public bool Success { get; set; }
        [DataMember(Name = "callback")]
        public string Callback { get; set; }
    }
}
