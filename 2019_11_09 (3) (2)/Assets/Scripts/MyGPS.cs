using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class MyGPS : MonoBehaviour
{
    public Text latitude;   // 나의 현재 위치의 위도 텍스트
    public Text longitude;  // 나의 현재 위치의 경도 텍스트
    //public Text altitude;   // 나의 현재 위치의 고도 텍스트
    public Text gpsLog;     // GPS 수신 결과를 표시하기 위한 변수

    public static float lat;       // 실제 나의 위도 데이터
    public static float lon;       // 실제 나의 경도 데이터
    //public float alt;       // 실제 나의 고도 데이터

    float currentTime = 0;  // 수신 체크용 변수

    [Header("testGPS")]

    public static string lati;
    public static string longi;
    //public static string alti;


    private void Start()
    {
        // 기기에 위치정보 이용에 관한 허가 여부를 받는다.    

        // 1-1 만일 위치 정보 동의를 받은적이 없으면,
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            //동의를 받는다.
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        // 1-2 그렇지 않고 이미 위치 정보 동의를 받았다면
        else
        {
            //GPS를 사용한다.
            StartCoroutine(MyGPS_On());
        }
    }

    // GPS 사용하기
    IEnumerator MyGPS_On()
    {
        // 1. 디바이스에 GPS가 없을 때의 예외 처리
        if (!Input.location.isEnabledByUser)
        {
            gpsLog.text = "당신 GPS 없어요!";
            yield break;
        }

        // GPS 장치 켜기
        Input.location.Start();

        // 2. 통신 상태에 따라 GPS 데이터의 송수신이 오래걸리거나, 수신 실패 상태일 때의 예외 처리
        float maxDelayTime = 10f;
        while (Input.location.status == LocationServiceStatus.Initializing && maxDelayTime > 0)
        {
            gpsLog.text = "응답 대기 남은 시간 " + maxDelayTime + "초";
            maxDelayTime--;
            yield return new WaitForSeconds(1f);
        }

        // 응답 지연 상태
        if (Input.location.status == LocationServiceStatus.Initializing)
        {
            gpsLog.text = "응답이 없네요...";
            yield break;
        }

        // 응답 실패 상태
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            gpsLog.text = "수신에 실패했어요...";
            yield break;
        }


    }

    private void Update()
    {
        // 응답 성공 상태(가장 최근 수신 데이터 읽어오기)

        //alt = li.altitude;
        //alti = alt.ToString();

        // UI로 출력한다.
        latitude.text = lat.ToString();
        longitude.text = lon.ToString();
        //altitude.text = "";

        gpsLog.text = "";
    }
}
