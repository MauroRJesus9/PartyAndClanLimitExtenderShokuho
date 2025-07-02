using HarmonyLib;
using MCM.Abstractions.Base.Global;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace PartyAndClanLimitExtenderShokuho
{
    public class ShokuhoPatcher
    {
        public static void ApplyPatches(Harmony harmony)
        {
            var clanTierType = AccessTools.TypeByName("Shokuho.ShokuhoCustomCampaign.Models.ShokuhoClanTierModel");
            if (clanTierType != null)
            {
                var method = AccessTools.Method(clanTierType, "GetPartyLimitForTier");
                var postfix = typeof(PartyLimitPatch).GetMethod("Postfix");
                harmony.Patch(method, postfix: new HarmonyMethod(postfix));
            }

            var workshopType = AccessTools.TypeByName("TaleWorlds.CampaignSystem.GameComponents.ShokuhoWorkshopModel");
            if (workshopType != null)
            {
                var method = AccessTools.Method(workshopType, "GetMaxWorkshopCountForClanTier");
                var postfix = typeof(WorkshopLimitPatch).GetMethod("Postfix");
                harmony.Patch(method, postfix: new HarmonyMethod(postfix));
            }

            var partySizeLimitType = AccessTools.TypeByName("Shokuho.CustomCampaign.CustomLocations.models.ShokuhoPartySizeLimitModel");
            if (partySizeLimitType != null)
            {
                var method = AccessTools.Method(partySizeLimitType, "GetPartyPrisonerSizeLimit");
                var postfix = typeof(PrisonerLimitPatch).GetMethod("Postfix");
                harmony.Patch(method, postfix: new HarmonyMethod(postfix));
            }
        }
    }

    public static class PartyLimitPatch
    {
        public static void Postfix(Clan clan, int clanTierToCheck, ref int __result)
        {
            if (clan == Clan.PlayerClan)
            {
                __result += GlobalSettings<ModSettings>.Instance.ClanPartiesBonus;
            }
        }
    }

    public static class WorkshopLimitPatch
    {
        public static void Postfix(int tier, ref int __result)
        {
            __result += GlobalSettings<ModSettings>.Instance.WorkshopBonus;
        }
    }

    public static class PrisonerLimitPatch
    {
        public static void Postfix(PartyBase party, ref ExplainedNumber __result)
        {
            if (party.MobileParty != null && party.MobileParty == MobileParty.MainParty)
            {
                __result.Add(GlobalSettings<ModSettings>.Instance.PrisonerBonus);
            }
        }
    }

    [HarmonyPatch(typeof(PartyBase), "get_PartySizeLimit")]
    public static class AiPartySizeLimitPatch
    {
        public static void Postfix(PartyBase __instance, ref int __result)
        {
            if (__instance.MobileParty != null && __instance.MobileParty != MobileParty.MainParty)
            {
                var leaderHero = __instance.MobileParty.LeaderHero;

                if (leaderHero != null && !__instance.MobileParty.IsCaravan && !__instance.MobileParty.IsMilitia && !__instance.MobileParty.IsBandit)
                {
                    __result += GlobalSettings<ModSettings>.Instance.AiPartySizeBonus;
                }
            }
        }
    }

    [HarmonyPatch(typeof(PartyBase), "get_PartySizeLimit")]
    public static class PartySizeLimitPatch
    {
        public static void Postfix(PartyBase __instance, ref int __result)
        {
            if (__instance.MobileParty != null && __instance.MobileParty == MobileParty.MainParty)
            {
                __result += GlobalSettings<ModSettings>.Instance.PartyBonus;
            }
        }
    }

    [HarmonyPatch(typeof(Clan), "get_CompanionLimit")]
    public static class ClanCompanionLimitPatch
    {
        public static void Postfix(Clan __instance, ref int __result)
        {
            if (__instance == Clan.PlayerClan)
            {
                __result += GlobalSettings<ModSettings>.Instance.CompanionBonus;
            }
        }
    }
}
