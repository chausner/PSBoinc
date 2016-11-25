using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Reset, "BoincProject", SupportsShouldProcess = true)]
    public class ResetBoincProjectCommand : RpcCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Project[] InputObject { get; set; }

        protected override void RpcProcessRecord()
        {
            foreach (Project project in InputObject)
            {
                if (!ShouldProcess(project.ProjectName))
                    continue;

                RpcClient.PerformProjectOperation(project, ProjectOperation.Reset);
            }
        }
    }
}
