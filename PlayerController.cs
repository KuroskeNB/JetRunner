using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     public float moveSpeed = 5f;
    public Transform cameraTransform; 
    public float maxTorsoRotation = 20f;
    public float minTorsoRotation = -20f;
    [SerializeField]
    private GameObject steamParticle;
    public bool isAiming = false;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public Rifle rifle;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    [SerializeField]
    private GameObject playerRig;
    private Rigidbody rb;
    public bool isGrounded;
    private CustomGravity gravity;
    public float MaxJetFuel=100;
    public float currentJetFuel;
    public float JetFuelCost=1;
    public float MaxRadiation=100;
    public float currentRadiation=0;
    bool bGrowRadiation=false;
    bool bCanRecharge=false;

    void Start()
    {
        currentJetFuel=MaxJetFuel;
        Cursor.lockState = CursorLockMode.Locked;
        rifle=GetComponentInChildren<Rifle>();
        rb = GetComponent<Rigidbody>();
        gravity=GetComponent<CustomGravity>();
        if(playerRig)
        playerRig.SetActive(false);
    }

    void Update()
    {
        Move();
        Jump();
        JetFly();
        // Проверка, приземлен ли персонаж
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        GetComponent<Animator>().SetBool("IsGrounded", isGrounded);
        
        if(bGrowRadiation && currentRadiation<MaxRadiation)
        currentRadiation+=0.1f;
        if(!bGrowRadiation && currentRadiation>0)
        currentRadiation-=0.1f;
        // Управление анимацией
        UpdateAnimations();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        if(gravity.isWallRunning)
        {
        moveX=0;
        moveZ=0;
        }
        // Направление движения относительно камеры
        Vector3 moveDirection = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        moveDirection.y = 0; // Игнорируем вертикальную составляющую

        // Двигаем персонажа
        transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
    }


    void Jump()
    {
        // Проверка нажатия кнопки прыжка и нахождения на земле
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !gravity.isWallRunning)
        {
            Debug.Log("jimp");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if(Input.GetKeyDown(KeyCode.Space) && gravity.isWallRunning)
        {
            gravity.StopWallRun();
             rb.AddForce(transform.forward * 20, ForceMode.Impulse);
            if (gravity.isWallRight) rb.AddForce(-transform.right * jumpForce * 10f);
            if (gravity.isWallLeft) rb.AddForce(transform.right * jumpForce * 10f);
            Debug.Log("juuump forward");
        }
        // Debug.Log("isGrounded" + isGrounded);
        // Debug.Log("gravity.isWallRunning" + gravity.isWallRunning);
    }
    void JetFly()
    {

        if(Input.GetKeyDown(KeyCode.LeftShift)&& isGrounded && currentJetFuel>=JetFuelCost*3)
        {
             bCanRecharge=false;
          rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
          Instantiate(steamParticle,transform.position,transform.rotation,transform);
          currentJetFuel-=JetFuelCost*3;
          //Time.timeScale=0.5f;
        }
        else if(Input.GetKey(KeyCode.LeftShift)&& !isGrounded&&currentJetFuel>=JetFuelCost)
        {
             bCanRecharge=false;
            rb.AddForce(transform.up * jumpForce*Time.deltaTime*5, ForceMode.Impulse);
            currentJetFuel-=JetFuelCost;
        }
        else if(!bCanRecharge)
        {
            if(currentJetFuel<MaxJetFuel)
         StartCoroutine(jetRecharge());
        }
        else if(bCanRecharge)
        {
            if(currentJetFuel<MaxJetFuel)
         currentJetFuel+=JetFuelCost/2;
         else
         bCanRecharge=false;
        }
        // if(Input.GetKey(KeyCode.LeftShift)&& !isGrounded)
        // {
        //   rb.AddForce(transform.forward  /10, ForceMode.Impulse);
        //   //Time.timeScale=0.5f;
        // }
    }

    void UpdateAnimations()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX != 0 || moveZ != 0 && Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<Animator>().SetBool("IsRunning", true);
        }
        else if (moveX != 0 || moveZ != 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<Animator>().SetBool("IsRunning", false);
        }

        if (moveX != 0 || moveZ != 0 && !GetComponent<Animator>().GetBool("IsRunning"))
        {
            GetComponent<Animator>().SetBool("IsWalking", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("IsWalking", false);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Radiation"))
        {
         bGrowRadiation=true;
         Debug.Log("rad enter");
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Radiation"))
       bGrowRadiation=false;
    }
    IEnumerator jetRecharge()
    {
        yield return new WaitForSeconds(2f);
        bCanRecharge=true;
    }
   
}