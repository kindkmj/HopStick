using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : EntityTest
{
    #region Variable

    private PhotonView pv;

    private Text RoomName;

    private Button RoomCreate;
    private Text RoomName_Create;

    private Button RoomJoin;
    private Text RoomName_Join;

    private Button BtnOk;
    private Text PlayerState;

    private MultiDrawing multiDrawing;

    #endregion

    #region Event

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        DontDestroyOnLoad(this);
    }

    public void Initroom()
    {
        if (SceneManager.GetActiveScene().name == "LOBBY")
        {
            //방 만드는데 필요한 목록
            RoomCreate = GameObject.Find("CreateRoom").GetComponent<Button>();
            RoomName_Create = GameObject.Find("RoomName_Create").GetComponent<Text>();

            RoomJoin = GameObject.Find("JoinRoom").GetComponent<Button>();
            RoomName_Join = GameObject.Find("RoomName_Join").GetComponent<Text>();

            BtnOk = GameObject.Find("btnOK").GetComponent<Button>();
            PlayerState = GameObject.Find("PlayerStateText").GetComponent<Text>();

            RoomCreate.onClick.AddListener(CreateRoom);
            RoomJoin.onClick.AddListener(JoinRoom);
            BtnOk.onClick.AddListener(MoveSecne);
            PlayerState.text = "대기중...";
        }
        else if (SceneManager.GetActiveScene().name == "MULTIPLESTART")
        {
            multiDrawing = GameObject.Find("MultiDrawing").GetComponent<MultiDrawing>();
        }
    }


    


    #endregion

    #region Fuction

    /// <summary>
    /// 서버 연결 회원정보 입력 후 로그인 버튼 입력시 해당 메서드 실행
    /// </summary>
    public void Connect()
    {
//        Player pp = new Player();
//        pp.UserId
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connect완료");
        //        StartCoroutine(FindPlayerNickName("http://14.52.43.228:801/GetFriendNickname.php", "loginUser",
        //            "test3"));
        //        {
        //            if (string.IsNullOrEmpty(Entity.PlayerNickName))
        //            {
        //                //오류메시지 발생
        //                Debug.Log("123");
        //            }
        //            else
        //            {
        //                PhotonNetwork.NickName = Entity.PlayerNickName;
        //                Debug.Log(PhotonNetwork.NickName + "닉네임 설정됨");
        //            }
        //        }
    }

    /// <summary>
    /// 정상적으로 포톤 연결시 MULTIPLE 신으로 연결됨
    /// </summary>
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
//        Debug.Log("정상적으로 연결됨"+name);
//        SceneManager.LoadScene("MULTIPLE");
    }

    /// <summary>
    /// Multiple 씬으로 연결시 마치 Start처럼 사용할 예정임
    /// </summary>
    public override void OnJoinedLobby()
    {
//        if (SceneManager.GetActiveScene().name == "MULTIPLE")
//        {
//
//        }
    }

    /// <summary>
    /// 연결 해제시 실행
    /// </summary>
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// 방 생성시 옵션 방제목이 없으면 실행하지않음
    /// </summary>
    public void CreateRoom()
    {
        if (PhotonNetwork.CreateRoom(RoomName_Create.text, new RoomOptions {IsVisible = false, MaxPlayers = (byte) 3}))
        {
            PlayerState.text = "현재 방 만드는중";
        }
        //PhotonNetwork.CreateRoom(방제목, new RoomOptions { MaxPlayers = 최대인원수 });
    }

    /// <summary>
    /// 방제목과 같은 방이 있을 경우 방에 진입 시도
    /// </summary>
    public void JoinRoom()
    {
        try
        {
            if (RoomName_Join.text.Trim() != string.Empty)
            {
                if (PhotonNetwork.JoinRoom(RoomName_Join.text.Trim(), null))
                {
                }
            }
//            PhotonNetwork.LoadLevel("SINGLE");
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// 방에 접속시 자동 실행
    /// </summary>
    public override void OnJoinedRoom()
    {
        PlayerState.text = "현재 방 입장한 상태";
    }

    /// <summary>
    /// 방만들기 실패시 재 시도
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoom();
        PlayerState.text = "방만들기 실패 다시 시도중";
        Debug.Log(message);
    }

    /// <summary>
    /// 방에 누가 들어왔을때
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("누가들어옴");
    }

    /// <summary>
    /// 방에서 누가 나갔을때 실행
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("누가나감");
    }

    #region PunRpc
    //multiDrawing

    /// <summary>
    /// 로비창에서 씬 넘길때 사용
    /// </summary>
    public void MoveSecne()
    {
        photonView.RPC("PunMultiPle", RpcTarget.All);
    }

    /// <summary>
    /// 로비창에서 씬 넘길때 사용
    /// </summary>
    [PunRPC]
    public void PunMultiPle()
    {
        PhotonNetwork.LoadLevel("MULTIPLESTART");
    }

    /// <summary>
    /// 그리고 있는 값을 상대방에게 전송함
    /// </summary>
    /// <param name="data">현재 그려지고있는 x,y,z값을 string으로 받음
    /// 최초의 데이터는 (xxx,xxx,xxx)형태로 전달받으며
    /// 전달받은 데이터의 '(',')' 를 지운뒤 지운string값을 PunRpc로 전송해줌</param>
    public void SendLocationValues(string data)
    {
        photonView.RPC("PunSendLocationValues", RpcTarget.All, data);
    }
    /// <summary>
    /// 변환된 데이터를 토대로 각자 자신의 그림판에 그림을 그리도록 유도함 내가 그리는 값인지 아닌지를 분간해줘서 전달해줄것임.
    /// </summary>
    /// <param name="data">위에서 전달받은 데이터 xxx,xxx,xxx 형태의 값을 나눈 뒤 그 데이터를 전달해줌</param>
    [PunRPC]
    public void PunSendLocationValues(string data)
    {
        multiDrawing.SplitDrawInfo(data);
    }

    #endregion

    #endregion

    #region Override
    public override IEnumerator FindPlayerNickName(string fileAddress, string getValue1, string setValue1)
    {
        WWWForm form = new WWWForm();
        form.AddField(getValue1, setValue1);

        using (UnityWebRequest www = UnityWebRequest.Post(fileAddress, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                PhotonNetwork.NickName = www.downloadHandler.text;
                name = PhotonNetwork.NickName;
            }
        }
    }
    #endregion
}