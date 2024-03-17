using HarmonyLib;
using RimWorld;
using Verse;

namespace BluntSocialFighting
{
    [StaticConstructorOnStartup]
    public static class BluntSocialFightingPatcher
    {
        private static readonly Harmony harmony = new Harmony("dani.BluntSocialFighting");

        static BluntSocialFightingPatcher()
        {
            harmony.Patch(typeof(Verb).GetMethod(nameof(Verb.Available)), postfix: new HarmonyMethod(typeof(BluntSocialFightingPatcher), nameof(IsAvailablePatch)));
            Log.Message($"<color=orange>[DN] Blunt Social Fighting</color> No more sharp attacks during social fights!");
        }

        public static void IsAvailablePatch(Verb __instance, ref bool __result)
        {
            if (!(__instance.CasterPawn is Pawn pawn)) return;
            if (PawnCanAttackSharp(pawn)) return;

            __result &= __instance.GetDamageDef()?.armorCategory != DamageArmorCategoryDefOf.Sharp;
            //Log.Message($"Attack with {__instance.GetDamageDef()?.LabelCap ?? "NULL"} attacked {(__result ? "successfully!" : "unsuccessfully!")}");
        }

        private static bool PawnCanAttackSharp(Pawn pawn) => pawn.MentalStateDef != MentalStateDefOf.SocialFighting || pawn.story?.traits.HasTrait(TraitDefOf.Bloodlust) == true;
    }
}
