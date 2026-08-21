using HarmonyLib;

namespace BetterNPCBoats
{
    internal class Storm
    {
        internal static bool IsNearby { get; private set; }

        [HarmonyPatch(typeof(WeatherStorms), "GetNormalizedDistance")]
        internal static class GetNormalizedDistancePatch
        {
            public static void Postfix(float __result)
            {
                    IsNearby = __result < 0.33f;
            }
        }
    }
}
