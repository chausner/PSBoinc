﻿using BoincRpc;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsLifecycle.Suspend, "BoincTask")]
    public class SuspendBoincTaskCommand : RpcCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Result[] InputObject { get; set; }

        protected override void RpcProcessRecord()
        {
            foreach (Result result in InputObject)
                RpcClient.PerformResultOperationAsync(result, ResultOperation.Suspend).GetAwaiter().GetResult();
        }
    }
}
