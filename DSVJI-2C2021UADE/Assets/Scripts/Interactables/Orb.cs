using UnityEngine;

public class Orb : Interactable
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private float degreesPerSecond = 15.0f;
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private string orbName;
#pragma warning restore 649
    #endregion

    private Vector3 _posOffset;
    private Vector3 _tempPos;
    
    private void Start ()
    {
        InteractableName = orbName;
        _posOffset = transform.position;
    }
    private 
    void Update () 
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
        
        _tempPos = _posOffset;
        _tempPos.y += Mathf.Lerp(_tempPos.y, (Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude), 10f / 2f);
 
        transform.position = _tempPos;
    }

    public override void Interaction()
    {
        if (!(Character is null)) Character.OrbAcquisition();
    }
}
