using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainAgent
{
    private float[,] heightmap;
    private System.Random rand;
    private int token;


    private float minAltitude;
    private float maxAltitude;
    private float mountainNeighbourMultiplier;

    private int mapWidth;
    private int mapHeight;
    private int[] position;
    public MountainAgent(float[,] heightmap, int token, System.Random rand)
    {
        this.heightmap = heightmap;
        this.token = token;
        this.rand = rand;
        this.mapWidth = heightmap.GetLength(0);
        this.mapHeight = heightmap.GetLength(1);
        this.position = new int[2];
    }

    public void run()
    {
        while(token > 0)
        {
            createMountain();
            moveForwards();
            token--;
        }
    }

    /*private bool isInMap(int[] pos)
    {
        return pos[0] >= 0 && pos[0] < mapWidth && pos[1] >= 0 && pos[1] < mapHeight;
    }*/

    /*private int[] changeDirection() // change current direction randomly +-45 deg
    {
        
    }*/
    // this function only optimize moutain on the range
    private void createMountain()
    {
        position[0] = rand.Next(0, mapWidth);
        position[1] = rand.Next(0, mapHeight);

        
    }

    private void moveForwards()
    {
        
    }
}
