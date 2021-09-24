using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopup : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject notificationGameObject;
    [SerializeField] private TextMeshProUGUI notificationTitleText;
    [SerializeField] private TextMeshProUGUI notificationBodyText;
#pragma warning restore 649
    #endregion
    
    private Character _character;

    private void Start()
    {
        _character = GetComponentInParent<Character>();
        _character.OnCharacterOrbAcquired += SetMessage;
        closeButton.onClick.AddListener(CloseMessagePopup);
    }
    
    public void SetMessage(string title, string body)
    {
        notificationGameObject.SetActive(true);
        _character.IsAnimationLocked = true;
        notificationTitleText.text = title;
        notificationBodyText.text = body;
        GameStatus.ChangeGameStatus(GameState.Paused);
    }
    
    private void CloseMessagePopup()
    {
        notificationGameObject.SetActive(false);
        _character.IsAnimationLocked = false;
        GameStatus.ChangeGameStatus(GameState.Playing);
    }
}
