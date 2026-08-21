using cakeslice;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BetterNPCBoats.BNB_Plugin;

namespace BetterNPCBoats
{
    internal class FishingBoats
    {
        internal static void Initialize()
        {
            LogDebug("Creating fishing boats");
            var wasActive = new bool[3];
            for (int i = 0; i < FishingBoatData.BoatTemplates.Length; i++)
            {
                var template = Refs.shiftingWorld.GetComponentsInChildren<Transform>(true)
                    .FirstOrDefault(bc => bc.name == FishingBoatData.BoatTemplateNames[i]);
                FishingBoatData.BoatTemplates[i] = template.gameObject;
                wasActive[i] = FishingBoatData.BoatTemplates[i].activeSelf;
                FishingBoatData.BoatTemplates[i].SetActive(false);
            }

            for (int i = 0; i < 10; i++)
            {
                FishingBoatScenes.AAFishingBoats.Push(CreateBoat(new FishingBoatData(BoatRegion.Alankh, 10 - i)));
                FishingBoatScenes.EAFishingBoats.Push(CreateBoat(new FishingBoatData(BoatRegion.Emerald, 10 - i)));
                FishingBoatScenes.AeFishingBoats.Push(CreateBoat(new FishingBoatData(BoatRegion.Aestrin, 10 - i)));
            }

            FishingBoatData.BoatTemplates[0].SetActive(wasActive[0]);
            FishingBoatData.BoatTemplates[1].SetActive(wasActive[1]);
            FishingBoatData.BoatTemplates[2].SetActive(wasActive[2]);
        }

        private static GameObject CreateBoat(FishingBoatData data)
        {
            try
            {
                var fishingBoat = GameObject.Instantiate(data.BoatTemplate);
                fishingBoat.name = data.BoatName;
                GameObject.Destroy(fishingBoat.GetComponent<SaveableObject>());

                List<Transform> crates = new List<Transform>
                {
                    fishingBoat.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "crate (static) (1)"),
                    fishingBoat.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "crate (static) (2)"),
                    fishingBoat.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "crate (static) (3)"),
                    fishingBoat.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "crate (static) (5)")
                };
                crates.ForEach(t => { if (t != null) GameObject.Destroy(t.gameObject); });
                fishingBoat.GetComponentsInChildren<ShipyardSailColChecker>().ToList().ForEach(c => GameObject.Destroy(c));
                fishingBoat.GetComponentsInChildren<ShipyardSailColCheckerSub>().ToList().ForEach(c => GameObject.Destroy(c));
                fishingBoat.GetComponentsInChildren<Outline>().ToList().ForEach(c => GameObject.Destroy(c));

                var oldController = fishingBoat.GetComponent<NPCBoatController>();
                var fishingController = fishingBoat.AddComponent<BetterNPCFishingBoat>();
                fishingController.sailAngleControllers = oldController.sailAngleControllers;
                fishingController.sailReefControllers = oldController.sailReefControllers;
                GameObject.Destroy(oldController);
                fishingController.pilot = fishingBoat.GetComponentsInChildren<Transform>()
                    .FirstOrDefault(t => t.name.Contains("Modular NPC")).gameObject;

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

                return fishingBoat;
            }
            catch (Exception ex)
            {
                LogError($"boat: {data.BoatName}\nexception: {ex.Message}");
                return null;
            }
        }
    }
}
