using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IAlive
{
    [SerializeField]
    private NavMeshAgent agent;
    public Transform target;
    public int MaxHealth=100;
    private int currentHealth;
    public LayerMask StationMask;
    public float delay = 2.0f;  // Задержка в секундах
private float nextActionTime = 0f;
    bool bExploed =false;
    // Start is called before the first frame update
    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
        currentHealth=MaxHealth;
        if(!target)
        target=GameObject.FindWithTag("Finish").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent&&agent.enabled)
        {
            if(Vector3.Distance(transform.position,target.position)>3.5)
            {
            agent.SetDestination(target.position);
            }
            else if(Time.time > nextActionTime)
            {
                nextActionTime = Time.time + delay;
                agent.isStopped = true;
             Attack();
            }
           // Debug.Log(agent.name + " destination is " + agent.destination);
        }
    }
    void Attack()
    {
     if(GetComponent<Animator>())
     {
        GetComponent<Animator>().SetBool("IsAttacking",true);
        if(Physics.Raycast(transform.position,transform.forward,out RaycastHit hit,15,StationMask))
        {
            if(hit.collider.gameObject&&hit.collider.gameObject.GetComponent<CentralObject>())
         hit.collider.gameObject.GetComponent<CentralObject>().ApplyDamage(2.5f);
        }
     }
    }
    public void ApplyDamage(int damage)
    {
     currentHealth-=damage;
     if(currentHealth<=0)
     {
        agent.enabled = false;
        bExploed =true;
    GetComponent<Rigidbody>().AddExplosionForce(10, transform.position - Vector3.up,5,1,ForceMode.Impulse);
     }
    }
    private void OnCollisionEnter(Collision other) {
        if(bExploed)
        Destroy(gameObject);
    }
}
