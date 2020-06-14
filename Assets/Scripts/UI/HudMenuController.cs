using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudMenuController : MonoBehaviour
{
    [SerializeField]
    private CarouselSelectorLogic _carouselLogic;
    [SerializeField]
    private Button _playButton;

    public void OnPlayButtonClick()
    {
        GameController.Instance.LoadGameplayScenario(_carouselLogic.CurrentIndex);
    }
}
