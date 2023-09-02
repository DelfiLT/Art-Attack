using UnityEngine;

public class PlayerPlatformDetector : MonoBehaviour
{
    [SerializeField] private PlayerPowerController powerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);

        var platform = collision.GetComponentInParent<PlatformController>();
        if (platform != null)
        {
            powerController.AddClosePlatform(platform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var platform = collision.GetComponentInParent<PlatformController>();
        if (platform != null)
        {
            powerController.RemoveClosePlatform(platform);
        }
    }
}
