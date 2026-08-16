using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BetterNPCBoats.BNB_Plugin;

namespace BetterNPCBoats
{
    internal class AddFishingBoats
    {
        internal static void CreateFishingBoats()
        {
            var boatData = new List<FishingBoatData>
            {
                new FishingBoatData(
                    BoatRegion.Alankh, "NPC fishing boat neverdin 200", 200, false, 3,
                    new Vector3(-49111.2f, 0f, -46092.4f),
                    new Vector3(143.6f, 180f, 12f)),
                new FishingBoatData(
                    BoatRegion.Emerald, "NPC fishing boat crab beach 220", 220, false, 11,
                    new Vector3(44114.5f, 150f, -38624f),
                    new Vector3(0f, 180f, 181f)),
                new FishingBoatData(
                    BoatRegion.Aestrin, "NPC fishing boat siren song 240", 240, false, 21,
                    new Vector3(5313f, 10f, 42744f),
                    new Vector3(-75.1f, 270f, 124.8f))
            };

            LogDebug("Creating fishing boats");
            var wasActive = new bool[3];
            for (int i = 0; i < FishingBoatData.BoatTemplates.Length; i++)
            {
                var template = Refs.shiftingWorld.GetComponentsInChildren<Transform>()
                    .FirstOrDefault(bc => bc.name == FishingBoatData.BoatTemplateNames[i]);
                FishingBoatData.BoatTemplates[i] = template.gameObject;
                wasActive[i] = FishingBoatData.BoatTemplates[i].activeSelf;
                FishingBoatData.BoatTemplates[i].SetActive(false);
            }

            foreach (var data in boatData)
                CreateBoat(data);

            FishingBoatData.BoatTemplates[0].SetActive(wasActive[0]);
            FishingBoatData.BoatTemplates[1].SetActive(wasActive[1]);
            FishingBoatData.BoatTemplates[2].SetActive(wasActive[2]);
        }

        private static void CreateBoat(FishingBoatData data)
        {
            LogDebug($"Creating fishing boat: {data.BoatName}");
            var fishingBoat = GameObject.Instantiate(data.BoatTemplate, Refs.shiftingWorld);
            fishingBoat.name = data.BoatName;
            fishingBoat.GetComponent<SaveableObject>().sceneIndex = data.SceneIndex;
            List<Transform> crates = new List<Transform>
            {
                fishingBoat.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "crate (static) (1)"),
                fishingBoat.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "crate (static) (2)"),
                fishingBoat.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "crate (static) (3)"),
                fishingBoat.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "crate (static) (5)")
            };
            crates.ForEach(t => { if (t != null) GameObject.Destroy(t.gameObject); });

            fishingBoat.transform.position = FloatingOriginManager.instance.RealPosToShiftingPos(data.Position);
            fishingBoat.transform.eulerAngles = data.EulerAngles;
            var fishingController = fishingBoat.AddComponent<BetterNPCFishingBoat>();
            var target = new GameObject($"{data.BoatName} target");
            target.transform.SetParent(fishingBoat.transform);
            target.transform.localPosition = data.TargetPos;
            target.transform.localEulerAngles = data.TargetEuler;
            fishingController.target = target.transform;
            fishingController.isNightFisher = data.IsNightFisher;
            fishingController.pilot = fishingBoat.GetComponentsInChildren<Transform>()
                .FirstOrDefault(t => t.name.Contains("Modular NPC")).gameObject;

            LogDebug("Creating lantern");
            var lantern = GameObject.Instantiate(PrefabsDirectory.instance.directory[data.Lantern]);
            if (lantern.name == "111 lantern E yellow(Clone)")
            {
                var meshRenderer = lantern.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
                fishingController.paperShade = meshRenderer;
                fishingController.paperOnMat = meshRenderer.sharedMaterial;
                fishingController.paperOffMat = lantern.GetComponent<ShipItemLight>()
                    .GetPrivateField<Material>("paperOffMat");                
            }
            GameObject.Destroy(lantern.GetComponent<SaveablePrefab>());
            GameObject.Destroy(lantern.GetComponent<HangableItem>());
            GameObject.Destroy(lantern.GetComponent<ShipItemLight>());
            GameObject.Destroy(lantern.GetComponent<BoxCollider>());
            GameObject.Destroy(lantern.GetComponent<Rigidbody>());
            lantern.transform.SetParent(fishingBoat.transform);
            lantern.transform.localPosition = data.LanternPos;
            lantern.transform.localEulerAngles = new Vector3(0f, 90f, 0f);

            LogDebug("Creating fisherman");
            var fisherman = new GameObject("fisherman");
            fisherman.transform.SetParent(fishingBoat.transform);
            fisherman.transform.localPosition = data.FishermanPos;
            fisherman.transform.localEulerAngles = data.FishermanEuler;

            var fishermanNPC = GameObject.Instantiate(Refs.islands[data.NpcIsland].GetComponentsInChildren<Transform>(true)
                .FirstOrDefault(t => t.name == "Modular NPC"));
            fishermanNPC.transform.SetParent(fisherman.transform);
            fishermanNPC.transform.localPosition = Vector3.zero;
            fishermanNPC.transform.localEulerAngles = Vector3.zero;
            fishermanNPC.transform.localScale = new Vector3(1f, 1f, 1f);
            fishermanNPC.name = "Modular NPC";

            var fishingRod = GameObject.Instantiate(PrefabsDirectory.instance.directory[95]);
            GameObject.Destroy(fishingRod.GetComponent<SaveablePrefab>());
            GameObject.Destroy(fishingRod.GetComponent<ShipItemFishingRod>());
            GameObject.Destroy(fishingRod.GetComponent<MeshCollider>());
            GameObject.Destroy(fishingRod.GetComponent<Rigidbody>());
            fishingRod.transform.SetParent(fisherman.transform);
            fishingRod.transform.localPosition = new Vector3(-0.04f, 1.3f, 0.9f);
            fishingRod.transform.localEulerAngles = Vector3.zero;

            fishingController.fisherman = fisherman;

            fishingBoat.SetActive(true);

            LogDebug("Fishing boat created");
        }
    }
}
