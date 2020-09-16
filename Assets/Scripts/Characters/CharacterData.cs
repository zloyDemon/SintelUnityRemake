using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] int maxHealt;

    public int MaxHealth => maxHealt;
    public int Health { get; private set; }
    public bool IsAlive => Health > 0;

    public event Action<int, int> OnHealthChange = (ov, nv) => { };

    private void Awake()
    {
        Health = maxHealt;
    }

    public void Damage(int damage)
    {
        HealthChange(-damage);
    }

    private void HealthChange(int minusValue)
    {
        var oldValue = Health;
        Health += minusValue;
        Health = Mathf.Clamp(Health, 0, maxHealt);
        OnHealthChange(oldValue, Health);
    }
}
