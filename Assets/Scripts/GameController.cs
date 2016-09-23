using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject[] piecePrefabPool;
    public float newPieceDelay;

    private System.Random random = new System.Random();
    private bool waitingToAddPiece;

    public void Start()
    {
        AddNewPiece();
    }

    public void FixedUpdate()
    {
        if (waitingToAddPiece && AllPiecesStill())
        {
            Invoke("AddNewPiece", newPieceDelay);
            waitingToAddPiece = false;
        }
    }

    public void AddNewPieceAfterTimeout()
    {
        waitingToAddPiece = true;
    }

    private void AddNewPiece()
    {
        var index = random.Next(0, piecePrefabPool.Length);
        var startPiecePrefab = piecePrefabPool[index];
        var nextPiece = Instantiate(startPiecePrefab);
        nextPiece.GetComponent<PieceController>().gameController = this;
        var camera = Camera.main.GetComponent<CameraController>();
        camera.Follow(nextPiece);
    }

    private bool AllPiecesStill()
    {
        foreach (var piece in GameObject.FindGameObjectsWithTag("Piece"))
        {
            if (piece.transform.hasChanged)
            {
                piece.transform.hasChanged = false;
                return false;
            }
        }

        return true;
    }
}
