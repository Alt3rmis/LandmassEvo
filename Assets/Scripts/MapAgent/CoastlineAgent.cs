using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoastlineAgent
{
    private System.Random rand;
    private int[] currentLocation = new int[2];
    private int[] startPoint = new int[2];
    private int[] attractor = new int[2]; // target point
    private int[] repulsor = new int[2]; // avoid point
    //private float[,] noiseMap;
    private int mapWidth;
    private int mapHeight;
    private int[,] directions = { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 },
                                  { -1, -1 }, { 1, -1 }, { -1, 1 }, { 1, 1 },};
    private int[] chosenDir = new int[2];
    private int numberOfChange;
    // private List<int[]> parentList = new List<int[]>();
    // private List<int[]> turningList = new List<int[]>();
    private static List<int[]> border = new List<int[]>();

    public CoastlineAgent(System.Random r, float[,] map)
    {
        // Debug.Log("Construct Coastline Agent!");
        rand = r;
        // noiseMap = map;
        mapWidth = map.GetLength(0);
        mapHeight = map.GetLength(1);
        // Init();
    }
    // setters & getters
    private void setStartPoint(int[] sp)
    {
        startPoint[0] = sp[0];
        startPoint[1] = sp[1];
    }
    private void Init()
    {
        // Debug.Log("Initing...");
        // get a random start location
        if(border.Count == 0) // if the turning list is empty, randomly choose
        {
            startPoint[0] = rand.Next(0, mapWidth);
            startPoint[1] = rand.Next(0, mapHeight);
        }
        else // choose from a given turning list
        {
            int index = rand.Next(0, border.Count);
            setStartPoint(border[index]);
        }
        // Debug.Log("just initialized startPoint(" + startPoint[0] + "," + startPoint[1] + ")");
        currentLocation[0] = startPoint[0];
        currentLocation[1] = startPoint[1];
        // get random direction
        int temp = rand.Next(0, 8);
        chosenDir[0] = directions[temp, 0];
        chosenDir[1] = directions[temp, 1];

        numberOfChange = rand.Next(50, 100);
        // Debug.Log("Init Finished!");
    }

    // get the score of a particular point
    private int Score(int[] point)
    {
        // Debug.Log("Scoring...");
        int dr = getDistanceSquare(point, repulsor);
        int da = getDistanceSquare(point, attractor);
        // TODO
        // add distance to edge func
        // this need the noise map being passed in adance as a member
        int[] distances = { point[0], point[1], mapWidth - point[0], mapHeight - point[1] };
        // get the smallest value
        // which is the closet value
        int min = int.MaxValue;
        for(int i = 0; i < distances.GetLength(0); ++i)
        {
            if(min > distances[i])
            {
                min = distances[i];
            }
        }
        return dr - da + 5*(min*min); // original equation in paper
    }

    // get teh distance square from a point to another
    private int getDistanceSquare(int[] p1, int[] p2)
    {
        int[] temp = { p1[0] - p2[0], p1[1] - p2[1] };
        return temp[0] * temp[0] + temp[1] * temp[1];
    }

    // check if agent is on coast?
    // this alg is not perfect
    // only check 4 directions
    private bool onCoast(float[,] noiseMap)
    {
        int[] temp = new int[2];
        for(int i = 0; i < 8; i++)
        {
            temp[0] = currentLocation[0] + directions[i, 0];
            temp[1] = currentLocation[1] + directions[i, 1];
            if(isInMap(temp) && noiseMap[temp[0], temp[1]] == 0)
            {
                return true;
            }
        }
        return false;
    }
    private bool isInMap(int[] point)
    {
        int x = point[0];
        int y = point[1];

        return 0 <= x && x < mapWidth && 0 <= y && y < mapHeight;
    }
    public float[,] run(float[,] noiseMap)
    {
        // Debug.Log("Agent running...");
        Init();
        // check if on coast
        while(!onCoast(noiseMap))
        {
            // go find coast
            currentLocation[0] += chosenDir[0];
            currentLocation[1] += chosenDir[1];
            if (!isInMap(currentLocation))
            {
                return noiseMap;
            }
        }
        /*if (!isInMap(currentLocation))
        {
            return noiseMap;
        }*/
        // Debug.Log("Find coastline!");
        // on the edge of coastline
        // turn sea to land
        // generate attractor and repulsor
        attractor[0] = rand.Next(0, mapWidth);
        attractor[1] = rand.Next(0, mapHeight);
        repulsor[0] = rand.Next(0, mapWidth);
        repulsor[1] = rand.Next(0, mapHeight);
        while (numberOfChange > 0 && isInMap(currentLocation))
        {
            //check 4 directions score
            int[] scores = new int[8];
            for(int i = 0; i < 8; i++)
            {
                int[] tempLoc = new int[2];
                tempLoc[0] = currentLocation[0] + directions[i, 0];
                tempLoc[1] = currentLocation[1] + directions[i, 1];
                if(isInMap(tempLoc) && noiseMap[tempLoc[0], tempLoc[1]] != 1)
                {
                    scores[i] = Score(tempLoc);
                }
                else
                {
                    scores[i] = int.MinValue;
                }
            }
            int maxValue = scores.Max();
            int maxIndex = scores.ToList().IndexOf(maxValue);
            
            noiseMap[currentLocation[0], currentLocation[1]] = 1;
            border.Add(currentLocation);
            currentLocation[0] += directions[maxIndex, 0];
            currentLocation[1] += directions[maxIndex, 1];
            numberOfChange--;
        }
        // noiseMap[attractor[0], attractor[1]] = 2;
        // noiseMap[repulsor[0], repulsor[1]] = 3;
        // Debug.Log("startPoint(" + startPoint[0] + "," + startPoint[1] + ")");
        // noiseMap[startPoint[0], startPoint[1]] = 4;
        // Debug.Log("startPoint(" + startPoint[0] + "," + startPoint[1] + ")");
        return noiseMap;
    }
}