
using ProtoBuf;

namespace TypingIndicator
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Packet_PlayerTyping
    {
        public bool isTyping = false;
        public long entityId;
    }
}