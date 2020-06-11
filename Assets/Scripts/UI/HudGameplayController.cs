using System;
using System.Collections;
using System.Collections.Generic;
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

    private ScenarioController _sceneController;

    public void Initialize(ScenarioController manager, Action onDown)
    {
        _sceneController = manager;
        _onTapListener.Initialize(onDown);
        OnUnpause();
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

    public void OnQuitGame()
    {
        _sceneController.OnQuit();
    }
}
