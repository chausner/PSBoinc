using BoincRpc;
using System;
using System.Management.Automation;
using System.Threading;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Add, "BoincAccountManager")]
    public class AddBoincAccountManagerCommand : RpcCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Url { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string UserName { get; set; }

        [Parameter(Position = 2, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Password { get; set; }

        protected override void RpcProcessRecord()
        {
            AccountManagerRpcReply reply = RpcClient.AccountManagerAttachAsync(Url, UserName, Password, CancellationToken.None).GetAwaiter().GetResult();

            if (reply.ErrorCode != ErrorCode.Success)
                throw new Exception(string.Format("Account manager operation failed. Error code: {0}. Error message: {1}", reply.ErrorCode, string.Join(" ", reply.Messages)));
        }
    }
}
