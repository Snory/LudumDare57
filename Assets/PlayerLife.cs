using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public GeneralEvent ShadowTouched;
    public GeneralEvent OutOfYourMind;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Shadow))
        {
            ShadowTouched?.Raise();
            Debug.Log("Shadow touched the player");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Consciousness))
        {
            OutOfYourMind?.Raise();
            Debug.Log("Player is out of their mind");
        }
    }
}
