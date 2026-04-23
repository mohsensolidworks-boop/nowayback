using Main.Infrastructure.Context;
using UnityEngine;

namespace Main.Context.Core.General
{
    public sealed class UserManager : ICoreContextUnit
    {
        public const int DEFAULT_USER_LEVEL = 1;
        private const string _USER_LEVEL_KEY = "UserLevel";
        
        public int UserLevel => _userLevel;
        
        private GamePropertiesManager _levelManager;
        private int _userLevel;
        
        public void Bind()
        {
            _levelManager = Contexts.Get<GamePropertiesManager>();
            
            _userLevel = PlayerPrefs.GetInt(_USER_LEVEL_KEY, 1);
        }
        
        public void OnActivateScene()
        {
        }
        
        private void WriteUserLevel()
        {
            PlayerPrefs.SetInt(_USER_LEVEL_KEY, _userLevel);
        }
        
        public void IncrementLevel()
        {
            if (_userLevel >= _levelManager.LevelsCount)
            {
                return;
            }
            
            SetUserLevel(_userLevel + 1);
        }
        
        public void SetUserLevel(int level)
        {
            _userLevel = level;
            WriteUserLevel();
        }
    }
}
