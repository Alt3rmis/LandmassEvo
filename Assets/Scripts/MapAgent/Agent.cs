using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Agent
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, int coastToken=0, int agentNumber = 5)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        System.Random rand = new System.Random(seed);
        // do some agent things
        for(int i = 0; i < noiseMap.GetLength(0); i++)
        {
            for(int j = 0; j < noiseMap.GetLength(1); j++)
            {
                noiseMap[i, j] = 0.3f;
            }
        }


        // MountainAgent ma = new MountainAgent(3, 0.3f, 1.0f, 500, 20, 30, rand, noiseMap);
        // SmoothAgent sa = new SmoothAgent(1000, 50, rand, noiseMap);
        // MAgent ma = new MAgent(5, 0.3f, 1.0f, 100, 5, 10, rand, noiseMap, 0.8f, 20);
        InitAgent ia = new InitAgent(noiseMap, rand);
        ia.run();
        for (int i = 0; i < 50; ++i)
        {
            SmoothAgent sa = new SmoothAgent(noiseMap, 1000, rand);
            sa.run();
        }


        return noiseMap;
    }
}