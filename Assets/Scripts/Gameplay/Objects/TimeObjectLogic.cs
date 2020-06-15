using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObjectLogic : MonoBehaviour
{
    [SerializeField]
    private LootRewardFeedback _feedbackObj;

    private Transform _hudTimeTarget;
    private HudGameplayController _hudGameplayController;
    private Action _onAfterCollected;

    public void Initialize(HudGameplayController hudGameplay, Action afterCollected)
    {
        _hudGameplayController = hudGameplay;
        _hudTimeTarget = hudGameplay.GetCoinHudTargetTransform();
        _onAfterCollected = afterCollected;
    }

    public void OnCollected(int amount)
    {
        GenerateRewardVfx(amount);
        _onAfterCollected.Invoke();
        Destroy(gameObject);
    }

    private void GenerateRewardVfx(int amount)
    {
        var rewardGo = Instantiate(_feedbackObj);
        LootRewardFeedback feedback = rewardGo.GetComponent<LootRewardFeedback>();
        var targetPosition = _hudTimeTarget.position;
        targetPosition.z = -Camera.main.transform.position.z;
        var data = new LootRewardFeedbackData
        {
            StartTransform = transform,
            TargetPosition = Camera.main.ScreenToWorldPoint(targetPosition),
            OnFeedbackStarts = null,
            OnFeedbackReachedEnd = _hudGameplayController.UpdateCoinCounter,
            Amount = amount
        };
        feedback.Init(data);
    }
}
