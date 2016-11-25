using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Exit, "BoincSession")]
    public class ExitBoincSessionCommand : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            BoincSession previousSession = SessionState.PSVariable.GetValue("BoincSession", null) as BoincSession;

            if (previousSession != null)
            {
                previousSession.RpcClient.Dispose();

                WriteVerbose("Session closed successfully.");
            }
            else
                WriteVerbose("There is no currently open session.");

            SessionState.PSVariable.Remove("BoincSession");
        }
    }
}
