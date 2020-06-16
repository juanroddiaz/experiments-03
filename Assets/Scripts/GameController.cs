using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string Name;
    public GameObject GamePrefab;
    public Sprite MenuImage;
    public Vector2 TimeBonusCellPosition;
}

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameDataLoader _dataLoader;
    [SerializeField]
    private SceneLoader _sceneLoader;

    [SerializeField]
    private List<LevelData> _levelData;

    public static GameController Instance { get; private set; }
    public SceneLoader SceneLoader { get { return _sceneLoader; } }
    public GameDataLoader DataLoader => _dataLoader;
    public int SelectedLevelIdx = 0;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        _dataLoader.Initialize(GetAllLevelNames());
        _sceneLoader.Initialize();
        SelectedLevelIdx = _dataLoader.LastSelectedLevel;
    }

    private List<string> GetAllLevelNames()
    {
        var list = new List<string>();
        foreach (var entry in _levelData)
        {
            list.Add(entry.Name);
        }
        return list;
    }

    public List<Sprite> GetMenuLevelSprites()
    {
        var list = new List<Sprite>();
        foreach (var entry in _levelData)
        {
            list.Add(entry.MenuImage);
        }
        return list;
    }

    public string GetLevelNameByIdx(int idx)
    {
        if (idx >= 0 && idx < _levelData.Count)
        {
            return _levelData[idx].Name;
        }
        Debug.LogError("Wrong index for level name!");
        return "???";
    }

    public void LoadGameplayScenario(int carouselIndex)
    {
        SelectedLevelIdx = carouselIndex;
        _dataLoader.SaveLastSelectedLevel(carouselIndex);
        _sceneLoader.LoadGameplayScene();
    }

    public LevelData GetSelectedLevelData()
    {
        return _levelData[SelectedLevelIdx];
    }
}
