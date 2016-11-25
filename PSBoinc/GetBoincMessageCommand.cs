using BoincRpc;
using System;
using System.Linq;
using System.Management.Automation;

namespace PSBoinc
{
    [Cmdlet(VerbsCommon.Get, "BoincMessage")]
    [OutputType(typeof(Message))]
    public class GetBoincMessageCommand : RpcCmdlet
    {
        [Parameter]
        [ValidateRange(0, int.MaxValue)]
        public int Last { get; set; } = -1;

        protected override void RpcProcessRecord()
        {
            Message[] messages;

            if (Last == -1)
                messages = RpcClient.GetMessages();
            else
            {
                int messageCount = RpcClient.GetMessageCount();

                messages = RpcClient.GetMessages(Math.Max(messageCount - Last, 0));

                if (messages.Length > Last)
                    messages = messages.Skip(messages.Length - Last).ToArray();
            }

            WriteObject(messages, true);
        }
    }
}
