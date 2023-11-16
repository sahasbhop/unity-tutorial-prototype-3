using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstacles;
    private readonly Vector3 _spawningPosition = new Vector3(25, 0, 0);
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        Invoke(nameof(Spawn), 0);
    }

    private void Spawn()
    {
        if (_playerController.IsGameOver()) return;

        var obstacle = obstacles[Random.Range(0, obstacles.Length)];
        Instantiate(obstacle, _spawningPosition, obstacle.transform.rotation);

        Invoke(nameof(Spawn), Random.Range(0.7f, 2f));
    }
}