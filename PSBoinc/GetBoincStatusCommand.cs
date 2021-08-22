using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Get, "BoincStatus")]
    [OutputType(typeof(CoreClientStatus))]
    public class GetBoincStatusCommand : RpcCmdlet
    {
        protected override void RpcProcessRecord()
        {
            WriteObject(RpcClient.GetCoreClientStatusAsync().GetAwaiter().GetResult());
        }
    }
}
