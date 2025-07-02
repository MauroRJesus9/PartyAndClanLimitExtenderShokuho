using HarmonyLib;
using PartyAndClanLimitExtenderShokuho;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace PartyAndClanLimitExtenderShokuho
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            InformationManager.DisplayMessage(new InformationMessage("[Mod] Mod carregado com sucesso!"));

            var harmony = new Harmony("com.iroku.partyandcompanionlimit.shokuho");

            // Aplica patches universais
            harmony.PatchAll();

            // Aplica patches específicos do Shokuho
            ShokuhoPatcher.ApplyPatches(harmony);

            InformationManager.DisplayMessage(new InformationMessage("[Mod] Patches aplicados com sucesso!"));
        }
    }
}
