using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsLifecycle.Stop, "BoincTask", SupportsShouldProcess = true)]
    public class StopBoincTaskCommand : RpcCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Result[] InputObject { get; set; }

        protected override void RpcProcessRecord()
        {
            foreach (Result result in InputObject)
            {
                if (!ShouldProcess(result.Name))
                    continue;

                RpcClient.PerformResultOperationAsync(result, ResultOperation.Abort).GetAwaiter().GetResult();
            }
        }
    }
}
