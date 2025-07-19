using HarmonyLib;
using RimWorld;
using Verse;

namespace BluntSocialFighting;

[StaticConstructorOnStartup]
public static class BluntSocialFightingPatcher
{
    private static Harmony Harmony { get; } = new("dani.BluntSocialFighting");

    static BluntSocialFightingPatcher()
    {
        Harmony.Patch(typeof(Verb).GetMethod(nameof(Verb.Available)), postfix: new HarmonyMethod(typeof(BluntSocialFightingPatcher), nameof(IsAvailablePatch)));
        Log.Message($"<color=orange>[DN] Blunt Social Fighting</color> No more sharp attacks during social fights!");
    }

    public static void IsAvailablePatch(Verb __instance, ref bool __result)
    {
        if (__instance.CasterPawn is not Pawn pawn) return;
        if (PawnCanAttackSharp(pawn)) return;

        __result &= __instance.GetDamageDef()?.armorCategory != DamageArmorCategoryDefOf.Sharp;

#if DEBUG
        Log.Message($"Attack with category: '{__instance.GetDamageDef()?.armorCategory.defName ?? "NULL"}' {(__result ? "<color=green>successfully!" : "<color=red>unsuccessfully!")}</color>");
#endif
    }

    private static bool PawnCanAttackSharp(Pawn pawn) => pawn.MentalStateDef != MentalStateDefOf.SocialFighting || pawn.story?.traits.HasTrait(TraitDefOf.Bloodlust) == true;
}
