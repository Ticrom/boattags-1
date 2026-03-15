using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace boattags
{
    public class boattagsModSystem : ModSystem
    {
        private Harmony? _harmony;

        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            api.Logger.Notification("Hello from template mod: " + api.Side);
            _harmony = new Harmony("boattags_deconstruct_protect");
            _harmony.PatchAll(typeof(boattagsModSystem).Assembly);
        }

        public override void Dispose()
        {
            _harmony?.UnpatchAll("boattags_deconstruct_protect");
            base.Dispose();
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            api.Logger.Notification("Hello from template mod server side: " + Lang.Get("boattags:hello"));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            api.Logger.Notification("Hello from template mod client side: " + Lang.Get("boattags:hello"));
        }

    }
}
