using System; using UnityEngine;

public class PieceController : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
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
                rb.rotation = Quaternion.identity;
                speed = afterJumpSpeed;
                canJump = false;
            }

            if (!canJump)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    rb.AddTorque(Vector3.right * rotationSpeed * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    rb.AddTorque(Vector3.left * rotationSpeed * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    rb.AddTorque(Vector3.forward * rotationSpeed * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    rb.AddTorque(Vector3.back * rotationSpeed * Time.deltaTime);
                }
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
