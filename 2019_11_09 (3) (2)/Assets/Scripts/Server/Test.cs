using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private RoomManager roml;
    // Start is called before the first frame update
    void Start()
    {
        roml = GameObject.Find("RoomManager").GetComponent<RoomManager>();
    }

    public void showname()
    {
        Debug.Log(roml.name);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
