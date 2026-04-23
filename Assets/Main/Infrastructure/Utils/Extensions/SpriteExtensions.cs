using System;
using Main.Context.Core.Logger;
using Main.Context.Scenes.Common.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Main.Infrastructure.Utils.Extensions
{
    public static class SpriteExtensions
    {
        public static SortingData GetSorting(this Renderer renderer)
        {
            return new SortingData(renderer.sortingLayerID, renderer.sortingOrder);
        }

        public static void SetSorting(this Renderer renderer, SortingData data)
        {
            renderer.sortingLayerID = data.Layer;
            renderer.sortingOrder = data.Order;
        }

        public static void SetSorting(this SortingGroup group, SortingData data)
        {
            group.sortingLayerID = data.Layer;
            group.sortingOrder = data.Order;
        }

        public static void SetSorting(this TextMeshPro textMeshPro, SortingData data)
        {
            textMeshPro.sortingLayerID = data.Layer;
            textMeshPro.sortingOrder = data.Order;
        }
        
        public static void SetSorting(this Canvas canvas, SortingData data)
        {
            canvas.sortingLayerID = data.Layer;
            canvas.sortingOrder = data.Order;
        }
        
        public static SortingData GetSorting(this TextMeshPro tmp)
        {
            return new SortingData(tmp.sortingLayerID, tmp.sortingOrder);
        }
        
        public static SortingData GetSorting(this SortingGroup group)
        {
            return new SortingData(group.sortingLayerID, group.sortingOrder);
        }
        
        public static Sprite ToSprite(this byte[] bytes, int width, int height, TextureFormat format, TextureWrapMode wrapMode = TextureWrapMode.Repeat)
        {
            return ToSprite(bytes, width, height, format, new Vector2(0.5f, 0.5f), wrapMode);
        }
        
        public static Sprite ToSprite(this TextAsset textAsset, int width, int height, TextureFormat format, TextureWrapMode wrapMode = TextureWrapMode.Repeat)
        {
            return ToSprite(textAsset, width, height, format, new Vector2(0.5f, 0.5f), wrapMode);
        }
        
        public static Sprite ToSprite(this TextAsset textAsset, int width, int height, TextureFormat format, Vector2 pivot, TextureWrapMode wrapMode = TextureWrapMode.Repeat)
        {
            return ToSprite(textAsset.bytes, width, height, format, pivot, wrapMode, textAsset.name);
        }
        
        private static Sprite ToSprite(this byte[] bytes, int width, int height, TextureFormat format, Vector2 pivot, TextureWrapMode wrapMode = TextureWrapMode.Repeat, string name = "")
        {
            try
            {
                var tex = new Texture2D(width, height, format, false)
                {
                    filterMode = FilterMode.Bilinear,
                    wrapMode = wrapMode
                };
                tex.LoadImage(bytes, true);
                var sprite = Sprite.Create(tex, new Rect(0, 0, width, height),
                                           pivot, 140.0f, 0, SpriteMeshType.FullRect);
                return sprite;
            }
            catch (Exception e)
            {
                Log.Error(typeof(SpriteExtensions), LogTag.UI, $"Can't create sprite from image {name}: {e.Message}");
                return null;
            }
        }
        
        public static void SetAlpha(this SpriteRenderer sprite, float alpha)
        {
            var tmpColor = sprite.color;
            tmpColor.a = alpha;
            sprite.color = tmpColor;
        }

        public static Rect GetAtlasUvRect(this Sprite sprite)
        {
            var uVs = sprite.rect;
            uVs.x /= sprite.texture.width;
            uVs.width /= sprite.texture.width;
            uVs.y /= sprite.texture.height;
            uVs.height /= sprite.texture.height;
            return uVs;
        }

        public static Vector4 GetAtlasUv(this Sprite sprite)
        {
            var rect = GetAtlasUvRect(sprite);
            var value = new Vector4(rect.x, rect.x + rect.width, rect.y, rect.y + rect.height);
            return value;
        }

        public static Vector4 GetAtlasUvFromUvs(this Sprite sprite)
        {
            var uvX = 1f;
            var uvY = 0f;
            var uvZ = 1f;
            var uvW = 0f;
            var spriteUv = sprite.uv;

            for (var i = 0; i < spriteUv.Length; i++)
            {
                uvX = Mathf.Min(uvX, spriteUv[i].x);
                uvY = Mathf.Max(uvY, spriteUv[i].x);
                uvZ = Mathf.Min(uvZ, spriteUv[i].y);
                uvW = Mathf.Max(uvW, spriteUv[i].y);
            }

            return new Vector4(uvX, uvY, uvZ, uvW);
        }
    }
}
