using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = transform.Find("Chicken/arms_assault_rifle_01").GetComponent<Animator>();
        //animator = GetComponentInChildren<Animator>();
    }

    public float MoveSpped
    {
        get => animator.GetFloat("movementSpeed");
        set => animator.SetFloat("movementSpeed", value);
    }

    public void OnReload()
    {
        animator.SetTrigger("onReload");
    }

    public bool AimModeIs
    { 
        set => animator.SetBool("isAimMode", value);
        get => animator.GetBool("isAimMode");
    }
    
    public void Play(string stateName, int layer, float normalizedTime)
    {
        animator.Play(stateName, layer, normalizedTime);
    }

    public bool CurretAnimationIs(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
