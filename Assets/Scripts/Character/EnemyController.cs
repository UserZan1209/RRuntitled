using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector] private Character chr;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        chr = new Character();
        chr.moveSpeed = 4.0f;

    
    }

    private void Update()
    {
        if(player != null && chr.canMove)
        {
            Vector2 movementVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            RotateEnemy(movementVector);

            /*            if(CheckRange() > 2)
                        {

                        }*/

            Vector2 translateVector = new Vector3(player.transform.position.x, transform.position.y);
            float magnitude = Mathf.Clamp01(translateVector.magnitude);
            translateVector.Normalize();
            
            transform.Translate(chr.moveSpeed * Time.deltaTime * translateVector * magnitude * Vector2.up);

            /*
             [ToDo]

            +movement
            +attack
             */
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.gameObject;
            chr.canMove = true;
        }
    }

    public void RotateEnemy(Vector2 mv)
    {

        //prevents further code execution if the players position results in a distance of 30 or greater
        if (CheckRange() >= 30) { player = null; }

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, mv);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    private float CheckRange()
    {
        float r = 999;

        if(player != null)
        {
            r = Vector3.Distance(transform.position, player.transform.position);
            //Debug.Log("Range: " + r.ToString());
        }

        return r; 
    }

    public void TakeDamage(float dm)
    {
        chr.ChangeHealthValue(dm);

        Debug.Log("Damaged");

        if (!chr.isAlive)
        {
            Destroy(gameObject);
        }
        else
        {
            // React
        }


    }

}
