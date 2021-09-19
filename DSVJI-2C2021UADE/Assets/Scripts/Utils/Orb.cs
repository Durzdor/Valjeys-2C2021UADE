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

    private Vector3 posOffset;
    private Vector3 tempPos;
    
    private void Start ()
    {
        InteractableName = orbName;
        posOffset = transform.position;
    }
    private 
    void Update () 
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
        
        tempPos = posOffset;
        tempPos.y += Mathf.Lerp(tempPos.y, (Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude), 10f / 2f);
 
        transform.position = tempPos;
    }

    public override void Interaction()
    {
        if (!(Character is null)) Character.OrbAcquisition();
    }
}
