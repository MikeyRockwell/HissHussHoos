using Data;
using UI.CustomiseMenu;
using UnityEngine.Serialization;

namespace Managers {
    public class DataWrangler : Singleton<DataWrangler> {
        
        public GameState GameState;
        public LoadSaveData LoadSaveData;
        public GameEventData EventData;
        public CustomizationEvents customizationEvents;
        public TargetData TargetData;
        public PlayerData PlayerData;
        public RoundData RoundData;
        public MusicData MusicData;
        public SoundData SoundData;
        public ColorData ColorData;
        public UIData UIData;
        public ItemData ItemData;

        public static LoadSaveData GetSaveData() {
            return Instance.LoadSaveData;
        }
        
        public static GameData GetGameData() {
            return new GameData(
                Instance.GameState,
                Instance.EventData,
                Instance.customizationEvents,
                Instance.TargetData, 
                Instance.PlayerData, 
                Instance.RoundData,
                Instance.MusicData,
                Instance.SoundData,
                Instance.ColorData,
                Instance.UIData,
                Instance.ItemData);
        }

        public struct GameData {

            public readonly GameState gameState;
            public readonly GameEventData eventData;
            public readonly CustomizationEvents customEvents;
            public readonly TargetData targetData;
            public readonly PlayerData playerData;
            public readonly RoundData roundData;
            public readonly MusicData musicData;
            public readonly SoundData soundData;
            public readonly ColorData colorData;
            public readonly UIData uIData;
            public readonly ItemData itemData;

            public GameData(
                GameState gameState,
                GameEventData eventData, 
                CustomizationEvents customEvents, 
                TargetData targetData,
                PlayerData playerData,
                RoundData roundData,
                MusicData musicData,
                SoundData soundData,
                ColorData colorData,
                UIData uIData,
                ItemData itemData) {

                this.gameState = gameState;
                this.eventData = eventData;
                this.customEvents = customEvents;
                this.targetData = targetData;
                this.playerData = playerData;
                this.roundData = roundData;
                this.musicData = musicData;
                this.soundData = soundData;
                this.colorData = colorData;
                this.uIData = uIData;
                this.itemData = itemData;
            }
        }

    }
}