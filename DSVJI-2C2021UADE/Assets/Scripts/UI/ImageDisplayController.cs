using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplayController : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private GameObject imageDisplayPrefab;
    [Header("Buttons")][Space(5)] 
    [SerializeField] private Button topDownMapButton;
    [SerializeField] private Button isometricMapButton;
    [Header("Sprites")][Space(5)]
    [SerializeField] private Sprite topDownMapSprite;
    [SerializeField] private Sprite isometricMapSprite;
#pragma warning restore 649

    #endregion

    private void Awake()
    {
        topDownMapButton.onClick.AddListener(delegate { SpawnImage(topDownMapSprite); });
        isometricMapButton.onClick.AddListener(delegate { SpawnImage(isometricMapSprite); });
    }

    private void SpawnImage(Sprite imageToSet)
    {
        var imageGo = Instantiate(imageDisplayPrefab);
        var imageGoScript = imageGo.GetComponent<ImageDisplay>();
        imageGoScript.SetImage(imageToSet);
    }
}
