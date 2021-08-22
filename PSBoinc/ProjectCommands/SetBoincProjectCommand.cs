using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Set, "BoincProject", SupportsShouldProcess = true)]
    public class SetBoincProjectCommand : RpcCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Project[] InputObject { get; set; }

        [Parameter]
        public SwitchParameter NoMoreWork { get; set; }

        [Parameter]
        public SwitchParameter AllowMoreWork { get; set; }

        [Parameter]
        public SwitchParameter DetachWhenDone { get; set; }

        [Parameter]
        public SwitchParameter DontDetachWhenDone { get; set; }

        protected override void RpcProcessRecord()
        {
            if (NoMoreWork.IsPresent && AllowMoreWork.IsPresent)
                throw new PSArgumentException("The parameters NoMoreWork and AllowMoreWork may not be specified together.");
            if (DetachWhenDone.IsPresent && DontDetachWhenDone.IsPresent)
                throw new PSArgumentException("The parameters DetachWhenDone and DontDetachWhenDone may not be specified together.");

            foreach (Project project in InputObject)
            {
                if (!ShouldProcess(project.ProjectName))
                    continue;

                if (NoMoreWork.IsPresent)
                    RpcClient.PerformProjectOperationAsync(project, ProjectOperation.NoMoreWork).GetAwaiter().GetResult();
                if (AllowMoreWork.IsPresent)
                    RpcClient.PerformProjectOperationAsync(project, ProjectOperation.AllowMoreWork).GetAwaiter().GetResult();
                if (DetachWhenDone.IsPresent)
                    RpcClient.PerformProjectOperationAsync(project, ProjectOperation.DetachWhenDone).GetAwaiter().GetResult();
                if (DontDetachWhenDone.IsPresent)
                    RpcClient.PerformProjectOperationAsync(project, ProjectOperation.DontDetachWhenDone).GetAwaiter().GetResult();
            }
        }
    }
}
