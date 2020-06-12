using System.Collections;
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

    private List<Vector3> _availableCells = new List<Vector3>();

    private void Awake()
    {
        _character.Initialize();
        _hud.Initialize(this, _character.OnTapDown);
        InitializeTilemap();
    }

    private void InitializeTilemap()
    {
        Tilemap tileMap = _tilemapGrid.GetComponentInChildren<Tilemap>();
        BoundsInt size = tileMap.cellBounds;

        for (int n = size.xMin; n < size.xMax; n++)
        {
            for (int p = size.yMin; p < size.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                if (tileMap.HasTile(localPlace))
                {
                    //Tile at "place"                    
                }
                else
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

    public void TogglePause(bool toggle)
    {
        // cheap but effective
        Time.timeScale = toggle ? 0.0f : 1.0f;
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
