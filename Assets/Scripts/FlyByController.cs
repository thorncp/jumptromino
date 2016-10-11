using UnityEngine;

public class FlyByController : MonoBehaviour
{
    public Camera mainCamera;

    public void SwitchToMainCamera()
    {
        Debug.Log("hello");
        Destroy(GetComponent<Animator>());
        mainCamera.gameObject.SetActive(true);
        mainCamera.enabled = true;
        gameObject.SetActive(false);
        enabled = false;
    }
}
