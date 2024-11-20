using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public Animator anim;
    public CharacterController characterController;

    public float moveSpeed = 4f;
    public float rotationSpeed = 10f;
    public float jumpHeight = 3f;
    public float runSpeed = 8f;

    private Vector3 moveDir;
    public LayerMask layer;

    private Camera mainCamera;
    private bool ground = false;
    private bool moveCheck;

    //private float groundCheckDistance = 0.25f; // 땅 체크 거리

    private float gravity = -9.81f; // 중력 가속도
    private float verticalVelocity = 0f; // 점프 및 낙하 속도

    private void Awake()
    {
        //PlayerManager.Instance.RegisterPlayer(this.gameObject);
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;       
    }
    private void Start()
    {
        PlayerManager.Instance.RegisterPlayer(this.gameObject);
    }
    private void OnDestroy()
    {
        PlayerManager.Instance.UnregisterPlayer(this.gameObject);
    }
    private void Update()
    {
        Movement(true);
        //CheckGround();
        if (characterController.isGrounded)
        {
            //ground = true;
            //anim.SetBool("IsJumping", false);
            //verticalVelocity = 0f; // 땅에 있을 때 수직 속도 초기화
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //ground = false;
                verticalVelocity = Mathf.Sqrt(jumpHeight * -1.5f * gravity); // 점프 계산
                anim.SetBool("IsJumping", true);
            }
            else
            {
                //ground = true;
                anim.SetBool("IsJumping", false);
                //verticalVelocity = 2f; // 땅에 있을 때 수직 속도 초기화
            }

        }
        else
        { 
            
        }
    }

    public void Movement(bool controll)
    {
        if (controll)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            moveDir = forward * vertical + right * horizontal;
            moveDir.Normalize();

            if (moveDir != Vector3.zero)
            {
                float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

                // 이동 방향으로 캐릭터 회전
                Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                // 이동
                characterController.Move(moveDir * speed * Time.deltaTime);
                anim.SetBool("IsRun", Input.GetKey(KeyCode.LeftShift));
                anim.SetFloat("IsWalk", moveDir.magnitude);
            }
            else
            {
                anim.SetBool("IsRun", false);
                anim.SetFloat("IsWalk", 0f);
            }

            // 중력 적용
            verticalVelocity += gravity * Time.deltaTime;
            characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
        }
    }

    private void CheckGround()
    {

        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f; // 발에서 조금 위로 시작
        if (Physics.Raycast(rayStart, Vector3.down, out hit, 0.3f, layer))
        //if (characterController.isGrounded)
        {
            ground = false;
            anim.SetBool("IsJumping", true);
            //ground = true;
            //verticalVelocity = 0f; // 땅에 있을 때 수직 속도 초기화
            //anim.SetBool("IsJumping", false);
        }
        else
        {
            ground = true;
            anim.SetBool("IsJumping", false);
        }
    }
}
