using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float _maxStamina = 100;
    public float _stamina;
    private int _health;
    private DateTime lastStaminaChangeTime;
    private DateTime _gameStartTime;

    private void Start()
    {
        _stamina = _maxStamina;
        _gameStartTime = DateTime.Now;
    }

    private void Update()
    {
        RecoverStamina();
    }

    public void loseStamina(float stamina)
    {
        _stamina -= stamina;
        lastStaminaChangeTime = DateTime.Now;
    }

    private float TimeSinceLastStaminaChange()
    {
        TimeSpan timeSinceLastChange = DateTime.Now - lastStaminaChangeTime;
        return (float)timeSinceLastChange.TotalSeconds;
    }

    private void RecoverStamina()
    {
        if (TimeSinceLastStaminaChange() >= 3f && _stamina < _maxStamina)
        {
            _stamina = Mathf.Lerp(_stamina, _maxStamina, Time.deltaTime * 2f);
        }
    }
}
