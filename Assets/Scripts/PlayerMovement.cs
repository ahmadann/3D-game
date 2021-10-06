using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    public float moveSpeed = 6f;
    public float turnSpeed = 4f;
    
    Animator anim;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var movement = new Vector3(horizontal, 0, vertical);

        controller.SimpleMove(movement * moveSpeed);

        anim.SetFloat("Speed", movement.magnitude);

        if (movement.magnitude > 0)
        {
            Quaternion newDirection = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * turnSpeed);
        }
        
    }
}
