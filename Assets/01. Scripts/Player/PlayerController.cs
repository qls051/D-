using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipWalk; // 걷기 사운드
    [SerializeField] private AudioClip audioClipRun; // 뛰기 사운드

    [Header("Input Keycods")]
    [SerializeField] private KeyCode keyCodeRun = KeyCode.LeftShift; // 달리기 키
    [SerializeField] private KeyCode keyCodeJump = KeyCode.Space; // 점프 키
    [SerializeField] private KeyCode keyCodeReload = KeyCode.R; // 재장전 키

    public float Movespeed = 7f; // 원래 주석 달려있었음
    private AudioSource audioSource; 
    private WeaponBase weapon; // 모든 무기가 상속받는 기반 클래스
    private RotateToMouse rotateToMouse; // 마우스 이동으로 카메라 회전
    private MovementCharacterController movement; // 키보드 입력으로 플레이어 이동
    private Status status; // 이동속도 등 플레이어 정보

    PhotonView PV;

    PlayerManager playerManager;

    private void Awake()
    {
        //마우스 커서 안 보이게 설정하고 현재 위치에 고정
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = GetComponent<RotateToMouse>();
        movement= GetComponent<MovementCharacterController>();
        audioSource = GetComponent<AudioSource>();
        status = GetComponent<Status>();
        weapon = GetComponent<WeaponBase>(); // 게임 시작하면 널 레퍼 나서 추가한 코드

        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>(); 
    }

    void Update()
    {

        UpdateRotate();
        UpdateMove();
        UPdateWeaponAction();
        UpdateJump();

        if (!PV.IsMine) return;

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

        if(x!= 0 || z!=0) // 이동중 일 때 걷기 or 뛰기
        {
            bool isRun = false;

            // 옆이나 뒤로 이동할 때는 달릴 수 없음 
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
        else // 제자리에 멈췄을 때
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

    /*public void TakeDamage(int damage)
    {
        bool isDie = status.DecreasHP(damage);

        if(isDie == true)
        {
            Debug.Log("Game Over"); 
        }
    }*/
    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine) return;

        bool isDie = status.DecreasHP(damage);

        if (isDie == true)
        {
            Die();
        }
    }

    private void Die()
    {
        playerManager.Die();
    }
}
 