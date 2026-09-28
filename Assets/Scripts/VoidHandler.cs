using UnityEngine;

public class VoidHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // get the PlayerHandler component script of the `other` that it collided with
        PlayerHandler ph = other.GetComponent<PlayerHandler>();
        // notice how PlayerHandler is a valid object data type

        // verify its the player if it has PlayerHandler script
        if (ph != null)
        {
            ph.ResetPosition();
        }

    }
}