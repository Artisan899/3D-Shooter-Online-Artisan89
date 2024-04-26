using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;


[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : NetworkBehaviour
{
    [SerializeField]
    private Camera cam; //установка камеры на игрока


    private Rigidbody rb;


    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 rotationCamera = Vector3.zero;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _vel)
    {
        velocity = _vel;
    }

    public void Rotate(Vector3 _rot)
    {
        rotation = _rot;
    }


    public void RotateCam(Vector3 _rotCam)
    {
        rotationCamera = _rotCam;
    }


    void FixedUpdate()
    {
        PerformMove();  // движение персонажа
        PerformRotation(); //вращение персонажа
    }


    void PerformMove()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }


    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam != null)
        {
            cam.transform.Rotate(-rotationCamera);
        }

    }

}