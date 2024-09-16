using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform[] gateTransforms;
    public GameObject zombiePrefab;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private CentralObject station;
    public int timeBetweenWaves=5;
    private int level =0;
    public int zombiePerGate=6;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaveCycle());
        station=GetComponent<CentralObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(station&&station.currentHealth<=0)
        {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(player&&player.currentRadiation>=player.MaxRadiation)
        {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void SpawnWave()
    {
        level++;
        int gateCount=2+(5%level);
        Debug.Log(gateCount);
        int spawnedGate=-1;
        while(gateCount>0)
        {
            int gateToSpawn=Random.Range(0,gateTransforms.Length);
            if(gateToSpawn==spawnedGate) continue;
            for(int i=0;i<zombiePerGate;i++)
            {
                Instantiate(zombiePrefab,GetRandomPointOnNavMesh(gateTransforms[gateToSpawn].position,3),gateTransforms[gateToSpawn].rotation);
            }
            spawnedGate=gateToSpawn;
            gateCount--;
        }
    }

    Vector3 GetRandomPointOnNavMesh(Vector3 center, float radius)
    {
        // Случайное смещение в пределах радиуса
        Vector3 randomDirection =Random.insideUnitSphere * radius;
        randomDirection += center;
        
        // Структура для хранения данных о точке на NavMesh
        NavMeshHit hit;
        
        // Ищем ближайшую точку на NavMesh в пределах максимальной дистанции
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position; // Возвращаем позицию на NavMesh
        }

        // Если точка не найдена, возвращаем исходную позицию
        return center;
    }
    IEnumerator WaveCycle()
    {
        Debug.Log("spawn cycle");
        SpawnWave();
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(WaveCycle());
    }
}
