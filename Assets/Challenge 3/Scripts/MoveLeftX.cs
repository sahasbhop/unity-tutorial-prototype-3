using UnityEngine;

public class MoveLeftX : MonoBehaviour
{
    public float speed;
    private const float LeftBound = -10;
    private PlayerControllerX _playerControllerScript;

    private void Start()
    {
        _playerControllerScript = GameObject.Find("Player").GetComponent<PlayerControllerX>();
    }

    private void Update()
    {
        // If game is not over, move to the left
        if (!_playerControllerScript.IsGameOver())
        {
            transform.Translate(Vector3.left * (speed * Time.deltaTime), Space.World);
        }

        // If object goes off screen that is NOT the background, destroy it
        if (transform.position.x < LeftBound && !gameObject.CompareTag("Background"))
        {
            Destroy(gameObject);
        }
    }
}