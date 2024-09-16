using UnityEngine;
using System;

public enum EGravityType
    {
        Regular,
        RightOriented,
        LeftOriented
    }
public class CustomGravity : MonoBehaviour 
{
      //Wallrunning
    public LayerMask whatIsWall;
    public float wallrunForce,maxWallrunTime, maxWallSpeed;
    public bool isWallRight, isWallLeft;
    public bool isWallRunning;
    private Rigidbody rb;
    private Vector3 LastHitPoint;
    private Vector3 directionAlongWall;
    private Transform orientation;
    public float maxWallRunCameraTilt, wallRunCameraTilt;
    
    void Start()
    {
     rb=GetComponent<Rigidbody>();
     orientation=transform;
    }
    void Update()
    {
    CheckForWall();
    WallRunInput();
    }
    private void WallRunInput() //make sure to call in void Update
    {
        //Wallrun
        if (Input.GetKey(KeyCode.D) && isWallRight) StartWallrun();
        if (Input.GetKey(KeyCode.A) && isWallLeft) StartWallrun();
    }
    public void StartWallrun()
    {
        rb.useGravity = false;
        isWallRunning = true;

        Vector3 directionToWall = (LastHitPoint - transform.position).normalized;
        directionToWall.y = 0;
        directionAlongWall.y=0;
       if (rb.velocity.magnitude <= maxWallSpeed)
        {
            rb.AddForce(directionAlongWall * wallrunForce * Time.deltaTime);
        //Make sure char sticks to wall
        rb.AddForce(directionToWall.normalized * wallrunForce / 2.5f * Time.deltaTime);
        }
    }
    public void StopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
    }
    private void CheckForWall() //make sure to call in void Update
    {
       RaycastHit rightHit= new RaycastHit();
       RaycastHit leftHit= new RaycastHit();
        isWallRight = Physics.Raycast(transform.position, orientation.right,out rightHit, 2f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right,out leftHit, 2f, whatIsWall);
        
        directionAlongWall = Vector3.ProjectOnPlane(transform.forward, isWallRight ? rightHit.normal : isWallLeft ? leftHit.normal:Vector3.zero).normalized;
        if(isWallRight)
        LastHitPoint=rightHit.point;
        if(isWallLeft)
        LastHitPoint=leftHit.point;
        //leave wall run
        if (!isWallLeft && !isWallRight) StopWallRun();
        //reset double jump (if you have one :D)
    }

    }

