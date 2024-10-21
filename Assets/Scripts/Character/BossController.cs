using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private string bossName;
    [SerializeField] private CircleCollider2D circleTrigger;

    [HideInInspector] public Character chr;
    [SerializeField] private Stats stats;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject player;
    [SerializeField] private Character playersChr;
    [SerializeField] private Animator anim;

    [SerializeField] private float experienceYeild;

    // Start is called before the first frame update
    void Start()
    {
        chr = new Character(15);
        chr.Renderer = GetComponent<SpriteRenderer>();
        chr.PC2Dcollider = GetComponent<PolygonCollider2D>();
        chr.C2Dcollider = GetComponent<CircleCollider2D>();

        stats = chr.myStats;
        experienceYeild = stats.Level * 10f;

        FindPlayer();
        playersChr = player.GetComponent<PlayerController>().chr;
        anim = GetComponent<Animator>();
        chr.anim = anim;
        chr.canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("healthVal", Mathf.Clamp(chr.myStats.Health, 0, 1));
        if (chr.myStats.attackDelay > 0)
        {
            chr.AttackDelay();
        }

        if (player != null && chr.aliveState == AliveState.Alive)
        {
            if (chr.canMove)
            {
                MoveEnemy();
            }

        }
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            FindPlayer();
        }
    }


    private void MoveEnemy()
    {
        float d = Vector3.Distance(transform.position, player.transform.position);

        float x = player.transform.position.x - transform.position.x;
        float y = player.transform.position.y - transform.position.y;
        Vector2 playerDirection = new Vector2(x, y);

        if (playerDirection == Vector2.zero || d < 2)
        {
            anim.SetBool("isMoving", false);
            if (chr.myStats.attackDelay <= 0)
            {
                chr.Attack(player.GetComponent<PlayerController>().chr);
                player.GetComponent<PlayerController>().TakeDamage();
            }


        }
        else
        {
            anim.SetFloat("xInput", playerDirection.x);
            anim.SetFloat("yInput", playerDirection.y);
            anim.SetBool("isMoving", true);
        }

        if (chr.aliveState == AliveState.Alive && CheckRange() > 1.5f)
        {
            Vector2 translateVector = player.transform.position - transform.position;
            float magnitude = Mathf.Clamp01(translateVector.magnitude);
            translateVector.Normalize();

            if (stats.isRunning)
            {
                transform.Translate(stats.moveSpeed * 2.5f * Time.deltaTime * translateVector * magnitude);
            }
            else
            {
                transform.Translate(stats.moveSpeed * Time.deltaTime * translateVector * magnitude);
            }

        }
    }

    private float CheckRange()
    {
        float r = 0;

        if (player != null)
        {
            r = Vector3.Distance(transform.position, player.transform.position);
        }

        return r;
    }

    public void TakeDamage(float dm)
    {
        if (chr.aliveState == AliveState.Alive)
        {
            chr.ChangeHealthValue(-dm);

            if (chr.myStats.Health <= 0)
            {
                player.GetComponent<PlayerController>().GainExp(experienceYeild);
                anim.SetTrigger("death");
                chr.KillCharacter();
                chr.DisableColliders();
            }
            else
            {
                //Animate damage taken
            }
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") 
        {
            BossInterface.Instance.ActivateFight(bossName, this);
            chr.canMove = true;
            circleTrigger.enabled = false;
        }
    }
}
