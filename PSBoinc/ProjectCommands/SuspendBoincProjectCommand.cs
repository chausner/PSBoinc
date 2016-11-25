using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsLifecycle.Suspend, "BoincProject")]
    public class SuspendBoincProjectCommand : RpcCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Project[] InputObject { get; set; }

        protected override void RpcProcessRecord()
        {
            foreach (Project project in InputObject)
                RpcClient.PerformProjectOperation(project, ProjectOperation.Suspend);
        }
    }
}
