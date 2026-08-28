using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public GameObject[] vehicles;
    public SmoothFollow smoothCam;
    int currentVehicle;
    int counter;
    public int coins;
    public Text coinsText;
    public Text alert;
    public GameObject alertPanel;
    public Button selectButton;
    public Transform spawnPoint;
    public GameObject vehicle;
    List<GameObject> loadedVehicles = new List<GameObject>();
    public Text carPrice;
    public GameObject buyButton;
    public Button[] levelButtons;
    int unlockedLevels;
    RCC_CarControllerV3 rcc;
    //public RectTransform levelsPanel;
    //bool movePanelNow;
    //public float levelPanelDistance;
    //public float minX, maxX;
    public Toggle steeringSelect, arrowSelect;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        unlockedLevels = PlayerPrefs.GetInt("SavedLevels");
        if (unlockedLevels == 0)
            unlockedLevels = 1;
        coins = PlayerPrefs.GetInt("TotalCoins");
       // coins = 1000;
        coinsText.text = coins.ToString();
        setGarage();
        
        //vehicles[currentVehicle].SetActive(true);
    }
   public void setGarage()
    {
        currentVehicle = PlayerPrefs.GetInt("CurrentVehicle");
        counter = currentVehicle;
        vehicle = Instantiate(vehicles[currentVehicle], spawnPoint.position, spawnPoint.rotation);
        loadedVehicles.Add(vehicle);
        for (int i = 0; i <= unlockedLevels; i++)
            levelButtons[i].interactable = true;
        smoothCam.target = vehicle.GetComponent<carInfo>().carView;
        engineSound(false);

    }
    public void engineSound(bool EngineSwitch)
    {
        rcc = vehicle.GetComponent<RCC_CarControllerV3>();
        rcc.engineRunning = EngineSwitch;
    }
    public void selectLevel(int level)
    {
        PlayerPrefs.SetInt("CurrentLevel", level);
        
    }
    public void selectLevelButton(Button button)
    {
        button.Select();
    }
    public void startGame()
    {
        SceneManager.LoadScene("gameScene");

    }
    public void Quit()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            new AndroidJavaClass("java.lang.System").CallStatic("exit", 0);
        }
        else
        {
            Application.Quit();
        }
    }
  
  
    public void selectVehicle()
    {
        if (vehicle.GetComponent<carInfo>().unLocked == 1)
            PlayerPrefs.SetInt("CurrentVehicle", counter);
        Debug.Log(counter);
    }
    public void buyVehicle()
    {
        carInfo info = vehicle.GetComponent<carInfo>();
        if (coins >= info.carPrice)
        {
            vehicle.GetComponent<carInfo>().unLocked = 1;
            PlayerPrefs.SetInt("Unlocked" + vehicle.GetComponent<carInfo>().carNumber.ToString(), 1);
            coins = coins - info.carPrice;
            PlayerPrefs.SetInt("TotalCoins", coins);
            coinsText.text = coins.ToString();
            
        }
        else 
        {
            alert.text = "You Dont Have Enough Coins ";
            alertPanel.SetActive(true);

        }
    }
    
    void instantiateVehicle()
    {
        if (loadedVehicles != null)
        {
            foreach (GameObject vehicle in loadedVehicles)
            {
                Destroy(vehicle);
            }

        }

        vehicle = Instantiate(vehicles[counter], spawnPoint.position, spawnPoint.rotation);
        loadedVehicles.Add(vehicle);
        smoothCam.target = vehicle.GetComponent<carInfo>().carView;
        
    }
    public void nextVehicle()

    {
        if (counter < vehicles.Length - 1)
            counter++;
        else
            counter = 0;

        instantiateVehicle();

    }
    public void previousVehicle()
    {
        if (counter > 0)
            counter--;
        else
            counter = vehicles.Length - 1;
        instantiateVehicle();
    }
   
    //public void openLevelsPanel()
    //{
    //    levelsPanel.DOAnchorPosX(maxX, 1.5f).SetEase(Ease.OutQuad); 
    //}
    public void layoutSelection()
    {
        if (PlayerPrefs.GetInt("savedLayout") == 0)
            steeringSelect.isOn = true;
        else if(PlayerPrefs.GetInt("savedLayout") == 1)
            arrowSelect.isOn = true;

        if (arrowSelect.isOn)
            steeringSelect.isOn = false;
        if (steeringSelect.isOn)
            arrowSelect.isOn = false;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (vehicle.GetComponent<carInfo>().unLocked == 0)
        {
            buyButton.SetActive(true);

            carPrice.text = vehicle.GetComponent<carInfo>().carPrice.ToString();
        }
        else
            buyButton.SetActive(false);

      layoutSelection();

    }
}