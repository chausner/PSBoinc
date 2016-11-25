using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsLifecycle.Stop, "BoincTransfer", SupportsShouldProcess = true)]
    public class StopBoincTransferCommand : RpcCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public FileTransfer[] InputObject { get; set; }

        protected override void RpcProcessRecord()
        {
            foreach (FileTransfer transfer in InputObject)
            {
                if (!ShouldProcess(transfer.Name))
                    continue;

                RpcClient.PerformFileTransferOperation(transfer, FileTransferOperation.Abort);
            }
        }
    }
}
