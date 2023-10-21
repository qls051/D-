using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionObject : MonoBehaviour
{
    [Header("Interaction Object")]
    [SerializeField] 
    protected int maxHp = 100;
    protected int currentHP;

    private void Awake()
    {
        currentHP = maxHp;
    }

    public abstract void TakeDamage(int damage);
}
