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
    #region SerializedFields
#pragma warning disable 649
    [Header("Objects")][Space(5)]
    [SerializeField] private GameObject mainWindow;
    [SerializeField] private GameObject helpWindow;
    [SerializeField] private GameObject creditsWindow;
    [SerializeField] private Button goBackButton;
#pragma warning restore 649
    #endregion
    
    private void Start()
    {
        ShowMainWindow();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1f;
    }
    public void ButtonAssign(int num)
    {
        switch ((ButtonSwitch) num)
        {
           case ButtonSwitch.PlayButton:
               SceneManager.LoadScene("TutorialLevel");
               return;
           case ButtonSwitch.HelpButton:
               ShowHelpWindow();
               return;
           case ButtonSwitch.CreditsButton:
               ShowControlsWindow();
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
    
    private void ShowMainWindow()
    {
        mainWindow.SetActive(true);
        helpWindow.SetActive(false);
        creditsWindow.SetActive(false);
    }
    
    private void ShowHelpWindow()
    {
        mainWindow.SetActive(false);
        helpWindow.SetActive(true);
        goBackButton.gameObject.SetActive(true);
    }
    
    private void ShowControlsWindow()
    {
        mainWindow.SetActive(false);
        creditsWindow.SetActive(true);
        goBackButton.gameObject.SetActive(true);
    }

    public void GoBackButtonHandler()
    {
        ShowMainWindow();
        goBackButton.gameObject.SetActive(false);
    }
}
