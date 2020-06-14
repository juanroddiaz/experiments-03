using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CoinObjectState
{
    Coin,
    Chest,
}

public class CoinObjectLogic : MonoBehaviour
{
    [SerializeField]
    private float _randomSpawnMinTime = 2.0f;
    [SerializeField]
    private float _randomSpawnMaxTime = 5.0f;
    [SerializeField]
    private Collider2D _collider;
    [SerializeField]
    private LootRewardFeedback _feedbackObj;
    [Header("Coins")]
    [SerializeField]
    private GameObject _coinSpriteObj;
    [Header("Chests")]
    [SerializeField]
    private float _chestChancePercentage = 2.0f;
    [SerializeField]
    private float _chestBonusPercentage = 10.0f;
    [SerializeField]
    private float _chestSpawnCountdown = 5.0f;
    [SerializeField]
    private GameObject _chestSpriteObj;

    private CoinObjectState _state;
    private GameObject _currentSpriteObj;
    private Transform _hudCoinTarget;

    private void Awake()
    {
        _state = CoinObjectState.Coin;
        _currentSpriteObj = _coinSpriteObj;
        _coinSpriteObj.SetActive(true);
        _chestSpriteObj.SetActive(false);
    }

    public void Initialize(Transform hudCoinTarget)
    {
        _hudCoinTarget = hudCoinTarget;
    }

    public int OnCollected(int currentCoins)
    {
        var ret = 0;
        switch(_state)
        {
            case CoinObjectState.Coin:
                ret = 1;
                break;
            case CoinObjectState.Chest:
                ret = Mathf.CeilToInt(currentCoins * (1.0f + _chestBonusPercentage / 100.0f));
                break;
        }
        _currentSpriteObj.SetActive(false);
        _collider.enabled = false;
        // collect FX
        GenerateRewardVfx();
        StartCoroutine(Respawn());
        return ret;
    }

    private void GenerateRewardVfx()
    {
        var rewardGo = Instantiate(_feedbackObj);
        LootRewardFeedback feedback = rewardGo.GetComponent<LootRewardFeedback>();
        var targetPosition = _hudCoinTarget.position;
        targetPosition.z = -Camera.main.transform.position.z;
        var data = new LootRewardFeedbackData
        {
            StartTransform = _currentSpriteObj.transform,
            TargetPosition = Camera.main.ScreenToWorldPoint(targetPosition),
            OnFeedbackStarts = null,
            OnFeedbackReachedEnd = null
        };
        feedback.Init(data);
    }

    private IEnumerator Respawn()
    {
        var countdown = Random.Range(_randomSpawnMinTime, _randomSpawnMaxTime);
        yield return new WaitForSeconds(countdown);
        _currentSpriteObj.SetActive(true);
        _collider.enabled = true;
        yield return null;
    }
}
