using System.IO;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

public class Configuration : MonoBehaviour 
{
    private static Dictionary<string, string> mConfigDict = new Dictionary<string, string>();

    private void Awake()
    {
        LoadConfig();

        SetResolution();
    }

    private void LoadConfig()
    {
        string configPath = Application.streamingAssetsPath + "/config.txt";

        if (!File.Exists(configPath)) return;

        using (StreamReader sr = new StreamReader(configPath, Encoding.UTF8))
        {
            string line = string.Empty;

            while (!string.IsNullOrEmpty(line = sr.ReadLine()))
            {
                if (line.Contains("="))
                {
                    string[] lineArray = line.Split('=');

                    string key = lineArray[0].Trim();
                    string value = lineArray[1].Trim();

                    Add(key, value);
                }
            }
        }
    }

    private void SetResolution()
    {
        int width = int.Parse(GetContent("Width"));
        int height = int.Parse(GetContent("Height"));

        Screen.SetResolution(width, height, false);
    }

    private void Add(string key, string value)
    {
        if (mConfigDict.ContainsKey(key)) return;

        mConfigDict.Add(key, value);
    }

    private void Remove(string key)
    {
        if (!mConfigDict.ContainsKey(key)) return;

        mConfigDict.Remove(key);
    }

    public static string GetContent(string key)
    {
        if (!mConfigDict.ContainsKey(key)) return null;

        return mConfigDict[key];
    }
}
