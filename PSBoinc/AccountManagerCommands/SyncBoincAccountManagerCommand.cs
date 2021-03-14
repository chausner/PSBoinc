using BoincRpc;
using System;
using System.Management.Automation;
using System.Threading;

namespace PSBoinc
{
    [Cmdlet(VerbsData.Sync, "BoincAccountManager", SupportsShouldProcess = true)]
    public class SyncBoincAccountManagerCommand : RpcCmdlet
    {
        protected override void RpcProcessRecord()
        {
            if (!ShouldProcess(BoincSession.Host))
                return;

            AccountManagerRpcReply reply = RpcClient.AccountManagerSync();

            if (reply.ErrorCode != ErrorCode.Success)
                throw new Exception(string.Format("Account manager operation failed. Error code: {0}. Error message: {1}", reply.ErrorCode, string.Join(" ", reply.Messages)));
        }
    }
}