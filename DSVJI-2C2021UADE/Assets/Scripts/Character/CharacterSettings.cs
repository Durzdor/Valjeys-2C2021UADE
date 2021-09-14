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
    
    private bool invertMouseY;
    private bool invertMouseX;
    private bool adStrafe;
    public bool InvertMouseY => invertMouseY;
    public bool InvertMouseX => invertMouseX;
    public bool AdStrafe => adStrafe;

    private void Start()
    {
        invertMouseYToggle.onValueChanged.AddListener(delegate { InvertYToggle(invertMouseYToggle); });
        invertMouseXToggle.onValueChanged.AddListener(delegate { InvertXToggle(invertMouseXToggle); });
        strafeToggle.onValueChanged.AddListener(delegate { StrafeToggle(strafeToggle); });
        ReadInitialValues();
    }

    private void ReadInitialValues()
    {
        InvertYToggle(invertMouseYToggle);
        InvertXToggle(invertMouseXToggle);
        StrafeToggle(strafeToggle);
    }

    private void InvertYToggle(Toggle check)
    {
        invertMouseY = check.isOn;
    }
    private void InvertXToggle(Toggle check)
    {
        invertMouseX = check.isOn;
    }
    private void StrafeToggle(Toggle check)
    {
        adStrafe = check.isOn;
    }
}
