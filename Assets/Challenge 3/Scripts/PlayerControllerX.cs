using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;

    private readonly float _gravityModifier = 1.5f;
    private readonly float _balloonFloatForce = 15f;
    private readonly float _balloonBounceForce = 5f;

    private bool _gameOver;
    private Rigidbody _playerRb;
    private AudioSource _playerAudio;

    private void Start()
    {
        Physics.gravity *= _gravityModifier;
        _playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        _playerRb = GetComponent<Rigidbody>();
        BounceBalloon(false);
    }

    private void Update()
    {
        // While space is pressed and player is low enough, float up
        var isLowEnough = transform.position.y < 12f;
        if (Input.GetKey(KeyCode.Space) && isLowEnough && !_gameOver)
        {
            KeepBalloonFloat();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            _playerAudio.PlayOneShot(explodeSound, 1.0f);
            _gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            _playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }

        // if player collides with ground, bounce
        else if (other.gameObject.CompareTag("Ground"))
        {
            BounceBalloon(true);
        }
    }

    private void KeepBalloonFloat()
    {
        _playerRb.AddForce(Vector3.up * _balloonFloatForce);
    }

    private void BounceBalloon(bool playSound)
    {
        _playerRb.AddForce(Vector3.up * _balloonBounceForce, ForceMode.Impulse);
        if (playSound) _playerAudio.PlayOneShot(bounceSound, 1.0f);
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }
}