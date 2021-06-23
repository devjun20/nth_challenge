using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isJump : MonoBehaviour
{
    public bool jumpState = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);



        jumpState = false;

    }
}
