using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public GameObject spawnPoint;
    public GameObject[] piecePrefabPool;
    public float pieceTorque = 100;

    private System.Random random = new System.Random();

    public void Start()
    {
        Cursor.visible = true;
        InvokeRepeating("SpawnPiece", 0, 4);
    }

    public void Play()
    {
        SceneManager.LoadScene("MindTheGap");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void SpawnPiece()
    {
        var index = random.Next(0, piecePrefabPool.Length);
        var startPiecePrefab = piecePrefabPool[index];
        var nextPiece = Instantiate(
            startPiecePrefab,
            spawnPoint.transform.position + RandomVector(),
            Random.rotation
        ) as GameObject;
        nextPiece.GetComponent<Rigidbody>().AddTorque(RandomVector() * pieceTorque);
    }

    private Vector3 RandomVector()
    {
        return new Vector3(
            Random.Range(-1.0f, 1.0f),
            Random.Range(-1.0f, 1.0f),
            Random.Range(-1.0f, 1.0f)
        );
    }
}
