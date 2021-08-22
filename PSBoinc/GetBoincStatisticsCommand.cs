using BoincRpc;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Get, "BoincStatistics")]
    [OutputType(typeof(ProjectStatistics))]
    public class GetBoincStatisticsCommand : RpcCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty]
        public string[] Project { get; set; }

        protected override void RpcProcessRecord()
        {
            ProjectStatistics[] statistics = RpcClient.GetStatisticsAsync().GetAwaiter().GetResult();

            if (Project != null && statistics.Length != 0)
            {
                Project[] projects = RpcClient.GetProjectStatusAsync().GetAwaiter().GetResult();

                HashSet<string> masterUrls = new HashSet<string>(
                    Utils.FilterByName(projects, p => p.ProjectName, Project, "Could not find a project with name \"{0}\".", "NoProjectFoundForGivenName", this)
                    .Select(p => p.MasterUrl));

                statistics = statistics.Where(r => masterUrls.Contains(r.MasterUrl)).ToArray();
            }

            WriteObject(statistics, true);
        }
    }
}
