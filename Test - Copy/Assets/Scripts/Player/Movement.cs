using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] [Range(0,10)]  private float mMovementSpeed;
    [SerializeField] [Range(0, 10)] private float mJumpHeight;
    [SerializeField]                private bool  mCanDoubleJump;
    [SerializeField]                private bool  mCanFly;
    [SerializeField] [Range(0, 10)] private float mFlightSpeed;
    [SerializeField] [Range(0, 10)] private float mFlightAcceleration;
    [SerializeField] [Range(0, 60)] private float mFlightLength;


    public bool  isGrounded = false;
    public bool  isInAir = true;
    public bool  doubleJumpReady = true;
    public float airTime = 0;
    public float flightMovementMultiplier = 1.5f;
    private Animator _anim;
    private Rigidbody2D _rb;
    private Vector3 _movement;
    private bool _jump;
    private Transform _character;


    private const string PlayerIdle = "Player_Idle";
    private const string PlayerMove = "Player_Move";

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _character = transform.Find("Character");
    }

    private void Update() {
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

        //Jump
        if (isGrounded) {
            _jump = Input.GetButtonDown("Jump") || _jump;
        }
        

        //Double Jump
        if(mCanDoubleJump && doubleJumpReady && isInAir) {
            _jump = Input.GetButtonDown("Jump") || _jump;
        }

        //Flight
        if (isInAir) airTime += Time.deltaTime;
        if (mCanFly && airTime < mFlightLength && airTime > 0.01) {
            _jump = Input.GetButton("Jump");
        }
    }

    private void FixedUpdate() {
        _rb.gravityScale = 1;
        //Jump
        if(mCanFly && _jump && airTime > 0.01) {
            _rb.gravityScale = 0;
            _rb.AddForce(new Vector2(0, mFlightAcceleration), ForceMode2D.Impulse);
            if (_rb.velocity.y >= 8.5f) _rb.velocity = new Vector2(_rb.velocity.x, mFlightSpeed);
        } else if (mCanDoubleJump && _jump) {
            _rb.AddForce(new Vector2(0f, mJumpHeight), ForceMode2D.Impulse);
            doubleJumpReady = false;
            _jump = false;
        } else if(_jump) {
            _rb.AddForce(new Vector2(0f, mJumpHeight), ForceMode2D.Impulse);
            _jump = false;
        }

        //Movement
        transform.position += _movement * (Time.deltaTime * mMovementSpeed * (mCanFly && isInAir ? flightMovementMultiplier : 1f));
        _anim.Play(_movement.x != 0 ? PlayerMove : PlayerIdle);

        //Flip Direction
        if (_movement.x != 0) {
            _character.localScale = _movement.x > 0 ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
        }
    }
}
