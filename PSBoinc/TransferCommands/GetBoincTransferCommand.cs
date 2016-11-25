using BoincRpc;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Get, "BoincTransfer")]
    [OutputType(typeof(FileTransfer))]
    public class GetBoincTransferCommand : RpcCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty]
        public string[] Name { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string[] Project { get; set; }

        protected override void RpcProcessRecord()
        {
            FileTransfer[] transfers = RpcClient.GetFileTransfers();

            if (Name != null)
                transfers = Utils.FilterByName(transfers, t => t.Name, Name, "Could not find a transfer with name \"{0}\".", "NoTransferFoundForGivenName", this);

            if (Project != null && transfers.Length != 0)
            {
                Project[] projects = RpcClient.GetProjectStatus();

                HashSet<string> masterUrls = new HashSet<string>(
                    Utils.FilterByName(projects, p => p.ProjectName, Project, "Could not find a project with name \"{0}\".", "NoProjectFoundForGivenName", this)
                    .Select(p => p.MasterUrl));

                transfers = transfers.Where(t => masterUrls.Contains(t.ProjectUrl)).ToArray();
            }

            WriteObject(transfers, true);
        }
    }
}
