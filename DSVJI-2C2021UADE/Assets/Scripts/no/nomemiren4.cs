using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nomemiren4 : Interactable
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private string sceneToLoad = "TutorialLevel";
    [SerializeField] private string teleportName = "Tp to location";
    [SerializeField] private GameObject thanksText;
    [SerializeField] private GameObject blackBackground;
#pragma warning restore 649

    #endregion
    
    private void Start()
    {
        InteractableName = teleportName;
    }
    
    public override void Interaction()
    {
        Character.Ui.FadeOutHandler();
        Invoke(nameof(DisplayText),5f);
    }
    
    private void SceneLoad()
    {
        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    private void DisplayText()
    {
        blackBackground.SetActive(true);
        thanksText.SetActive(true);
        Invoke(nameof(SceneLoad), 10f);
    }
}
