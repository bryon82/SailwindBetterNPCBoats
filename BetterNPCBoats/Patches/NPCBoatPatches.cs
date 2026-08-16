using HarmonyLib;
using UnityEngine;

namespace BetterNPCBoats
{
    internal class NPCBoatPatches
    {
        [HarmonyPatch(typeof(NPCBoatController), "AddRotationTowards")]
        private class NPCBoatControllerPatches
        {
            public static bool Prefix(NPCBoatController __instance, Transform target, Rigidbody ___rigidbody)
            {
                Vector3 normalized = (target.position - __instance.transform.position).normalized;
                float angle = Vector3.SignedAngle(__instance.transform.forward, normalized, Vector3.up);

                // Proportional term: scale torque by how far off-heading we are
                float angleTorque = angle * __instance.turnSpeed * 0.5f;

                // Derivative term: damp existing yaw rate so we don't overshoot/oscillate
                float yawRate = ___rigidbody.angularVelocity.y * Mathf.Rad2Deg;
                float dampingTorque = -yawRate * __instance.turnSpeed * 0.1f;

                ___rigidbody.AddTorque(Vector3.up * (angleTorque + dampingTorque) * ___rigidbody.mass);
                return false;
            }
        }
    }
}
