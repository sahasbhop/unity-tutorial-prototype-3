using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int jumpForce = 25;
    public int gravityModifier = 10;
    public long score;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpAudio;
    public AudioClip crashAudio;

    private Rigidbody _playerRigidBody;
    private Animator _playerAnimator;
    private AudioSource _playerAudioSource;

    private const float SpeedNormal = 20;
    private const float SpeedFast = 35;
    private const float ObstacleSpawnRateNormalMin = 1f;
    private const float ObstacleSpawnRateNormalMax = 3f;
    private const float ObstacleSpawnRateFastMin = 0.8f;
    private const float ObstacleSpawnRateFastMax = 1.6f;
    private bool _isOnGround = true;
    private bool _isSecondJumpActivated;
    private bool _isGameStart;
    private bool _isGameOver;
    private bool _isDashing;
    private static readonly int JumpTrig = Animator.StringToHash("Jump_trig");
    private static readonly int DeathTypeINT = Animator.StringToHash("DeathType_int");
    private static readonly int DeathB = Animator.StringToHash("Death_b");
    private static readonly int SpeedF = Animator.StringToHash("Speed_f");

    private void Start()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponent<Animator>();
        _playerAudioSource = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    private void Update()
    {
        if (!StartGame()) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _isDashing = !_isDashing;
        }

        if (!_isGameOver)
        {
            score += (long)(Time.deltaTime * (_isDashing ? 2000 : 1000));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) OnHitTheGround();
        else if (other.gameObject.CompareTag("Obstacle")) OnHitTheObstacle();
    }

    public bool IsGameStart()
    {
        return _isGameStart;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public float Speed()
    {
        return _isDashing ? SpeedFast : SpeedNormal;
    }

    public float ObstacleSpawnRateMin()
    {
        return _isDashing ? ObstacleSpawnRateFastMin : ObstacleSpawnRateNormalMin;
    }

    public float ObstacleSpawnRateMax()
    {
        return _isDashing ? ObstacleSpawnRateFastMax : ObstacleSpawnRateNormalMax;
    }

    private void OnHitTheGround()
    {
        _isOnGround = true;
        _isSecondJumpActivated = false;
        if (_isGameStart && !_isGameOver) dirtParticle.Play();
    }

    private void OnHitTheObstacle()
    {
        explosionParticle.Play();
        _playerAudioSource.PlayOneShot(crashAudio);

        _isGameOver = true;
        Debug.Log($"Game Over, Score={score}");

        Death();
    }

    private void Jump()
    {
        if (_isGameOver) return;
        if (!_isOnGround && _isSecondJumpActivated) return;

        _playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _playerAnimator.SetTrigger(JumpTrig);
        _playerAudioSource.PlayOneShot(jumpAudio);
        dirtParticle.Stop();

        if (_isOnGround)
        {
            _isOnGround = false;
        }
        else if (!_isSecondJumpActivated)
        {
            _isSecondJumpActivated = true;
        }
    }

    private void Death()
    {
        _playerAnimator.SetInteger(DeathTypeINT, 1);
        _playerAnimator.SetBool(DeathB, true);
        dirtParticle.Stop();
    }

    private bool StartGame()
    {
        if (_isGameStart) return true;

        if (transform.position.x < 0)
        {
            transform.Translate(Vector3.right * (3 * Time.deltaTime), Space.World);
            return false;
        }

        _isGameStart = true;
        _playerAnimator.SetFloat(SpeedF, 1.0f);
        dirtParticle.Play();
        return true;
    }
}