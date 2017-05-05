using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
    public MazeCell north;
    public MazeCell south;
    public MazeCell east;
    public MazeCell west;

    public bool northWall = true;
    public bool eastWall = true;

    bool visited = false;

    int xPos, yPos;

    public MazeCell(int xPos, int yPos)
    {
        this.yPos = yPos;
        this.xPos = xPos;
    }

    public void SetNeighbors(MazeCell theNorth, MazeCell theSouth, MazeCell theEast, MazeCell theWest)
    {
        north = theNorth;
        south = theSouth;
        east = theEast;
        west = theWest;
    }

    public bool IsVisited()
    {
        return visited;
    }

    public int[] GetPosition()
    {
        int[] positions = new int[2];
        positions[0] = xPos;
        positions[1] = yPos;
        return positions;
    }

    public void Visit()
    {
        visited = true;
        //Debug.Log("Visited cell: " + xPos + ", " + yPos);
    }

    public bool HasUnvisited()
    {
        if((north != null && !north.visited) || (south != null && !south.visited) || (east != null && !east.visited) || (west != null && !west.visited))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public MazeCell UnvisitedNeighbor()
    {
        if(!HasUnvisited())
        {
            return null;
        }

        List<MazeCell> unvisited = new List<MazeCell>();
        if(north != null && !north.visited)
        {
            unvisited.Add(north);
        }
        if (south != null && !south.visited)
        {
            unvisited.Add(south);
        }
        if (west != null && !west.visited)
        {
            unvisited.Add(west);
        }
        if (east != null && !east.visited)
        {
            unvisited.Add(east);
        }

        int random = UnityEngine.Random.Range(0, unvisited.Count);
        //Debug.Log("choosing " + random + " for cell " + xPos + ", " + yPos);
        return unvisited[random];

    }
}

