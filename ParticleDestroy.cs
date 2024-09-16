using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        if (_particleSystem == null)
        {
            Debug.LogError("ParticleSystem component not found on this GameObject.");
        }
        else
        {
            // Уничтожаем объект после завершения работы ParticleSystem
            Destroy(gameObject, _particleSystem.main.duration + _particleSystem.main.startLifetime.constantMax);
        }
    }

    void Update()
    {
        // Проверяем, если частиц больше нет и система не зациклена
        if (!_particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
