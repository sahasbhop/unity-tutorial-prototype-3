using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int jumpForce = 25;
    public int gravityModifier = 10;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpAudio;
    public AudioClip crashAudio;

    private Rigidbody _playerRigidBody;
    private Animator _playerAnimator;
    private AudioSource _playerAudioSource;
    
    private bool _isOnGround = true;
    private bool _isGameOver;
    private static readonly int JumpTrig = Animator.StringToHash("Jump_trig");
    private static readonly int DeathTypeINT = Animator.StringToHash("DeathType_int");
    private static readonly int DeathB = Animator.StringToHash("Death_b");

    private void Start()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponent<Animator>();
        _playerAudioSource = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) OnHitTheGround();
        else if (other.gameObject.CompareTag("Obstacle")) OnHitTheObstacle();
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    private void OnHitTheGround()
    {
        _isOnGround = true;
        dirtParticle.Play();
    }

    private void OnHitTheObstacle()
    {
        explosionParticle.Play();
        _playerAudioSource.PlayOneShot(crashAudio);
        
        _isGameOver = true;
        Debug.Log("Game Over");
        
        Death();
        dirtParticle.Stop();
    }

    private void Jump()
    {
        if (!_isOnGround || _isGameOver) return;
        
        _playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _playerAnimator.SetTrigger(JumpTrig);
        _playerAudioSource.PlayOneShot(jumpAudio);
        _isOnGround = false;
        dirtParticle.Stop();
    }

    private void Death()
    {
        _playerAnimator.SetInteger(DeathTypeINT, 1);
        _playerAnimator.SetBool(DeathB, true);
        dirtParticle.Stop();
    }
}