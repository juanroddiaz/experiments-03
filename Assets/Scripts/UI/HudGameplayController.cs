using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudGameplayController : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField]
    private OnTapEventListener _onTapListener;
    [Header("Panels")]
    [SerializeField]
    private GameObject _controlsPanel;
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private GameObject _topPanel;
    [SerializeField]
    private GameObject _startLevelPanel;
    [SerializeField]
    private GameObject _endLevelPanel;
    [Header("Counters")]
    [SerializeField]
    private TextMeshProUGUI _timeCountdown;
    [SerializeField]
    private TextMeshProUGUI _coinsCounter;
    [SerializeField]
    private TextMeshProUGUI _startLevelCountdown;
    [SerializeField]
    private TextMeshProUGUI _endLevelCoinsCounter;
    [SerializeField]
    private TextMeshProUGUI _endLevelMaxCoinsCounter;
    [SerializeField]
    private Transform _coinTarget;

    private ScenarioController _sceneController;

    public void Initialize(ScenarioController controller, Action onDown)
    {
        _sceneController = controller;
        _onTapListener.Initialize(onDown);
        OnStartLevel();
    }

    private void OnStartLevel()
    {
        _topPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _controlsPanel.SetActive(false);
        _endLevelPanel.SetActive(false);
        _startLevelPanel.SetActive(true);
        StartCoroutine(StartLevelCountdown());
    }

    private IEnumerator StartLevelCountdown()
    {
        _startLevelCountdown.text = "3";
        yield return new WaitForSeconds(1.0f);
        _startLevelCountdown.text = "2";
        yield return new WaitForSeconds(1.0f);
        _startLevelCountdown.text = "1";
        yield return new WaitForSeconds(1.0f);
        _startLevelCountdown.text = "GO!";
        yield return new WaitForSeconds(1.0f);
        _controlsPanel.SetActive(true);
        _startLevelPanel.SetActive(false);
        _topPanel.SetActive(true);
        _sceneController.StartLevel();
        yield return null;
    }

    public Transform GetCoinHudTargetTransform()
    {
        return _coinTarget;
    }

    public void OnPause()
    {
        _sceneController.TogglePause(true);
        _pausePanel.SetActive(true);
        _controlsPanel.SetActive(false);
    }

    public void OnUnpause()
    {
        _sceneController.TogglePause(false);
        _pausePanel.SetActive(false);
        _controlsPanel.SetActive(true);
    }

    public void OnReplay()
    {
        GameController.Instance.SceneLoader.ReloadScene();
    }

    public void OnBackToMainMenu()
    {
        GameController.Instance.SceneLoader.LoadMainMenu();
    }

    public void OnQuitGame()
    {
        _sceneController.OnQuit();
    }

    public void UpdateCoinCounter(int amount)
    {
        _coinsCounter.text = "x" + amount.ToString();
    }

    public void UpdateLevelCountdown(float countdown, bool levelFinished)
    {
        _timeCountdown.text = countdown.ToString("00.00");
        if(levelFinished)
        {
            OnFinishLevel();
        }
    }

    private void OnFinishLevel()
    {
        _topPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _controlsPanel.SetActive(false);
        _endLevelPanel.SetActive(true);
        _sceneController.FinishLevel();
        _endLevelCoinsCounter.text = "x" + _sceneController.LevelCoins.ToString();
        _endLevelMaxCoinsCounter.text = "x" + _sceneController.GetMaxLevelCoins().ToString();
    }
}