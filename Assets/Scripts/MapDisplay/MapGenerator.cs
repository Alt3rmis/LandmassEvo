using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap, Mesh};
    public DrawMode drawMode;

    public const int mapSize = 50;
    //some other parameter for agents
    public bool autoUpdate;
    public TerrainType[] regions;
    public int seed;
    public AnimationCurve meshHeightCurve;

    public int coastlineAgentTokens;
    public int coastlineAgentCount = 5;

    public void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        float[,] noiseMap;
        noiseMap = Agent.GenerateNoiseMap(mapSize, mapSize, seed, coastlineAgentTokens, coastlineAgentCount);// other parameters for agent
        float maxHeight = float.MinValue;
        Color[] colourMap = new Color[mapSize * mapSize];
        for(int y = 0; y < mapSize; ++y)
        {
            for(int x = 0; x < mapSize; ++x)
            {
                float currentHeight = noiseMap[x, y];
                for(int i = 0; i < regions.Length; ++i)
                {
                    if(currentHeight > maxHeight)
                    {
                        maxHeight = currentHeight;
                    }
                    if(currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapSize, mapSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, 120, meshHeightCurve, 2), TextureGenerator.TextureFromColourMap(colourMap, mapSize, mapSize));
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
