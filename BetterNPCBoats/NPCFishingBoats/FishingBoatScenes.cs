using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterNPCBoats
{    
    internal class FishingBoatScenes
    {
        internal static Stack<GameObject> AAFishingBoats { get; } = new Stack<GameObject>();
        internal static Stack<GameObject> EAFishingBoats { get; } = new Stack<GameObject>();
        internal static Stack<GameObject> AeFishingBoats { get; } = new Stack<GameObject>();
        private static List<GameObject> activeFishingBoats = new List<GameObject>();

        private static Dictionary<int, List<ActiveFBData>> fishingBoatScenes = new Dictionary<int, List<ActiveFBData>>
        {
            {
                // gold rock city
                1, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(1169.6f, 176f, -590.8f), new Vector3(454.5f, 90f, -542.5f)),
                    new ActiveFBData(new Vector3(998.8f, 176f, -471.7f), new Vector3(445f, 90f, -581.7f), true),
                    new ActiveFBData(new Vector3(891.1f, 176f, -395.2f), new Vector3(150.6f, 70f, -326.7f)),
                    new ActiveFBData(new Vector3(822.6f, 176f, -372.9f), new Vector3(293.7f, 100f, -436f)),
                    new ActiveFBData(new Vector3(767.2f, 176f, -322.5f), new Vector3(257.5f, 270f, -201.8f), true)
                }
            },
            {
                // al'nilem
                2, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(174.3f, 100f, 262f), new Vector3(226.3f, 240f, 557f)),
                    new ActiveFBData(new Vector3(-68.4f, 10f, -50.1f), new Vector3(-333f, 70f, -265f), true)
                }
            },
            {
                // neverdin
                3, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-82f, 0f, 382f), new Vector3(300f, 180f, 470f))
                }
            },
            {
                // albacore town
                4, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(195.8f, 160f, 28.7f), new Vector3(-59.7f, 40f, 756.5f)),
                    new ActiveFBData(new Vector3(-166f, 180f, -213f), new Vector3(-462.4f, 80f, 441.3f)),
                    new ActiveFBData(new Vector3(-123.3f, 170f, -361.2f), new Vector3(-558.7f, 60f, -99.2f)),
                    new ActiveFBData(new Vector3(-15.9f, 60f, -512.9f), new Vector3(-372.7f, 60f, -718.9f), true)
                }
            },
            {
                // dragon cliffs
                9, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-250.8f, 170f, -630.2f), new Vector3(11f, 210f, -1016.2f), true),
                    new ActiveFBData(new Vector3(-239.4f, 170f, -659.9f), new Vector3(-18.8f, 20f, -891.9f), true),
                    new ActiveFBData(new Vector3(-245.3f, 190f, -554.3f), new Vector3(-172.8f, 172f, -770.2f), true),
                    new ActiveFBData(new Vector3(-236.3f, 170f, -689.1f), new Vector3(-2.1f, 62f, -936.6f), true)
                }
            },
            {
                // sanctuary
                10, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(203.1f, 10f, 41.5f), new Vector3(459.4f, 180f, -42.4f))
                }
            },
            {
                // crab beach
                11, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(595.2f, 190f, 110.6f), new Vector3(426f, 180f, 471.5f)),
                    new ActiveFBData(new Vector3(754.7f, 190f, 214.6f), new Vector3(784.1f, 260f, 562.7f)),
                    new ActiveFBData(new Vector3(692.2f, 190f, 185f), new Vector3(1012.2f, 62f, 600f), true),
                }
            },
            {
                // new port
                12, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(439.4f, 70f, -234.9f), new Vector3(621f, 0f, 221f))
                }
            },
            {
                // sage hills
                13, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-91.7f, 70f, -227.1f), new Vector3(-132.8f, 270f, -309.9f)),
                    new ActiveFBData(new Vector3(-74.6f, 340f, -337f), new Vector3(-87.1f, 100f, -570.6f)),
                    new ActiveFBData(new Vector3(57.5f, 280f, -392.8f), new Vector3(191.7f, 10f, -552.5f), true)
                }
            },
            {
                // ft. aestrin
                15, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(106f, 10f, 80f), new Vector3(144.9f, 80f, 300.6f)),
                    new ActiveFBData(new Vector3(131f, 260f, 65f), new Vector3(183f, 40f, 278.5f)),
                    new ActiveFBData(new Vector3(167f, 290f, 59f), new Vector3(220f, 60f, 215.2f)),
                    new ActiveFBData(new Vector3(195f, 240f, 26f), new Vector3(281.4f, 40f, 138.7f), true),
                    new ActiveFBData(new Vector3(227f, 290f, -30f), new Vector3(357.3f, 60f, -6.4f), true),
                }
            },
            {
                // sunspire
                16, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-593.9f, 290f, -436.8f), new Vector3(-723.6f, 30f, -801.5f)),
                    new ActiveFBData(new Vector3(-572.1f, 270f, -431.6f), new Vector3(-430.7f, 1f, -599.1f), true),
                }
            },
            {
                // mt malefic
                17, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-579.4f, 160f, 268.4f), new Vector3(-729.9f, 220f, -67.4f)),
                    new ActiveFBData(new Vector3(-482.2f, 90f, 406.5f), new Vector3(-234.7f, 20f, 813.2f)),
                    new ActiveFBData(new Vector3(-556.8f, 160f, 210.7f), new Vector3(-765.5f, 211f, -37.9f), true),
                }
            },
            {
                // happy bay
                18, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-4.7f, 81f, 20.92f), new Vector3(367.3f, 130f, -1.1f)),
                    new ActiveFBData(new Vector3(-27f, 81f, 18.85f), new Vector3(93.28f, 180f, -59.03f), true),
                    new ActiveFBData(new Vector3(-16.6f, 90f, 85.8f), new Vector3(129.3f, 310f, 346.2f)),
                }
            },
            {
                // eastwind
                19, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(332.3f, 0f, -67.1f), new Vector3(603.9f, 90f, 13.7f)),
                }
            },
            {
                // oasis
                20, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(112.2f, 364f, -318f), new Vector3(145.5f, 270f, -831.7f)),
                    new ActiveFBData(new Vector3(-141.5f, 270f, -528.4f), new Vector3(-321f, 60f, -851.6f)),
                    new ActiveFBData(new Vector3(129.6f, 120f, 34.1f), new Vector3(772.1f, 240f, 305.6f)),
                    new ActiveFBData(new Vector3(48.9f, 200f, 191.6f), new Vector3(753.6f, 260f, 160.6f), true),
                }
            },
            {
                // siren song
                21, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-171f, 260f, 11.9f), new Vector3(-349.1f, 100f, 86.3f)),
                }
            },
            {
                // fire fish town
                26, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-176.53f, 156f, -118.04f), new Vector3(-311.8f, 90f, -144.4f)),
                    new ActiveFBData(new Vector3(63.16f, 240f, -73.4f), new Vector3(288.5f, 340f, -342.1f)),
                    new ActiveFBData(new Vector3(-130.8f, 60f, -165f), new Vector3(-314.39f, 140f, -271f), true),
                }
            },
            {
                // kicia bay
                27, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-88.5f, 140f, 81.3f), new Vector3(-325.17f, 210f, 246.65f)),
                    new ActiveFBData(new Vector3(-205.93f, 120f, 230.59f), new Vector3(-471.9f, 160f, 399.2f)),
                    new ActiveFBData(new Vector3(-63.2f, 160f, 32.2f), new Vector3(-316f, 60f, -45.64f), true),
                }
            },
            {
                // senna
                28, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-85.54f, 80f, 147.71f), new Vector3(26.2f, 260f, 295.2f), true),
                    new ActiveFBData(new Vector3(-163.6f, 240f, 47.2f), new Vector3(-377.2f, 60f, -66.4f), true),
                }
            },
            {
                // onna
                29, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(17.7f, 80f, 60.9f), new Vector3(119.8f, 200f, 238.5f)),
                }
            },
            {
                // firefly grotto
                33, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-13.27f, 156f, -1086.42f), new Vector3(22.6f, 100f, 205.1f)),
                }
            },
            {
                // dead cove
                37, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(78, 270f, -200.8f), new Vector3(36.7f, 180f, -411.2f)),
                }
            },
            {
                // turtle island
                38, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(-31.4f, 160f, -72f), new Vector3(-113.1f, 220f, -242.7f)),
                }
            },
            {
                // old ankh
                40, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(97.3f, 300f, 43f), new Vector3(305f, 350f, 53f)),
                    new ActiveFBData(new Vector3(242.1f, 320f, -58.7f), new Vector3(369.6f, 160f, 32.3f)),
                    new ActiveFBData(new Vector3(262.2f, 300f, -85.5f), new Vector3(395.5f, 280f, -140.5f), true),
                }
            },
            {
                // saffron island
                42, new List<ActiveFBData>
                {
                    new ActiveFBData(new Vector3(223.74f, 180f, 102.9f), new Vector3(433f, 240f, 112f)),
                }
            }
        };

        internal static void SceneLoaded(Scene scene, LoadSceneMode _)
        {
            var index = scene.buildIndex;
            if (!fishingBoatScenes.ContainsKey(index))
                return;

            var island = Refs.islands[index];
            var region = island.gameObject.GetComponentInChildren<Port>().region;
            foreach (var data in fishingBoatScenes[index])
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
                boat.SetActive(true);

                var controller = boat.GetComponent<BetterNPCFishingBoat>();
                controller.isNightFisher = data.isNightFisher;                
                controller.target.SetParent(island);                
                controller.target.localPosition = new Vector3(data.targetPosition.x, 0f, data.targetPosition.z);
                controller.target.localEulerAngles = new Vector3(180f, data.targetPosition.y, 180f);
                controller.sceneIndex = index;
                controller.Init();
                activeFishingBoats.Add(boat);
                
            }
        }

        internal static void SceneUnloaded(Scene scene)
        {
            var index = scene.buildIndex;
            if (!fishingBoatScenes.ContainsKey(index))
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

    internal struct ActiveFBData
    {
        internal Vector3 position;
        internal Vector3 targetPosition;
        internal bool isNightFisher;

        internal ActiveFBData(Vector3 position, Vector3 targetPosition, bool isNightFisher = false)
        {
            this.position = position;
            this.targetPosition = targetPosition;
            this.isNightFisher = isNightFisher;
        }
    }
}
