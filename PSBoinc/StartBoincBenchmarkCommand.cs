using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsLifecycle.Start, "BoincBenchmark")]
    public class StartBoincBenchmarkCommand : RpcCmdlet
    {
        protected override void RpcProcessRecord()
        {
            RpcClient.RunBenchmarks();
        }
    }
}
