using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNickName : MonoBehaviour
{
    private RoomManager _roomManager;
    // Start is called before the first frame update
    void Start()
    {
        _roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
    }

    public void Con()
    {
        _roomManager.Connect();
    }

}
