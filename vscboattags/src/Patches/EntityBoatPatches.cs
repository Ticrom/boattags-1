using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace vscboattags.Patches
{
    /// <summary>
    /// Patches EntityDeconstructTool.OnHeldInteractStart — the actual entry point for
    /// saw-based deconstruction. Blocks non-owners from deconstructing tagged boats.
    /// CivClaims patches the same method for block protection; Harmony runs both
    /// prefixes independently so there is no interference.
    /// </summary>
    [HarmonyPatch(typeof(EntityDeconstructTool), "OnHeldInteractStart")]
    public static class EntityDeconstructToolPatch
    {
        static bool Prefix(ItemSlot slot, EntityAgent byEntity, EntitySelection entitySel,
                            ref EnumHandHandling handling)
        {
            if (byEntity.Api.Side != EnumAppSide.Server) return true;

            var boat = entitySel?.Entity as EntityBoat;
            if (boat == null) return true;

            var ownedByTree = boat.WatchedAttributes.GetTreeAttribute("ownedby");
            string ownerUid = ownedByTree?.GetString("uid") ?? "";

            var ownedByTreeConcrete = ownedByTree as TreeAttribute;
            string keys = ownedByTreeConcrete != null ? string.Join(",", ownedByTreeConcrete.Keys) : "null";
            byEntity.Api.Logger.Notification(
                $"[BoatTags] Deconstruct start on boat — ownerUid=\"{ownerUid}\", ownedByKeys={keys}");

            if (string.IsNullOrEmpty(ownerUid)) return true;

            string playerUid = (byEntity as EntityPlayer)?.PlayerUID;
            if (playerUid == ownerUid) return true;

            handling = EnumHandHandling.PreventDefault;
            ((byEntity as EntityPlayer)?.Player as IServerPlayer)
                ?.SendMessage(GlobalConstants.GeneralChatGroup,
                    "You cannot deconstruct a boat that belongs to someone else.",
                    EnumChatType.Notification);

            return false;
        }
    }
}
