using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAgent
{
    // parameters needed to be passed
    private int numMoutains;
    private float minAltitude;
    private float maxAltitude;
    private System.Random rand;
    private float[,] heightMap;
    private int token;
    private int minStep;
    private int maxStep;
    private float mountainNeighbourMultiplier;
    private int jumpInterval;



    private int mapWidth;
    private int mapHeight;
    private int[] currentPos;
    private int currentDir;

    public MAgent(int numMoutains, float minAltitude, float maxAltitude, int moutainAgentToken,
                    int minStep, int maxStep, System.Random rand, float[,] heightMap, float mountainNeighbourMultiplier, int jumpInterval)
    {
        this.numMoutains = numMoutains;
        this.minAltitude = minAltitude;
        this.maxAltitude = maxAltitude;
        this.token = moutainAgentToken;
        this.minStep = minStep;
        this.maxStep = maxStep;
        this.rand = rand;
        this.heightMap = heightMap;
        this.mapWidth = heightMap.GetLength(0);
        this.mapHeight = heightMap.GetLength(1);
        this.currentPos = new int[2];
        this.mountainNeighbourMultiplier = mountainNeighbourMultiplier;
        this.jumpInterval = jumpInterval;
    }

    public void run()
    {
        int steps;
        while(token > 0)
        {
            // initialize current position randomly
            currentPos[0] = rand.Next(0, mapWidth);
            currentPos[1] = rand.Next(0, mapHeight);

            // set moutain length randomly
            steps = rand.Next(minStep, maxStep);

            currentDir = rand.Next(0, 4);
            while(steps > 0 && token > 0)
            {
                int minAl = (int)minAltitude * 100;
                int maxAl = (int)maxAltitude * 100;
                heightMap[currentPos[0], currentPos[1]] = rand.Next(minAl, maxAl) / 100.0f;
                createMoutain();
                move();

                if(token % jumpInterval == 0) // need to jump to a new point
                {
                    currentPos[0] = rand.Next(0, mapWidth);
                    currentPos[1] = rand.Next(0, mapHeight);
                }
                steps--;
                token--;
                //
            }
        }
    }

    private void createMoutain()
    {
        // create the moutain
        if (currentDir == 0 || currentDir == 1) // vertical movement
        {
            if (currentPos[1] > 0)
            {
                heightMap[currentPos[0], currentPos[1] - 1] *= mountainNeighbourMultiplier;
            }
            if (currentPos[1] < mapHeight - 1)
            {
                heightMap[currentPos[0], currentPos[1] + 1] *= mountainNeighbourMultiplier;
            }
        }
        else // horizontal movement
        {
            if (currentPos[0] > 0)
            {
                heightMap[currentPos[0] - 1, currentPos[1]] *= mountainNeighbourMultiplier;
            }
            if (currentPos[0] < mapHeight - 1)
            {
                heightMap[currentPos[0] + 1, currentPos[1]] *= mountainNeighbourMultiplier;
            }
        }
    }

    private void move()
    {
        // check if in heightmap
        switch (currentDir)
        {
            case 0: // up
                if (currentPos[1] - 1 > 0) // in boundary
                {
                    currentPos[1] -= 1;
                } else
                {
                    int[] directionArray = { 1, 2, 3 };
                    currentDir = directionArray[rand.Next(directionArray.Length)];
                }
                break;
            case 1: // down
                if (currentPos[1] + 1 < mapHeight) // in boundary
                {
                    currentPos[1] += 1;
                }
                else
                {
                    int[] directionArray = { 0, 2, 3 };
                    currentDir = directionArray[rand.Next(directionArray.Length)];
                }
                break;
            case 2: // left
                if (currentPos[0] - 1 > 0) // in boundary
                {
                    currentPos[0] -= 1;
                }
                else
                {
                    int[] directionArray = { 0, 1, 3 };
                    currentDir = directionArray[rand.Next(directionArray.Length)];
                }
                break;
            case 3: // right
                if (currentPos[0] + 1 < mapWidth) // in boundary
                {
                    currentPos[0] += 1;
                }
                else
                {
                    int[] directionArray = { 0, 1, 2 };
                    currentDir = directionArray[rand.Next(directionArray.Length)];
                }
                break;
        }
    }
}