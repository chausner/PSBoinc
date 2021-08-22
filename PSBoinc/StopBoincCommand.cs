using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsLifecycle.Stop, "Boinc", SupportsShouldProcess = true)]
    public class StopBoincCommand : RpcCmdlet
    {
        protected override void RpcProcessRecord()
        {
            if (!ShouldProcess(BoincSession.Host))
                return;

            RpcClient.QuitAsync().GetAwaiter().GetResult();
        }
    }
}
