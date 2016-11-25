using BoincRpc;
using System;
using System.Management.Automation;

namespace PSBoinc
{
    public abstract class RpcCmdlet : PSCmdlet
    {
        protected BoincSession BoincSession { get; private set; }

        protected RpcClient RpcClient
        {
            get
            {
                return BoincSession?.RpcClient;
            }
        }

        protected abstract void RpcProcessRecord();

        protected override void ProcessRecord()
        {
            BoincSession = SessionState.PSVariable.GetValue("BoincSession", null) as BoincSession;

            if (RpcClient == null)
                throw new Exception("There is currently no active BOINC session. Open a session first with Enter-BoincSession.");

            RpcProcessRecord();
        }
    }
}
