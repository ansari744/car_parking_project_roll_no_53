using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameplayManager : MonoBehaviour
{
    public  leveData[] ld;
    public GameObject[] vehicles;
    public GameObject currentVehicle;
    int currentVehicleNumber;
    int currentLevel;
    public SmoothFollow camScript;
    public PlayerScript playerScript;
    int currentCamera;
    public GameObject completePanel, failedPanel;
    public GameObject[] hideOnComplete;
    RCC_UIDashboardButton controls;
    public GameObject[] stars;
    public Text hitsText;
    int coinsWon;
    public Text coins;
    public GameObject loadingPanel;
    public Slider loadingBar;
    public RawImage rearView;
    public bool hoodCam;
    public Text levelText;
    public GameObject[] healthunits;
    RCC_CarControllerV3 Rcc;

    //public Layout[] inGameLayouts;
    //[System.Serializable]
    //public class Layout
    //{
    //    public string name;
    //}
    // Start is called before the first frame update
    void Start()
    {
        
        activateLoadingPanel();

        controls = FindAnyObjectByType<RCC_UIDashboardButton>();
        Time.timeScale = 1f;
        setEnvironment();
        LoadButtonLayout();

        camPosition();
        Rcc = FindAnyObjectByType<RCC_CarControllerV3>();
        Rcc.engineRunning = false;
        Rcc.canControl = false;
        Invoke("changeGear", 1f);

        Invoke("deactivateLoadingPanel", 2f);

        Invoke("EngineSwitch", 2f);

        Rcc.canControl = true; 

    }
    public void EngineSwitch()
    {
        Rcc.KillOrStartEngine();
    }
    public void loadGame()
    {
       
        SceneManager.LoadScene("gameScene");
    }
    public void loadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }
    
   
    void changeGear()
    {
        controls.ChangeGear(1);
        controls.ChangeGear(0);
    }
    // Update is called once per frame
    void Update()
    {
        //if (!engine)
        //    playerScript.rb.isKinematic = true;
        //else
        //    playerScript.rb.isKinematic = false;
    }
    public List<Button> inGameButtons = new List<Button>();

    int currentLayout;
    private void LoadButtonLayout()
    {
        currentLayout = PlayerPrefs.GetInt("savedLayout");
        if (currentLayout == 1)
            RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.TouchScreen;

        else if(currentLayout == 0)
            RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.SteeringWheel;


        for (int i = 0; i < inGameButtons.Count; i++)
        {
            // Load the position of the button
            float x = PlayerPrefs.GetFloat("button" + i + "x");
            float y = PlayerPrefs.GetFloat("button" + i + "y");

            // Load the scale of the button
            float scaleX = PlayerPrefs.GetFloat("button" + i + "scaleX");
            float scaleY = PlayerPrefs.GetFloat("button" + i + "scaleY");


            // Set the position and scale of the button
            if (x != 0 && y != 0)
                inGameButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            if (scaleX == 0)
                scaleX = 1;
            if (scaleY == 0)
                scaleY = 1;
           inGameButtons[i].GetComponent<RectTransform>().localScale = new Vector3(scaleX, scaleY, 1f);
        }
    }
    void activateLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }
    void deactivateLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }
    public void setEnvironment()
    {
        currentVehicleNumber = PlayerPrefs.GetInt("CurrentVehicle");
        if (PlayerPrefs.GetInt("CurrentLevel") == 0)
            PlayerPrefs.SetInt("CurrentLevel", 1);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
       SceneManager.LoadScene(ld[currentLevel].levelName, LoadSceneMode.Additive);
        currentVehicle = Instantiate(vehicles[currentVehicleNumber], ld[currentLevel].spawnpoint.position, ld[currentLevel].spawnpoint.rotation);
        playerScript = currentVehicle.GetComponent<PlayerScript>();
        playerScript.gm = this;
        camScript.target = playerScript.target ;
        hitsText.text = remainingHits.ToString();
        levelText.text = ("Level: " + currentLevel.ToString());

    }

    public void pause()
    {
        
        Time.timeScale = 0f;

    }
    public void resume()
    {
        Time.timeScale = 1f;
    }
    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void restart()
    {
        SceneManager.LoadScene("gameScene");
    }
    public void changeCamera(bool pressed)
    {
        currentCamera = PlayerPrefs.GetInt("savedCam");

        if (pressed)
        {
            if (currentCamera == 0)
            {
                PlayerPrefs.SetInt("savedCam", 1);
                currentCamera = PlayerPrefs.GetInt("savedCam") ;
            }
            else if (currentCamera == 1)
            {
                PlayerPrefs.SetInt("savedCam", 2);
                currentCamera = PlayerPrefs.GetInt("savedCam");
            }
            else if (currentCamera == 2)
            {
                PlayerPrefs.SetInt("savedCam", 0);
                currentCamera = PlayerPrefs.GetInt("savedCam");
            }

            //Debug.Log(currentCamera);
        }
        camPosition();
    }
    public void camPosition()
    {
        hoodCam = false;

        currentCamera = PlayerPrefs.GetInt("savedCam");
        if (currentCamera == 0) // 0 means simple near camera
        {

            camScript.target = playerScript.target;
            camScript.distance = 8f;
            camScript.height = 2f;
            camScript.rotationDamping = 5f;
            camScript.heightDamping = 2f;
        }
        else if (currentCamera == 1) // 1 means simple far camera 
        {

            camScript.target = playerScript.target;
            camScript.distance = 12f;
            camScript.height = 4f;
            camScript.rotationDamping = 8f;
            camScript.heightDamping = 2f;
        }
        else if (currentCamera == 2) // 2 means hood camera
        {
            camScript.target = playerScript.hoodCamTarget;
            camScript.distance = 0.03f;
            camScript.height = 0f;
            camScript.rotationDamping = 50f;
            camScript.heightDamping = 50f;
            hoodCam = true;
        }
    }
    int counter = 0;
    public void addCoins()
    {
        counter++;
        if (counter == 1)
        {
            totalCoins = PlayerPrefs.GetInt("TotalCoins");
            coinsWon = 500 * remainingHits;
            coins.text = coinsWon.ToString();

            totalCoins = totalCoins + coinsWon;

            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            playerScript.parked = false;
        }
        
    }
   
    public void check()
    {


        //  if(!engine)
        if (playerScript.rb.linearVelocity.magnitude <= 0.05f)
        {
         //   missionCompleted();
            
           Invoke("missionCompleted", 1f) ;
            
        }
    }
    int totalCoins = 0;
    
    public void missionCompleted()
    {
        addCoins();
        playerScript.rb.isKinematic = true;
        for (int i = 0; i < hideOnComplete.Length; i++)
        {
            hideOnComplete[i].SetActive(false);
        }
        completePanel.SetActive(true);
        
        for (int i = 0; i < remainingHits; i++)
        {
            stars[i].SetActive(true);
        }
        // EngineSwitch();
        Rcc.KillEngine();
    }
    public void nextLevel()
    {
        if (currentLevel < ld.Length)
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            if (PlayerPrefs.GetInt("SavedLevels") < currentLevel)
                PlayerPrefs.SetInt("SavedLevels", PlayerPrefs.GetInt("SavedLevels") + 1);
           
            SceneManager.LoadScene("gameScene");
        }
       
    }
    public void camDirection(int dir)
    {

        if (PlayerPrefs.GetInt("savedCam") != 2)
        {
            if (dir == 1)
            {
                Vector3 targetAngle = new Vector3(camScript.target.localEulerAngles.x, 0, camScript.target.localEulerAngles.z);
                camScript.target.localEulerAngles = targetAngle;
            }
            else if (dir == -1)
            {
                Vector3 targetAngle = new Vector3(camScript.target.localEulerAngles.x, 180, camScript.target.localEulerAngles.z);
                camScript.target.localEulerAngles = targetAngle;
            }

            //if (dir == 1)
            //{
            //    camScript.target.localPosition = new Vector3(camScript.target.localPosition.x, camScript.target.localPosition.y, 0.297f);
            //    Vector3 targetAngle = new Vector3(camScript.target.localEulerAngles.x, 0, camScript.target.localEulerAngles.z);
            //    camScript.target.localEulerAngles = targetAngle;
            //}
            //if (dir == -1)
            //{
            //    camScript.target.localPosition = new Vector3(camScript.target.localPosition.x, camScript.target.localPosition.y, -1.297f);
            //    Vector3 targetAngle = new Vector3(camScript.target.localEulerAngles.x, 180, camScript.target.localEulerAngles.z);
            //    camScript.target.localEulerAngles = targetAngle;
            //}
        }
       
        
    }
    public int remainingHits ;
    public void checkHealth()
    {
        remainingHits--;
        healthunits[remainingHits].SetActive(false);

        if (remainingHits > 0)
        {
            //  healthBar.value = hits;
        }
        hitsText.text = remainingHits.ToString();
        StartCoroutine(DecreaseHealthSmoothly());

        if (remainingHits <= 0)
        {
            playerScript.rb.isKinematic = true;
            Invoke("failed", 1.5f);
        }
    }
    float time = 0.5f;
    private IEnumerator DecreaseHealthSmoothly()
    {
        float startTime = Time.time;
        float endTime = startTime + time;
        float startValue = healthBar.value;
        float endValue = remainingHits;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / time;
            healthBar.value = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }

        healthBar.value = remainingHits;
    }
    // int hits = 3;
    public Slider healthBar;
    public void failed()
    {
        Rcc.KillEngine();


        for (int i = 0; i < hideOnComplete.Length; i++)
        {
            hideOnComplete[i].SetActive(false);
        }

        failedPanel.SetActive(true);
        
    }
}
