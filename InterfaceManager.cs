using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public Rifle PlayerRifle;
    public PlayerController player;
    [SerializeField]
    private Button startGame;
    [SerializeField]
    private Button exitGame;
    public Text ammoText;
     public Text timerText;
    
    private float elapsedTime = 0f;
    public Image JetFuelImage;
    public Image RadiationImage;
    public Text ReminderText;
    // Start is called before the first frame update
    void Start()
    {
        ReminderText.gameObject.SetActive(false);
        startGame.onClick.AddListener(OnStartClicked);
        exitGame.onClick.AddListener(OnExitClicked);
        Time.timeScale=0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            // Включаем видимость курсора
    Cursor.visible = true;

    // Разблокируем курсор (чтобы он мог свободно двигаться)
    Cursor.lockState = CursorLockMode.None;
        }
        else if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
             Cursor.visible = false;
             // Разблокируем курсор (чтобы он мог свободно двигаться)
    Cursor.lockState = CursorLockMode.Locked;
        }
        if(PlayerRifle)
        {
            ammoText.text=PlayerRifle.currentBullets + "/" + PlayerRifle.MaxBullets;
            if(PlayerRifle.currentBullets<=0)
            ReminderText.gameObject.SetActive(true);
            else
            ReminderText.gameObject.SetActive(false);
        }
        if(player)
        {
            JetFuelImage.fillAmount = player.currentJetFuel/player.MaxJetFuel; 
            RadiationImage.fillAmount = player.currentRadiation/player.MaxRadiation; 
           // Debug.Log("Radiation is: " + player.currentRadiation);
        }

         elapsedTime += Time.deltaTime;

        // Преобразуем прошедшее время в минуты и секунды
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        
        // Форматируем строку как 00:00
        if(timerText)
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void OnStartClicked()
    {
      Time.timeScale=1f;
    }
    void OnExitClicked()
    {
     Application.Quit();
    }
}
