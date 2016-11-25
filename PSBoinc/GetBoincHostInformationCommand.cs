using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Get, "BoincHostInformation")]
    [OutputType(typeof(HostInfo))]
    public class GetBoincHostInformationCommand : RpcCmdlet
    {
        protected override void RpcProcessRecord()
        {
            WriteObject(RpcClient.GetHostInfo());
        }
    }
}
