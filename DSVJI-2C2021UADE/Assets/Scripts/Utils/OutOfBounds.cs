using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = GetComponent<Character>();
            var teleportPos = player.CheckpointRespawn;
            player.Teleport(teleportPos);
        }
    }
}
