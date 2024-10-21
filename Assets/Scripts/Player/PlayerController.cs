using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 spawnPoint;

    [SerializeField] public Character chr;
    [SerializeField] private Stats stats;

    [SerializeField] private Animator animator;

    [SerializeField] private float xInput;
    [SerializeField] private float yInput;
    [SerializeField] private float rayDistance;

    [SerializeField] private RaycastHit2D ray;

    [HideInInspector] private Vector2 mouseWorldPos;
    [SerializeField] private GameObject LockOnObject;
    [SerializeField] private GameObject CursorPoint;
    [SerializeField] private GameObject CursorRef;

    [SerializeField] private GameObject slashVFX;

    [SerializeField] private bool useDebugInputs;
    [SerializeField] private bool useTargetedCombat = false;

    [SerializeField] private GameObject activeTarget;

    [SerializeField] public Inventory inventory;

    private void Start()
    {
        InitalizePlayer();     
    }

    private void Update()
    {
        GetInput();

        if (useTargetedCombat) { LockOn(); }


        if (stats.Stamina < stats.mStamina && stats.staminaDelay <= 0)
        {
            chr.RegenerateStamina();
            PlayerUserInterFace.instance.UpdateStaminaUI(stats.Stamina);
        }
        else
        {
            PlayerUserInterFace.instance.UpdateStaminaUI(stats.Stamina);
            chr.StaminaDelay();
        }

    }

    private void LockOn()
    {
        if (activeTarget != null)
        {
            LockOnObject.GetComponent<SpriteRenderer>().enabled = true;

            Vector2 targetPos = new Vector2(activeTarget.transform.position.x, activeTarget.transform.position.y-0.7f);
            LockOnObject.transform.position = targetPos;
            CameraFollow.instance.ChangeFollowObject(activeTarget);
        }
        else
        {
            LockOnObject.GetComponent<SpriteRenderer>().enabled = false;
            LockOnObject.transform.position = transform.position;
            CameraFollow.instance.ChangeFollowObject(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    //Stores all components 
    private void InitalizePlayer()
    {
        chr = new Character(1);
        chr.Sprite = gameObject; // change to body (Need Assets)
        chr.Renderer = GetComponent<SpriteRenderer>();

        stats = chr.myStats;
        //stats.LogStats();   

        chr.myRB = GetComponent<Rigidbody2D>();
        chr.PC2Dcollider = GetComponent<PolygonCollider2D>();
        chr.C2Dcollider = GetComponent<CircleCollider2D>();

        PlayerUserInterFace.instance.init(chr);

        animator = GetComponent<Animator>();
        chr.anim = animator;

        CursorRef = Instantiate(CursorPoint, transform.position, Quaternion.identity);
        inventory.ClearInventory();

    }

    #region inputs
    private void GetInput()
    {
        AxisInput();
        CheckStatus();

        if (Input.GetKeyUp(KeyCode.Mouse0) && stats.Stamina != 0)
        {
            Interact();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1) && stats.Stamina != 0)
        {
            Parry();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            useTargetedCombat = !useTargetedCombat;
            if (!useTargetedCombat) 
            {
                LockOnObject.GetComponent<SpriteRenderer>().enabled = false;
                LockOnObject.transform.position = transform.position;
                CameraFollow.instance.ChangeFollowObject(gameObject); 
            }
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            chr.ToggleRun();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Roll();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //run
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            InventoryController.Instance.ToggleInventory();
        }

        if (chr.myStats.IFrameTimer > 0)
        {
            chr.IFrameCountdown();
        }

        DebugInputs(useDebugInputs);
    }

    private void AxisInput()
    {
        xInput = Input.GetAxis("Horizontal");
        animator.SetFloat("xInput", xInput);
        yInput = Input.GetAxis("Vertical");
        animator.SetFloat("yInput", yInput);
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //moves cursor guide
        
    }

    #region debug-inputs
    private void DebugInputs(bool active)
    {
        /*
         For debug purposes for testing that needs additonal function
         */
        if (!active) { return; }

        if (Input.GetKeyUp(KeyCode.J))
        {
            GainExp(2);
        }

        chr.myStats.isInvincible = true;
    }
    
    #endregion

    #endregion

    private void CheckStatus()
    {
        #region update-UI
        DebugCanvasController.Instance.SetHealthOnUI(stats.Health);
        #endregion
    }

    #region movement-rotation
    private void Movement()
    {
        CursorRef.transform.transform.position = mouseWorldPos;
        #region movement
        //create a vector using the WASD or --> keys to be used for movement and some rotation
        Vector3 movementVector = new Vector3(xInput, 0.0f, yInput);

        if (movementVector == Vector3.zero) 
        {
            animator.SetFloat("healthVal", Mathf.Clamp(chr.myStats.Health, 0, 1));
            animator.SetBool("isMoving", false); 

        }
        else 
        {
            animator.SetBool("isMoving", true); 
        }

        Vector2 mV = new Vector2(xInput, yInput);
        //gets the length of the vector to give movement direction
        float magnitude = Mathf.Clamp01(mV.magnitude);
        mV.Normalize();

        //Moves player
        if (chr.aliveState == AliveState.Alive)
        {
            if (chr.myStats.isRunning)
            {
                transform.Translate(mV * chr.myStats.moveSpeed*2.5f * magnitude * Time.deltaTime, Space.World);
            }
            else
            {
                transform.Translate(mV * chr.myStats.moveSpeed * magnitude * Time.deltaTime, Space.World);
            }
            
        }
        
        #endregion
    }

    private void Parry()
    {
        //needs a cooldown
        animator.SetTrigger("roll");
        chr.UseIFrames();
        
    }

    private void Roll()
    {
        if(chr.myStats.Stamina > 0)
        {
            animator.SetTrigger("roll");
            chr.UseIFrames();
            chr.UseStamina(chr.myStats.mStamina/4);
        }
        else
        {
            Debug.Log("No stamina");
        }


    }

    #endregion

    private void Interact()
    {
        //Check click location
        #region cast-ray
        //Gets a 2D positon based on where the mouse is when interacting
        Vector2 targetPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        ray = Physics2D.Raycast(transform.position, targetPos);

        //prevent going further if nothing is detected
        if (ray.collider == null)
        {
            return;
        }
        GameObject hit = ray.collider.gameObject;
        #endregion

        if (hit != null)
        {
            #region distance-check
            //Prevents further code if the selected position is to far
            float distance = Vector2.Distance( transform.position, mouseWorldPos);
            if(distance > 3f) 
            {
                Debug.Log("Too far");
                return; 
            }
            #endregion

            #region check-for-object-tags
            switch (hit.tag)
            {
                case "Enemy":
                    Debug.Log("Hit registered");
                    activeTarget = hit.gameObject;
                    if (hit.GetComponent<EnemyController>() != null)
                    {
                        hit.GetComponent<EnemyController>().TakeDamage(stats.baseAttack);
                    }
                    else
                    {
                        hit.GetComponent<BossController>().TakeDamage(stats.baseAttack);
                    }

                    UseStamina(stats.StaminaUseage);
                    Instantiate(slashVFX, transform.position, Quaternion.identity);
                    break;
                case "NPC":
                    DialoogueSystem.instance.OpenDialogueCanvas();
                    break;
                default:
                    Debug.Log("Nothing Important Selected");
                    break;
            }
            #endregion
        }
    }

    #region stat-changes
    public void TakeDamage()
    {
        //checks if already dead before damaging
        if (chr.aliveState != AliveState.Dead)
        {
            PlayerUserInterFace.instance.UpdateHealthUI(stats.Health);
        }
        else
        {
            PlayerUserInterFace.instance.UpdateHealthUI(0.0f);
            DeathScreen.Instance.OpenDeathMenu(gameObject);
        }
    }

    private void UseStamina(float staminaUse)
    {
        stats.Stamina -= staminaUse;
        if(stats.Stamina < 0) { stats.Stamina = 0; }
        PlayerUserInterFace.instance.UpdateStaminaUI(stats.Stamina);
    }

    public void GainExp(float experience)
    {
        stats.AddExp(experience);
        PlayerUserInterFace.instance.UpdateRequiredExperience(stats.CalculateRequiredExperience());

        if (stats.experiencePoints >= stats.requiredExperiencePoints)
        {
            stats.Level++;
            PlayerUserInterFace.instance.UpdateLevelUI(stats.Level);
            stats.CalculateStats(stats.Level);
        }

        PlayerUserInterFace.instance.UpdateEXPUI(stats.experiencePoints);
    }
    #endregion

    public void Respawn()
    {
        transform.position = spawnPoint;
        chr.Revive();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Bonfire":
                spawnPoint = collision.gameObject.GetComponent<Bonfires>().GetPosition();
                break;
        }
    }


}
