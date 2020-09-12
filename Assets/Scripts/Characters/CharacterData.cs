using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] int maxHealt;

    public int MaxHealth => maxHealt;
    public int Health { get; private set; }

    public event Action<int, int> OnHealthChange = (ov, nv) => { };

    private void Awake()
    {
        Health = maxHealt;

        DevelopViewManager.Instance.AddFunction("DamageFromDATA", () => Damage(10));
        DevelopViewManager.Instance.AddFunction("RiseHealth", () => HealthChange(5));
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
