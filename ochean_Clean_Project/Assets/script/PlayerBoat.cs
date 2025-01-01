using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoat : MonoBehaviour
{
    public float maxSpeed = 10f;       
    public float turnSpeed = 50f;      
    public float acceleration = 2f;   
    public float deceleration = 2f;   

    public int maxTrashCapacity = 15; 
    private int currentTrash = 0;     

    private Rigidbody rb;
    private float currentSpeed = 0f;  

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        if (moveInput != 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, moveInput * maxSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
        }

        Vector3 moveDirection = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        if (turnInput != 0)
        {
            Quaternion turnRotation = Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    public bool CanCollectTrash()
    {
        return currentTrash < maxTrashCapacity;
    }

    public void CollectTrash(int amount)
    {
        currentTrash = Mathf.Clamp(currentTrash + amount, 0, maxTrashCapacity);
        ScoreManager.instance.UpdateTrashUI(currentTrash, maxTrashCapacity);
    }

    public void TransferTrashToStorage()
    {
        if (currentTrash > 0)
        {
            ScoreManager.instance.AddToStorage(currentTrash);
            currentTrash = 0;
            ScoreManager.instance.UpdateTrashUI(currentTrash, maxTrashCapacity);
        }
    }
}
