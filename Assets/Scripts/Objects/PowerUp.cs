using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUp", menuName = "PowerUp")]

public class PowerUp : ScriptableObject
{
    public string name;
    public GameObject prefab;
    public PowerUpType powerUpType;
    public float _duration;
}

public enum PowerUpType { speed, stamina  };
