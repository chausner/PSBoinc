using BoincRpc;

namespace PSBoinc
{
    public class BoincSession
    {
        public RpcClient RpcClient { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public bool Authenticated { get; internal set; }

        internal BoincSession(string host, int port)
        {
            RpcClient = new RpcClient();
            Host = host;
            Port = port;
            Authenticated = false;
        }
    }
}
