using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace TypingIndicator
{
    public class EntityBehaviorTypingIndicator : EntityBehavior
    {

        static int typingIndicatorPacketId = 25950;

        ICoreAPI api;

        public bool IsTyping
        {
            get
            {
               return (bool)entity.WatchedAttributes.GetTreeAttribute(PropertyName())?.GetBool("typing");
            }
            set
            {
                entity.WatchedAttributes.GetTreeAttribute(PropertyName())?.SetBool("typing", value);
            }
        }

        public int RenderRange
        {
            get
            {
                return entity.WatchedAttributes.GetTreeAttribute("typingindicator").GetInt("renderRange");
            }
            set
            {
                entity.WatchedAttributes.GetTreeAttribute("typingindicator")?.SetInt("renderRange", value);
            }
        }

        public EntityBehaviorTypingIndicator(Entity entity) : base(entity)
        {
            api = entity.Api;

            entity.WatchedAttributes.SetAttribute(PropertyName(), new TreeAttribute());
            IsTyping = false;
            RenderRange = 100;
        }

        public void MarkDirty()
        {
            entity.WatchedAttributes.MarkPathDirty("typingindicator");
        }


        public override void OnReceivedServerPacket(int packetid, byte[] data, ref EnumHandling handled)
        {
            base.OnReceivedServerPacket(packetid, data, ref handled);

            if(api is ICoreClientAPI capi)
            {
                if(packetid == typingIndicatorPacketId)
                {                    
                    var packet = SerializerUtil.Deserialize<Packet_TypingIndicator>(data);
                    var nearbyPlayers = capi.World.GetPlayersAround(capi.World.Player.Entity.Pos.XYZ, RenderRange, RenderRange).Where(p => p.Entity.EntityId != capi.World.Player.Entity.EntityId);

                    nearbyPlayers.Foreach(p => {
                        if(p.Entity.EntityId == packet.entityId)
                        {
                            p.Entity.GetBehavior<EntityBehaviorTypingIndicator>().IsTyping = packet.isTyping;
                        }
                    });
                    
                }
            }
        }

        public override void OnReceivedClientPacket(IServerPlayer player, int packetid, byte[] data, ref EnumHandling handled)
        {
            base.OnReceivedClientPacket(player, packetid, data, ref handled);

            if(api is ICoreServerAPI sapi)
            {
                if(packetid == typingIndicatorPacketId)
                {                    
                    
                    var packet = SerializerUtil.Deserialize<Packet_TypingIndicator>(data);                 
                    sapi.Network.BroadcastEntityPacket(entity.EntityId, typingIndicatorPacketId, data);
                }
            }
        }

        public override void FromBytes(bool isSync)
        {
            base.FromBytes(isSync);            
        }

        /// <summary>
        /// Sets the value of IsTyping and sends to the server if it's been changed
        /// </summary>
        /// <param name="typing"></param>
        public void SetTyping(bool typing)
        {
            if(IsTyping == typing) return;

            IsTyping = typing;

            MarkDirty();

            if(api is ICoreClientAPI capi)
            {
                var packet = SerializerUtil.Serialize(new Packet_TypingIndicator(){isTyping = typing, entityId = entity.EntityId});

                if(packet == null) capi.ShowChatMessage("The packet was null");

                capi.Network.SendEntityPacket(entity.EntityId, typingIndicatorPacketId, packet);
            }
        }

        public override string PropertyName()
        {
            return "typingindicator";
        }
    }
}