using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace PartyAndClanLimitExtenderShokuho
{
    public class SubModule : MBSubModuleBase
    {
        private static bool _harmonyApplied = false;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            if (!_harmonyApplied)
            {
                var harmony = new Harmony("com.iroku.partyandclanlimitextender.shokuho");

                harmony.PatchAll();
                ShokuhoPatcher.ApplyPatches(harmony);

                _harmonyApplied = true; // Garante que nunca mais aplicas os patches

                //InformationManager.DisplayMessage(new InformationMessage("[Mod] Patches aplicados com sucesso!"));
            }
        }
    }
}
