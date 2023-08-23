using System;
using UnityEngine;
using Data.Customization;
using UnityEngine.Serialization;

namespace Managers {
    public class CharacterManager : MonoBehaviour {
        [SerializeField] private SO_Category character;
        [SerializeField] private Transform williamWaiirua;
        [SerializeField] private Transform terryTamati;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            character.OnChangeItem.AddListener(ChangeCharacter);
        }

        private void Start() {
            ChangeCharacter(character.CurrentItem);
        }

        private void ChangeCharacter(SO_Item newChar) {
            // Disable all characters
            williamWaiirua.gameObject.SetActive(false);
            terryTamati.gameObject.SetActive(false);

            switch (newChar.character) {
                case CharacterData.Character.William:
                    williamWaiirua.gameObject.SetActive(true);
                    break;
                case CharacterData.Character.TerryTamati:
                    terryTamati.gameObject.SetActive(true);
                    break;
                case CharacterData.Character.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            gd.characterData.currentCharacter = newChar.character;
        }
    }
}