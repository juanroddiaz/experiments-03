﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScenarioController : MonoBehaviour
{
    [SerializeField]
    private CharacterManager _character;
    [SerializeField]
    private HudGameplayController _hud;
    [SerializeField]
    private Grid _tilemapGrid;
    [SerializeField]
    private Transform _emptyCellsParent;
    [SerializeField]
    private CoinObjectLogic _coinPrefab;
    [SerializeField]
    private float _levelTotalTime = 20.0f;
    [SerializeField]
    private Vector2 _characterInitialPosition;

    private List<Vector3> _availableCells = new List<Vector3>();
    private GameLevelData _levelData;

    public float CurrentLevelTime { get; private set; }
    public bool LevelStarted { get; private set; }
    public int LevelCoins { get; private set; }

    private void Awake()
    {
        _character.Initialize(this);
        _hud.Initialize(this, _character.OnTapDown);
        InitializeTilemap();
        // get level name
        _levelData = new GameLevelData
        {
            Name = "TestLevel_01",
            MaxCoins = 0
        };
        int levelMaxCoins = GameController.Instance.DataLoader.GetLevelMaxCoins(_levelData.Name);
        Debug.Log("Level " + _levelData.Name + " max coins: " + levelMaxCoins.ToString());
        CurrentLevelTime = _levelTotalTime;
        LevelStarted = false;
    }

    private void InitializeTilemap()
    {
        Tilemap tileMap = _tilemapGrid.GetComponentInChildren<Tilemap>();
        BoundsInt size = tileMap.cellBounds;

        for (int n = size.xMin; n < size.xMax; n++)
        {
            for (int p = size.yMin; p < size.yMax; p++)
            {
                if(_characterInitialPosition.x == n && _characterInitialPosition.y == p)
                {
                    continue;
                }

                Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                if (!tileMap.HasTile(localPlace))
                {
                    //No tile at "place"
                    Vector3 place = tileMap.CellToWorld(localPlace);
                    _availableCells.Add(place);
                    var emptyCellObj = Instantiate(_coinPrefab, _emptyCellsParent);
                    emptyCellObj.name = "cell_" + n.ToString() + "_" + p.ToString();
                    emptyCellObj.transform.position = place;
                }
            }
        }
    }

    public void StartLevel()
    {
        LevelStarted = true;
    }

    public void FinishLevel()
    {
        _levelData.MaxCoins = LevelCoins;
        GameController.Instance.DataLoader.TrySaveLevelMaxCoins(_levelData);
    }

    public void TogglePause(bool toggle)
    {
        // cheap but effective
        Time.timeScale = toggle ? 0.0f : 1.0f;
    }

    public void OnCoinCollected(int amount)
    {
        LevelCoins += amount;
        _hud.UpdateCoinCounter(LevelCoins);
    }

    private void Update()
    {
        if (!LevelStarted)
        {
            return;
        }

        CurrentLevelTime -= Time.deltaTime;
        if (CurrentLevelTime <= 0.0f)
        {
            // end level
            LevelStarted = false;
            CurrentLevelTime = 0.0f;
        }

        // hud update time and check level end
        _hud.UpdateLevelCountdown(CurrentLevelTime, !LevelStarted);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
