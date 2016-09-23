using System; using UnityEngine;

public class PieceController : MonoBehaviour
{
    public float speed;
    public float afterJumpSpeed;
    public float jumpStrength;
    public GameController gameController;

    private bool canJump;
    private bool playerHasControl;
    private Rigidbody rb;

    void Start()
    {
        canJump = true;
        playerHasControl = true;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (playerHasControl)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            var movement = new Vector3(moveHorizontal, 0f, moveVertical);
            rb.AddForce(movement * speed);

            if (canJump && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(
                    new Vector3(0, jumpStrength, 0f),
                    ForceMode.Impulse
                );
                speed = afterJumpSpeed;
                canJump = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Control Trigger"))
        {
            gameController.AddNewPieceAfterTimeout();
            Destroy(this);
        }
        else if (other.gameObject.CompareTag("Camera Trigger"))
        {
            var camera = Camera.main.GetComponent<CameraController>();
            camera.TransitionToBirdsEye();
        }
    }
}
