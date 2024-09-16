using UnityEngine;
using UnityEngine.UI;

public class CentralObject : MonoBehaviour
{
    public int  MaxHealth = 100;
    public Image healthBar;
    public GameObject canvas;
    public float currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth=MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBar)
        healthBar.fillAmount = currentHealth/MaxHealth;

        if(canvas&&canvas.transform.rotation!=Camera.main.transform.rotation)
        {
            canvas.transform.rotation=Camera.main.transform.rotation;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PlayerController>())
        {
            if(!other.GetComponent<PlayerController>().rifle)
            Debug.Log("you dont have rifle");
         other.GetComponent<PlayerController>().rifle.Recharge();
        }
    }
    public void ApplyDamage(float damage)
    {
        currentHealth-=damage;
        Debug.Log("station damaged");
    }
}
