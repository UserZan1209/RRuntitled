using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [HideInInspector] public Character chr;
    [SerializeField] private Stats stats;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject player;
    [SerializeField] private Character playersChr;
    [SerializeField] private Animator anim;
    [SerializeField] private float experienceYeild;

    private void Awake()
    {
        int r = Random.Range(1, 6);
        chr = new Character(r);
        chr.Renderer = GetComponent<SpriteRenderer>();
        chr.PC2Dcollider = GetComponent<PolygonCollider2D>();
        chr.C2Dcollider = GetComponent<CircleCollider2D>();

        stats = chr.myStats;
        stats.moveSpeed = 4.0f;

        experienceYeild = stats.Level * 10f;

        FindPlayer();
        playersChr = player.GetComponent<PlayerController>().chr;
        anim = GetComponent<Animator>();
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            FindPlayer();
        }
    }

    private void LookAtPlayer()
    {
        float d = Vector3.Distance(transform.position, player.transform.position);

        float x = player.transform.position.x - transform.position.x;
        float y = player.transform.position.y - transform.position.y;
        Vector2 playerDirection = new Vector2(x, y);

        anim.SetFloat("xInput", playerDirection.x);
        anim.SetFloat("yInput", playerDirection.y);
        anim.SetBool("isMoving", true);


    }

    private void Update()
    {
        LookAtPlayer();
    }

}
