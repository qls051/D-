using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponName { AssultRifle = 0, Revlover, CombatKnife, HandGrenade}
[System.Serializable]
public struct WeaponSetting
{
    public WeaponName weaponName; // ���� �̸�
    public int damage; // ���� ���ݷ�
    public int curretMagazine; // ���� źâ ��
    public int maxMagazine; // �ִ� źâ ��
    public int currentAmmo; // ���� ź�� ��
    public int maxAmmo; // �ִ� ź�� ��
    public float attackRate; // ���� �ӵ�
    public float attackDistance; // ���� ��Ÿ�
    public bool isAutomaticAttack; // ���� ���� ����
}
