using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPSData : MonoBehaviour
{
    IEnumerator StartGPS()
    {
        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start();

        //타임아웃
        //float waitRemainTime = gpsInitialize;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
