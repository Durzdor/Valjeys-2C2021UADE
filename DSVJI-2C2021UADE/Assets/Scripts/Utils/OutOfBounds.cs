using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.GetComponent<Character>();
            var teleportPos = player.CheckpointRespawn;
            player.Teleport(teleportPos);
        }
    }
}
