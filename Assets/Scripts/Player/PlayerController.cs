using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float xInput;
    [SerializeField] private float yInput;

    [SerializeField] private bool useDebugInputs;
    [SerializeField] private bool hasTarget = false;

    //character.cs ->>
    [HideInInspector] private Rigidbody2D myRB;

    private void Start()
    {
        InitalizePlayer();
    }

    private void FixedUpdate()
    {
        GetInput();

        MovementAndRotation();
    }

    //Store all components 
    private void InitalizePlayer()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    #region inputs
    private void GetInput()
    {
        AxisInput();

        DebugInputs(useDebugInputs);
    }

    private void AxisInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    #region debug-inputs
    private void DebugInputs(bool active)
    {
        if (!active) { return; }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            hasTarget = !hasTarget;
        }
    }
    
    #endregion

    #endregion

    #region movement-rotation
    private void MovementAndRotation()
    {
        #region movement
        //create a vector using the WASD or --> keys to be used for movement and some rotation
        Vector3 movementVector = new Vector3(xInput, 0.0f, yInput);
        Vector2 mV = new Vector2(xInput, yInput);
        //gets the length of the vector to give movement direction
        float magnitude = Mathf.Clamp01(mV.magnitude);
        mV.Normalize();

        //Moves player
        transform.Translate(mV * moveSpeed * magnitude * Time.deltaTime, Space.World);
        #endregion

        if (!hasTarget)
        {
            /*
             =Normal Movement=

            -uses the movement vector to calculate the target rotation while moving, using the transforms forward as the start point

             */

            if (movementVector == Vector3.zero) { return; }

            //Rotate toward target
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, mV);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }
        else
        {
            /*
             =Targeted Movement=

            -Uses the mouse position relative to the world to determine where to look while moving

            - [TODO] When an enemy is attacked set use the enemy as the target to look at

            */


            //convert mouse position to the world position
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            //Rotate toward target
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, mouseWorldPosition);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, (rotationSpeed * 2.0f) * Time.deltaTime);

        }
    }
    #endregion
}
