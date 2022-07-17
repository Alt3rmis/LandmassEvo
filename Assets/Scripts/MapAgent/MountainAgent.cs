using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainAgent
{
    private int numMountains;
    private float minAltitude; // 0.0f
    private float maxAltitude; // 1.0f
    private int token;
    private int minStep;
    private int maxStep;
    
    
    
    private int[,] directions = { { 0, -1 }, { 1, 0 }, {  0, 1 }, { -1,  0 },
                                  { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
    private int mapWidth;
    private int mapHeight;
    private System.Random rand;
    private float[,] heightMap;
    private int[] currentPos;
    private int[] direction;

    public MountainAgent(int numMountains, float minAltitude, float maxAltitude, int moutainAgentToken, int minStep, int maxStep, System.Random rand, float[,] heightMap)
    {
        this.numMountains = numMountains;
        this.minAltitude = minAltitude;
        this.maxAltitude = maxAltitude;
        this.mapWidth = heightMap.GetLength(0);
        this.mapHeight = heightMap.GetLength(1);
        this.rand = rand;
        this.heightMap = heightMap;
        this.token = moutainAgentToken;
        this.minStep = minStep;
        this.maxStep = maxStep;
        this.currentPos = new int[2];
        direction = new int[2];
    }

    public void run()
    {
        int jumpInterval = token / numMountains;
        int steps;
        while(numMountains > 0)
        {
            //set random start point
            currentPos[0] = rand.Next(0, mapWidth);
            currentPos[1] = rand.Next(0, mapHeight);

            // set random steps for mountain generation
            steps = rand.Next(minStep, maxStep);

            // set random direction for mountain grow
            int dirIdx = rand.Next(0, directions.GetLength(0));
            // Debug.Log("dirIdx: " + dirIdx);
            direction[0] = directions[dirIdx, 0];
            direction[1] = directions[dirIdx, 1];

            // moutain genrate loop
            for (int i = 0; i < steps || i > jumpInterval; i++)
            {
                createMountain();
                moveForwards();
            }
            numMountains--;
        }
    }

    private bool isInMap(int[] pos)
    {
        return pos[0] >= 0 && pos[0] < mapWidth && pos[1] >= 0 && pos[1] < mapHeight;
    }

    private int[] changeDirection(int[] currentDirection) // change current direction randomly +-45 deg
    {
        int[] newDirection = new int[2];
        int decision = rand.Next(0, 2);
        if(decision == 0)// counterclockwise
        {
            // vector rotation
            newDirection[0] = direction[0] * 1 + direction[1] * -1;
            newDirection[1] = direction[0] * 1 + direction[1] *  1;
        }
        else // clockwise
        {
            // vector rotation
            newDirection[0] = direction[0] *  1 + direction[1] * 1;
            newDirection[1] = direction[0] * -1 + direction[1] * 1;
        }
        // change back to 1
        for(int i = 0; i < 2; i++)
        {
            if (Mathf.Abs(newDirection[i]) > 1)
            {
                newDirection[i] = newDirection[i] / Mathf.Abs(newDirection[i]);
            }
        }
        return newDirection;
    }
    // this function only optimize moutain on the range
    private void createMountain()
    {
        // Debug.Log("Current Position (" + currentPos[0]+" ," +currentPos[1]+")");
        heightMap[currentPos[0], currentPos[1]] = rand.Next((int)minAltitude*100, (int)maxAltitude*100)/100.0f;
        float mountainNeighborMultiplier = 0.8f;
        if (direction[0] == 0) // vertical movement
        {
            if (currentPos[0] > 0)
            {
                heightMap[currentPos[0] - 1, currentPos[1]] *= mountainNeighborMultiplier;
            }
            if (currentPos[0] < mapWidth - 1)
            {
                heightMap[currentPos[0] + 1, currentPos[1]] *= mountainNeighborMultiplier;
            }
        }
        else if (direction[1] == 0) // horizotal movement
        {
            if (currentPos[1] > 0)
            {
                heightMap[currentPos[0], currentPos[1] - 1] *= mountainNeighborMultiplier;
            }
            if (currentPos[1] < mapHeight - 1)
            {
                heightMap[currentPos[0], currentPos[1] + 1] *= mountainNeighborMultiplier;
            }
        }
        else if (direction[0] * direction[1] < 0) // diagonal movement
        {
            int[] tmp1 = new int[2];
            int[] tmp2 = new int[2];
            tmp1[0] = currentPos[0] + 1;
            tmp1[1] = currentPos[1] - 1;
            tmp2[0] = currentPos[0] - 1;
            tmp2[1] = currentPos[1] + 1;
            if(isInMap(tmp1))
            {
                heightMap[tmp1[0], tmp1[1]] *= mountainNeighborMultiplier;
            }
            if(isInMap(tmp2))
            {
                heightMap[tmp2[0], tmp2[1]] *= mountainNeighborMultiplier;
            }
        }
        else // if(direction[0] * direction[1] > 0) // reverse diagonal movement
        {
            int[] tmp1 = new int[2];
            int[] tmp2 = new int[2];
            tmp1[0] = currentPos[0] + 1;
            tmp1[1] = currentPos[1] + 1;
            tmp2[0] = currentPos[0] - 1;
            tmp2[1] = currentPos[1] - 1;
            if (isInMap(tmp1))
            {
                heightMap[tmp1[0], tmp1[1]] *= mountainNeighborMultiplier;
            }
            if (isInMap(tmp2))
            {
                heightMap[tmp2[0], tmp2[1]] *= mountainNeighborMultiplier;
            }
        }
    }

    private void moveForwards()
    {
        int[] tempPos = new int[2];
        tempPos[0] = currentPos[0] + direction[0];
        tempPos[1] = currentPos[1] + direction[1];
        if(!isInMap(tempPos))
        {
            // TODO
            // change direction +-45 deg

            // reverse direction first
            direction[0] = -direction[0];
            direction[1] = -direction[1];
            direction = changeDirection(direction);
            tempPos[0] = currentPos[0] + direction[0];
            tempPos[1] = currentPos[1] + direction[1];
        }
        currentPos[0] += direction[0];
        currentPos[1] += direction[1];
    }
}
