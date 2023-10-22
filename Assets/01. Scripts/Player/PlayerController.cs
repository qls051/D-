using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipWalk; // �ȱ� ����
    [SerializeField] private AudioClip audioClipRun; // �ٱ� ����

    [Header("Input Keycods")]
    [SerializeField] private KeyCode keyCodeRun = KeyCode.LeftShift; // �޸��� Ű
    [SerializeField] private KeyCode keyCodeJump = KeyCode.Space; // ���� Ű
    [SerializeField] private KeyCode keyCodeReload = KeyCode.R; // ������ Ű

    //public float Movespeed = 7f;
    private AudioSource audioSource;
    private WeaponAssaultRifle weapon; // ���⸦ �̿��� ���� ����

    private RotateToMouse rotateToMouse; // ���콺 �̵����� ī�޶� ȸ��
    private MovementCharacterController movement; // Ű���� �Է����� �÷��̾� �̵�
    private Status status; // �̵��ӵ� �� �÷��̾� ����

    private void Awake()
    {
        //���콺 Ŀ�� �� ���̰� �����ϰ� ���� ��ġ�� ����
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = GetComponent<RotateToMouse>();
        movement= GetComponent<MovementCharacterController>();
        audioSource = GetComponent<AudioSource>();
        weapon = GetComponentInChildren<WeaponAssaultRifle>();
        status = GetComponent<Status>();
    }

    void Update()
    {
        UpdateRotate();
        UpdateMove();
        UPdateWeaponAction();
        UpdateJump();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if(x!= 0 || z!=0) // �̵��� �� �� �ȱ� or �ٱ�
        {
            bool isRun = false;

            // ���̳� �ڷ� �̵��� ���� �޸� �� ���� 
            if (z > 0) isRun = Input.GetKey(keyCodeRun);

            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            weapon.Animator.MoveSpped = isRun == true ? 1 : 0.5f;
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            if(audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else // ���ڸ��� ������ ��
        {
            movement.MoveSpeed = 0;
            weapon.Animator.MoveSpped = 0;
            
            if(audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }

        movement.MoveTo(new Vector3(x, 0, z));
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }

    private void UPdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }

        if (Input.GetMouseButtonDown(1))
        {
            weapon.StartWeaponAction(1);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            weapon.StopWeaponAction(1);
        }

        if (Input.GetKeyDown(keyCodeReload))
        {
            weapon.StartReload();
        }
    }

    public void TakeDamage(int damage)
    {
        bool isDie = status.DecreasHP(damage);

        if(isDie == true)
        {
            Debug.Log("Game Over"); 
        }
    }
}
 