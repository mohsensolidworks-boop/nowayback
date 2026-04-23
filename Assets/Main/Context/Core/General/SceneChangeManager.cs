using Main.Context.Scenes.Gameplay.General;
using Main.Context.Scenes.Home.General;
using Main.Infrastructure.Context;
using UnityEngine.SceneManagement;

namespace Main.Context.Core.General
{
    public sealed class SceneChangeManager : ICoreContextUnit
    {
        public bool IsHomeScene => _currentScene == _HOME_SCENE;
        
        private const int _CORE_SCENE = 0;
        private const int _HOME_SCENE = 1;
        private const int _GAMEPLAY_SCENE = 2;
        
        private int _currentScene;
        private int _loadingScene;
        
        public void Bind()
        {
        }
        
        public void OnActivateScene()
        {
        }
        
        public void LoadHomeScene()
        {
            LoadScene(_HOME_SCENE);
        }
        
        public void LoadGameplayScene()
        {
            LoadScene(_GAMEPLAY_SCENE);
        }
        
        private void LoadScene(int scene)
        {
            Contexts.DeactivateSceneContext();
            
            _loadingScene = scene;
            SceneManager.LoadScene(_loadingScene);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        
        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (_loadingScene != scene.buildIndex)
            {
                return;
            }
            
            SceneManager.sceneLoaded -= SceneLoaded;
            
            _currentScene = _loadingScene;
            Contexts.ActivateSceneContext(IsHomeScene ? new HomeContextFlow() : new GameplayContextFlow());
        }
    }
}
