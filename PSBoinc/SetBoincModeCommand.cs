using BoincRpc;
using System;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Set, "BoincMode")]
    public class SetBoincModeCommand : RpcCmdlet
    {
        [Parameter]
        public Mode TaskMode { get; set; } = Unset;

        [Parameter]
        public Mode GpuMode { get; set; } = Unset;

        [Parameter]
        public Mode NetworkMode { get; set; } = Unset;

        [Parameter]
        public TimeSpan Duration { get; set; } = TimeSpan.Zero;

        const Mode Unset = (Mode)(-1);

        protected override void RpcProcessRecord()
        {
            if (TaskMode == Unset && GpuMode == Unset && NetworkMode == Unset)
                throw new PSArgumentException("At least one of the TaskMode, GpuMode and NetworkMode parameters needs to be specified.");

            if (TaskMode != Unset)
                RpcClient.SetRunModeAsync(TaskMode, Duration).GetAwaiter().GetResult();

            if (GpuMode != Unset)
                RpcClient.SetGpuModeAsync(GpuMode, Duration).GetAwaiter().GetResult();

            if (NetworkMode != Unset)
                RpcClient.SetNetworkModeAsync(NetworkMode, Duration).GetAwaiter().GetResult();
        }
    }
}
