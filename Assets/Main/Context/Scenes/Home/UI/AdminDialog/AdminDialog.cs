using System;
using Main.Context.Core.General;
using Main.Context.Scenes.Common.UI.Components;
using Main.Infrastructure.Context;
using TMPro;
using UnityEngine;

namespace Main.Context.Scenes.Home.UI.AdminDialog
{
    public class AdminDialog : UIDialog
    {
        [SerializeField] private TMP_InputField _levelInputField;
        [SerializeField] private Canvas _inputFiledCanvas;
        
        protected override void OnShow()
        {
            UpdateLevelText();
            var headSortingGroup = GetSortingGroup();
            _inputFiledCanvas.sortingLayerID = headSortingGroup.sortingLayerID;
            _inputFiledCanvas.sortingOrder = headSortingGroup.sortingOrder + 2;
        }
        
        public void ClickResetUserLevelButton()
        {
            var userManager = Contexts.Get<UserManager>();
            userManager.SetUserLevel(UserManager.DEFAULT_USER_LEVEL);
            UpdateLevelText();
        }

        public void ClickIncrementLevelButton()
        {
            var userManager = Contexts.Get<UserManager>();
            var gamePropertiesManager = Contexts.Get<GamePropertiesManager>();
            var level = Math.Clamp(userManager.UserLevel + 1, 1, gamePropertiesManager.LevelsCount);
            userManager.SetUserLevel(level);
            UpdateLevelText();
        }
        
        public void ClickDecrementLevelButton()
        {
            var userManager = Contexts.Get<UserManager>();
            var gamePropertiesManager = Contexts.Get<GamePropertiesManager>();
            var level = Math.Clamp(userManager.UserLevel - 1, 1, gamePropertiesManager.LevelsCount);
            userManager.SetUserLevel(level);
            UpdateLevelText();
        }
        
        public void OnLevelInputFieldEditEnd()
        {
            if (int.TryParse(_levelInputField.text, out var number))
            {
                var userManager = Contexts.Get<UserManager>();
                var gamePropertiesManager = Contexts.Get<GamePropertiesManager>();
                var level = Math.Clamp(number, 1, gamePropertiesManager.LevelsCount);
                userManager.SetUserLevel(level);
            }
            
            UpdateLevelText();
        }
        
        private void UpdateLevelText()
        {
            var userManager = Contexts.Get<UserManager>();
            _levelInputField.SetTextWithoutNotify(userManager.UserLevel.ToString());
        }
        
        protected override void OnClose()
        {
            var homeUIManager = Contexts.Get<HomeUIManager>();
            homeUIManager.Refresh();
        }
    }
}
