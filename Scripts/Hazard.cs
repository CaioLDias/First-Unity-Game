using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    Vector3 rotation;
    
    [SerializeField]
    private ParticleSystem breakingEffect;
    
    private void Start()
    {
        var xRotation = Random.Range(90f, 180f);
        rotation = new Vector3(-xRotation,0);
    }
    
    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }
    
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Hazard"))
        {
            Destroy(gameObject);
            Instantiate(breakingEffect, transform.position, Quaternion.identity);
        }  
    }
}
