using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
     public float damage = 10f;  // Урон от пули
     public LayerMask whatIsEnemy;
    public GameObject impactEffect;  // Эффект при попадании

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if(!other.gameObject.CompareTag("Enemy")) Destroy(gameObject);
        // Проверка, есть ли у объекта компонент здоровья
        //Health targetHealth = other.GetComponent<Health>();
        // if (targetHealth != null)
        // {
        //     // Нанесение урона
        //     targetHealth.TakeDamage(damage);
        // }

        // Создание эффекта при попадании
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            RaycastHit[] hits=Physics.SphereCastAll(transform.position,5,transform.forward,whatIsEnemy);
            foreach(RaycastHit hit in hits)
            {
                IAlive attackable = hit.collider.GetComponentInParent<IAlive>();
                if(attackable!=null)
                attackable.ApplyDamage(50);
            }
        }

        // Уничтожение пули после попадания
        Destroy(gameObject);
    }
}
