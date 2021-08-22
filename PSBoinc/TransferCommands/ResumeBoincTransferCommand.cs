using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsLifecycle.Resume, "BoincTransfer")]
    public class ResumeBoincTransferCommand : RpcCmdlet
    { 
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public FileTransfer[] InputObject { get; set; }

        protected override void RpcProcessRecord()
        {
            foreach (FileTransfer transfer in InputObject)
                RpcClient.PerformFileTransferOperationAsync(transfer, FileTransferOperation.Retry).GetAwaiter().GetResult();
        }
    }
}
