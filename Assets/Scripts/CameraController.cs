using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject spawnPoint;
    public float birdsEyeYOffset = 4;

    private Vector3 startPosition;
    private GameObject piece;
    private bool birdsEye;
    private Vector3 offset;

    void Start()
    {
        birdsEye = false;
        startPosition = transform.position;
        offset = transform.position - spawnPoint.transform.position;
    }

    void LateUpdate()
    {
        if (piece != null)
        {
            if (birdsEye)
            {
                var birdsEyePostition = new Vector3(
                    piece.transform.position.x,
                    piece.transform.position.y + offset.y * birdsEyeYOffset,
                    piece.transform.position.z - 2
                );
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    birdsEyePostition,
                    30f * Time.deltaTime
                );
            }
            else
            {
                transform.position = piece.transform.position + offset;
            }

            transform.LookAt(piece.transform);
        }
    }

    public void TransitionToBirdsEye()
    {
        birdsEye = true;
    }

    public void Follow(GameObject piece)
    {
        birdsEye = false;
        this.piece = piece;
        offset = startPosition - piece.transform.position;
    }
}
