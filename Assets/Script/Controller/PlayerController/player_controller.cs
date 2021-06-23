using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    public GameObject goPlayer;

    public GameObject goPlayerEffector;


    public List<controll_state> listControllState = new List<controll_state>();//현재(n번째) 움직임을 실시간으로 기록하는 변수

    public enum View
    {
        player, wide,  world, enumEnd
    }
    public View view = View.player;

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
        bool jumpState = goPlayer.transform.GetChild(0).GetComponent<isJump>().jumpState;
        if (Input.GetButtonDown("Jump") && !jumpState)
        {
            jump(goPlayer, jumpPower, true);
        }
        if(view == View.player)
        {
            follow(goPlayerEffector, goPlayer, 0.1f);
        }
        else if (view == View.wide)
        {
            GameObject goTmp = new GameObject();

            goTmp.transform.position = goPlayerEffector.transform.position;
            goTmp.transform.position = new Vector3(
                goPlayer.transform.position.x,
                goPlayer.transform.position.y,
                goPlayer.transform.position.z - 20
                );

            follow(goPlayerEffector, goTmp, 0.1f);
        }
        else if (view == View.world)
        {
            GameObject goTmp = new GameObject();

            goTmp.transform.position = goPlayerEffector.transform.position;
            goTmp.transform.position = new Vector3(
                goPlayer.transform.position.x,
                goPlayer.transform.position.y,
                goPlayer.transform.position.z - 40
                );

            follow(goPlayerEffector, goTmp, 0.1f);
        }
        Debug.Log(view);
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
        int dir = 0;
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            dir = -1;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            dir = 1;
        }
        moveHorizontal(goPlayer, dir, moveHorizontalSpeed, true);
    }


    public void moveHorizontal(GameObject go, int dir, float speed, bool isPlayer)
    {
        Vector3 velocity = Vector3.zero;
        
        if(dir == -1)
        {
            velocity = Vector3.left;
            go.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }else if(dir == 1)
        {
            velocity = Vector3.right;
            go.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        if(isPlayer)
        {

            controll_state cs = new controll_state();
            cs.currentTime = currentTime;
            cs.direction = dir;
            listControllState.Add(cs);
        }

        velocity = velocity * Time.deltaTime * speed;
        velocity.y = go.GetComponent<Rigidbody>().velocity.y;
        go.GetComponent<Rigidbody>().velocity = velocity;
    }
    public void jump(GameObject go, float power, bool isPlayer)
    {

        if (isPlayer)
        {
            go.transform.GetChild(0).GetComponent<isJump>().jumpState = true;
            listControllState[listControllState.Count - 1].jump = true;
        }
        go.GetComponent<Rigidbody>().velocity = Vector2.zero;
        Vector2 jumpVelocity = new Vector2(0, power);
        go.GetComponent<Rigidbody>().velocity = jumpVelocity;
        
    }
    
    
}
