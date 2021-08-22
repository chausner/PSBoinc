using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Remove, "BoincProject", SupportsShouldProcess = true)]
    public class RemoveBoincProjectCommand : RpcCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Project[] InputObject { get; set; }

        protected override void RpcProcessRecord()
        {
            foreach (Project project in InputObject)
            {
                if (!ShouldProcess(project.ProjectName))
                    continue;

                RpcClient.PerformProjectOperationAsync(project, ProjectOperation.Detach).GetAwaiter().GetResult();
            }
        }
    }
}
