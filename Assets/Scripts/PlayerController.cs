using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    [SerializeField]
    HealthBar healthBar;

    public bool IsGuarding;

    public bool Stunned;

    float stunTimer;

    [SerializeField]
    float basedStunTimer;

    [SerializeField]
    GameObject enemy;

    Rigidbody2D myRigidbody;
    Animator anim;

    [SerializeField]
    Transform[] groundPoints;

    [SerializeField]
    float groundRadius;

    [SerializeField]
    LayerMask whatIsGround;

    bool isGrounded;

    bool jump;

    bool guarding;

    bool throwing;

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
    KeyCode kickKey;

    [SerializeField]
    KeyCode throwKey;

    [SerializeField]
    KeyCode hadoukenKey;

    [SerializeField]
    KeyCode dashKey;

    [SerializeField]
    KeyCode punchKey;

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

    BoxCollider2D myCollider;

    [SerializeField]
    Color myColliderColor;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        CurrentHealth = MaximumHealth;
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

        // DOUBLE TAP CODE
        /*if(Input.KeyDown())
        {
            if (buttonCooldown > 0 && buttonCount == 1Number of Taps you want Minus One)
            {
                Debug.Log("Double Tapped");//Has double tapped
            }
            else
            {
                buttonCooldown = dashTimerCooldown;
                buttonCount += 1;
            }
        }

        if (buttonCooldown > 0)
        {
            buttonCooldown -= 1 * Time.deltaTime;
        }
        else
        {
            buttonCount = 0;
        }*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsDead())
        {
            healthBar.SetSize(CurrentHealth / MaximumHealth);
            return;
        }
        isGrounded = IsGrounded();

        if (CheckStun())
        {
            return;
        }

        HandleMovement();

        FlipPlayer();

        //Handle Attacks

        ResetValues();

        healthBar.SetSize(CurrentHealth / MaximumHealth);
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

    void HandleInput()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            jump = true;
        }
        if (Input.GetKey(guardKey))
        {
            guarding = true;
        }
        if (Input.GetKeyDown(throwKey))
        {
            throwing = true;
        }
        if (Input.GetKeyDown(hadoukenKey))
        {
            hadouken = true;
        }
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            dash = true;
        }
    }

    bool CheckStun()
    {
        if (Stunned && stunTimer < basedStunTimer)
        {
            stunTimer += Time.deltaTime;
            if (anim.GetBool("stunned") == false)
            {
                anim.SetBool("stunned", true);
            }

            return true;
        }
        else if (Stunned && stunTimer >= basedStunTimer)
        {
            stunTimer = 0f;
            Stunned = false;
            anim.SetBool("stunned", false);
            return false;
        }
        else
        {
            stunTimer = 0f;
            anim.SetBool("stunned", false);
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
        if (hadouken && isGrounded)
        {
            Instantiate(hadoukenObject, new Vector2(transform.position.x + 2f, transform.position.y), Quaternion.identity);
            return;
        }
        if (throwing && isGrounded)
        {
            anim.SetTrigger("throw");
        }
        if (guarding && isGrounded)
        {
            anim.SetBool("isGuarding", true);
            IsGuarding = true;
            anim.SetBool("isWalking", false); //Deal with the stutter when you walk and guard
            return;
        }
        else
        {
            anim.SetBool("isGuarding", false);
            IsGuarding = false;
        }
        if (isGrounded || airControl)
        {
           dirX = Input.GetAxisRaw(horizontalAxis) * moveSpeed * Time.deltaTime;
        }

        if (dash && isGrounded)
        {
            Instantiate(dashCloud, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity);

            if (dirX < 0)
            {
                transform.position = new Vector2(transform.position.x + dirX - 3, transform.position.y);
            }
            else if (dirX > 0)
            {
                transform.position = new Vector2(transform.position.x + dirX + 3, transform.position.y);
            }
        }
        else
        {
            transform.position = new Vector2(transform.position.x + dirX, transform.position.y);
        }

        if (dirX != 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("MiddleKick"))
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if (Input.GetKeyDown(kickKey) && !anim.GetCurrentAnimatorStateInfo(0).IsName("MiddleKick"))
        {
            anim.SetBool("isWalking", false);
            anim.SetTrigger("hit");
        }

        if (Input.GetKeyDown(punchKey) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {
            anim.SetBool("isWalking", false);
            anim.SetTrigger("punch");
        }

        if (isGrounded && jump)
        {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
        }
    }

    void ResetValues()
    {
        jump = false;
        guarding = false;
        throwing = false;
        hadouken = false;
        dash = false;
    }

    bool IsGrounded()
    {
        if(myRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for(int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void TakeDamage(float damageValue)
    {
        CurrentHealth -= damageValue;

        healthBar.SetSize(CurrentHealth / MaximumHealth);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = myColliderColor;
        //Gizmos.matrix = Matrix4x4.TRS(transform.parent.position, transform.parent.transform.rotation, transform.parent.transform.localScale);
        Gizmos.DrawCube(myCollider.bounds.center, new Vector3(myCollider.bounds.extents.x * 2, myCollider.bounds.extents.y * 2, myCollider.bounds.extents.z * 2)); // Because size is halfExtents
        //Gizmos.DrawWireCube(hurtBoxCollider.bounds.center, new Vector3(hurtBoxCollider.bounds.extents.x * 2, hurtBoxCollider.bounds.extents.y * 2, hurtBoxCollider.bounds.extents.z * 2)); // Because size is halfExtents
    }
}