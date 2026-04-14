using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace vscboattags
{
    public class vscboattagsModSystem : ModSystem
    {
        private Harmony? _harmony;

        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            api.Logger.Notification("Hello from Boat Tags: " + api.Side);
            _harmony = new Harmony("boattags_deconstruct_protect");
            _harmony.PatchAll(typeof(vscboattagsModSystem).Assembly);
        }

        public override void Dispose()
        {
            _harmony?.UnpatchAll("boattags_deconstruct_protect");
            base.Dispose();
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            api.Logger.Notification("Hello from Boat Tags server side: " + Lang.Get("boattags:hello"));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            api.Logger.Notification("Hello from Boat Tags client side: " + Lang.Get("boattags:hello"));
        }

    }
}
