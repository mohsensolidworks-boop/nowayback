using Main.Context.Scenes.Common.UI.Components;
using Main.Infrastructure.General;
using Main.Infrastructure.Utils.Extensions;
using UnityEngine;

namespace Main.Context.Scenes.Common.UI
{
    public class DialogBackgroundDark : ManualBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        
        public bool IsAllocated { get; private set; }
        
        public void Init(Vector2 size)
        {
            _background.size = size;
            _background.color = new Color(0f, 0f, 0f, 0.8f);
            gameObject.SetActive(false);
        }
        
        public void Show(SortingData sortingData)
        {
            IsAllocated = true;
            
            _background.SetSorting(sortingData);
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            
            IsAllocated = false;
        }
    }
}
