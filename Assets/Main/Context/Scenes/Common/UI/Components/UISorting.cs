using UnityEngine;

namespace Main.Context.Scenes.Common.UI.Components
{
    public static class UISorting
    {
        private const int _DIALOG_SORTING_ORDER = 1000;
        
        private static readonly int _DEFAULT_LAYER_ID;
        private static readonly int _UI_LAYER_ID;
        
        static UISorting()
        {
            _DEFAULT_LAYER_ID = SortingLayer.NameToID("Default");
            _UI_LAYER_ID = SortingLayer.NameToID("UI");
        }
        
        #region DialogLayer
        
        public static SortingData GetDialogSorting()
        {
            return new SortingData(_UI_LAYER_ID, _DIALOG_SORTING_ORDER);
        }
        
        #endregion

        #region Extensions

        public static SortingData GetSortingWithOffset(this SortingData data, int offset)
        {
            return new SortingData(data.Layer, data.Order + offset);
        }
        
        #endregion
    }
    
    public readonly struct SortingData
    {
        public readonly int Layer;
        public readonly int Order;
        
        public SortingData(int layer, int order)
        {
            Layer = layer;
            Order = order;
        }

        public bool IsAboveOther(SortingData other)
        {
            return Layer > other.Layer || (Layer == other.Layer && Order > other.Order);
        }
    }
}
