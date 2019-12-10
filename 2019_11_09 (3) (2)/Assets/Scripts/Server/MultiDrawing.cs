using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDrawing : MonoBehaviour
{
    #region Variable

    //유저이름
    public string PlayerName = "";

    //라인렌더러 포지션값
    public string PlayerPosition = "";

    //라인렌더러의 컬러 값
    public string PlayerColor = "0,0,0";

    //라인렌더러 글씨 크기 값
    public string PlayerFontSize = "0";
    LineRenderer a = new LineRenderer();
    //유저의 이름을 가지고 만들어질 게임 오브젝트
    public List<GameObject> GameList = new List<GameObject>();
    public List<LineRenderer> LineList = new List<LineRenderer>();

    //유저의 이름에 관련된 라인렌더러
    public Dictionary<string, GameObject> DicList = new Dictionary<string, GameObject>();

    private RoomManager RoomManager;

    #endregion

    #region Event

    void Start()
    {
        RoomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        RoomManager.Initroom();
        PlayerName = Entity.testEmail;
    }

    #endregion

    #region Fuction

    /// <summary>
    /// 여러 유저가 전송한 데이터 중 내 이름과 같은 이름이 있으면 그 이름의 렌더러에 포지션 값을 추가하며
    /// 그 이름이 있지 않으면 그 이름을 새로 만들며 새로 만든 이름에 포지션 값을 추가시켜줌
    /// </summary>
    /// <param name="data">이름,x,y,z값 컬러 r,g,b값 폰트크기 순으로 데이터가 들어옴</param>
    public void SplitDrawInfo(string data)
    {
        string[] dataSplit = new string[data.Length];
        dataSplit = data.Split(new char[1] {','});
        for (int i = 0; i < dataSplit.Length; i++)
        {
            if (string.IsNullOrEmpty(dataSplit[i]))
            {
                return;
            }
            //잘못함 이름과 다를경우 진행하지 않음,이메일로 관리
            //다른유저의 이름을 비교하지않음!!!!//
            //
            //
            //
            ///
            ///
            /// 
            else if (dataSplit[i] == PlayerName)
            {
                if (GameList.Count == 0)
                {
                    CreateLine(dataSplit[0], dataSplit[1], dataSplit[2], dataSplit[3], dataSplit[4], dataSplit[5],
                        dataSplit[6], dataSplit[7]);
                    return;
                }

                for (int j = 0; j < GameList.Count; j++)
                {
                    //현재 게임오브젝트가 플레이어 이름과 같은게 있다면 그 데이터에 추가시킴
                    if (GameList[i].name == PlayerName)
                    {
                        UpdateLine(dataSplit[0], dataSplit[1], dataSplit[2], dataSplit[3], dataSplit[4], dataSplit[5],
                            dataSplit[6], dataSplit[7]);
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 기존 라인렌더러에 값을 추가 시킴
    /// </summary>
    /// <param name="GameObjectName">게임오브젝트 이름</param>
    /// <param name="LineRendererPositionX">라인렌더러의 X값</param>
    /// <param name="LineRendererPositionY">라인렌더러의 Y값</param>
    /// <param name="LineRendererPositionZ">라인렌더러의 Z값</param>
    /// <param name="LineRendererColorR">라인렌더러의 컬러 R값</param>
    /// <param name="LineRendererColorG">라인렌더러의 컬러 G값</param>
    /// <param name="LineRendererColorB">라인렌더러의 컬러 B값</param>
    /// <param name="LineRendererFontSize">라인렌더러의 크기 값</param>
    public void UpdateLine(string GameObjectName, string LineRendererPositionX, string LineRendererPositionY,
        string LineRendererPositionZ, string LineRendererColorR, string LineRendererColorG, string LineRendererColorB,
        string LineRendererFontSize)
    {
        a = DicList[GameObjectName].GetComponent<LineRenderer>();
        DrawLine_3(GameObjectName, LineRendererPositionX, LineRendererPositionY, LineRendererPositionZ,
            LineRendererColorR, LineRendererColorG, LineRendererColorB,false);
    }


    /// <summary>
    /// 라인렌더러를 추가 하고 초기 값을 지정함
    /// </summary>
    /// <param name="GameObjectName">게임오브젝트 이름</param>
    /// <param name="LineRendererPositionX">라인렌더러의 X값</param>
    /// <param name="LineRendererPositionY">라인렌더러의 Y값</param>
    /// <param name="LineRendererPositionZ">라인렌더러의 Z값</param>
    /// <param name="LineRendererColorR">라인렌더러의 컬러 R값</param>
    /// <param name="LineRendererColorG">라인렌더러의 컬러 G값</param>
    /// <param name="LineRendererColorB">라인렌더러의 컬러 B값</param>
    /// <param name="LineRendererFontSize">라인렌더러의 크기 값</param>
    public void CreateLine(string GameObjectName, string LineRendererPositionX, string LineRendererPositionY,
        string LineRendererPositionZ, string LineRendererColorR, string LineRendererColorG, string LineRendererColorB,
        string LineRendererFontSize)
    {
        DicList.Add(GameObjectName, new GameObject(GameObjectName));
        GameList.Add(DicList[GameObjectName]);
        a = DicList[GameObjectName].AddComponent<LineRenderer>();
        DrawLine_3(GameObjectName, LineRendererPositionX, LineRendererPositionY, LineRendererPositionZ,
            LineRendererColorR,LineRendererColorG, LineRendererColorB);
    }

    /// <summary>
    /// 직접적으로 라인랜더러를 그려줌
    /// </summary>
    /// <param name="GameObjectName">게임오브젝트 이름</param>
    /// <param name="LineRendererPositionX">라인렌더러의 X값</param>
    /// <param name="LineRendererPositionY">라인렌더러의 Y값</param>
    /// <param name="LineRendererPositionZ">라인렌더러의 Z값</param>
    /// <param name="LineRendererColorR">라인렌더러의 컬러 R값</param>
    /// <param name="LineRendererColorG">라인렌더러의 컬러 G값</param>
    /// <param name="LineRendererColorB">라인렌더러의 컬러 B값</param>
    /// <param name="LineRendererFontSize">라인렌더러의 크기 값</param>
    private void DrawLine_3(string GameObjectName, string LineRendererPositionX, string LineRendererPositionY,
        string LineRendererPositionZ, string LineRendererColorR, string LineRendererColorG, string LineRendererColorB,
        bool NewType = true)
    {
//        if (a == null)
//        {
//            a.GetComponent<>()
//        }
        //DicList[GameObjectName].material.color = new Color(float.Parse(LineRendererColorR),
//            float.Parse(LineRendererColorG), float.Parse(LineRendererColorB));
        //새로만드는것과 구분
        if (NewType == true)
        {
           a.positionCount = 0;
        }
        a.SetPosition(a.positionCount++,
            new Vector3(float.Parse(LineRendererPositionX), float.Parse(LineRendererPositionY),
                float.Parse(LineRendererPositionZ)));
    }

    /// <summary>
    /// 선택한 컬러의 R,G,B값을 저장함
    /// 매개변수로 어떤 형태의 데이터가 들어올지 모르겠으므로 R,G,B를 따로 받는 형태를 취하였지만
    /// 만약 따로 들어오지 않는다면 float RGB 하나의 매개변수로만 받으면 될거 같아 보임
    /// </summary>
    /// <param name="R"></param>
    /// <param name="G"></param>
    /// <param name="B"></param>
    public void SetColor(float R, float G, float B)
    {
        PlayerColor = R + "," + G + "," + B + ",";
    }

    /// <summary>
    /// 화면에 라인렌더러를 생성해서 그림을 그리게끔 함
    /// </summary>
    public void DrawLine()
    {
        if ((Input.touchCount > 0 &&
             Input.GetTouch(0).phase == TouchPhase.Moved) //모바일에서 동작 하는 것 -> TouchPhase.Moved : 터치 움직임
            || Input.GetMouseButton(0))
        {
            a = new LineRenderer();
            PlayerName = csUIL.testEmail;
            PlayerPosition = LinePosition();
            PlayerColor = "0,0,0";
            PlayerFontSize = "0";
            string data = PlayerName + "," + PlayerPosition + "," + PlayerColor + "," + PlayerFontSize;
            RoomManager.SendLocationValues(data);
        }
    }

    /// <summary>
    ///  터치한 값의 포지션을 구하는 함수 
    /// </summary>
    /// <returns>터치한 값의 포지션값</returns>
    private string LinePosition()
    {
        Vector3 TouchPosition = new Vector3();
        var objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
        Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        //raycast 거리변수 선언
        if (objPlane.Raycast(mRay, out rayDistance))
        {
        }
            TouchPosition = mRay.GetPoint(rayDistance);
        string data = TouchPosition.ToString().Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);
        return data;
    }
    #endregion
}