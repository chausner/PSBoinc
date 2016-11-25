using Microsoft.Win32;
using System;
using System.IO;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Enter, "BoincSession")]
    public class EnterBoincSessionCommand : PSCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Host { get; set; }

        [Parameter(Position = 1)]
        [ValidateNotNullOrEmpty]
        public string Password { get; set; }

        [Parameter]
        [ValidateRange(1, 65535)]
        public int Port { get; set; } = 31416;     

        protected override void ProcessRecord()
        {
            BoincSession previousSession = SessionState.PSVariable.GetValue("BoincSession", null) as BoincSession;

            if (previousSession != null)
                previousSession.RpcClient.Dispose();

            SessionState.PSVariable.Remove("BoincSession");

            BoincSession session = ConnectAndAuthorize();

            SessionState.PSVariable.Set("BoincSession", session);
        }

        private BoincSession ConnectAndAuthorize()
        {
            if (Host == null)
            {
                WriteVerbose("No host was specified. Connecting to the local BOINC client.");

                Host = "localhost";

                if (Password == null)
                {
                    Password = ReadLocalGuiRpcPasswordFile();

                    if (Password == null)
                        WriteWarning(string.Format("Could not read local BOINC password from gui_rpc_auth.cfg. BOINC session will not be authenticated."));
                }
            }

            BoincSession session = new BoincSession(Host, Port);

            session.RpcClient.Connect(Host, Port);

            if (Password != null)
            {
                bool authorized = session.RpcClient.Authorize(Password);

                if (!authorized)
                    throw new Exception("Error authenticating to BOINC client. Make sure the password is correct.");

                session.Authenticated = true;
            }

            if (session.Authenticated)
                WriteVerbose("Authenticated session established successfully.");
            else
                WriteVerbose("Unauthenticated session established successfully.");

            return session;
        }

        private string ReadLocalGuiRpcPasswordFile()
        {
            string dataDir = null;

            using (RegistryKey boincSetupKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Space Sciences Laboratory, U.C. Berkeley\BOINC Setup", false))
                if (boincSetupKey != null)
                    dataDir = boincSetupKey.GetValue("DATADIR") as string;

            if (dataDir != null)
                dataDir = Environment.ExpandEnvironmentVariables(dataDir);
            else
                dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BOINC");

            string passwordFilePath = Path.Combine(dataDir, "gui_rpc_auth.cfg");

            if (File.Exists(passwordFilePath))
                return File.ReadAllText(passwordFilePath).Trim();
            else
                return null;
        }
    }
}
