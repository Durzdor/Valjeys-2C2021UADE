using TMPro;
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
    [SerializeField] private TextMeshProUGUI controlsText;
    [SerializeField] private InputData inputData;
    
#pragma warning restore 649
    #endregion

    private CharacterInput _input;

    private void OnEnable()
    {
        _input = GetComponent<CharacterInput>();
    }

    private void Start()
    {
        ShowMainWindow();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1f;
        ControlsTextUpdate();
    }
    
    private void ControlsTextUpdate()
    {
        var title = "Controls\n\n";
        var movement = "WASD - Movement\n";
        var jump = $"{inputData.jump} - Jump\n";
        var run = $"{inputData.changeSpeed} - Run\n";
        var swap = $"{inputData.switchCharacter} - Switch Character\n";
        var interact = $"{inputData.interact} - Interact\n";
        var pause = $"{inputData.pause} - Pause\n";
        controlsText.text = $"{title}{movement}{jump}{run}{swap}{interact}{pause}";
    }
    
    public void ButtonAssign(int num)
    {
        switch ((ButtonSwitch) num)
        {
           case ButtonSwitch.PlayButton:
               SceneManager.LoadScene("Level1");
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

    // private void ControlsTextUpdate()
    // {
    //     var title = "Controls\n\n";
    //     var movement = "WASD - Movement\n";
    //     var jump = $"{_input.Jump} - Jump\n";
    //     var run = $"{_input.ChangeSpeed} - Run\n";
    //     var swap = $"{_input.SwitchCharacter} - Switch Character\n";
    //     var interact = $"{_input.Interact} - Interact\n";
    //     var pause = $"{_input.Pause} - Pause\n";
    //     controlsText.text = $"{title}{movement}{jump}{run}{swap}{interact}{pause}";
    // }
}
