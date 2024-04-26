using UnityEngine;
using UnityEngine.Networking;
using Mirror;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMotor))]



public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float speed = 4f;  //�������� ������
    [SerializeField]
    private float lookSpeed = 2f; // �������� �������� ������ ������

    private bool canSprint = true;

    private Animator _anim;

    private Rigidbody _rigidbody;
    private bool canjump = true;
    private float jumpCooldown = 1f;
    private float currentJumpCooldown = 0f;

    private PlayerMotor motor;
 

    void Start()  // ��������� ������ �����������
    {
        _rigidbody = GetComponent<Rigidbody>();
        motor = GetComponent<PlayerMotor>();
        _anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (!isLocalPlayer)
            return;

        //������������ �� WASD

        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 movHor = transform.right * xMov;
        Vector3 movVer = transform.forward * zMov;

        Vector3 velocity = (movHor + movVer).normalized * speed;


        motor.Move(velocity);

        // ������� ������

        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSpeed;

        motor.Rotate(rotation);

        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 camRotation = new Vector3(xRot, 0f, 0f) * lookSpeed;

        motor.RotateCam(camRotation);

        if (canSprint && Input.GetKeyDown(KeyCode.LeftShift)) //����� ���� ��� ���� ���������
        {
            canSprint = false;
            speed = 7f;

        }

        else if (!canSprint && Input.GetKeyDown(KeyCode.LeftShift)) //���������� ���� ����� ������
        {
            canSprint = true;
            speed = 4f;

        }



        if (canjump && Input.GetKeyDown(KeyCode.Space))  //������ ���������
        {
            RigidbodyJump();
            canjump = false;
            currentJumpCooldown = jumpCooldown;
        }
        if (!canjump)
        {
            currentJumpCooldown -= Time.deltaTime; //����������� �� ������
            if (currentJumpCooldown <= 0f)
            {
                canjump = true;
            }
        }

        _anim.SetBool("IsWalk", velocity.magnitude > 0.1f); // �������� ����/������
    }

    private void RigidbodyJump()  // ������� ��� ������ ���������
    {
        Rigidbody _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(Vector3.up * speed, ForceMode.VelocityChange);

    }
}
