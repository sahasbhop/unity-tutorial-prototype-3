using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private const int LeftBound = -15;
    private GameObject _player;
    private PlayerController _playerController;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _playerController = _player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_player.transform.position.x < 0) return;
        
        if (_playerController.IsGameOver()) return;
        transform.Translate(Vector3.left * (Time.deltaTime * _playerController.Speed()));

        if (transform.CompareTag("Obstacle") && transform.position.x < LeftBound)
        {
            Destroy(gameObject);
        }
    }
}