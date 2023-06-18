using Managers;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Animation
{
    public class PunchBagSwing : MonoBehaviour
    {
        [SerializeField] private float punchStrength;


        private DataWrangler.GameData gd;
        private Rigidbody2D rb2D;


        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchNormal.AddListener(PunchBag);
            gd.eventData.OnPunchWarmup.AddListener(PunchBag);

            rb2D = GetComponent<Rigidbody2D>();
        }

        private void PunchBag(TARGET punch)
        {
            rb2D.AddTorque(punchStrength);
        }
    }
}