using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour {

    public Transform wall;
    public Transform wallParent;

	public Transform floor;
	public Transform mazeBase;
	public Transform exit;

    private List<string> urls;
    public int size = 8;
	public string subreddit = "earthporn";

    const int maxPictures = 50;

    private List<Transform> walls = new List<Transform>();

    void Start ()
    {
        GenerateMaze();
		GenerateFloor();
		GenerateExit ();
        StartCoroutine(GetComponent<RedditLoader>().GetJSON(subreddit, "month", Mathf.Min(maxPictures, walls.Count)));
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

	private void GenerateFloor() {
		Transform ground = Instantiate (floor, mazeBase);
		Vector3 scale = ground.localScale; 
		scale.x *= size / 10.0f;
		scale.z *= size / 10.0f;
		ground.localScale = scale;
		ground.localPosition = new Vector3 (size / 2.0f, -0.5f, size / 2.0f);
	}

	private void GenerateExit() {
		Transform theExit = Instantiate (exit, mazeBase);
//		Vector3 scale = ground.localScale; 
//		scale.x *= size / 10.0f;
//		scale.z *= size / 10.0f;
//		ground.localScale = scale;
		theExit.localPosition = new Vector3 (size, 0, size);
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

			Texture2D tex = new Texture2D (0, 0); 
			walls [i].GetComponent<Renderer> ().material.mainTexture = tex;

            www.LoadImageIntoTexture(walls[i].GetComponent<Renderer>().material.mainTexture as Texture2D);

            Debug.Log((float)(i+1)/ walls.Count);

			yield return null;
        }

      


    }
}
