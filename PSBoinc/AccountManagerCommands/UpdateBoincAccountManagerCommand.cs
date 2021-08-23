using BoincRpc;
using System;
using System.Management.Automation;
using System.Threading;

namespace PSBoinc
{
    [Cmdlet(VerbsData.Update, "BoincAccountManager")]
    public class UpdateBoincAccountManagerCommand : RpcCmdlet
    {
       protected override void RpcProcessRecord()
       {
           AccountManagerRpcReply reply = RpcClient.AccountManagerUpdateAsync(CancellationToken.None).GetAwaiter().GetResult();

           if (reply.ErrorCode != ErrorCode.Success)
               throw new Exception(string.Format("Account manager operation failed. Error code: {0}. Error message: {1}", reply.ErrorCode, string.Join(" ", reply.Messages)));
       }
    }
}
