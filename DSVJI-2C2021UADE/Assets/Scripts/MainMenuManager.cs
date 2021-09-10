using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ButtonSwitch
{
    PlayButton,
    HelpButton,
    CreditsButton,
    QuitButton
}
public class MainMenuManager : MonoBehaviour
{
    public List<Button> buttons;
    public RectTransform selectorObject;
    public GameObject mainWindow;
    public GameObject helpWindow;
    public GameObject creditsWindow;
    public Button goBackButton;
    private Button currentButton;
    private int buttonIndex;

    private void Start()
    {
        ResetSelector();
        mainWindow.SetActive(true);
        helpWindow.SetActive(false);
        creditsWindow.SetActive(false);
    }

    private void ResetSelector()
    {
        buttonIndex = 0;
        currentButton = buttons[0];
        selectorObject.position = new Vector3(selectorObject.position.x, currentButton.transform.position.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (mainWindow.activeInHierarchy)
            {
                buttonIndex--;
                if (buttonIndex < 0)
                {
                    buttonIndex = buttons.Count - 1;
                }
                currentButton = buttons[buttonIndex];
                selectorObject.position = new Vector3(selectorObject.position.x, currentButton.transform.position.y);
                
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (mainWindow.activeInHierarchy)
            {
                buttonIndex++;
                if (buttonIndex > buttons.Count - 1)
                {
                    buttonIndex = 0;
                }
                currentButton = buttons[buttonIndex];
                selectorObject.position = new Vector3(selectorObject.position.x, currentButton.transform.position.y);
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!mainWindow.activeInHierarchy)
            {
                goBackButton.onClick.Invoke();
                return;
            }
            currentButton.onClick.Invoke(); 
        }
    }

    public void ButtonAssign(int num)
    {
        Enum aux = (ButtonSwitch) num;
        switch (aux)
        {
           case ButtonSwitch.PlayButton:
               SceneManager.LoadScene("TutorialLevel");
               return;
           case ButtonSwitch.HelpButton:
               mainWindow.SetActive(false);
               helpWindow.SetActive(true);
               goBackButton.gameObject.SetActive(true);
               selectorObject.position = new Vector3(selectorObject.position.x, goBackButton.transform.position.y);
               return;
           case ButtonSwitch.CreditsButton:
               mainWindow.SetActive(false);
               creditsWindow.SetActive(true);
               goBackButton.gameObject.SetActive(true);
               selectorObject.position = new Vector3(selectorObject.position.x, goBackButton.transform.position.y);
               return;
           case ButtonSwitch.QuitButton:
#if UNITY_EDITOR
               UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE_WIN
               Application.Quit();
#endif
               return;
           default: return;
        } 
    }

    public void GoBackButtonHandler()
    {
        mainWindow.SetActive(true);
        helpWindow.SetActive(false);
        creditsWindow.SetActive(false);
        goBackButton.gameObject.SetActive(false);
        ResetSelector();
    }
}
