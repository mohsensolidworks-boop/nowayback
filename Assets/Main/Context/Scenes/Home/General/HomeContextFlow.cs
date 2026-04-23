using DG.Tweening;
using Main.Context.Core.Audio;
using Main.Context.Scenes.Common.UI;
using Main.Context.Scenes.Common.General;
using Main.Context.Scenes.Home.UI;
using Main.Infrastructure.Context;

namespace Main.Context.Scenes.Home.General
{
    public class HomeContextFlow : AContextFlow
    {
        protected override IContextUnit[] GetContextUnits()
        {
            return new IContextUnit[]
            {
                new CameraManager(),
                new FlowManager(),
                new DialogManager(),
                new InputManager(),
                new InputListener(),
                new UIInputListener(),
                new HomeInputIterator(),
                new HomeUIManager()
            };
        }

        protected override void StartFlow()
        {
            var audioManager = Contexts.Get<AudioManager>();
            audioManager.PlaySoundInLoop(AudioClipType.HomeBackgroundAmbience);
            var seq = DOTween.Sequence();
            seq.AppendInterval(10f);
            seq.AppendCallback(() =>
            {
                var random = UnityEngine.Random.Range(0, 2);
                var audioClipType = random == 0 ? AudioClipType.HomeBackgroundMusic1 : AudioClipType.HomeBackgroundMusic1;
                audioManager.PlaySound(audioClipType);
            });
        }

        protected override void PauseFlow()
        {
        }

        protected override void ResumeFlow()
        {
        }

        protected override void EndFlow()
        {
            Contexts.Get<AudioManager>().StopSoundInLoop(AudioClipType.HomeBackgroundAmbience);
        }
    }
}
