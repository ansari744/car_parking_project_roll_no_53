using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonLayoutSettings : MonoBehaviour
{
    

    private bool isDragging = false;
    private Button selectedButton = null;
    Button tempSelectedButton;
    public List<Button> buttons = new List<Button>();

    int currentLayout;
    public List<GameObject> limitPanels;
    float minX, maxX, minY, maxY;
    float deltaX, deltaY;


    //public GameObject[] layouts;
    private void Start()
    {
             CalculateLimits();
             LoadButtonLayout();
    }
    public void selectLayout(int number)
    {
        PlayerPrefs.SetInt("savedLayout", number);
       
     //   layout[number].layoutPanel.gameObject.SetActive(true);
    }
   
    private void Update()
    {
        //if (selectedButton == null)
        //{
        //    Debug.Log(" NULL !!");
        //}
        //else
        //    Debug.Log("NOt NULL ");
        if (isDragging && selectedButton != null)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = touch.position;
                
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        deltaX = touchPosition.x - selectedButton.transform.position.x;
                        deltaY = touchPosition.y - selectedButton.transform.position.y;
                        break;
                    case TouchPhase.Moved:
                        Vector3 newPosition = new Vector3(touchPosition.x - deltaX, touchPosition.y - deltaY, 0f);

                        if (newPosition.x > minX && newPosition.y > minY && newPosition.x < maxX && newPosition.y < maxY)
                        {
                            selectedButton.transform.position = newPosition;
                        }
                        break;

                }
              
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 newPosition = mousePosition;
                if (newPosition.x > minX && newPosition.y > minY && newPosition.x < maxX && newPosition.y < maxY)
                {
                    selectedButton.transform.position = newPosition;
                }
            }
        }
       
       

    }
    void CalculateLimits()
    {
        foreach (GameObject limitPanel in limitPanels)
        {
            RectTransform rectTransform = limitPanel.GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            minX = Mathf.Min(minX, corners[0].x);
            maxX = Mathf.Max(maxX, corners[2].x);
            minY = Mathf.Min(minY, corners[0].y);
            maxY = Mathf.Max(maxY, corners[2].y);
        }
    }

    public void moveWithArrowsX(int scale)
    {
        Vector3 position = tempSelectedButton.transform.position;
        tempSelectedButton.transform.position = new Vector3(position.x + scale, position.y, position.z);
    } public void moveWithArrowsY(int scale)
    {
        Vector3 position = tempSelectedButton.transform.position;
        tempSelectedButton.transform.position = new Vector3(position.x, position.y + scale, position.z);
    }
    private void SaveButtonLayout()
    {
        for (int i = 0; i <buttons.Count; i++)
        {
            // Save the position of the button
            PlayerPrefs.SetFloat("button" + i + "x", buttons[i].GetComponent<RectTransform>().anchoredPosition.x);
            PlayerPrefs.SetFloat("button" + i + "y", buttons[i].GetComponent<RectTransform>().anchoredPosition.y);

            // Save the scale of the button
            PlayerPrefs.SetFloat("button" + i + "scaleX",buttons[i].GetComponent<RectTransform>().localScale.x);
            PlayerPrefs.SetFloat("button" + i + "scaleY", buttons[i].GetComponent<RectTransform>().localScale.y);
        }

        PlayerPrefs.Save();
    }

    private void LoadButtonLayout()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            // Load the position of the button
            float x = PlayerPrefs.GetFloat("button" + i + "x");
            float y = PlayerPrefs.GetFloat("button" + i + "y");

            // Load the scale of the button
            float scaleX = PlayerPrefs.GetFloat("button" + i + "scaleX");
            float scaleY = PlayerPrefs.GetFloat("button" + i + "scaleY");


            // Set the position and scale of the button
            if (x != 0 && y != 0)
               buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            if (scaleX == 0)
                scaleX = 1;
            if (scaleY == 0)
                scaleY = 1;
            buttons[i].GetComponent<RectTransform>().localScale = new Vector3(scaleX, scaleY, 1f);
        }
    }

  

    public void ApplyButtonLayout()
    {
        SaveButtonLayout();
    }

    public void PlayGame()
    {
       // SaveButtonLayout(); // Save the button positions and scale before loading the game scene
        SceneManager.LoadScene("gameScene");
    }
    public Slider slider;
    public void ButtonSelected(Button button)
    {
        selectedButton = button;
        isDragging = true;
        tempSelectedButton = selectedButton;
        slider.value = selectedButton.GetComponent<RectTransform>().localScale.x;
    }

    public void ButtonDeselected()
    {
        selectedButton = null;
        isDragging = false;
       // SaveButtonLayout();
    }
   
    public void ChangeButtonScale(float scale)
    {
        
        if (tempSelectedButton != null)
        {
           
            tempSelectedButton.transform.localScale = new Vector3(scale,scale, 1f);
          
        }
    }
}
