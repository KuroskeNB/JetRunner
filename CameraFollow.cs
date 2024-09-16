using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // Трансформ самолёта


    void Start()
    {
    }

    void Update()
    {
        // Камера фиксируется по вертикали и горизонтали, не меняя своего наклона по Z
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        // Убедитесь, что камера всегда смотрит на самолёт
        //transform.LookAt(target.position);
    }
    void LateUpdate()
    {
        // Камера фиксируется по вертикали и горизонтали, не меняя своего наклона по Z
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        // Убедитесь, что камера всегда смотрит на самолёт
        //transform.LookAt(target.position);
    }
}
