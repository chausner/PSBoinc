using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsLifecycle.Resume, "BoincProject")]
    public class ResumeBoincProjectCommand : RpcCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Project[] InputObject { get; set; }

        protected override void RpcProcessRecord()
        {
            foreach (Project project in InputObject)
                RpcClient.PerformProjectOperationAsync(project, ProjectOperation.Resume).GetAwaiter().GetResult();
        }
    }
}
