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
