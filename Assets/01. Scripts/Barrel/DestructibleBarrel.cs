using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DestructibleBarrel : InteractionObject
{
    [Header("Destructible Barrel")]
    [SerializeField] private GameObject destructibleBarralPieces;

    private bool isDestoryed = false;
    
    public override void TakeDamage(int damage)
    {
        currentHP -= damage;

        if(currentHP <= 0 && isDestoryed == false)
        {
            isDestoryed = true;

            Instantiate(destructibleBarralPieces, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
