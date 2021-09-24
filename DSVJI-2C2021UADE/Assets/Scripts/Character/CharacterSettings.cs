using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSettings : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("References")] [Space(2)] 
    [SerializeField] private Toggle invertMouseYToggle;
    [SerializeField] private Toggle invertMouseXToggle;
    [SerializeField] private Toggle strafeToggle;
#pragma warning restore 649
    #endregion
    
    private bool _invertMouseY;
    private bool _invertMouseX;
    public bool InvertMouseY => _invertMouseY;
    public bool InvertMouseX => _invertMouseX;

    private void Start()
    {
        invertMouseYToggle.onValueChanged.AddListener(delegate { InvertYToggle(invertMouseYToggle); });
        invertMouseXToggle.onValueChanged.AddListener(delegate { InvertXToggle(invertMouseXToggle); });
        ReadInitialValues();
    }

    private void ReadInitialValues()
    {
        InvertYToggle(invertMouseYToggle);
        InvertXToggle(invertMouseXToggle);
    }

    private void InvertYToggle(Toggle check)
    {
        _invertMouseY = check.isOn;
    }
    private void InvertXToggle(Toggle check)
    {
        _invertMouseX = check.isOn;
    }
}
