using BoincRpc;
using System;
using System.Management.Automation;
using System.Threading;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Add, "BoincProject", DefaultParameterSetName = "EmailPassword")]
    public class AddBoincProjectCommand : RpcCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProjectUrl { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "EmailPassword")]
        [ValidateNotNullOrEmpty]
        public string EmailAddress { get; set; }

        [Parameter(Position = 2, Mandatory = true, ParameterSetName = "EmailPassword")]
        [ValidateNotNullOrEmpty]
        public string Password { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Authenticator")]
        [ValidateNotNullOrEmpty]
        public string Authenticator { get; set; }

        protected override void RpcProcessRecord()
        {
            if (ParameterSetName == "EmailPassword")
            {
                AccountInfo accountInfo = RpcClient.LookupAccount(ProjectUrl, EmailAddress, Password, CancellationToken.None);

                if (accountInfo.ErrorCode != ErrorCode.Success)
                    throw new Exception(string.Format("Error looking up account. Error code: {0}. Error message: {1}", accountInfo.ErrorCode, accountInfo.ErrorMessage));

                Authenticator = accountInfo.Authenticator;
            }

            ProjectConfig projectConfig = RpcClient.GetProjectConfig(ProjectUrl, CancellationToken.None);            

            ProjectAttachReply attachReply = RpcClient.ProjectAttach(ProjectUrl, Authenticator, projectConfig.Name, CancellationToken.None);

            if (attachReply.ErrorCode != ErrorCode.Success)
                throw new Exception(string.Format("Error attaching project. Error code: {0}. Error message: {1}", attachReply.ErrorCode, string.Join(" ", attachReply.Messages)));
        }
    }
}
