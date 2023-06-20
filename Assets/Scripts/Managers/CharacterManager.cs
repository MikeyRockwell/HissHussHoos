using System;
using UnityEngine;
using Data.Customization;
using UnityEngine.Serialization;

namespace Managers
{
    public class CharacterManager : MonoBehaviour
    {
        [FormerlySerializedAs("character")] [SerializeField] private SO_CharacterPart characters;
        [SerializeField] private Transform williamWaiirua;
        [SerializeField] private Transform terryTamati;
        
        private void Awake()
        {
            characters.OnChangeItem.AddListener(ChangeCharacter);
        }

        private void Start()
        {
            ChangeCharacter(characters.CurrentItem);
        }

        private void ChangeCharacter(SO_Item newChar)
        {
            // Disable all characters
            williamWaiirua.gameObject.SetActive(false);
            terryTamati.gameObject.SetActive(false);

            switch (newChar.character)
            {
                case SO_Item.Character.William:
                    williamWaiirua.gameObject.SetActive(true);
                    break;
                case SO_Item.Character.TerryTamati:
                    terryTamati.gameObject.SetActive(true);
                    break;
                case SO_Item.Character.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}