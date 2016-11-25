using BoincRpc;
using System.Linq;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Get, "BoincNotice")]
    [OutputType(typeof(Notice))]
    public class GetBoincNoticeCommand : RpcCmdlet
    {
        [Parameter]
        [ValidateRange(0, int.MaxValue)]
        public int Last { get; set; } = -1;

        protected override void RpcProcessRecord()
        {
            Notice[] notices = RpcClient.GetNotices(0, !BoincSession.Authenticated);

            if (Last != -1 && notices.Length > Last)
                notices = notices.Skip(notices.Length - Last).ToArray();

            WriteObject(notices, true);
        }
    }
}
