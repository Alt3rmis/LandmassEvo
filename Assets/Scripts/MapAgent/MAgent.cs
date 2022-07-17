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



    private int mapWidth;
    private int mapHeight;
    private int[] currentPos;
    private int[] currentDir;

    public MAgent(int numMoutains, float minAltitude, float maxAltitude, int moutainAgentToken,
                    int minStep, int maxStep, System.Random rand, float[,] heightMap, float mountainNeighbourMultiplier)
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
        this.currentDir = new int[2];
        this.mountainNeighbourMultiplier = mountainNeighbourMultiplier;
    }

    public void run()
    {
        int steps;
        int choice;
        while(numMoutains > 0)
        {
            // initialize current position randomly
            currentPos[0] = rand.Next(0, mapWidth);
            currentPos[1] = rand.Next(0, mapHeight);

            // set moutain length randomly
            steps = rand.Next(minStep, maxStep);

            choice = rand.Next(0, 4);
            switch (choice)
            {
                case 0: // UP
                    currentDir[0] = 0;
                    currentDir[1] = -1;
                    break;
                case 1: // DOWN
                    currentDir[0] = 0;
                    currentDir[1] = 1;
                    break;
                case 2: // LEFT
                    currentDir[0] = -1;
                    currentDir[1] = 0;
                    break;
                case 3: // RIGHT
                    currentDir[0] = 1;
                    currentDir[1] = 0;
                    break;
                default:
                    break;
            }
            while(steps > 0 && token > 0)
            {
                int minAl = (int)minAltitude * 100;
                int maxAl = (int)maxAltitude * 100;
                heightMap[currentPos[0], currentPos[1]] = rand.Next(minAl, maxAl) / 100.0f;
                

            }
        }
    }
}