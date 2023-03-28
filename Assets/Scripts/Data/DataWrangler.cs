using Data;
using UI.CustomiseMenu;

namespace Managers {
    public class DataWrangler : Singleton<DataWrangler> {

        public SO_SaveData SaveData;
        public GameEventData EventData;
        public CustomiseEvents CustomiseEvents;
        public TargetData TargetData;
        public PlayerData PlayerData;
        public RoundData RoundData;
        public MusicData MusicData;
        public SoundData SoundData;
        public ColorData ColorData;

        public static SO_SaveData GetSaveData() {
            return Instance.SaveData;
        }
        
        public static GameData GetGameData() {
            return new GameData(
                Instance.EventData,
                Instance.CustomiseEvents,
                Instance.TargetData, 
                Instance.PlayerData, 
                Instance.RoundData,
                Instance.MusicData,
                Instance.SoundData,
                Instance.ColorData);
        }

        public struct GameData {

            public readonly GameEventData eventData;
            public readonly CustomiseEvents customEvents;
            public readonly TargetData targetData;
            public readonly PlayerData playerData;
            public readonly RoundData roundData;
            public readonly MusicData musicData;
            public readonly SoundData soundData;
            public readonly ColorData colorData;

            public GameData(
                GameEventData eventData, 
                CustomiseEvents customEvents, 
                TargetData targetData,
                PlayerData playerData,
                RoundData roundData,
                MusicData musicData,
                SoundData soundData,
                ColorData colorData) {
                
                this.eventData = eventData;
                this.customEvents = customEvents;
                this.targetData = targetData;
                this.playerData = playerData;
                this.roundData = roundData;
                this.musicData = musicData;
                this.soundData = soundData;
                this.colorData = colorData;
            }
        }

    }
}