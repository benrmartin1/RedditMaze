using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour {

    public Transform wall;
    public Transform wallParent;

    private List<string> urls;
    private int size = 10;

    private List<Transform> walls = new List<Transform>();

    // Use this for initialization
    void Start ()
    {
        GenerateMaze();

        StartCoroutine(GetComponent<RedditLoader>().GetJSON("catsstandingup", "month", walls.Count));
	}

    private void GenerateMaze()
    {
        Maze maze = new Maze(size);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                MazeCell mazeCell = maze.mazeCells[x, y];
                if (mazeCell.northWall)
                {
                    if (y == size - 1 && x == size - 1)
                    {
                        // finish
                    }
                    else
                    {
                        walls.Add(Instantiate(wall, new Vector3(x + 0.5f, 0, y + 1) * wallParent.parent.localScale.x, Quaternion.Euler(0, 90, 0), wallParent));
                    }
                }
                if (mazeCell.eastWall)
                {
                    walls.Add(Instantiate(wall, new Vector3(x + 1, 0, y + 0.5f) * wallParent.parent.localScale.z, Quaternion.identity, wallParent));
                }
            }
        }
        for (int x = 1; x < size; x++)
        {
            walls.Add(Instantiate(wall, new Vector3(x + 0.5f, 0, 0) * wallParent.parent.localScale.x, Quaternion.Euler(0, 90, 0) , wallParent));
        }
        for (int y = 0; y < size; y++)
        {
            walls.Add(Instantiate(wall, new Vector3(0, 0, y + 0.5f) * wallParent.parent.localScale.z, Quaternion.identity, wallParent));
        }
    }

    bool loaded = false;
    void Update ()
    {
        if (!loaded && GetComponent<RedditLoader>().pictureUrls.Count != 0)
        {
            loaded = true;
            urls = GetComponent<RedditLoader>().pictureUrls;
            StartCoroutine(LoadPictures());
        }
    }

    IEnumerator LoadPictures()
    {
        List<Texture2D> texs = new List<Texture2D>();
        for(int i = 0; i < walls.Count; i++)
        {
            WWW www = new WWW(urls[i %  urls.Count]);
            yield return www;

            walls[i].GetComponent<Renderer>().material.mainTexture = new Texture2D(0,0);
            www.LoadImageIntoTexture(walls[i].GetComponent<Renderer>().material.mainTexture as Texture2D);

            Debug.Log((float)(i+1 )/ walls.Count);
        }

      


    }
}
