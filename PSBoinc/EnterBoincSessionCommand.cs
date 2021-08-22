using Microsoft.Win32;
using System;
using System.IO;
using System.Management.Automation;
using System.Runtime.InteropServices;

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

            session.RpcClient.ConnectAsync(Host, Port).GetAwaiter().GetResult();

            if (Password != null)
            {
                bool authorized = session.RpcClient.AuthorizeAsync(Password).GetAwaiter().GetResult();

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

        private string GetLocalGuiRpcPasswordFilePath()
        {
            string dataDir = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (RegistryKey boincSetupKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Space Sciences Laboratory, U.C. Berkeley\BOINC Setup", false))
                    if (boincSetupKey != null)
                        dataDir = boincSetupKey.GetValue("DATADIR") as string;

                if (dataDir != null)
                    dataDir = Environment.ExpandEnvironmentVariables(dataDir);
                else
                    dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BOINC");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                dataDir = "/var/lib/boinc-client";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                dataDir = "/Library/Application Support/BOINC Data";

            return Path.Combine(dataDir, "gui_rpc_auth.cfg");
        }

        private string ReadLocalGuiRpcPasswordFile()
        {
            string passwordFilePath = GetLocalGuiRpcPasswordFilePath();

            if (!File.Exists(passwordFilePath))
                return null;

            string[] lines;

            try
            {
                lines = File.ReadAllLines(passwordFilePath);
            }
            catch (UnauthorizedAccessException)
            {
                // read access to gui_rpc_auth.cfg is often restricted to the BOINC user on non-Windows platforms
                return null;
            }

            if (lines.Length >= 1)
                return lines[0].Trim();
            else
                return null;
        }
    }
}
