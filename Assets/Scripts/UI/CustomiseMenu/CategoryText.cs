﻿using TMPro;
using Managers;
using UnityEngine;
using Data.Customization;

namespace UI.CustomiseMenu
{
    public class CategoryText : MonoBehaviour
    {
        private DataWrangler.GameData gd;
        private TextMeshProUGUI textMesh;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.customEvents.OnChangeCategory.AddListener(UpdateText);
            gd.customEvents.OnMenuOpened.AddListener(UpdateText);
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void UpdateText(SO_Category part)
        {
            textMesh.text = part.UIName;
        }
    }
}