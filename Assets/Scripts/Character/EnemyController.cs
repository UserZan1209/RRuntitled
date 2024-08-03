using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool canMove = false;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private GameObject player;

    private void Update()
    {
        if(player != null)
        {
            //prevents further code execution if the players position results in a distance of 30 or greater
            if(CheckRange() >= 30) { player = null; }

            if (canMove)
            {
                Vector2 movementVector = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);

                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, -movementVector);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                
                /*
                 [ToDo]

                +movement
                +attack
                 */
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.gameObject;
            canMove = true;
        }
    }

    private float CheckRange()
    {
        float r = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log("Range: " + r.ToString());
        return r;
    }

    public void TakeDamage()
    {
        Debug.Log("Damaged");
        Destroy(gameObject);
    }

}
