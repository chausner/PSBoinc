using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Get, "BoincProject")]
    [OutputType(typeof(Project))]
    public class GetBoincProjectCommand : RpcCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty]
        public string[] Name { get; set; }

        protected override void RpcProcessRecord()
        {
            Project[] projects = RpcClient.GetProjectStatus();

            if (Name != null)
                projects = Utils.FilterByName(projects, p => p.ProjectName, Name, "Could not find a project with name \"{0}\".", "NoProjectFoundForGivenName", this);

            WriteObject(projects, true);
        }
    }
}
