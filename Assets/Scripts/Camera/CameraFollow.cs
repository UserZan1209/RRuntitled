using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject playerObject;

    [Header("Modifyers")]
    [SerializeField] private float followSpeed;

    [SerializeField] private bool canFollow = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        if(playerObject == null) { FindPlayer(); }
    }

    private void Update()
    {
        if(cam != null)
        {
            Transform playerTransform = playerObject.transform;

            Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            float magnitude = Mathf.Clamp01(targetPosition.magnitude);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
        }
    }

    //A recursive loop to make sure the player is always being searched for if the player object variable is null
    private void FindPlayer()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if(playerObject != null)
        {
            return;
        }
        else
        {
            FindPlayer();
        }
    }

}
