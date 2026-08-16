using UnityEngine;

namespace BetterNPCBoats
{
    internal class BetterNPCFishingBoat : MonoBehaviour
    {
        public Transform target;
        private NPCBoatController controller;
        private Vector3 fishingRealPos;
        private Vector3 parkedRealPos;
        private bool goingFishing;
        private bool atFishingSpot;
        private bool atParkingSpot;
        public bool isNightFisher;
        internal GameObject pilot;
        internal GameObject fisherman;
        internal MeshRenderer paperShade;
        internal Material paperOffMat;
        internal Material paperOnMat;
        private bool lanternsOn = true;
        private float arrivalDistanceSqr = 9f;

        private void Start()
        {
            controller = GetComponent<NPCBoatController>();
            fishingRealPos = FloatingOriginManager.instance.ShiftingPosToRealPos(target.position);
            parkedRealPos = FloatingOriginManager.instance.ShiftingPosToRealPos(base.transform.position);
            target.parent = base.transform.parent;
            GoHome();
        }

        private void Update()
        {
            var isDayTime = Sun.sun.localTime > 5.5f && Sun.sun.localTime < 17.5f;
            HasReachedTarget();

            if (isDayTime && !isNightFisher)
            {
                if (!goingFishing)
                    GoFishing();
            }
            else if (!isDayTime && isNightFisher)
            {
                if (!goingFishing)
                    GoFishing();
            }
            else if (goingFishing)
                GoHome();

            if (isDayTime || atParkingSpot)
                ToggleLanterns(false);
            else
                ToggleLanterns(true);

            if (atFishingSpot)
            {
                pilot.SetActive(false);
                fisherman.SetActive(true);
            }
            else if (atParkingSpot)
            {
                pilot.SetActive(false);
                fisherman.SetActive(false);
            }
            else
            {
                pilot.SetActive(true);
                fisherman.SetActive(false);
            }
        }

        private void ToggleLanterns(bool state)
        {
            if (lanternsOn == state)
                return;

            lanternsOn = state;
            var lights = GetComponentsInChildren<Light>();
            foreach (var light in lights)
            {
                light.enabled = state;
            }

            var particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particleSystems)
            {
                var emissionModule = ps.emission;
                emissionModule.enabled = state;
            }

            if (paperShade != null)
            {
                paperShade.sharedMaterial = state ? paperOnMat : paperOffMat;
            }
        }

        private void GoFishing()
        {
            target.transform.localPosition = FloatingOriginManager.instance.RealPosToShiftingPos(fishingRealPos);
            controller.currentDock = null;
            controller.currentTarget = target;
            goingFishing = true;
        }

        private void GoHome()
        {
            target.transform.localPosition = FloatingOriginManager.instance.RealPosToShiftingPos(parkedRealPos);
            controller.currentDock = null;
            controller.currentTarget = target;
            goingFishing = false;
        }

        private void HasReachedTarget()
        {
            if (target == null)
                return;
            if ((atFishingSpot && goingFishing) || (atParkingSpot && !goingFishing))
                return;


            var offset = transform.position - target.position;
            var hasReached = offset.sqrMagnitude <= arrivalDistanceSqr;
            if (hasReached)
            {
                controller.currentTarget = null;
                //arrivalDistanceSqr = 16f;
                if (goingFishing)
                    atFishingSpot = true;
                else
                    atParkingSpot = true;
            }
            else
            {
                atFishingSpot = false;
                atParkingSpot = false;
                //arrivalDistanceSqr = 9f;
            }
        }
    }
}
