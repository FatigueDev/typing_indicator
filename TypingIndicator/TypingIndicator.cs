using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace TypingIndicator
{
    class TypingIndicator : ModSystem
    {
        ICoreClientAPI? capi;

        public override void Start(ICoreAPI api)
        {
            api.RegisterEntityBehaviorClass("typingindicator", typeof(EntityBehaviorTypingIndicator));

            base.Start(api);
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            capi = api;

            api.Event.PlayerEntitySpawn += (player) =>
            {
                if(api.Side != EnumAppSide.Client) return;

                player.Entity.AddBehavior(new EntityBehaviorTypingIndicator(player.Entity));
                api.Event.RegisterRenderer(new EntityTypingIndicatorRenderer(api, player.Entity), EnumRenderStage.Ortho);

                api.Event.RegisterGameTickListener((dt) =>
                {
                    if(player == api.World.Player)
                    {
                        var openedDialogs = from g in api.Gui.OpenedGuis select g;
                        var hudDialogChat = (from d in openedDialogs where d.GetType() == typeof(HudDialogChat) select d).SingleOrDefault();

                        if(hudDialogChat == null) return;

                        var chatInput = hudDialogChat.Composers["chat"].GetChatInput("chatinput");

                        if(chatInput.HasFocus && !string.IsNullOrEmpty(chatInput.GetText()))
                        {
                            player.Entity.GetBehavior<EntityBehaviorTypingIndicator>().SetTyping(true);
                        }
                        else
                        {
                            player.Entity.GetBehavior<EntityBehaviorTypingIndicator>().SetTyping(false);
                        }
                    }                    
                }, 1000);
            };

            base.StartClientSide(api);
        }
    }
}
