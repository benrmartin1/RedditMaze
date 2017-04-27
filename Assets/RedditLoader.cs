using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;

//https://www.reddit.com/r/pics/top.json?sort=top&t=month&limit=5

public class RedditLoader : MonoBehaviour
{

    [Serializable]
    public class RedditData
    {
        public string kind;
        public DataJSON data;

    }
    [Serializable]
    public class DataJSON
    {
        public RedditData[] children;

        public string url;
    }


    public static RedditData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<RedditData>(jsonString);
    }

    private string url = "https://www.reddit.com/r/<subreddit>/top.json?sort=top&t=<time>&limit=<limit>";
    private RedditData redditData;

    public IEnumerator GetJSON(string subreddit, string time, int limit)
    {
        SetTime(time);
        SetLimit(limit);
        SetSubreddit(subreddit);

        print("Getting JSON for: " + url);

        WWW www = new WWW(url);
        yield return www;

        redditData = CreateFromJSON(www.text);
        LoadPicturesFromJson();
    }


    private void SetTime(string time)
    {
        string[] times = { "hour", "day", "week", "month", "year", "all" };
        if(Array.IndexOf(times, time) >= 0)
{
            url = url.Replace("<time>", time);
        }
    }

    private void SetSubreddit(string subreddit)
    {
        url = url.Replace("<subreddit>", subreddit);
    }

    private void SetLimit(int limit)
    {
        url = url.Replace("<limit>", limit.ToString());
    }

    public List<string> pictureUrls;
    private void LoadPicturesFromJson()
    {
        RedditData[] childrenData = redditData.data.children;
        pictureUrls = new List<string>();

        for (int i = 0; i < childrenData.Length; i++)
        {
            DataJSON childData = childrenData[i].data;
            if (childData.url.EndsWith(".jpg") || childData.url.EndsWith(".png"))
            {
                pictureUrls.Add(childData.url);
            }
        }
    }

}
