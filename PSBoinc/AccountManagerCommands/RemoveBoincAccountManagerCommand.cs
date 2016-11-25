using BoincRpc;
using System;
using System.Management.Automation;
using System.Threading;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Remove, "BoincAccountManager", SupportsShouldProcess = true)]
    public class RemoveBoincAccountManagerCommand : RpcCmdlet
    {
        protected override void RpcProcessRecord()
        {
            if (!ShouldProcess(BoincSession.Host))
                return;

            AccountManagerRpcReply reply = RpcClient.AccountManagerAttach(string.Empty, string.Empty, string.Empty, CancellationToken.None);

            if (reply.ErrorCode != ErrorCode.Success)
                throw new Exception(string.Format("Account manager operation failed. Error code: {0}. Error message: {1}", reply.ErrorCode, string.Join(" ", reply.Messages)));
        }
    }
}
