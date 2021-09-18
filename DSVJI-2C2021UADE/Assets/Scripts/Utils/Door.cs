using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private void Start()
    {
        InteractableName = "Door";
    }
    
    public override void Interaction()
    {
        if (Character == null) return;
        print(transform.position - Character.transform.position);
    }
}
