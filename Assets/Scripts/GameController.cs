using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject[] piecePrefabPool;
    public GameObject heightGoalPlane;
    public Text goalText;
    public Text heightText;
    public Text gameOverText;
    public float newPieceDelay;
    public float baskInTheGloryTime = 5f;

    private System.Random random = new System.Random();
    private bool waitingToAddPiece;
    private int pieceCount = 0;
    private float heightGoal
    {
        get { return heightGoalPlane.transform.position.y; }
    }

    public void Start()
    {
        Cursor.visible = false;
        goalText.text = String.Format("Goal: {0:0.0##}", heightGoal);
        heightText.text = String.Format("Current: {0:0.0##}", DetermineHeight());
        AddNewPiece();
    }

    public void FixedUpdate()
    {
        if (waitingToAddPiece && AllPiecesStill())
        {
            goalText.text = String.Format("Goal: {0:0.0##}", heightGoal);
            heightText.text = String.Format("Current: {0:0.0##}", DetermineHeight());
            if (goalReached())
            {
                gameOverText.text = String.Format(
                    "You did it!\n" +
                    "Goal: {0}\n" +
                    "Achieved: {1}\n" +
                    "Pieces: {2}",
                    heightGoal,
                    DetermineHeight(),
                    pieceCount
                );
                gameOverText.gameObject.SetActive(true);
                Invoke("ReloadScene", baskInTheGloryTime);
            }
            else
            {
                Invoke("AddNewPiece", newPieceDelay);
                waitingToAddPiece = false;
            }
        }
    }

    public void AddNewPieceAfterTimeout()
    {
        waitingToAddPiece = true;
    }

    private void AddNewPiece()
    {
        pieceCount += 1;
        var index = random.Next(0, piecePrefabPool.Length);
        var startPiecePrefab = piecePrefabPool[index];
        var nextPiece = Instantiate(
            startPiecePrefab,
            spawnPoint.transform.position,
            startPiecePrefab.transform.rotation
        ) as GameObject;
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

    private float DetermineHeight()
    {
        return GameObject.FindGameObjectsWithTag("Piece").
            Select(p => p.GetComponent<Collider>().bounds.max.y).
            DefaultIfEmpty(0).
            Max();
    }

    private bool goalReached()
    {
        return DetermineHeight() >= heightGoal;
    }

    private void ReloadScene()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
