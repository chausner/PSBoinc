using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsLifecycle.Resume, "BoincTask")]
    public class ResumeBoincTaskCommand : RpcCmdlet
    { 
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Result[] InputObject { get; set; }

        protected override void RpcProcessRecord()
        {
            foreach (Result result in InputObject)
                RpcClient.PerformResultOperation(result, ResultOperation.Resume);
        }
    }
}
