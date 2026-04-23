using Main.Context.Core.Audio;
using Main.Context.Scenes.Common.UI;
using Main.Context.Scenes.Common.General;
using Main.Context.Scenes.Gameplay.UI;
using Main.Infrastructure.Context;

namespace Main.Context.Scenes.Gameplay.General
{
    public class GameplayContextFlow : AContextFlow
    {
        protected override IContextUnit[] GetContextUnits()
        {
            return new IContextUnit[]
            {
                new GameplayExitManager(),
                new CameraManager(),
                new FlowManager(),
                new DialogManager(),
                new InputManager(),
                new InputListener(),
                new UIInputListener(),
                new GameplayOtherInputListener(),
                new GameplayInputIterator(),
                new GameplayUIManager(),
                new GameplayOperationsManager()
            };
        }

        protected override void StartFlow()
        {
            Contexts.Get<AudioManager>().PlaySoundInLoop(AudioClipType.AreaAmbienceBackground);
        }

        protected override void PauseFlow()
        {
        }

        protected override void ResumeFlow()
        {
        }

        protected override void EndFlow()
        {
            Contexts.Get<AudioManager>().StopSoundInLoop(AudioClipType.AreaAmbienceBackground);
        }
    }
}
