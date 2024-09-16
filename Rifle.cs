using UnityEngine;

public class Rifle : MonoBehaviour
{
   public GameObject bulletPrefab;  // Префаб пули
   public GameObject boomBulletPrefab;  // Префаб бомбо пули
    [SerializeField]
   private GameObject steamParticle;
    public Transform firePoint;
    public Transform cameraTransform;
    public Transform debugObject;      // Точка, из которой будет выходить пуля
    public float fireRate = 0.1f;    // Скорострельность (время между выстрелами)
    public float bulletSpeed = 20f;  // Скорость полета пули
    public int MaxBullets=20;
    public int currentBullets;
    public LayerMask shootingLayer; 
    private Animator animator;
    private float nextFireTime = 0f; // Время следующего выстрела
    
    void Start()
    {
     animator=GetComponentInParent<Animator>();
     currentBullets=MaxBullets;
    }
    void Update()
    {
        Vector2 ScreenCenterPoint = new Vector2(Screen.width/2f,Screen.height/2f);
        Vector3 mouseWorldPosition= Vector3.zero;
        Vector3 shootDirection=firePoint.forward;
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenterPoint);
        if(Physics.Raycast(ray,out RaycastHit hitok,999f,shootingLayer))
        {
           mouseWorldPosition=hitok.point;
           shootDirection=(mouseWorldPosition-firePoint.position).normalized;
           debugObject.position=mouseWorldPosition;
        }
        // Если кнопка "Огонь" удерживается и прошло достаточно времени
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            if(currentBullets>=1)
            Shoot(shootDirection);
            animator.SetBool("IsShooting",true);
        }
        else
        {
           animator.SetBool("IsShooting",false);
        }
    }

    void Shoot(Vector3 direction)
    {
        // Создаем пулю в точке огня
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction,Vector3.up));
        // Добавляем пуле движение вперед
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = rb.transform.forward * bulletSpeed;
            if(steamParticle)
            Instantiate(steamParticle,bullet.transform.position,bullet.transform.rotation,bullet.transform);
            currentBullets--;
        }

        // Здесь можно добавить звук выстрела, вспышку и т.д.
    }
    public void Recharge()
    {
        Debug.Log("recharge");
     currentBullets=MaxBullets;
    }
}
