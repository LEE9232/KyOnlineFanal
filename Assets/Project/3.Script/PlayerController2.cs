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

    //private float groundCheckDistance = 0.25f; // �� üũ �Ÿ�

    private float gravity = -9.81f; // �߷� ���ӵ�
    private float verticalVelocity = 0f; // ���� �� ���� �ӵ�

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
            //verticalVelocity = 0f; // ���� ���� �� ���� �ӵ� �ʱ�ȭ
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //ground = false;
                verticalVelocity = Mathf.Sqrt(jumpHeight * -1.5f * gravity); // ���� ���
                anim.SetBool("IsJumping", true);
            }
            else
            {
                //ground = true;
                anim.SetBool("IsJumping", false);
                //verticalVelocity = 2f; // ���� ���� �� ���� �ӵ� �ʱ�ȭ
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

                // �̵� �������� ĳ���� ȸ��
                Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                // �̵�
                characterController.Move(moveDir * speed * Time.deltaTime);
                anim.SetBool("IsRun", Input.GetKey(KeyCode.LeftShift));
                anim.SetFloat("IsWalk", moveDir.magnitude);
            }
            else
            {
                anim.SetBool("IsRun", false);
                anim.SetFloat("IsWalk", 0f);
            }

            // �߷� ����
            verticalVelocity += gravity * Time.deltaTime;
            characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
        }
    }

    private void CheckGround()
    {

        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f; // �߿��� ���� ���� ����
        if (Physics.Raycast(rayStart, Vector3.down, out hit, 0.3f, layer))
        //if (characterController.isGrounded)
        {
            ground = false;
            anim.SetBool("IsJumping", true);
            //ground = true;
            //verticalVelocity = 0f; // ���� ���� �� ���� �ӵ� �ʱ�ȭ
            //anim.SetBool("IsJumping", false);
        }
        else
        {
            ground = true;
            anim.SetBool("IsJumping", false);
        }
    }
}
