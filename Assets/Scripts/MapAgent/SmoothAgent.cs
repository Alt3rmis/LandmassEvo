using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothAgent
{
    private float[,] heightMap;
    private int token;
    private System.Random rand;

    private int[] position;
    private int mapWidth;
    private int mapHeight;
    public SmoothAgent(float[,] heightMap, int token, System.Random rand)
    {
        this.heightMap = heightMap;
        this.token = token;
        
        this.rand = rand;
        this.position = new int[2];
        this.mapWidth = heightMap.GetLength(0);
        this.mapHeight = heightMap.GetLength(1);
    }

    public void run()
    {
        // generate random position
        position[0] = rand.Next(0, mapWidth);
        position[1] = rand.Next(0, mapHeight);

        while(token > 0) // this agent alive
        {
            smooth();
            move();
            token--;
        }
    }

    private void smooth()
    {
        // do the smooth
        float value = 0;
        int count = 0;
        int[] temp_pos = new int[2];
        for (int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                temp_pos[0] = position[0] + i;
                temp_pos[1] = position[1] + j;
                if (isInBound(temp_pos)) // in the height map boundary
                {
                    value += heightMap[temp_pos[0], temp_pos[1]];
                    count += 1;
                }
            }
        }
        heightMap[position[0], position[1]] = value / count;
    }

    private void move()
    {
        // move to the next random location
        int[] temp_pos = new int[2];
        ArrayList ints = new ArrayList();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if(i == 0 && j == 0)
                {
                    continue;
                }
                temp_pos[0] = position[0] + i;
                temp_pos[1] = position[1] + j;
                if (isInBound(temp_pos)) // in the height map boundary
                {
                    ints.Add(temp_pos);
                }
            }
        }
    }

    private bool isInBound(int[] pos)
    {
        return pos[0] >= 0 && pos[0] < mapWidth && pos[1] >= 0 && pos[1] < mapHeight;
    }
}
