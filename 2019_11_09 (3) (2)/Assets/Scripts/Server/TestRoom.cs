using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestRoom : MonoBehaviour
{
    /// <summary>
    /// 로비씬에서 RoomManager의 초기화를 담당함
    /// </summary>
    private RoomManager roomManager;
    // Start is called before the first frame update
    void Start()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        roomManager.Initroom();
    }

}
