using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    public GameObject goPlayer;

    public GameObject goPlayerEffector;


    public List<controll_state> listControllState = new List<controll_state>();//현재(n번째) 움직임을 실시간으로 기록하는 변수


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
        follow(goPlayerEffector, goPlayer, 0.1f);
    }

    void follow(GameObject go1, GameObject go2, float delay)
    {
        Vector3 velo = Vector3.zero;
        go1.transform.position = Vector3.SmoothDamp(go1.transform.position, go2.transform.position, ref velo, delay);
        //go1.transform.position = go2.transform.position;
    }

    float currentTime = 0f;
    private void FixedUpdate()
    {

        currentTime += Time.deltaTime;

        moveHorizontal(goPlayer, moveHorizontalSpeed, true);
    }


    void moveHorizontal(GameObject go, float speed, bool isPlayer)
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

        if(isPlayer)
        {

            controll_state cs = new controll_state();
            cs.currentTime = currentTime;
            cs.vec = goPlayer.transform.position;
            listControllState.Add(cs);
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
