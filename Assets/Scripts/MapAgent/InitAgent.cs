using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAgent
{
    private float[,] heightmap;
    private System.Random rand;
    public InitAgent(float[,] heightmap, System.Random rand)
    {
        this.heightmap = heightmap;
        this.rand = rand;
    }

    public void run()
    {
        for(int i = 0; i < heightmap.GetLength(0); i++)
        {
            for( int j = 0; j < heightmap.GetLength(1); j++)
            {
                heightmap[i, j] = rand.Next(0, 100) / 100.0f;
            }
        }
    }
}
