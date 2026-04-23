using System.Linq;
using UnityEngine;

namespace Main.Infrastructure.Utils.Extensions
{
    public static class TerrainExtensions
    {
        private static TerrainData GetTerrainData(this Terrain terrain)
        {
            return terrain.terrainData;
        }
        
        private static int GetHeightmapResolution(this Terrain terrain)
        {
            return terrain.GetTerrainData().heightmapResolution;
        }
        
        private static Vector3 WorldToTerrainPosition(this Terrain terrain, Vector3 worldPosition)
        {
            var terrainPosition = worldPosition - terrain.GetPosition();
            var terrainSize = terrain.GetTerrainData().size;
            var heightmapResolution = terrain.GetHeightmapResolution();
            terrainPosition = new Vector3(terrainPosition.x / terrainSize.x, terrainPosition.y / terrainSize.y, terrainPosition.z / terrainSize.z);
     
            return new Vector3(terrainPosition.x * heightmapResolution, 0, terrainPosition.z * heightmapResolution);
        }
        
        private static Vector2Int GetBrushPosition(this Terrain terrain, Vector3 worldPosition, int brushWidth, int brushHeight)
        {
            var terrainPosition = terrain.WorldToTerrainPosition(worldPosition);
            var heightmapResolution = terrain.GetHeightmapResolution();

            return new Vector2Int((int)Mathf.Clamp(terrainPosition.x - brushWidth / 2.0f, 0.0f, heightmapResolution),
                                  (int)Mathf.Clamp(terrainPosition.z - brushHeight / 2.0f, 0.0f, heightmapResolution));
        }
        
        private static Vector2Int GetSafeBrushSize(this Terrain terrain, Vector2Int brushPosition, int brushWidth, int brushHeight)
        {
            var heightmapResolution = terrain.GetHeightmapResolution();
            
            while (heightmapResolution - (brushPosition.x + brushWidth) < 0)
            {
                brushWidth--;
            }
            
            while (heightmapResolution - (brushPosition.y + brushHeight) < 0)
            {
                brushHeight--;
            }
            
            return new Vector2Int(brushWidth, brushHeight);
        }
        
        public static void RaiseTerrain(this Terrain terrain, Vector3 worldPosition, float strength, int brushWidth, int brushHeight)
        {
            terrain.SetHeights(worldPosition, strength, brushWidth, brushHeight);
        }
        
        public static void LowerTerrain(this Terrain terrain, Vector3 worldPosition, float strength, int brushWidth, int brushHeight)
        {
            terrain.SetHeights(worldPosition, -strength, brushWidth, brushHeight);
        }
        
        private static void SetHeights(this Terrain terrain, Vector3 worldPosition, float strength, int brushWidth, int brushHeight)
        {
            var brushPosition = terrain.GetBrushPosition(worldPosition, brushWidth, brushHeight);
            var brushSize = terrain.GetSafeBrushSize(brushPosition, brushWidth, brushHeight);
            var terrainData = terrain.GetTerrainData();
            var heights = terrainData.GetHeights(brushPosition.x, brushPosition.y, brushSize.x, brushSize.y);
     
            for (var y = 0; y < brushSize.y; y++)
            {
                for (var x = 0; x < brushSize.x; x++)
                {
                    heights[y, x] += strength * Time.deltaTime;
                }
            }
            
            terrainData.SetHeights(brushPosition.x, brushPosition.y, heights);
        }

        public static void FlattenTerrain(this Terrain terrain, Vector3 worldPosition, float height, int brushWidth, int brushHeight)
        {
            var brushPosition = terrain.GetBrushPosition(worldPosition, brushWidth, brushHeight);
            var brushSize = terrain.GetSafeBrushSize(brushPosition, brushWidth, brushHeight);
            var terrainData = terrain.GetTerrainData();
            var heights = terrainData.GetHeights(brushPosition.x, brushPosition.y, brushSize.x, brushSize.y);
            
            for (var y = 0; y < brushSize.y; y++)
            {
                for (var x = 0; x < brushSize.x; x++)
                {
                    heights[y, x] = height;
                }
            }
            
            terrainData.SetHeights(brushPosition.x, brushPosition.y, heights);
        }
     
        public static float SampleHeight(this Terrain terrain, Vector3 worldPosition)
        {
            var terrainPosition = terrain.WorldToTerrainPosition(worldPosition);
            return terrain.GetTerrainData().GetInterpolatedHeight((int)terrainPosition.x, (int)terrainPosition.z);
        }
        
        public static float SampleAverageHeight(this Terrain terrain, Vector3 worldPosition, int brushWidth, int brushHeight)
        {
            var brushPosition = terrain.GetBrushPosition(worldPosition, brushWidth, brushHeight);
            var brushSize = terrain.GetSafeBrushSize(brushPosition, brushWidth, brushHeight);
            var heights2D = terrain.GetTerrainData().GetHeights(brushPosition.x, brushPosition.y, brushSize.x, brushSize.y);
            var heights = new float[heights2D.Length];
     
            var i = 0;
            for (var y = 0; y <= heights2D.GetUpperBound(0); y++)
            {
                for (var x = 0; x <= heights2D.GetUpperBound(1); x++)
                {
                    heights[i++] = heights2D[y, x];
                }
            }
            
            return heights.Average();
        }
    }
}
