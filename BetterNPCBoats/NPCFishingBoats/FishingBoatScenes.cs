using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterNPCBoats
{
    internal struct ActiveFishingBoatData
    {
        internal Vector3 position;
        internal Vector3 targetPosition;
        internal bool isNightFisher;

        internal ActiveFishingBoatData(Vector3 position, Vector3 targetPosition,  bool isNightFisher = false)
        {
            this.position = position;
            this.targetPosition = targetPosition;
            this.isNightFisher = isNightFisher;
        }
    }
    internal class FishingBoatScenes
    {
        internal static Stack<GameObject> AAFishingBoats { get; } = new Stack<GameObject>();
        internal static Stack<GameObject> EAFishingBoats { get; } = new Stack<GameObject>();
        internal static Stack<GameObject> AeFishingBoats { get; } = new Stack<GameObject>();
        private static List<GameObject> activeFishingBoats = new List<GameObject>();

        private static Dictionary<int, List<ActiveFishingBoatData>> dataDict = new Dictionary<int, List<ActiveFishingBoatData>>
        {
            {
                // neverdin
                3, new List<ActiveFishingBoatData>
                {
                    new ActiveFishingBoatData(new Vector3(-82f, 0f, 382f), new Vector3(300f, 180f, 470f))
                }
            },
            {
                // gold rock city
                1, new List<ActiveFishingBoatData>
                {
                    new ActiveFishingBoatData(new Vector3(1169.6f, 176f, -590.8f), new Vector3(454.5f, 90f, -542.5f)),
                    new ActiveFishingBoatData(new Vector3(998.8f, 176f, -471.7f), new Vector3(445f, 90f, -581.7f), true),
                    new ActiveFishingBoatData(new Vector3(891.1f, 176f, -395.2f), new Vector3(150.6f, 70f, -326.7f)),
                    new ActiveFishingBoatData(new Vector3(822.6f, 176f, -372.9f), new Vector3(293.7f, 100f, -436f)),
                    new ActiveFishingBoatData(new Vector3(767.2f, 176f, -322.5f), new Vector3(257.5f, 270f, -201.8f), true)
                }
            },
            {
                // dragon cliffs
                9, new List<ActiveFishingBoatData>
                {
                    new ActiveFishingBoatData(new Vector3(-250.8f, 170f, -630.2f), new Vector3(11f, 210f, -1016.2f), true),
                    new ActiveFishingBoatData(new Vector3(-239.4f, 170f, -659.9f), new Vector3(-18.8f, 20f, -891.9f), true),
                    new ActiveFishingBoatData(new Vector3(-245.3f, 190f, -554.3f), new Vector3(-172.8f, 172f, -770.2f), true),
                    new ActiveFishingBoatData(new Vector3(-236.3f, 170f, -689.1f), new Vector3(-2.1f, 62f, -936.6f), true)
                }
            },
            {
                // sanctuary
                10, new List<ActiveFishingBoatData>
                {
                    new ActiveFishingBoatData(new Vector3(203.1f, 10f, 41.5f), new Vector3(459.4f, 180f, -42.4f))
                }
            },
            {
                // crab beach
                11, new List<ActiveFishingBoatData>
                {
                    new ActiveFishingBoatData(new Vector3(595.2f, 190f, 110.6f), new Vector3(426f, 180f, 471.5f)),
                    new ActiveFishingBoatData(new Vector3(754.7f, 190f, 214.6f), new Vector3(784.1f, 260f, 562.7f)),
                    new ActiveFishingBoatData(new Vector3(692.2f, 190f, 185f), new Vector3(1012.2f, 62f, 600f), true),
                }
            },
            {
                // new port
                12, new List<ActiveFishingBoatData>
                {
                    new ActiveFishingBoatData(new Vector3(439.4f, 70f, -234.9f), new Vector3(621f, 0f, 221f))
                }
            },
            {
                // sage hills
                13, new List<ActiveFishingBoatData>
                {
                    new ActiveFishingBoatData(new Vector3(-91.7f, 70f, -227.1f), new Vector3(-132.8f, 270f, -309.9f))
                }
            },
            {
                // siren song
                21, new List<ActiveFishingBoatData>
                {
                    new ActiveFishingBoatData(new Vector3(-171f, 260f, 11.9f), new Vector3(-349.1f, 100f, 86.3f)),
                }
            },
            {
                // ft. aestrin
                15, new List<ActiveFishingBoatData>
                {
                    new ActiveFishingBoatData(new Vector3(106f, 10f, 80f), new Vector3(144.9f, 80f, 300.6f)),
                    new ActiveFishingBoatData(new Vector3(131f, 260f, 65f), new Vector3(183f, 40f, 278.5f)),
                    new ActiveFishingBoatData(new Vector3(167f, 290f, 59f), new Vector3(220f, 60f, 215.2f)),
                    new ActiveFishingBoatData(new Vector3(195f, 240f, 26f), new Vector3(281.4f, 40f, 138.7f), true),
                    new ActiveFishingBoatData(new Vector3(227f, 290f, -30f), new Vector3(357.3f, 60f, -6.4f), true),
                }
            }
        };

        internal static void SceneLoaded(Scene scene, LoadSceneMode _)
        {
            var index = scene.buildIndex;
            if (!dataDict.ContainsKey(index))
                return;

            var island = Refs.islands[index];
            var region = island.gameObject.GetComponentInChildren<Port>().region;
            foreach( var data in dataDict[index])
            {
                GameObject boat;
                if (region == PortRegion.alankh)
                    boat = AAFishingBoats.Pop();
                else if (region == PortRegion.emerald)
                    boat = EAFishingBoats.Pop();
                else
                    boat = AeFishingBoats.Pop();
                boat.transform.SetParent(island);
                boat.transform.localPosition = new Vector3(data.position.x, 0f, data.position.z);
                boat.transform.localEulerAngles = new Vector3(180f, data.position.y, 180f);
                var controller = boat.GetComponent<BetterNPCFishingBoat>();
                controller.isNightFisher = data.isNightFisher;
                var target = new GameObject($"target {boat.name}");
                target.transform.SetParent(island);                
                target.transform.localPosition = new Vector3(data.targetPosition.x, 0f, data.targetPosition.z);
                target.transform.localEulerAngles = new Vector3(180f, data.targetPosition.y, 180f);
                controller.target = target.transform;
                controller.sceneIndex = index;
                activeFishingBoats.Add(boat);
                boat.SetActive(true);
            }
        }

        internal static void SceneUnloaded(Scene scene)
        {
            var index = scene.buildIndex;
            if (!dataDict.ContainsKey(index))
                return;

            var island = Refs.islands[index];
            var region = island.gameObject.GetComponentInChildren<Port>().region;

            for (int i = activeFishingBoats.Count - 1; i >= 0; i--)
            {
                var boat = activeFishingBoats[i];
                var controller = boat.GetComponent<BetterNPCFishingBoat>();

                if (controller.sceneIndex != index)
                    continue;

                if (region == PortRegion.alankh)
                    AAFishingBoats.Push(boat);
                else if (region == PortRegion.emerald)
                    EAFishingBoats.Push(boat);
                else
                    AeFishingBoats.Push(boat);

                activeFishingBoats.RemoveAt(i);
                controller.sceneIndex = -1;
                boat.SetActive(false);
                boat.transform.SetParent(null);
            }
        }
    }
}
