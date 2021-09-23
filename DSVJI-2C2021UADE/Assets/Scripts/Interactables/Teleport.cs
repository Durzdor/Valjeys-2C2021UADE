using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : Interactable
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private bool isSceneTeleport = false;
    [SerializeField] private bool isEndGameTeleport = false;
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
            SceneManager.LoadSceneAsync(sceneToLoad);
            return;
        }
        if (!(Character is null)) Character.Teleport(teleportLocation);
    }
}
