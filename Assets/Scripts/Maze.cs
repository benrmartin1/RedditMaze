using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{

    public MazeCell[,] mazeCells;
    int size;

    public Maze(int size)
    {
        this.size = size;
        mazeCells = new MazeCell[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                mazeCells[x, y] = new MazeCell(x,y);
            }
        }

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                MazeCell cell = mazeCells[x, y];
                if(y == size - 1)
                {
                    cell.north = null;
                }
                else
                {
                    cell.north = mazeCells[x, y + 1];
                }
                if(y <= 0)
                {
                    cell.south = null;
                }
                else
                {
                    cell.south = mazeCells[x, y - 1];
                }
                if(x == size - 1)
                {
                    cell.east = null;
                }
                else
                {
                    cell.east = mazeCells[x + 1, y];
                }
                if(x <= 0)
                {
                    cell.west = null;
                }
                else
                {
                    cell.west = mazeCells[x - 1, y];
                }

            }
        }

        Stack<MazeCell> mazeCellStack = new Stack<MazeCell>();

        // Make the initial cell the current cell and mark it as visited
        MazeCell currentCell = mazeCells[0, 0];
        currentCell.Visit();

        // While there are unvisited cells
        while (true)
        {
            // 1. If the current cell has any neighbours which have not been visited
            if (currentCell.HasUnvisited())
            {
                // Choose randomly one of the unvisited neighbours
                MazeCell neighbor = currentCell.UnvisitedNeighbor();
                // Push the current cell to the stack
                mazeCellStack.Push(currentCell);
                // Remove the wall between the current cell and the chosen cell
                //Debug.Log("Current--x: " + currentCell.GetPosition()[0] + ", y: " + currentCell.GetPosition()[1]);
                //Debug.Log("Neighbor--x: " + neighbor.GetPosition()[0] + ", y: " + neighbor.GetPosition()[1]);
                if(currentCell.GetPosition()[0] == neighbor.GetPosition()[0] - 1)
                {
                    // neighbor is east of current
                    currentCell.eastWall = false;
                }
                else if (currentCell.GetPosition()[0] == neighbor.GetPosition()[0] + 1)
                {
                    // neighbor is west of current
                    neighbor.eastWall = false;
                }
                else if (currentCell.GetPosition()[1] == neighbor.GetPosition()[1] - 1)
                {
                    // neighbor is north of current
                    currentCell.northWall = false;
                }
                else if (currentCell.GetPosition()[1] == neighbor.GetPosition()[1] + 1)
                {
                    // neighbor is south of current
                    neighbor.northWall = false;
                }
                else
                {
                    // something went wrong in neighbor picking
                    Debug.Log("Something went wrong picking neighbors");
                }
                // Make the chosen cell the current cell and mark it as visited
                currentCell = neighbor;
                currentCell.Visit();
            }
            // Else if stack is not empty
            else if(mazeCellStack.Count != 0)
            {
                // Pop a cell from the stack, make it the current cell
                currentCell = mazeCellStack.Pop();
            }
            else
            {
                break;
            }
        }
    }

    // Retuns true if there are still unvisited cells
    private bool UnvisitedCells()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // if the cell is unvisited, there are still unvisited cells, so return true
                if(mazeCells[x, y].IsVisited())
                {
                    return true;
                }
            }
        }
        return false;
    }

}
