using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudGameplayController : MonoBehaviour
{
    [Header("Controls")]
    [Header("Panels")]
    [SerializeField]
    private GameObject _controlsPanel;
    [SerializeField]
    private GameObject _pausePanel;

    private ScenarioController _sceneController;

    public void Initialize(ScenarioController manager)
    {
        _sceneController = manager;
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
