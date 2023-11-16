using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private const int LeftBound = -15;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_playerController.IsGameOver()) return;
        transform.Translate(Vector3.left * (Time.deltaTime * _playerController.Speed()));

        if (transform.CompareTag("Obstacle") && transform.position.x < LeftBound)
        {
            Destroy(gameObject);
        }
    }
}