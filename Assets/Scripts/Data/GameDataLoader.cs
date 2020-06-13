using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameLevelData
{
    public string Name = "";
    public int MaxCoins = 0;
}

public class GameDataLoader : MonoBehaviour
{
    public List<GameLevelData> GameData { get; private set; }

    public void Initialize(List<string> levelNames)
    {
        GameData = new List<GameLevelData>();
        foreach(var name in levelNames)
        {
            if (PlayerPrefs.HasKey(name))
            {
                GameData.Add(new GameLevelData {
                    Name = name,
                    MaxCoins = PlayerPrefs.GetInt(name)
                });
            }            
        }
    }

    public int GetLevelMaxCoins(string levelName)
    {
        var key = GameData.Find(k => string.Equals(k.Name, levelName));
        if (key == null)
        {
            return 0;
        }
        return key.MaxCoins;
    }

    public bool TrySaveLevelMaxCoins(GameLevelData data)
    {
        var key = GameData.Find(k => string.Equals(k.Name, data.Name));
        if (key == null)
        {
            GameData.Add(new GameLevelData
            {
                Name = data.Name,
                MaxCoins = data.MaxCoins
            });

            // save
            PlayerPrefs.SetInt(data.Name, data.MaxCoins);
            return true;
        }

        if(key.MaxCoins < data.MaxCoins)
        {
            // save
            PlayerPrefs.SetInt(data.Name, data.MaxCoins);
            return true;
        }

        return false;
    }
}
