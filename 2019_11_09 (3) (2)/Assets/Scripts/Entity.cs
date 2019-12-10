using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public static bool ttt = true;
    public static bool isLoadLine = false;

    [Header("현재 사용자의 이메일 값을 저장하는 변수")]
    public static string testEmail;
    public static string conditionOfResult;

    public static IEnumerator GetServer()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://14.52.43.228:801/GetUsers.php"))
        {
            yield return www.SendWebRequest();

            string errord;

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("서버 연결 문제:" + www.error);
                errord = "서버 연결 문제:" + www.error.ToString();
            }
            else
            {
                //show results as text
                Debug.Log(www.downloadHandler.text);
                errord = www.downloadHandler.data.ToString();
                string test1 = errord;

                //or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
        }
    }


    //StartCoroutine(Entity.SELECT("http://14.52.43.228:801/GetFriendNickname.php", "loginUser","이메일값"));
    public static string PlayerNickName;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tableType"></param>
    /// <param name="condition"></param>
    /// <param name="fileAddress"></param>
    /// <param name="getValue1"></param>
    /// <param name="getValue2"></param>
    /// <param name="setValue1"></param>
    /// <param name="setValue2"></param>
    /// <returns></returns>
    public  static IEnumerator FindPlayerNickName(string fileAddress, string getValue1, string setValue1)
    {
        WWWForm form = new WWWForm();
        form.AddField(getValue1, setValue1);


        using (UnityWebRequest www = UnityWebRequest.Post(fileAddress, form))
        {
            //yield return waituntil(()=>)
                
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                ttt = false;
                PlayerNickName = www.downloadHandler.text;
                Debug.Log(PlayerNickName+"!111111111111");
            }
        }
    }

    public static string RegisterState;

    //INSERT
    /// <summary>
    /// INSERT
    /// </summary>
    /// <param name="tableState"></param>
    /// <param PAGE설정="condition"></param> //'1' IS LOGIN
    /// <param name="fileAddress"></param>
    /// <param name="getValue1"></param>
    /// <param name="getValue2"></param>
    /// <param name="getValue3"></param>
    /// <param name="setValue1"></param>
    /// <param name="setValue2"></param>
    /// <param name="setValue3"></param>
    /// <returns></returns>
    public static IEnumerator INSERT(string tableState, int condition, string fileAddress, string getValue1, string getValue2, string getValue3,
      string setValue1, string setValue2, string setValue3)
    {
        WWWForm form = new WWWForm();
        form.AddField(getValue1, setValue1);
        form.AddField(getValue2, setValue2);
        form.AddField(getValue3, setValue3);

        if (tableState == "userinfo")
        {
            testEmail = setValue1;
            using (UnityWebRequest www = UnityWebRequest.Post(fileAddress, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    RegisterState = www.downloadHandler.text;
                    conditionOfResult = www.downloadHandler.text;
                    if (www.downloadHandler.text == "1")
                    {
                       
                        //SceneManager.LoadScene("LOGIN");
                    }
                    else if (www.downloadHandler.text == "3")
                    {

                    }

                }
            }
        }
    }

    public static IEnumerator SaveMemo(string fileAddress, string getValue1, string getValue2, string getValue3, string getValue4, string getValue5,
        string getValue6,string setValue1, string setValue2, string setValue3, string setValue4, string setValue5, string setValue6)
    {
        WWWForm form = new WWWForm();
        form.AddField(getValue1, setValue1);
        form.AddField(getValue2, setValue2);
        form.AddField(getValue3, setValue3);
        form.AddField(getValue4, setValue4);
        form.AddField(getValue5, setValue5);
        form.AddField(getValue6, setValue6);
       
        using (UnityWebRequest www = UnityWebRequest.Post(fileAddress, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "1")
                {
                    Debug.Log("저장성공");
                }
                else if (www.downloadHandler.text == "2")
                {
                    Debug.Log("저장실패:"+www.downloadHandler.text);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param PAGE설정="condition"></param> //'1' IS 'LOGIN' // '2' IS 'CHECKEDPW
    /// <param php파일주소="fileAddress"></param>
    /// <param php변수="getValue1"></param>
    /// <param php변수="getValue2"></param>
    /// <param 사용자입력변수="setValue1"></param>
    /// <param 사용자입력변수="setValue2"></param>
    /// <returns></returns>
    /// 


    static public string loginState;
    public static IEnumerator SELECT(string tableType, int condition, string fileAddress, string getValue1, string getValue2,
        string setValue1, string setValue2)
    {
        WWWForm form = new WWWForm();
        form.AddField(getValue1, setValue1);
        form.AddField(getValue2, setValue2);
        if (tableType == "userinfo")
        {
            if (condition == 1)
            {
                csUIL.testEmail = setValue1;
            }
            using (UnityWebRequest www = UnityWebRequest.Post(fileAddress, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    loginState = www.downloadHandler.text;
                    if (www.downloadHandler.text == "1")
                    {                        
                        SceneManager.LoadScene("SELECTION");
                    }
                    else if (www.downloadHandler.text == "2")
                    {
                        conditionOfResult = "비밀번호가 틀렸습니다";
                        Debug.Log("비밀번호가 틀렸습니다");
                    }
                    else if (www.downloadHandler.text == "3")
                    {
                        conditionOfResult = "등록되어 있는 사용자가 아닙니다";
                        Debug.Log("입력하신 이메일로 등록된 정보가 없습니다");
                    }
                    else if (www.downloadHandler.text == "4")
                    {
                        Debug.Log("비밀번호 인증 성공 did");
                        SceneManager.LoadScene("UPDATEINFO");
                    }
                }
            }
        }
        else if (tableType == "memolist")
        {
            using (UnityWebRequest www = UnityWebRequest.Post(fileAddress, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    conditionOfResult = www.downloadHandler.text;
                }
            }
        }
    }

    public static string[] lines;
    public static string line;

    public static string[] linesN;
    public static string lineN;
    public static int lineCN;

    //배열 길이
    public static IEnumerator lineSELECT(string fileAddress, string getValue1,
       string setValue1)
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
                lineN = www.downloadHandler.text;        
                 linesN = lineN.Split(',');  
                
                lineCN = (linesN.Length - 1);
                              
            }
        }
    }




    //배열 값
    public static IEnumerator lineSEL(string fileAddress, string getValue1, string getValue2,
      string setValue1, string setValue2)
    {
        WWWForm form = new WWWForm();

        form.AddField(getValue1, setValue1);
        form.AddField(getValue2, setValue2);



        using (UnityWebRequest www = UnityWebRequest.Post(fileAddress, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                line = www.downloadHandler.text;
                lines = line.Split(',');
                Debug.Log(lines.Length);
            }
        }

        isLoadLine = true;
    }



    public static string[] flines;
    public static string fline;

    public static void lineValue(string fileAddress, string getValue1, string getValue2,
   string setValue1, string setValue2)
    {
        WWWForm form = new WWWForm();

        form.AddField(getValue1, setValue1);
        form.AddField(getValue2, setValue2);


        using (UnityWebRequest www = UnityWebRequest.Post(fileAddress, form))
        {

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                fline = www.downloadHandler.text;
                flines = fline.Split(',');

            }
        }

    }




    public static int testc;

    public static string getX;
    public static string getY;
    public static string getZ;
    public static string[] positionX;
    public static string[] positionY;
    public static string[] positionZ;

    /// <summary>
    /// MEMOLIST에서 좌표를 가져오는 함수
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="getValue1"></param>
    /// <param name="getValue2"></param>
    /// <param name="getValue3"></param>
    /// <param name="setValue1"></param>
    /// <param name="setValue2"></param>
    /// <param name="setValue3"></param>
    /// <returns></returns>
    /// 
    static int numm;
    public static IEnumerator MEMOSELECT(int dataType, string getValue1, string getValue2, string getValue3,
       string setValue1, string setValue2, string setValue3)
    {
        WWWForm form = new WWWForm();
        form.AddField(getValue1, setValue1);
        form.AddField(getValue2, setValue2);
        form.AddField(getValue3, setValue3);

        if (dataType == 1) //좌표X값을 가져오는 함수
        {
            using (UnityWebRequest www = UnityWebRequest.Post("http://14.52.43.228:801/SelectMemo.php", form))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("X결과 값" + www.downloadHandler.text);
                    getX = www.downloadHandler.text;
                    positionX = getX.Split(',');
                    Debug.Log("positionX:" + positionX[2]);
                }
            }
        }
        else if (dataType == 2) //좌표Y값을 가져오는 함수
        {
            using (UnityWebRequest www = UnityWebRequest.Post("http://14.52.43.228:801/GetPosition.php", form))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Y결과 값:" + www.downloadHandler.text);
                    getY = www.downloadHandler.text;
                    positionY = getY.Split(',');
                    Debug.Log("positionY:" + positionY[2]);
                }
            }
        }
        else if (dataType == 3) //좌표 Z값을 가져오는 함수
        {
            using (UnityWebRequest www = UnityWebRequest.Post("http://14.52.43.228:801/GetPositionZ.php", form))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Z결과 값:" + www.downloadHandler.text);
                    getZ = www.downloadHandler.text;
                    positionZ = getZ.Split(',');
                    Debug.Log("positionZ:" + positionZ[2]);
                }
            }
        }
    }


    public static IEnumerator TESTMEMOSELECT(int dataType, string getValue1, string getValue2, string getValue3,
   string setValue1, string setValue2, string setValue3)
    {
        WWWForm form = new WWWForm();
        form.AddField(getValue1, setValue1);
        form.AddField(getValue2, setValue2);
        form.AddField(getValue3, setValue3);


        if (dataType == 1) //좌표X값을 가져오는 함수
        {
            using (UnityWebRequest www = UnityWebRequest.Post("http://14.52.43.228:801/TestX.php", form))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("X결과 값" + www.downloadHandler.text);
                    getX = www.downloadHandler.text;
                    positionX = getX.Split(',');
                    Debug.Log("positionX:" + positionX[2]);
                }
            }
        }
        else if (dataType == 2) //좌표Y값을 가져오는 함수
        {
            using (UnityWebRequest www = UnityWebRequest.Post("http://14.52.43.228:801/TestY.php", form))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Y결과 값:" + www.downloadHandler.text);
                    getY = www.downloadHandler.text;
                    positionY = getY.Split(',');
                    Debug.Log("positionY:" + positionY[2]);
                }
            }
        }
        else if (dataType == 3) //좌표 Z값을 가져오는 함수
        {
            using (UnityWebRequest www = UnityWebRequest.Post("http://14.52.43.228:801/TestZ.php", form))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Z결과 값:" + www.downloadHandler.text);
                    getZ = www.downloadHandler.text;
                    positionZ = getZ.Split(',');
                    Debug.Log("positionZ:" + positionZ[2]);
                }
            }
        }
    }

    //UPDATE
    /// </summary>
    /// <param 페이지설정="condition"></param> //'1' IS 'MYPAGE-PASSWORD'// '2' IS 'FINDPW' //'3' IS 'MYPAGE-NICKNAME'
    /// <param 변경정보타입="type"></param> // 'password' is password // nickname is nickname
    /// <param php파일주소="fileAddress"></param>
    /// <param php입력변수="getvalue1"></param>
    /// <param php입력변수="getvalue2"></param>
    /// <param 사용자입력변수="setvalue1"></param>
    /// <param 사용자입력변수="setvalue2"></param>
    /// <returns></returns>
    public static IEnumerator UPDATE(int condition, string type, string fileAddress,
        string getvalue1, string getvalue2, string setvalue1, string setvalue2)
    {
        WWWForm form = new WWWForm();

        form.AddField(getvalue1, setvalue1);
        form.AddField(getvalue2, setvalue2);
        using (UnityWebRequest www = UnityWebRequest.Post(fileAddress, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                conditionOfResult = www.downloadHandler.text;

                if (www.downloadHandler.text == "1" || www.downloadHandler.text == "3")
                {
                    Debug.Log("정보가 업로드 되었습니다");
                    if (condition == 1 || condition == 3)
                    {
                        SceneManager.LoadScene("SELECTION");
                    }
                    else if (condition == 2)
                    {
                        SceneManager.LoadScene("LOGIN");
                    }
                }
                else if (www.downloadHandler.text == "2" || www.downloadHandler.text == "4")
                {
                    Debug.Log("정보 업로드에 실패했습니다");
                }
            }
        }
    }
}