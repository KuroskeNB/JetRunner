using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float horizontalAngle =0;
    public float verticalAngle=0;
    public float shipSpeed=50;
    public float rollSpeed = 50f;      // Скорость наклона
    public float turnSpeed = 10f;      // Скорость поворота
    public float maxRollAngle = 15f;   // Максимальный угол наклона
    public float pitchSpeed = 30f;     // Скорость наклона по вертикали
    public float maxPitchAngle = 60f;  // Максимальный угол наклона по вертикали
    public float rollPower = 1;
    public float pitchPower=1;

    public enum EEnemyStatuses
    {
        Seeking,
        Avoiding,
        Fighting
    }
    public EEnemyStatuses status=EEnemyStatuses.Seeking;
    public float visionRange = 1000f;     // Радиус видимости
    public float visionAngle = 45f;     // Угол поля зрения
    public Transform player;            // Ссылка на игрока
    public LayerMask playerMask;      // Маска для игрока
    public LayerMask obstacleMask;      // Маска для помех
    public int obstacleDistance=10;
    public Transform CurrentObstacle=null;
    private void Update()
    {
        // Проверяем, видит ли враг игрока
        if (IsPlayerInSight())
        {
            status=EEnemyStatuses.Fighting;
            //Chase();
            // Реакция врага на обнаружение игрока (например, начать преследование)
        }
        else
        {
            status=EEnemyStatuses.Seeking;
        }

        if(ThereIsObstacle())
        {
         //status=EEnemyStatuses.Avoiding;
         Avoid();
        }
        else
        {

        }

        Fly();
    }

    void Fly()
    {
      switch(status)
      {
        case EEnemyStatuses.Fighting:
        Chase();
        break;
        case EEnemyStatuses.Seeking:
        Seek();
        break;
        case EEnemyStatuses.Avoiding:
        Avoid();
        break;
      }
         // Вычисление направления к цели
        Vector3 directionToTarget = (player.position - transform.position).normalized;

        // Плавный поворот в направлении цели
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4 * Time.deltaTime);

        // Применение того же управления, что и в SpaceshipController
        float rollAmount = rollSpeed * Time.deltaTime;
        //transform.Rotate(Vector3.up, rollAmount * turnSpeed * Time.deltaTime);

        // Постоянное движение вперед
        transform.Translate(Vector3.forward * shipSpeed * Time.deltaTime);
    }
    void ShipDirect(float rollInput,float pitchInput)
    {
       horizontalAngle += rollInput * rollSpeed * Time.deltaTime;
        horizontalAngle = Mathf.Clamp(horizontalAngle, -maxRollAngle, maxRollAngle);
    
    verticalAngle += pitchInput * pitchSpeed * Time.deltaTime;
        verticalAngle = Mathf.Clamp(verticalAngle, -maxPitchAngle, maxPitchAngle);

   float turnAmount = (horizontalAngle / maxRollAngle) * 4;
        transform.Rotate(Vector3.up, turnAmount * turnSpeed * Time.deltaTime);

        // Устанавливаем крен и наклон
        Vector3 targetRotation = transform.rotation.eulerAngles;
        targetRotation.z = -horizontalAngle;  // Крен по оси Z
        targetRotation.x = verticalAngle;     // Наклон по оси X

        //transform.rotation = Quaternion.Euler(targetRotation);
    }

    void Chase()
    {
        float sideDifference = 0;
    float heightDifference = 0;

    string playerSide = CheckSideOf(player, ref sideDifference);
    string playerHeight = CheckHeightDifference(player, ref heightDifference);

    float zPower = 0;
    float xPower = 0;
    Debug.Log(playerSide);
    switch (playerSide)
    {
        case "right":
            zPower =rollPower;
            break;
        case "left":
            zPower = -rollPower;
            break;
        case "infront":
            zPower = 0;
            break;
    }
    Debug.Log(playerHeight);
    switch (playerHeight)
    {
        case "above":
            xPower = -pitchPower;
            break;
        case "below":
            xPower = pitchPower;
            break;
        case "same":
            xPower = 0;
            break;
    }

    ShipDirect(zPower, xPower);
    }

    void Avoid()
    {
        float sideDifference=0 ;
        float heightDifference=0;
     string obstacleSide = CheckSideOf(CurrentObstacle,ref sideDifference);
     string obstacleHeight = CheckHeightDifference(CurrentObstacle, ref heightDifference);
     Debug.Log("obstacle by "+obstacleSide);
     switch(obstacleSide)
     {
        case "right":
        ShipDirect(rollPower*2,0);
        break;
        case "left":
        ShipDirect(-rollPower*2,0);
        break;
        case "infront":
        ShipDirect(rollPower*2,0);
        break;
     }
     switch(obstacleHeight)
     {
        case "above":
        ShipDirect(0,-pitchPower);
        break;
        case "below":
        ShipDirect(0,pitchPower);
        break;
        case "same":
        ShipDirect(0,0);
        break;
     }
    }

    void Seek()
    {
    }

    private bool IsPlayerInSight()
    {
        // Проверяем расстояние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= visionRange)
        {
            // Проверяем угол между направлением взгляда врага и направлением на игрока
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer <= visionAngle || Vector3.Distance(player.position,transform.position)<100)
            {
                // Проверяем, есть ли преграды между врагом и игроком
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRange, playerMask))
                {
                    if (hit.transform == player)
                    {
                        return true;  // Игрок видим
                    }
                }
            }
        }
        return false;  // Игрок не видим
    }

    private bool ThereIsObstacle()
    {
        Vector3 forwardDirection = transform.forward;

        // Используем Raycast для обнаружения препятствий спереди
        RaycastHit hit;
        if (Physics.Raycast(transform.position, forwardDirection, out hit, obstacleDistance, obstacleMask))
        {
            Debug.Log("Obstacle detected: " + hit.collider.name);
            CurrentObstacle=hit.transform;
        return true;
        }
        return false;
    }

    private string CheckSideOf(Transform targetTransform, ref float difference)
    {
        // Вектор от врага к игроку
        Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;

    // Определяем сторону
    float side = Vector3.Dot(transform.right, directionToTarget);
    difference=Mathf.Abs(side);
    // Введение порога для проверки
    float threshold = 0.04f;  // Чем меньше значение, тем точнее определение, но выше риск ошибки

    if (side > threshold)
    {
        return "right";
    }
    else if (side < -threshold)
    {
        return "left";
    }
    else
    {
        return "infront";
    }
    }

     private string CheckHeightDifference(Transform targetTransform, ref float difference)
    {
        // Вектор от врага к игроку
        Vector3 directionToPlayer = (targetTransform.position - transform.position).normalized;

        // Вектор вверх относительно врага
        Vector3 up = transform.up;

        // Определяем, находится ли игрок выше или ниже
        float heightDifference = Vector3.Dot(up, directionToPlayer);
        difference=Mathf.Abs(heightDifference);
        if (heightDifference > 0.01)
        {
            return "above";
        }
        else if (heightDifference < -0.01f)
        {
            return "below";
        }
        else
        {
            return "same";
        }
    }
}
