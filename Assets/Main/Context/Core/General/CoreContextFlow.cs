using Main.Context.Core.Audio;
using Main.Context.Core.Logger;
using Main.Infrastructure.Context;
using UnityEngine;

namespace Main.Context.Core.General
{
    public class CoreContextFlow : AContextFlow
    {
        protected override IContextUnit[] GetContextUnits()
        {
            return new IContextUnit[]
            {
                new LogManager(),
                new SceneChangeManager(),
                new ResourceManager(),
                new ConfigManager(),
                new AssetManager(),
                new GizmosManager(),
                new AudioManager(),
                new FileDownloadManager(),
                new UserManager(),
                new GamePropertiesManager(),
                new RandomManager(),
                new CursorManager()
            };
        }

        protected override void StartFlow()
        {
            Application.targetFrameRate = 60;
            AppDelegates.CallStartListeners();
            Contexts.Get<SceneChangeManager>().LoadHomeScene();
        }

        protected override void PauseFlow()
        {
            AppDelegates.CallPauseListeners();
        }

        protected override void ResumeFlow()
        {
            AppDelegates.CallResumeListeners();
        }

        protected override void EndFlow()
        {
            AppDelegates.CallQuitListeners();
            AppDelegates.Clear();
        }
    }
}
