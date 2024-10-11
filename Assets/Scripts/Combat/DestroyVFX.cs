using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVFX : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
