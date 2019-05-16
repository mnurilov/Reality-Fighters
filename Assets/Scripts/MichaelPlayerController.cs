using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichaelPlayerController : MonoBehaviour
{
    [SerializeField]
    FightingCamera fightingCamera;

    private float currentHealth;

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value <= 0)
            {
                currentHealth = 0;
            }
            else if (value >= MaximumHealth)
            {
                currentHealth = MaximumHealth;
            }
            else
            {
                currentHealth = value;
            }
        }
    }

    public float MaximumHealth;

    public SimpleHealthBar healthBar;

    public bool IsGuarding;

    public bool Stunned;

    float stunTimer;

    [SerializeField]
    float basedStunTimer;

    [SerializeField]
    GameObject enemy;

    public MichaelPlayerController enemyScript;

    Rigidbody2D myRigidbody;
    
    public Animator anim;

    [SerializeField]
    Transform[] groundPoints;

    [SerializeField]
    float groundRadius;

    [SerializeField]
    LayerMask whatIsGround;

    bool isGrounded;

    bool jump;

    bool guard;

    bool throwing;

    bool light;

    bool medium;

    bool heavy;

    bool hadouken;

    bool dash;

    [SerializeField]
    bool airControl;

    [SerializeField]
    float jumpForce;

    float dirX;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    KeyCode jumpKey;

    [SerializeField]
    KeyCode guardKey;

    [SerializeField]
    KeyCode lightKey;

    [SerializeField]
    KeyCode mediumKey;

    [SerializeField]
    KeyCode heavyKey;

    [SerializeField]
    KeyCode hadoukenKey;

    [SerializeField]
    KeyCode throwKey;

    [SerializeField]
    KeyCode dashKey;

    [SerializeField]
    string horizontalAxis;

    [SerializeField]
    GameObject hadoukenObject;

    [SerializeField]
    GameObject dashCloud;

    /*[SerializeField]
    float dashTimerCooldown; // Half a second before reset

    float buttonCooldown;

    int buttonCount;*/

    float dashTimer;

    [SerializeField]
    float baseDashTimer;

    bool canDash;

    [SerializeField]
    BoxCollider2D myCollider;

    [SerializeField]
    Color myColliderColor;

    public bool control;

    AudioSource audioSource;

    public void PlaySound(string sound)
    {
        AudioClip audioClip;
        
        audioClip = Resources.Load("Sounds/" + sound) as AudioClip;

        audioSource.PlayOneShot(audioClip, 1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //myCollider = GetComponent<BoxCollider2D>();
        CurrentHealth = MaximumHealth;
        dashTimer = 0f;

        // Make sure the player can move when starting the game
        control = true;
    }

    void Update()
    {
        if (CheckDash())
        {
            canDash = true;
        }
        else
        {
            canDash = false;
        }
        HandleInput();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsDead())
        {
            healthBar.UpdateBar(CurrentHealth, MaximumHealth);
            return;
        }
        isGrounded = IsGrounded();

        if (!control)
        {
            return;
        }

        if (CheckStun())
        {
            return;
        }

        HandleMovement();

        FlipPlayer();

        //Handle Attacks

        ResetValues();
        
        healthBar.UpdateBar(CurrentHealth, MaximumHealth);
    }

    public void ActivateControls()
    {
        control = true;
    }

    public void DisableControls()
    {
        control = false;
    }

    bool IsDead()
    {
        if (CurrentHealth <= 0)
        {
            anim.SetBool("dead", true);
            return true;
        }
        anim.SetBool("dead", false);
        return false;
    }

    public bool HoldingGuard()
    {
        if (Input.GetKey(guardKey))
        {
            return true;
        }
        return false;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            jump = true;
        }
        if (Input.GetKey(guardKey))
        {
            guard = true;
        }
        if (Input.GetKeyDown(lightKey))
        {
            light = true;
        }
        if (Input.GetKeyDown(mediumKey))
        {
            medium = true;
        }
        if (Input.GetKeyDown(heavyKey))
        {
            heavy = true;
        }
        if (Input.GetKeyDown(hadoukenKey))
        {
            hadouken = true;
        }
        if (Input.GetKeyDown(throwKey))
        {
            throwing = true;
        }
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            dash = true;
            canDash = false;
        }
    }

    public void StunPlayer(float time)
    {
        stunTimer = 0f;
        basedStunTimer = time;
        Stunned = true;
       // anim.SetBool("stunned", true);
    }

    bool CheckStun()
    {
        if (Stunned && stunTimer < basedStunTimer)
        {
            stunTimer += Time.deltaTime;
            if (anim.GetBool("stunned") == false)
            {
                //anim.SetBool("stunned", true);
            }

            return true;
        }
        else if (Stunned && stunTimer >= basedStunTimer)
        {
            stunTimer = 0f;
            Stunned = false;
            //anim.SetBool("stunned", false);
            return false;
        }
        else
        {
            stunTimer = 0f;
           // anim.SetBool("stunned", false);
            return false;
        }
    }

    bool CheckDash()
    {
        if (!canDash && dashTimer < baseDashTimer)
        {
            dashTimer += Time.deltaTime;

            return false;
        }
        else
        {
            dashTimer = 0f;

            return true;
        }
    }

    //Makes sure the players are facing each other at all times
    void FlipPlayer()
    {
        Vector2 localPlayerScale = gameObject.transform.localScale;
        Vector2 localEnemyScale = enemy.gameObject.transform.localScale;

        //Turn left
        if (transform.position.x < enemy.transform.position.x)
        {
            //Debug.Log(gameObject.name + "is to the left of" + enemy.gameObject.name);
            localPlayerScale.x = Mathf.Abs(localPlayerScale.x);
            localEnemyScale.x = Mathf.Abs(localEnemyScale.x);

            localEnemyScale.x *= -1;

            transform.localScale = localPlayerScale;
            enemy.transform.localScale = localEnemyScale;
        }
        //Turn right
        else if (transform.position.x >= enemy.transform.position.x)
        {
            //Debug.Log(gameObject.name + "is to the right of" + enemy.gameObject.name);
            localPlayerScale.x = Mathf.Abs(localPlayerScale.x);
            localEnemyScale.x = Mathf.Abs(localEnemyScale.x);

            localPlayerScale.x *= -1;

            transform.localScale = localPlayerScale;
            enemy.transform.localScale = localEnemyScale;
        }
    }

    void HandleMovement()
    {
        if (jump && isGrounded)
        {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            anim.SetBool("jump", true);
            return;
        }
        if (light && isGrounded)
        {
            anim.SetTrigger("light");
            return;
        }
        if (medium && isGrounded)
        {
            anim.SetTrigger("medium");
            return;
        }
        if (heavy && isGrounded)
        {
            anim.SetTrigger("heavy");
            return;
        }
        if (hadouken && isGrounded)
        {
            anim.SetTrigger("hadouken");
            return;
        }
        if (throwing && isGrounded)
        {
            anim.SetTrigger("throw");
            return;
        }

        if (guard && isGrounded)
        {
            anim.SetBool("guard", true);
            IsGuarding = true;
            anim.SetBool("walk", false); //Deal with the stutter when you walk and guard
            return;
        }
        else
        {
            anim.SetBool("guard", false);
            IsGuarding = false;
        }
        if (isGrounded || airControl)
        {
            if (fightingCamera.MaximumDistance)
            {
                myRigidbody.velocity = new Vector2(0, 0);

                if (transform.localScale.x * Input.GetAxisRaw(horizontalAxis) < 0)
                {
                    dirX = 0;
                    myRigidbody.velocity = new Vector2(0, 0);
                }
                else if(myRigidbody.velocity.x > 0 && transform.localScale.x > 0)
                {
                    dirX = 0;
                    myRigidbody.velocity = new Vector2(0, 0);
                }
                else if (myRigidbody.velocity.x < 0 && transform.localScale.x < 0)
                {
                    dirX = 0;
                }
                else
                {
                    dirX = Input.GetAxisRaw(horizontalAxis) * moveSpeed * Time.deltaTime;
                }
            }
            else
            {
                dirX = Input.GetAxisRaw(horizontalAxis) * moveSpeed * Time.deltaTime;
            }
        }

        if (dash && isGrounded)
        {
            Instantiate(dashCloud, new Vector2(transform.position.x, transform.position.y - 2f), Quaternion.identity);

            if (dirX > 0 && transform.localScale.x > 0)
            {
                PushForwardForce(700);
                anim.SetTrigger("forwarddash");
            }
            else if (dirX < 0 && transform.localScale.x > 0)
            {
                PushBackForce(700);
                anim.SetTrigger("backwarddash");
            }
            else if (dirX > 0 && transform.localScale.x < 0)
            {
                PushBackForce(700);
                anim.SetTrigger("backwarddash");
            }
            else if (dirX < 0 && transform.localScale.x < 0)
            {
                PushForwardForce(700);
                anim.SetTrigger("forwarddash");
            }

            return;
        }
        else if (dirX != 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Hadouken"))
        {
            transform.position = new Vector2(transform.position.x + dirX, transform.position.y);
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }

       /* if (dirX != 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }*/

        if (!jump && isGrounded)
        {
            anim.SetBool("jump", false);
            return;
        }
    }

    void ResetValues()
    {
        jump = false;
        guard = false;
        throwing = false;
        light = false;
        medium = false;
        heavy = false;
        hadouken = false;
        dash = false;
    }

    bool IsGrounded()
    {
        if (myRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void ShootHadouken()
    {
        GameObject generatedHadouken;
        if (transform.localScale.x > 0)
        {
            generatedHadouken = Instantiate(hadoukenObject, new Vector2(transform.position.x + 3f, transform.position.y + 1f), Quaternion.identity);
            generatedHadouken.GetComponent<MoveForward>().IsGoingRight = true;
        }
        else
        {
            generatedHadouken = Instantiate(hadoukenObject, new Vector2(transform.position.x - 3f, transform.position.y + 1f), Quaternion.identity);
            generatedHadouken.GetComponent<MoveForward>().IsGoingRight = false;
        }
    }

    public void MediumForce(float force)
    {
        if (transform.localScale.x > 0)
        {
            myRigidbody.AddForce(new Vector2(force, 0));
        }
        else
        {
            myRigidbody.AddForce(new Vector2(-force, 0));
        }
    }

    public void ShoryukenForce(float force)
    {
        myRigidbody.AddForce(new Vector2(0, force));
    }

    public void KnockDownForce(float xForce)
    {
        if (transform.localScale.x > 0)
        {
            myRigidbody.AddForce(new Vector2(-xForce, 1500));
        }
        else
        {
            myRigidbody.AddForce(new Vector2(xForce, 1500));
        }
    }

    public void PushBackForce(float force)
    {
        if (transform.localScale.x > 0)
        {
            myRigidbody.AddForce(new Vector2(-force, 0));
        }
        else
        {
            myRigidbody.AddForce(new Vector2(force, 0));
        }
    }

    public void PushForwardForce(float force)
    {
        if (transform.localScale.x > 0)
        {
            myRigidbody.AddForce(new Vector2(force, 0));
        }
        else
        {
            myRigidbody.AddForce(new Vector2(-force, 0));
        }
    }

    

    public void TakeDamage(float damageValue)
    {
        CurrentHealth -= damageValue;

        //healthBar.SetSize(CurrentHealth / MaximumHealth);
        healthBar.UpdateBar(CurrentHealth, MaximumHealth);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = myColliderColor;
        //Gizmos.matrix = Matrix4x4.TRS(transform.parent.position, transform.parent.transform.rotation, transform.parent.transform.localScale);
        Gizmos.DrawCube(myCollider.bounds.center, new Vector3(myCollider.bounds.extents.x * 2, myCollider.bounds.extents.y * 2, myCollider.bounds.extents.z * 2)); // Because size is halfExtents
        //Gizmos.DrawWireCube(hurtBoxCollider.bounds.center, new Vector3(hurtBoxCollider.bounds.extents.x * 2, hurtBoxCollider.bounds.extents.y * 2, hurtBoxCollider.bounds.extents.z * 2)); // Because size is halfExtents
    }
}
