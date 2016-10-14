using UnityEngine;

public class FlyByController : MonoBehaviour
{
    public Camera mainCamera;

    public void SwitchToMainCamera()
    {
        Destroy(GetComponent<Animator>());
        mainCamera.gameObject.SetActive(true);
        mainCamera.enabled = true;
        gameObject.SetActive(false);
        enabled = false;
    }
}
