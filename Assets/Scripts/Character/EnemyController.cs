using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class EnemyController : MonoBehaviour
{
    [HideInInspector] public Character chr;
    [SerializeField] private Stats stats;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject player;
    [SerializeField] private Character playersChr;
    [SerializeField] private Animator anim;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private float experienceYeild;

    private void Awake()
    {
        int r = Random.Range(1, 6);
        chr = new Character(r);
        chr.Renderer = GetComponent<SpriteRenderer>();
        stats = chr.myStats;
        stats.moveSpeed = 4.0f;

        levelText.text = stats.Level.ToString();
        healthText.text = stats.Health.ToString();

        healthSlider.maxValue = stats.Health;

        experienceYeild = stats.Level * 2.5f;

        FindPlayer();
        playersChr = player.GetComponent<PlayerController>().chr;
        anim = GetComponent<Animator>();
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if(player == null)
        {
            FindPlayer();
        }
    }

    private void Update()
    {
        anim.SetFloat("healthVal", Mathf.Clamp(chr.myStats.Health, 0, 1));
        if(chr.myStats.attackDelay > 0)
        {
            chr.AttackDelay();
        }

        if (player != null)
        {
            MoveEnemy();
        }
    }

    private void MoveEnemy()
    {
        float d = Vector3.Distance(transform.position, player.transform.position);

        float x = player.transform.position.x - transform.position.x;
        float y = player.transform.position.y - transform.position.y;
        Vector2 playerDirection = new Vector2(x, y);

        if(playerDirection == Vector2.zero || d < 2)
        {
            anim.SetBool("isMoving", false);
            if(chr.myStats.attackDelay <= 0)
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

            transform.Translate(stats.moveSpeed * Time.deltaTime * translateVector * magnitude);
        }
    }

    private float CheckRange()
    {
        float r = 0;

        if(player != null)
        {
            r = Vector3.Distance(transform.position, player.transform.position);
        }

        return r; 
    }

    public void TakeDamage(float dm)
    {
        if(chr.aliveState == AliveState.Alive)
        {
            chr.ChangeHealthValue(-dm);
            healthSlider.value = chr.myStats.Health;
            healthText.text = chr.myStats.Health.ToString();

            if (chr.myStats.Health <= 0)
            {
                player.GetComponent<PlayerController>().GainExp(experienceYeild);
                anim.SetTrigger("death");
                chr.KillCharacter();
            }
            else
            {
                //Animate damage taken
            }
        }



    }

    private void LosePlayer()
    {
        anim.SetFloat("healthVal", Mathf.Clamp(chr.myStats.Health, 0, 1));
        chr.canMove = false;
    }

}
