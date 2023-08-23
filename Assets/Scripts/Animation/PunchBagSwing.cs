using Managers;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Animation {
    // Applies a torque to the punching bag when punched
    // Rotates the punching bag using the rigidbody
    public class PunchBagSwing : MonoBehaviour {

        [SerializeField] private float punchStrength;

        private DataWrangler.GameData gd;
        private Rigidbody2D rb2D;


        private void Awake() {
            // EVENTS
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchNormal.AddListener(PunchBag);
            gd.eventData.OnPunchWarmup.AddListener(PunchBag);
            // COMPONENTS
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void PunchBag(TARGET punch) {
            rb2D.AddTorque(punchStrength);
        }
    }
}