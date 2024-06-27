
using ProtoBuf;

namespace TypingIndicator
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Packet_ServerConfig
    {
        public Dictionary<string, string> ServerLocalizations;
        public float MaxTimeout;
        public int MaxRange;
        public string PlayerLanguageCode;
    }
}