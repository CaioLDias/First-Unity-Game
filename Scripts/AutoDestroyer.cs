using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
    public float delay;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delay);
    }
}
