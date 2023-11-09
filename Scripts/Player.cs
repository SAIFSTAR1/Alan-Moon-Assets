using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
    public float MaxJumps;
    float NumberOfJumps;
    [Header("Gravity")]
    public float GravityScale;
    public float FallGravityMultiplier;
    public float MaxFallSpeed;

    private float MoveInput;

    public Rigidbody2D PlayerRB;
    private SpriteRenderer PlayerSR;

    public Transform GroundCheckPoint;
    public Vector2 GroundCheckRaduis;
    public LayerMask GroundLayer;

    private float LastOnGroundTime;
    private float LastJumpTime;
    private bool Isgrounded;
    private bool isIdle;
    private bool isJumping, isMoving, isFalling, isAttacking, special;
    private bool CanJump;
    private bool TryingToJump;
    private bool JumpInputReleased;
    [HideInInspector] public float direction;

    private Vector3 _mousePos, _reference;

    [SerializeField] private WeaponHolder weaponSystem;
    public Animator animator;
    [HideInInspector]
    public string CurrentState;

    [SerializeField] private Weapon specialWeapon;

    public static class AnimationStates
    {
        public static string
            Idle = "Idle",
            Walk = "Walk",
            Jump = "Jump",
            Fall = "Fall",
            Attack = "Attack",
            Die = "Die",
            DoubleJump = "DoubleJump",
            SpecialAttack = "SpecialAttack";
    }

    public void PlayAnimationState(string state)
    {
        if (state == CurrentState) return;
        animator.Play(state);
        CurrentState = state;
    }

    private void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        PlayerSR = GetComponent<SpriteRenderer>();
        NumberOfJumps = MaxJumps;
    }
    
    private void FixedUpdate()
    {
        IdleCheck();
        GroundCheck();
        AnimatorController();
        Movement();
        CheckMovement();
        Jump();
        FallingPhases();
    }

    private void Update()
    {
        if (Physics2D.OverlapBox(GroundCheckPoint.position, GroundCheckRaduis, 0, GroundLayer) && PlayerRB.velocity.y <= 0)
        {
            LastOnGroundTime = JumpCoyoteTime;
        }
        if (MoveInput != 0)
            direction = MoveInput;
        
        Controls();
        Timers();
        Gravity();
        Flip();
    }

    private void CheckMovement()
    {
        if (MoveInput != 0)
            isMoving = true;
        else
            isMoving = false;
    }

    private void Gravity()
    {
        if (PlayerRB.velocity.y < 0)
        {
            PlayerRB.gravityScale = GravityScale * FallGravityMultiplier;
            isFalling = true;
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, Mathf.Max(PlayerRB.velocity.y, -MaxFallSpeed));
        }
        else
        {
            PlayerRB.gravityScale = GravityScale;
            isFalling = false;
        }
    }

    private void Timers()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastJumpTime -= Time.deltaTime;
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

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SpecialAttack();
        }
    }

    private void Movement()
    {
        var TargetSpeed = MoveInput * Maxspeed;
        var SpeedDif = TargetSpeed - PlayerRB.velocity.x;
        var AccelRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? Acceleration : Decceleration;
        var movement = Mathf.Pow(Mathf.Abs(SpeedDif) * AccelRate, VelPower) * Mathf.Sign(SpeedDif);
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
            NumberOfJumps--;
        }
        else
        {
            if(NumberOfJumps > 0 && LastJumpTime > 0)
            {
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 0);
                PlayerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                LastOnGroundTime = 0;
                LastJumpTime = 0;
                PlayAnimationState(AnimationStates.DoubleJump);
                JumpInputReleased = false;
                NumberOfJumps--;
            }
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
    
    public void DamagePlayer(int damageAmount)
    {
        gameObject.GetComponent<PlayerHealth>().Damage(damageAmount);
        GetHurt();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GroundCheckPoint.position, GroundCheckRaduis);
    }

    private void Flip()
    {
        Quaternion look;
        var reset = Quaternion.Euler(0, 0,0);
        if (direction == 1)
            look = Quaternion.Euler(0, 0, 0);
        else
            look = Quaternion.Euler(0, -180, 0);
        transform.rotation = Quaternion.Slerp(reset, look, 1f);
    }

    private void AnimatorController()
    {
        if (isIdle)
            PlayAnimationState(AnimationStates.Idle);
        if (isMoving && Isgrounded)
            PlayAnimationState(AnimationStates.Walk);
        if (isJumping)
        {
            PlayAnimationState(AnimationStates.Jump);
        }

        if (isAttacking && !isMoving)
            PlayAnimationState(AnimationStates.Attack);
        
        animator.SetBool(AnimationStates.Attack, isAttacking);
    }

    private void FallingPhases()
    {
        if (!isFalling) return;
        isJumping = false;
        var origin = transform.position;
        var dir = new Vector2(0, -1);
        var hit = Physics2D.Raycast(origin, dir, Mathf.Infinity, GroundLayer);

        if (hit.distance > 1f)
        {
            PlayAnimationState(AnimationStates.Fall);
        }
        else if (hit.distance <= 1f)
        {
            animator.SetInteger("Fall", 1);
        }
    }

    private void IdleCheck()
    {
        isIdle = false;
        if (isMoving) return;
        if (isJumping) return;
        if (isFalling) return;
        if (isAttacking) return;
        if (special) return;
        isIdle = true;
    }

    private void GroundCheck()
    {
        var hit = Physics2D.Raycast(GroundCheckPoint.position, new Vector2(0, -1),Mathf.Infinity, GroundLayer);
        Debug.DrawRay(GroundCheckPoint.position, new Vector2(0, -1) * hit.distance, Color.magenta);

        if (hit.distance < 0.01f)
        {
            Isgrounded = true;
            NumberOfJumps = MaxJumps;
        }
        else
            Isgrounded = false;
    }

    private void Attack()
    {
        isAttacking = true;
    }

    private void DamageAttack()
    {
        weaponSystem.currentWeapon.Attack(direction);
    }

    public void StopAttack()
    {
        isAttacking = false;
    }

    private void SpecialAttack()
    {
        special = true;
        PlayAnimationState(AnimationStates.SpecialAttack);
    }

    private void SpecialDamage()
    {
        specialWeapon.Attack(direction);
    }

    private void StopSpecial()
    {
        special = false;
    }

    private async void GetHurt()
    {
        PlayerSR.color = Color.red;
        await Task.Delay(500);
        PlayerSR.color = Color.white;
    }
}
