using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColourMap
    };
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;


    public bool autoUpdate;

    public TerrainTypes[] netherRegions;

    public void GenerateMap()
    {//when verts get generated the constantly get lowered so you can snowboard infinitly
        float[,] noiseMap = Noise.generateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        //colour shit 
        Color[] colourMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < netherRegions.Length; i++)
                {
                    if(currentHeight <= netherRegions[i].height)
                    {
                        colourMap[y * mapWidth + x] = netherRegions[i].colour;

                        break;
                    }
                }
            }
        }


        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap)
        {
            display.DrawNoiseMap(noiseMap);
        }
        else if(drawMode == DrawMode.ColourMap)
        {

        }
    }

    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if(mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity < 1) //watch back the first ep in the series
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
    }
}

[System.Serializable]
public struct TerrainTypes
{
    public string terrainName;
    public float height;
    public Color colour;
}
