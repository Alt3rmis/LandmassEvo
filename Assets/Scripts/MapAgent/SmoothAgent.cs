using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothAgent
{
    private int token;
    private int backInterval;
    System.Random rand;
    private int mapWidth;
    private int mapHeight;
    private float[,] heightMap;

    private int[] currentLocation;
    private int[] startLocation;

    public SmoothAgent(int token, int jumpInterval, System.Random rand, float[,] heightMap)
    {
        this.token = token;
        this.backInterval = jumpInterval;
        this.rand = rand;
        this.currentLocation = new int[2];
        this.startLocation = new int[2];
        this.mapWidth = heightMap.GetLength(0);
        this.mapHeight = heightMap.GetLength(1);
        this.heightMap = heightMap;
    }

    public void run()
    {
        // get random start point
        currentLocation[0] = rand.Next(0, mapWidth);
        currentLocation[1] = rand.Next(0, mapHeight);
        // foreach token
        while(token > 0)
        {
            smooth();
            getNeighbour();
            if(token % backInterval == 0)
            {
                randomJump();
            }
            token--;
        }
    }

    private bool isInMap(int[] pos)
    {
        return pos[0] >= 0 && pos[0] < mapWidth && pos[1] >= 0 && pos[1] < mapHeight;
    }

    private void smooth()
    {
        float sum = 0.0f;
        Debug.Log("Current Location:" + currentLocation[0] + "," + currentLocation[1]);
        sum += 3 * heightMap[currentLocation[0], currentLocation[1]];
        int counter = 3; // weight
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                int[] tempLoc = new int[2];
                tempLoc[0] = currentLocation[0] + i;
                tempLoc[1] = currentLocation[1] + j;
                if (isInMap(tempLoc) && i != 0 && j != 0)
                {
                    sum += heightMap[tempLoc[0], tempLoc[1]];
                    counter++;
                }
            }
        }
        heightMap[currentLocation[0], currentLocation[1]] = sum / counter;
    }
    private void getNeighbour()
    {
        int horizontalChoice = rand.Next(0, 3) - 1;
        int verticalChoice = rand.Next(0, 3) - 1;
        int[] tempLoc = new int[2];
        tempLoc[0] = currentLocation[0] + horizontalChoice;
        tempLoc[1] = currentLocation[1] + verticalChoice;
        while(horizontalChoice == 0 && verticalChoice == 0 && !isInMap(tempLoc))
        {
            horizontalChoice = rand.Next(0, 3) - 1;
            verticalChoice = rand.Next(0, 3) - 1;
            tempLoc[0] = currentLocation[0] + horizontalChoice;
            tempLoc[1] = currentLocation[1] + verticalChoice;
        }
        currentLocation[0] = tempLoc[0];
        currentLocation[1] = tempLoc[1];
    }

    private void randomJump()
    {
        currentLocation[0] = rand.Next(0, mapWidth);
        currentLocation[1] = rand.Next(0, mapHeight);
    }
}
