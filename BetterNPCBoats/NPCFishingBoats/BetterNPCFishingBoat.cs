using UnityEngine;
using static BetterNPCBoats.BNB_Plugin;

namespace BetterNPCBoats
{
    internal class BetterNPCFishingBoat : MonoBehaviour
    {
        public Transform target;
        private Vector3 fishingPos;
        private Vector3 parkedPos;
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
        private float timeAdjustment;

        public float speed = 1f;
        public float turnSpeed = 1f;
        public float sailSpeed = 1f;
        public float sailResistance = 5f;
        public RopeControllerSailAngle[] sailAngleControllers;
        public RopeControllerSailReef[] sailReefControllers;
        public Transform currentTarget;
        private bool otherBoatInRange;
        private float boatColCheckTimer;
        private Rigidbody rigidbody;
        private Collider col;
        internal int sceneIndex;

        private void Start()
        {
            LogDebug($"BetterNPCFishingBoat Start: {gameObject.name} in scene {sceneIndex}");
            col = GetComponent<Collider>();
            rigidbody = GetComponent<Rigidbody>();
            fishingPos = target.localPosition;
            parkedPos = transform.localPosition;
            timeAdjustment = Random.Range(-0.4f, 0.4f);
            GoHome();
        }

        private void CheckOtherBoatCol()
        {
            boatColCheckTimer = Random.Range(0.5f, 1.5f);
            otherBoatInRange = false;
            var array = Physics.OverlapSphere(transform.position, 15f);
            foreach (var collider in array)
            {
                if (collider.CompareTag("Boat") && collider != col)
                    otherBoatInRange = true;
            }
        }

        private void FixedUpdate()
        {
            if (!GameState.playing)
                return;
            if (boatColCheckTimer <= 0f)
                CheckOtherBoatCol();
            else
                boatColCheckTimer -= Time.fixedDeltaTime;

            if (otherBoatInRange)
                return;

            if ((bool)currentTarget)
            {
                AddForceTowards(currentTarget);
                AddRotationTowards(currentTarget);
            }
        }

        private void Update()
        {
            if (!GameState.playing)
                return;
            var time = Sun.sun.localTime + timeAdjustment;
            var isDayTime = time > 5.5f && time < 17.5f;

            HasReachedTarget();

            if (Storm.IsNearby)
            {
                if (goingFishing)
                    GoHome();
            }
            else
            {
                var shouldBeFishing = isDayTime != isNightFisher;

                if (goingFishing != shouldBeFishing)
                {
                    if (shouldBeFishing)
                        GoFishing();
                    else
                        GoHome();
                }
            }

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

            if (currentTarget != null)
            {
                var array = sailReefControllers;
                foreach (var ropeControllerSailReef in array)
                {
                    ropeControllerSailReef.currentLength += 0.15f * Time.deltaTime;
                    if (ropeControllerSailReef.currentLength > 1f)
                        ropeControllerSailReef.currentLength = 1f;
                }
            }
            else
            {
                var array = sailReefControllers;
                foreach (var ropeControllerSailReef2 in array)
                {
                    ropeControllerSailReef2.currentLength -= 0.15f * Time.deltaTime;
                    if (ropeControllerSailReef2.currentLength < 0f)
                        ropeControllerSailReef2.currentLength = 0f;
                }
            }

            if (currentTarget == null)
                return;

            var array2 = sailAngleControllers;
            foreach (var ropeController in array2)
            {
                ropeController.changed = true;
                if (ropeController.currentResistance > Wind.currentWind.magnitude)
                {
                    ropeController.currentLength += sailSpeed * Time.deltaTime * 0.05f;
                    if (ropeController.currentLength > 1f)
                        ropeController.currentLength = 1f;
                }
                else
                {
                    ropeController.currentLength -= sailSpeed * Time.deltaTime * 0.05f;
                    if (ropeController.currentLength < 0f)
                        ropeController.currentLength = 0f;
                }
            }
        }

        private void AddForceTowards(Transform target)
        {
            var toTarget = target.position - transform.position;
            var distance = toTarget.magnitude;
            var normalized = toTarget / distance;

            var decelRadius = 20f;
            var speedScale = Mathf.Clamp01(distance / decelRadius);

            var num = Wind.currentWind.magnitude * speed * 0.05f;
            rigidbody.AddForce(normalized * (speed + num) * speedScale * rigidbody.mass * 1f);
        }

        private void AddRotationTowards(Transform target)
        {
            var normalized = (target.position - transform.position).normalized;
            var angle = Vector3.SignedAngle(transform.forward, normalized, Vector3.up);

            // Proportional term: scale torque by how far off-heading we are
            var angleTorque = angle * turnSpeed * 0.5f;

            // Derivative term: damp existing yaw rate so we don't overshoot/oscillate
            var yawRate = rigidbody.angularVelocity.y * Mathf.Rad2Deg;
            var dampingTorque = -yawRate * turnSpeed * 0.1f;

            rigidbody.AddTorque(Vector3.up * (angleTorque + dampingTorque) * rigidbody.mass);
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
            target.transform.localPosition = fishingPos;
            currentTarget = target;
            goingFishing = true;
        }

        private void GoHome()
        {
            target.transform.localPosition = parkedPos;
            currentTarget = target;
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
                currentTarget = null;
                if (goingFishing)
                    atFishingSpot = true;
                else
                    atParkingSpot = true;
            }
            else
            {
                atFishingSpot = false;
                atParkingSpot = false;
            }
        }
    }
}
