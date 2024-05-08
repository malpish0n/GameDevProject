using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    private Slider _slider;
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    void Update()
    {
        _slider.value = _playerStats._stamina / 100f;
    }
}
