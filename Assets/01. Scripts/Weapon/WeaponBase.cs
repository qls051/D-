using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum WeaponType { Main = 0, Sub, Melee, Throw }

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase")]
    [SerializeField] protected WeaponType weaponType; // ���� ����
    [SerializeField] protected WeaponSetting weaponSetting; // ���� ����

    protected float lastAttackTime = 0; // ������ �߻� üũ��
    protected bool isReload = false; // ������ ������ üũ
    protected bool isAttack = false; // ���� ���� üũ��
    protected AudioSource audioSource; // ���� ��� ������Ʈ
    protected PlayerAnimatorController animator; // �ִϸ��̼� ��� ����

    // �ܺο��� �̺�Ʈ �Լ� ����� �� �� �ֵ��� public ����
    [HideInInspector] public AmmoEvent onAmmoEvent = new AmmoEvent();
    [HideInInspector] public MagazineEvent onMagazineEvent = new MagazineEvent();

    // �ܺο��� �ʿ��� ������ �����ϱ� ���� ������ Get Property's
    public PlayerAnimatorController Animator => animator;
    public WeaponName WeaponName => weaponSetting.weaponName;
    public int CurrentMagazine => weaponSetting.curretMagazine;
    public int MaxMagazine => weaponSetting.maxMagazine;

    public abstract void StartWeaponAction(int type = 0);   
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();

    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop(); // ������ ����ϴ� ���� �����ϰ�,
        audioSource.clip = clip; // ���ο� ���� clip�� ��ü ��
        audioSource.Play(); // ���� ���
    }

    protected void Setup()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<PlayerAnimatorController>();
    }

    public virtual void IncreaseMagazine(int magazine)
    {
        weaponSetting.curretMagazine = CurrentMagazine + magazine > MaxMagazine ? MaxMagazine : CurrentMagazine + magazine;
        onMagazineEvent?.Invoke(CurrentMagazine);
    }
    
}
