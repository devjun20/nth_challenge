using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzle_game_controller : MonoBehaviour
{
    public float cycle = 10.0f;
    public GameObject player;
    public GameObject pc;
    public GameObject fakePlayer;

    List<List<controll_state>> listAllControllStates = new List<List<controll_state>>();//전체(1 ~ n-1) 움직임 저장하는 변수.


    private List<Vector3> initObjectPosition = new List<Vector3>();
    private List<Quaternion> initObjectRotation = new List<Quaternion>();
    private List<Vector3> initObjectLocalScale = new List<Vector3>();

    public GameObject[] initObject;

    Vector3 vecInit;

    private float currentTime;
    int frame = -1;//frame * 0.2 = currentTime.
    

    int invokeCount = 0;

    void Start()
    {
        currentTime = 0f;
        vecInit = player.transform.position;

        for (int i = 0; i < initObject.Length; i++)
        {
            initObjectPosition.Add(initObject[i].transform.position);
            initObjectRotation.Add(initObject[i].transform.rotation);
            initObjectLocalScale.Add(initObject[i].transform.localScale);

        }
    }

    void initializeObject()//사이클이 돌 때마다 움직이는 오브젝트들 원위
    {
        invokeCount++;
        if (invokeCount > 10)
        {
            CancelInvoke("initializeObject");
            invokeCount = 0;
        }

        for (int i = 0; i < initObject.Length; i++)
        {
            initObject[i].transform.position = initObjectPosition[i];
            initObject[i].transform.rotation = initObjectRotation[i];
            initObject[i].transform.localScale = initObjectLocalScale[i];
            initObject[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            initObject[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void initialize()//각 생명주기 시작 할 때마다 맵 초기화 & n개의 플레이어 생성
    {
        
        GameObject goTmp = Instantiate(fakePlayer, vecInit, fakePlayer.transform.rotation);
        goTmp.transform.parent = gameObject.transform;
        
        player.transform.position = vecInit;

        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;


        currentTime = 0f;
        frame = -1;

        listAllControllStates.Add(new List<controll_state>(pc.GetComponent<player_controller>().listControllState));
        pc.GetComponent<player_controller>().listControllState.Clear();



        for (int i = 0; i < listAllControllStates.Count; i++)
        {
            transform.GetChild(i).transform.position = vecInit;

            transform.GetChild(i).GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;

        }
        InvokeRepeating("initializeObject", 0f, 0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        


        frame++;
        currentTime += Time.deltaTime;

        if(currentTime > cycle)
        {
            


            initialize();
        }
        else
        {
            for (int i = 0; i < listAllControllStates.Count; i++)
            {
                if(listAllControllStates[i].Count < frame + 1)
                {
                    continue;
                }
                pc.GetComponent<player_controller>().moveHorizontal(transform.GetChild(i).gameObject,
                    listAllControllStates[i][frame].direction,
                    pc.GetComponent<player_controller>().moveHorizontalSpeed,
                    false);

                if(listAllControllStates[i][frame].jump)
                {
                    pc.GetComponent<player_controller>().jump(transform.GetChild(i).gameObject,
                        pc.GetComponent<player_controller>().jumpPower,
                        false);
                }

            }



        }
        
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            initialize();
        }
        if(Input.GetKeyUp(KeyCode.R))
        {
            if ((pc.GetComponent<player_controller>().view + 1) != player_controller.View.enumEnd)
            {
                pc.GetComponent<player_controller>().view++;
            }
            else
            {
                pc.GetComponent<player_controller>().view = 0;
            }
           
        }
    }
}
