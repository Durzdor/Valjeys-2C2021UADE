using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplay : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private Button closeButton;
    [SerializeField] private Image displayedImage;
#pragma warning restore 649

    #endregion

    private void Awake()
    {
        closeButton.onClick.AddListener(CloseImage);
    }
    
    private void CloseImage()
    {
        Destroy(gameObject);
    }

    public void SetImage(Sprite image)
    {
        displayedImage.sprite = image;
    }
}
