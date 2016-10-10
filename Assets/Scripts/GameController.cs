using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject[] piecePrefabPool;
    public GameObject heightGoalPlane;
    public GameObject floorPlane;
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
        get
        {
            return Math.Abs(baseHeight - heightGoalPlane.transform.position.y);
        }
    }
    private float baseHeight
    {
        get { return floorPlane.GetComponent<Renderer>().bounds.max.y; }
    }

    public void Start()
    {
        Cursor.visible = false;
        UpdateUI();
        AddNewPiece();
    }

    public void FixedUpdate()
    {
        if (waitingToAddPiece && AllPiecesStill())
        {
            UpdateUI();
            if (goalReached())
            {
                gameOverText.text = String.Format(
                    "You did it!\n" +
                    FormatHeight("Goal", heightGoal) + "\n" +
                    FormatHeight("Achieved", DetermineHeight()) + "\n" +
                    "Pieces: {0}",
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
        var height = GameObject.FindGameObjectsWithTag("Piece").
            SelectMany(p => MaxYs(p)).
            DefaultIfEmpty(baseHeight).
            Max();

        return Math.Abs(baseHeight - height);
    }

    private bool goalReached()
    {
        return DetermineHeight() - heightGoal >= 0.0;
    }

    private void ReloadScene()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private IEnumerable<float> MaxYs(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            yield return
                child.gameObject.GetComponent<MeshRenderer>().bounds.max.y;
        }
    }

    private void UpdateUI()
    {
        goalText.text = FormatHeight("Goal", heightGoal);
        heightText.text = FormatHeight("Current", DetermineHeight());
    }

    private string FormatHeight(string context, float height)
    {
        return String.Format("{0}: {1:0.0##}\"", context, height);
    }
}
