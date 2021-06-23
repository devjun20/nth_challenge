using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    public GameObject goPlayer;
    public float moveHorizontalSpeed = 500.0f;
    public float jumpPower = 20.0f;

    public bool jumpState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        jump(goPlayer, jumpPower);
    }
    private void FixedUpdate()
    {
        moveHorizontal(goPlayer, moveHorizontalSpeed);
    }


    void moveHorizontal(GameObject go, float speed)
    {
        Vector3 velocity = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            velocity = Vector3.left;
            go.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            velocity = Vector3.right;
            go.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        velocity = velocity * Time.deltaTime * speed;
        velocity.y = go.GetComponent<Rigidbody>().velocity.y;
        go.GetComponent<Rigidbody>().velocity = velocity;
    }
    void jump(GameObject go, float power)
    {
        bool jumpState = go.transform.GetChild(0).GetComponent<isJump>().jumpState;
        if (Input.GetButtonDown("Jump") && !jumpState)
        {
            go.transform.GetChild(0).GetComponent<isJump>().jumpState = true;
            go.GetComponent<Rigidbody>().velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, power);
            go.GetComponent<Rigidbody>().velocity = jumpVelocity;
        }
    }
    
    
}
