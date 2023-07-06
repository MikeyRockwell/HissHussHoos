using MoreMountains.Feedbacks;
using UnityEngine;

namespace Managers
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] private MMF_Player feedbacks;

        private DataWrangler.GameData gd;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnMiss.AddListener(RemoveHealth);
            
            gd.eventData.OnGameInit.AddListener(NewGame);
            gd.eventData.initMethods++;

            // This will be triggered when the end game menu closes
            gd.eventData.OnGameOver.AddListener(NewGame);
        }

        private void NewGame()
        {
            gd.playerData.ResetHealth();
            gd.eventData.RegisterCallBack();
        }

        private void RemoveHealth()
        {
            feedbacks.PlayFeedbacks();
            gd.playerData.ChangeHealth(-1);
            if (gd.playerData.health == 0) gd.eventData.GameOver();
        }
    }
}