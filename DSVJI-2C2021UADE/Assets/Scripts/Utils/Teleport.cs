using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : Interactable
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private bool isSceneTeleport = false;
    [SerializeField] private string sceneToLoad = "TutorialLevel";
    [SerializeField] private Transform teleportLocation;
    [SerializeField] private string teleportName = "Tp to location";
#pragma warning restore 649

    #endregion

    private void Start()
    {
        InteractableName = teleportName;
    }

    public override void Interaction()
    {
        if (isSceneTeleport)
        {
            SceneManager.LoadScene(sceneToLoad);
            return;
        }
        if (!(Character is null)) Character.Teleport(teleportLocation);
    }
}
