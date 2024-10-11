using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [HideInInspector] public static CameraFollow instance;

    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject followObject;

    [Header("Modifyers")]
    [SerializeField] private float followSpeed;

    [SerializeField] private bool canFollow = false;

    private void Awake()
    {
        instance = this;
        cam = GetComponent<Camera>();

        if(followObject == null) { FindPlayer(); }
    }

    private void Update()
    {
        if(cam != null)
        {
            Transform targetTransform = followObject.transform;

            Vector3 targetPosition = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
            float magnitude = Mathf.Clamp01(targetPosition.magnitude);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
        }
    }

    public void ChangeFollowObject(GameObject obj)
    {
        followObject = obj;
    }

    //A recursive loop to make sure the player is always being searched for if the player object variable is null
    private void FindPlayer()
    {
        followObject = GameObject.FindGameObjectWithTag("Player");
        
        if(followObject != null)
        {
            return;
        }
        else
        {
            FindPlayer();
        }
    }

}
