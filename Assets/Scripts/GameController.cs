using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameDataLoader _dataLoader;
    [SerializeField]
    private SceneLoader _sceneLoader;

    [SerializeField]
    private List<string> _levelNames;

    public static GameController Instance { get; private set; }
    public SceneLoader SceneLoader { get { return _sceneLoader; } }

    public GameDataLoader DataLoader => _dataLoader;

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

        _dataLoader.Initialize(_levelNames);
        _sceneLoader.Initialize();
    }
}
