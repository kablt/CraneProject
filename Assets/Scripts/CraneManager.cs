using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneManager : MonoBehaviour
{
    enum CraneState
    {
        Idle,
        XMove, //CraneRoot
        ZMove, //HoistRoot
        Down,
        SecondDown,
        Up,
        Return,
        OpenHoist,
        CloseHoist,

    }
    CraneState craneState;
    float moveSpeed = 2.5f; //이동 속도
    float downSpeed = 2.5f; //하강 속도
    float seconddownSpeed = 3.2f;
    float downTime = 2.5f; //하강 시간
    float grabSpeed = 0.15f; //잡는 속도
    float grabTime = 0.6f; //잡는 시간
    Vector3 originPos;
    //private bool coilAttached = false;

    public GameObject coil;
    public GameObject liftRoot;
    
    // Start is called before the first frame update
    void Start()
    {
        craneState = CraneState.Idle;
        originPos = transform.position;
        

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") != 0)
        {
            craneState = CraneState.ZMove;
        }
        if(Input.GetAxis("Horizontal") != 0)
        {
            craneState = CraneState.XMove;
        }

        switch(craneState)
        {
            case CraneState.Idle:
                Idle();
                break;
            case CraneState.XMove:
                XMove();
                break;
            case CraneState.ZMove:
                ZMove();
                break;
            case CraneState.Down:
                Down();
                break;         
            case CraneState.Up:
                Up();
                break;
            case CraneState.SecondDown:
                SecondDown();
                break;
            case CraneState.Return:
                Return();
                break;
            case CraneState.OpenHoist:
                break;
            case CraneState.CloseHoist:
                break;
                 
        }
    }

    void Idle()
    {
        craneState = CraneState.XMove;
    }

    void XMove()
    {
        GameObject craneRoot = GameObject.Find("CraneRoot");
        float h = Input.GetAxis("Horizontal");
        Vector3 dir = new Vector3(h, 0, 0);
        dir = dir.normalized;
        craneRoot.transform.position += dir * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            craneState = CraneState.Down;
            StartCoroutine(CraneDownStop());
        }
        if (Input.GetMouseButtonDown(0))
        {
            craneState = CraneState.SecondDown;
            StartCoroutine(CraneDownStop());
        }
    }

    void ZMove()
    {
        GameObject hoistRoot = GameObject.Find("HoistRoot");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(0, 0, v);
        dir = dir.normalized;
        hoistRoot.transform.position += dir * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            craneState = CraneState.Down;
            StartCoroutine(CraneDownStop());
        }
        if (Input.GetMouseButtonDown(0))
        {
            craneState = CraneState.SecondDown;
            StartCoroutine(CraneDownStop());
        }
    }

    void Down()
    {
        GameObject liftRoot = GameObject.Find("LiftRoot");
        Vector3 dir = new Vector3(0, -6f, 0); //방향(하강)
        dir = dir.normalized;
        liftRoot.transform.position += dir * downSpeed * Time.deltaTime; //오브젝트 하강 
    }

    void SecondDown()
    {
        GameObject liftRoot = GameObject.Find("LiftRoot");
        Vector3 dir = new Vector3(0, -6f, 0); //방향(하강)
        dir = dir.normalized;
        liftRoot.transform.position += dir * 3.2f * Time.deltaTime; //오브젝트 하강 
    }



    void Up()
    {
        GameObject liftRoot = GameObject.Find("LiftRoot");
        Vector3 dir = new Vector3(0, 6f, 0); //방향(상승)
        dir = dir.normalized;
        liftRoot.transform.position += dir * downSpeed * Time.deltaTime; //오브젝트 상승 
    }

    void Return()
    {
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime; //원위치로 복귀
        }
        else
        {
            transform.position = originPos;
            craneState = CraneState.OpenHoist;
            liftRoot.GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(CraneLay());
        }
    }
    

    IEnumerator CraneDownStop()
    {
        yield return new WaitForSeconds(downTime); //오브젝트 1초 하강 후 하강 멈춤
        craneState = CraneState.CloseHoist;
        StopAllCoroutines();
        StartCoroutine(CraneGrab());
    }

    IEnumerator CraneUpStop()
    {
        yield return new WaitForSeconds(downTime); //오브젝트 1초 하강 후 하강 멈춤
        craneState = CraneState.Return;
    }

    IEnumerator CraneGrab()
    {
        yield return new WaitForSeconds(grabTime); //n초간 오므린 후 잡기 멈춤
        craneState = CraneState.Up;
        StopAllCoroutines();
        StartCoroutine(CraneUpStop());
    }
    IEnumerator CraneLay()
    {
        yield return new WaitForSeconds(grabTime); //n초간 놓기
        craneState = CraneState.Idle;
        StopAllCoroutines();
    }


}
