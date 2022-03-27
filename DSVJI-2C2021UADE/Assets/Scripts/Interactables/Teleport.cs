using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : Interactable
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private bool isSceneTeleport;
    [SerializeField] private bool isEndGameTeleport;
    [SerializeField] private string sceneToLoad = "TutorialLevel";
    [SerializeField] private Transform teleportLocation;
    [SerializeField] private string teleportName = "Tp to location";
#pragma warning restore 649

    #endregion

    private bool CanGoToEnd => isEndGameTeleport && !(Character is null) && Character.GotAllOrbs;
    private void Start()
    {
        InteractableName = teleportName;
    }

    public override void Interaction()
    {
        if (CanGoToEnd || isSceneTeleport)
        {
            Character.Ui.FadeOutHandler();
            Invoke(nameof(SceneLoad),4.5f);
            return;
        }
        if (!(Character is null)) Character.Teleport(teleportLocation);
    }

    private void SceneLoad()
    {
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
