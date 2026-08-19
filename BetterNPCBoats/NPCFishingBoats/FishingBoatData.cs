using UnityEngine;

namespace BetterNPCBoats
{
    internal class FishingBoatData
    {
        internal static string[] BoatTemplateNames { get; } = new string[3]
        {
            "BOAT NPC dhow small (100)",
            "BOAT NPC junk small (110)",
            "BOAT NPC medi small (120)"
        };

        internal static GameObject[] BoatTemplates { get; } = new GameObject[3];

        public GameObject BoatTemplate { get; private set; }
        public string BoatName { get; private set; }
        public int NpcIsland { get; private set; }
        public int Lantern { get; private set; }
        public Vector3 LanternPos { get; private set; }
        public Vector3 FishermanPos { get; private set; }
        public Vector3 FishermanEuler { get; private set; }

        public FishingBoatData(BoatRegion region, int boatNumber)
        {
            if (region == BoatRegion.Alankh)
            {
                Lantern = 110;
                BoatTemplate = BoatTemplates[0];
                FishermanPos = new Vector3(-1f, 2.02f, -0.019f);
                FishermanEuler = new Vector3(0f, 270f, 0f);
                LanternPos = new Vector3(-1.3f, 3.8f, -1.7f);
                NpcIsland = 1;
                BoatName = $"alankh NPC fishing boat {boatNumber}";
            }
            else if (region == BoatRegion.Emerald)
            {
                Lantern = 111;
                BoatTemplate = BoatTemplates[1];
                FishermanPos = new Vector3(1.068f, 2.147f, 1.087f);
                FishermanEuler = new Vector3(0f, 90f, 0f);
                LanternPos = new Vector3(0f, 4.431f, 0.212f);
                NpcIsland = 9;
                BoatName = $"emerald NPC fishing boat {boatNumber}";
            }
            else
            {
                Lantern = 114;
                BoatTemplate = BoatTemplates[2];
                FishermanPos = new Vector3(-1.518f, 1.365f, -0.87f);
                FishermanEuler = new Vector3(0f, 270f, 0f);
                LanternPos = new Vector3(-0.306f, 3.506f, -0.083f);
                NpcIsland = 15;
                BoatName = $"aestrin NPC fishing boat {boatNumber}";
            }
        }
    }
}
