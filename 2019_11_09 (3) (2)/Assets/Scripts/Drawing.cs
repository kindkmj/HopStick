﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Runtime.Serialization;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Photon.Pun;


public class Drawing : MonoBehaviourPunCallbacks
{
    bool isLoadLineEnd = false;

    int colorCnt = 0;

    int sizeCnt = 0;

    //색깔이나 글자 굵기를 지정해놓은 숫자
    private Plane objPlane;
    //3D 공간에서 평면 표현, 원근이 적용되게 설정

    Color colorFromUI;

    float sizeFromUI;

    //색깔과 글자 굵기 형태 지정
    //int listCnt = 0;
    //static bool drawing;
    private bool isClick = false;
    private bool isFirst = true;

    private LineRenderer line;
    private List<LineRenderer> listLine = new List<LineRenderer>();

    private GameObject lineObject;
    private List<GameObject> lineList = new List<GameObject>();

    public GameObject Draw;
    public PhotonView pv;
    
    float checkCor = 0f;
    int cnt = 0;

    int loadcnt = 0;

    public static bool checkedPanel;

    [Header("database")] int memonum = 0;
    int updatenum = 1;


    public static int dataCnt = 0;
    int orderNum = 0;

    public InputField MemoName;

    public void ChangeColor()
    {
        colorCnt++;
        if (colorCnt == 3)
        {
            colorCnt = 0;
        }
    }

    public void ChangeSize()
    {
        sizeCnt++;
        if (sizeCnt == 3)
        {
            sizeCnt = 0;
        }
    }

    public void btnCloseDrawPanel()
    {
        if (Drawing.checkedPanel == true)
        {
            Draw.SetActive(false);
        }
    }

    public void StartLine()
    {
        if (isFirst)
        {

            if ((Input.touchCount > 0 &&
                 Input.GetTouch(0).phase == TouchPhase.Moved) //모바일에서 동작 하는 것 -> TouchPhase.Moved : 터치 움직임
                || Input.GetMouseButton(0)) // 스크린에서 동작 하는 것 => GetMouseButton : 마우스 움직임
            {
                Draw.SetActive(false);
                isClick = true;
                isFirst = false;
                objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
                Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                //raycast 거리변수 선언
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    this.transform.position = mRay.GetPoint(rayDistance);
                }

                CreateLineObject();


            }
        }
    }

    public void EndLine()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0))
        {
            listLine.Add(line);
            listLine[cnt] = line;

            lineList.Add(lineObject);
            lineList[cnt] = lineObject;
            cnt++;
            isFirst = true;
            isClick = false;
        }
    }
    [PunRPC]
    public void OtherDraw(LineRenderer _line, Vector3 _transform)
    {
        _line.positionCount++;
        _line.SetPosition(_line.positionCount - 1, _transform);
    }

    public void DrawLine()
    {
        if (isClick)
        {
            if ((Input.touchCount > 0 &&
                 Input.GetTouch(0).phase == TouchPhase.Moved) //모바일에서 동작 하는 것 -> TouchPhase.Moved : 터치 움직임
                || Input.GetMouseButton(0)) // 스크린에서 동작 하는 것 => GetMouseButton : 마우스 움직임
            {
                objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
                Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                 float rayDistance;
                //raycast 거리변수 선언
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    this.transform.position = mRay.GetPoint(rayDistance);
                }
            }

            Debug.Log("Move");
            Vector3 position = this.transform.position;
            //라인렌더러의 노드 갯수를 증가
            //새로 만든 노드의 위치값을 설정
            ++line.positionCount;
            line.SetPosition(line.positionCount - 1, position);
            pv.RPC("OtherDraw", RpcTarget.AllBuffered, line, position);
        }
    }

    public void Start()
    {
        StartCoroutine(Entity.lineSELECT("http://14.52.43.228:801/LineNum.php", "email", "test3"));
        pv = GameObject.Find("Panel").GetComponent<PhotonView>();
    }



    void Update()
    {
        if (sizeCnt == 0)
        {
            sizeFromUI = 0.001f;
        }
        else if (sizeCnt == 1)
        {
            sizeFromUI = 0.005f;
        }
        else if (sizeCnt == 2)
        {
            sizeFromUI = 0.01f;
        }

        if (colorCnt == 0)
        {
            colorFromUI = Color.black;
        }
        else if (colorCnt == 1)
        {
            colorFromUI = Color.white;
        }
        else if (colorCnt == 2)
        {
            colorFromUI = Color.red;
        }
    }

    public void IsClickScreen()
    {
        if ((Input.touchCount > 0 &&
             Input.GetTouch(0).phase == TouchPhase.Moved) //모바일에서 동작 하는 것 -> TouchPhase.Moved : 터치 움직임
            || Input.GetMouseButton(0)) // 스크린에서 동작 하는 것 => GetMouseButton : 마우스 움직임
        {

            objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            //raycast 거리변수 선언
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                this.transform.position = mRay.GetPoint(rayDistance);
            }
        }

        Debug.Log("Move");
        Vector3 position = this.transform.position;

        //라인렌더러의 노드 갯수를 증가
        ++line.positionCount;
        //새로 만든 노드의 위치값을 설정
        line.SetPosition(line.positionCount - 1, position);

    }

    public void Eraser() //지우기 버튼을 클릭시 Line을 지우는것
    {
        try //listLine을 관리하기 위해서 try catch문을 사용
        {
            // 실행하고자 하는 문장들
            if (listLine != null && listLine.Count > 0) //Stroke가 존재하고 개수가 0개 이상일때만 작동하도록
            {
                Destroy(listLine[cnt - 1]); //cnt의 값이 ++됨으로 하나를 빼준 Stroke 값을 제거해준다                
                Destroy(lineList[cnt - 1]);
                cnt--; //List의 stroke가 지워졌으므로 값을 cnt값을 감소시킨다.
            }
        }
        catch (System.Exception ex) //Stroke가 더이상 하이라키에 없을때 실행된다
        {
            Debug.Log("더이상 지울것이 없습니다." + ex); //오류구문이 없으면 Exception오류가 발생하게 됩니다.
            cnt = 0; //cnt = 0을 초기화 안시켜주면 계속해서 증가하기 때문에 아예 없을때 0으로 초기화 시켜준다.
        }
    }

    void CreateLineObject(LineRenderer line, Vector3 position, Material mt, float width)
    {
        line.material.color = mt.color;
        line.useWorldSpace = false;

        //라인의 끝부분을 부드럽게 하기위한 버텍스 갯수 설정
        line.numCapVertices = 20;

        //라인렌더러의 폭 설정
        line.widthMultiplier = width;
        line.startWidth = width;
        line.endWidth = width;

        //라인렌더러의 노드 갯수를 1로 설정
        line.positionCount = 0;

        //line.SetPosition(0, position);
        line.sortingOrder = orderNum++;
    }

    /// <summary>
    /// 멀티에필요
    /// </summary>
    void CreateLineObject()
    {
        //라인렌더러를 추가할 게임오브젝트를 생성
        GameObject lineObject = new GameObject("Line");
        line = lineObject.AddComponent<LineRenderer>();
        line.material.color = colorFromUI;
        line.useWorldSpace = false;

        //라인의 끝부분을 부드럽게 하기위한 버텍스 갯수 설정
        line.numCapVertices = 20;

        //라인렌더러의 폭 설정
        line.widthMultiplier = sizeFromUI;

        //line.startWidth = sizeFromUI;
        //line.endWidth = sizeFromUI;

        //라인렌더러의 노드 갯수를 1로 설정
        line.positionCount = 0;

        //첫 번째 노드의 위치를 컨트롤러의 위치로 설정
        Vector3 position = this.transform.position;
//        position.z = this.transform.position.z;
//        position.x = this.transform.position.x;
//        position.y = this.transform.position.y;
        line.SetPosition(0, position);
        //line.sortingOrder = orderNum++;
    }

    public void SaveLineTextAll()
    {

        // memonum++;
        //int fCnt = 1;
        StartCoroutine(Entity.SaveMemo("http://14.52.43.228:801/MemoSave.php", "KeyValue", "email", "memoType", "lati",
            "longit", "memoName", csUIL.testEmail + updatenum.ToString(), csUIL.testEmail.ToString(),
            csUIL.MemoType.ToString(), MyGPS.lati, MyGPS.longi, MemoName.text));
        Debug.Log("email:" + csUIL.testEmail);
        Debug.Log("memoType:" + csUIL.MemoType);
        Debug.Log("memoName:" + MemoName.text);
        updatenum++;

        //foreach (LineRenderer _line in listLine)
        //{
        //    if (_line != null)
        //    {
        //        Vector3[] posArray = new Vector3[_line.positionCount];
        //        int cnt = _line.GetPositions(posArray);
        //        string strColor = string.Format("{0},{1},{2},{3}", _line.material.color.r, _line.material.color.g, _line.material.color.b, _line.material.color.a);
        //        foreach (Vector3 p in posArray)
        //        {
        //            //StartCoroutine(Entity.SaveMemo("http://14.52.43.228:801/MemoSave.php", "Uniquenum", "UserEmail", "Memonum", "Colour", "Size", "memoType",
        //            //"corX", "corY", "corZ", "lati", "longit", "alti", "memoName", "LineNum", csUIL.testEmail.ToString() + (updatenum), csUIL.testEmail.ToString(), memonum.ToString(), strColor, _line.endWidth.ToString(), "public",
        //            //p.x.ToString(), p.y.ToString(), p.z.ToString(), MyGPS.lati, MyGPS.longi, MyGPS.alti, "이름", fCnt.ToString()));
        //            updatenum++;
        //            Debug.Log("아이디 들어옴:" + csUIL.testEmail);
        //            Debug.Log(" GPS:" + MyGPS.lati + MyGPS.longi + MyGPS.alti);
        //            Debug.Log("db저장 성공:" + p.x.ToString() + p.y.ToString() + p.z.ToString());
        //        }
        //        fCnt++;
        //    }
        //    else
        //    {
        //        Debug.Log("null");
        //    }
        //}
    }

    IEnumerator CoCheckLoadLine()
    {
        while (true)
        {
            if (Entity.isLoadLine == false)
            {
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                //라인렌더러를 추가할 게임오브젝트를 생성            
                GameObject lineObject = new GameObject("Line");
                line = lineObject.AddComponent<LineRenderer>();

                Vector3 pos = new Vector3();
                //pos.x = float.Parse(Entity.lines[0]);
                //pos.y = float.Parse(Entity.lines[1]);
                //pos.z = float.Parse(Entity.lines[2]);

                line.material.color = Color.red;
                float width = 0.005f;
                CreateLineObject(line, pos, line.material, width);
                Debug.Log(Entity.lines.Length + " CreateLineObject");
                for (int j = 3; j < (Entity.lines.Length) - 5; j = j + 3)
                {
                    Debug.Log(j + " Add Position");
                    pos.x = float.Parse(Entity.lines[j]);
                    pos.y = float.Parse(Entity.lines[j + 1]);
                    pos.z = float.Parse(Entity.lines[j + 2]);
                    Debug.Log(pos.x);
                    Debug.Log(pos.y);
                    Debug.Log(pos.z);

                    ++line.positionCount;
                    line.SetPosition(line.positionCount - 1, pos);
                }

                listLine.Add(line);
                Debug.Log(iCnt + " End");
                iCnt++;
                break;
            }
        }

        isLoadLineEnd = true;
    }

    IEnumerator coLoadLineAll()
    {
        listLine.Clear();
        while (true)
        {
            if (iCnt > Entity.lineCN)
            {
                iCnt = 0;
                Debug.Log("---End All Line---");
                break;
            }

            isLoadLineEnd = false; //가져온 데이터를 다 그렸을 때
            Entity.isLoadLine = false; //Entity.lineSEL 함수가 데이터를 가져왔을 때 사용
            Debug.Log(iCnt + " Start");
            StartCoroutine(Entity.lineSEL("http://14.52.43.228:801/Line.php", "email", "LineNum",
                csUIL.testEmail.ToString(), iCnt.ToString()));
            StartCoroutine(CoCheckLoadLine());
            while (isLoadLineEnd == false)
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    int iCnt = 1;

    public void LoadLineTextAll()
    {
        StartCoroutine(coLoadLineAll());
    }
}