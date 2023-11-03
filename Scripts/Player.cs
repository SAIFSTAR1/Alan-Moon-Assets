using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Movement")]
    public float Maxspeed;
    public float Acceleration;
    public float Decceleration;
    public float VelPower;
    public float FrictionAmount;
    [Header("Jumping")]
    public float jumpForce;
    [Range(0,1)]
    public float JumpCutMultiplier;
    public float JumpCoyoteTime;
    public float JumpBufferTime;
    [Header("Gravity")]
    public float GravityScale;
    public float FallGravityMultiplier;
    public float MaxFallSpeed;

    private float MoveInput;

    public Rigidbody2D PlayerRB;

    public Transform GroundCheckPoint;
    public Vector2 GroundCheckRaduis;
    public LayerMask GroundLayer;

    private float LastOnGroundTime;
    private float LastJumpTime;
    private bool Isgrounded;
    private bool isJumping;
    private bool CanJump;
    private bool TryingToJump;
    private bool JumpInputReleased;

    private Camera _cam; 

    [SerializeField] 
    private GameObject weapon;

    private void Start()
    {
        _cam = Camera.main;
        PlayerRB = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Physics2D.OverlapBox(GroundCheckPoint.position, GroundCheckRaduis, 0, GroundLayer) && PlayerRB.velocity.y <= 0)
        {
            LastOnGroundTime = JumpCoyoteTime;
        }

        Controls();
        Timers();
        Gravity();
    }

    private void Gravity()
    {
        if (PlayerRB.velocity.y < 0)
        {
            PlayerRB.gravityScale = GravityScale * FallGravityMultiplier;

            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, Mathf.Max(PlayerRB.velocity.y, -MaxFallSpeed));
        }
        else
        {
            PlayerRB.gravityScale = GravityScale;
        }
    }

    private void Timers()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastJumpTime -= Time.deltaTime;
    }

    private void FixedUpdate() {
        Movement();
        Jump();
    }

    private void Controls()
    {
        MoveInput = Input.GetAxisRaw("Horizontal");
   
        if (Input.GetButtonDown("Jump"))
        {
            LastJumpTime = JumpBufferTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            OnJumpUp();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Dash(MoveInput);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Attack");
        }
        
        WeaponControl();
    }

    private void WeaponControl()
    {
        Utilities.RotateObject(transform.position, _cam.ScreenToWorldPoint(Input.mousePosition), ref weapon);
    }

    private void Movement()
    {
        float TargetSpeed = MoveInput * Maxspeed;
        float SpeedDif = TargetSpeed - PlayerRB.velocity.x;
        float AccelRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? Acceleration : Decceleration;
        float movement = Mathf.Pow(Mathf.Abs(SpeedDif) * AccelRate, VelPower) * Mathf.Sign(SpeedDif);

        PlayerRB.AddForce(movement * Vector2.right);

        #region Friction
        if(LastOnGroundTime > 0 && Mathf.Abs(MoveInput) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(PlayerRB.velocity.x), Mathf.Abs(FrictionAmount));
            amount *= Mathf.Sign(PlayerRB.velocity.x);
            PlayerRB.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion

    }

    private void Jump()
    {
        if (LastOnGroundTime > 0 && LastJumpTime > 0)
        {
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
            PlayerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            LastOnGroundTime = 0;
            LastJumpTime = 0;
            isJumping = true;
            JumpInputReleased = false;
        }

    }

    private void OnJumpUp()
    {
        if (PlayerRB.velocity.y > 0)
        {
            PlayerRB.AddForce(Vector2.down * PlayerRB.velocity.y * (1 - JumpCutMultiplier), ForceMode2D.Impulse);
        }

        JumpInputReleased = true;
        LastJumpTime = 0;
    }
    
    private void Dash(float x)
    {
        PlayerRB.AddForce(20f * new Vector2(x, 0), ForceMode2D.Impulse);
    }
    
    void DamagePlayer(int DamageAmount)
    {
        gameObject.GetComponent<PlayerHealth>().Damage(DamageAmount);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GroundCheckPoint.position, GroundCheckRaduis);
    }
}
