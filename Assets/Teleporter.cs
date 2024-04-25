using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform player; // Assign your player's transform in the inspector
    public Vector3 newPosition; // Set the new position coordinates in the inspector

    public void TeleportPlayer()
    {
        player.position = newPosition; // Teleport the player
    }
}