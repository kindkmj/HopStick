using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csBtnSetting : MonoBehaviour
{

    //Button[] friendBtns;


    //static GameObject[] btns = null;
    //public static void Create(GameObject prefabButton, string[] names)
    //{
    //    if (btns != null) 
    //        Delete();
    //    GameObject parent = GameObject.FindGameObjectWithTag("UIBackground");
    //    float startheight = Screen.height - Screen.height / 5f;
    //    btns = new GameObject[names.Length];
    //    for(int i = 0; i < names.Length; i++)
    //    {
    //        GameObject lastbtn = Instantiate(prefabButton);
    //        RectTransform rect = lastbtn.GetComponent<RectTransform>();
    //        rect.sizeDelta = new Vector2(Screen.width / 3f, Screen.height / 8f);
    //        lastbtn.name = "SelectButton" + (i + 1);
    //        lastbtn.transform.SetParent(parent.transform, false);
    //        lastbtn.transform.position = new Vector2(Screen.width / 2f, startheight - (i * rect.sizeDelta.y) * 1.3f);
    //        lastbtn.GetComponent<Button>().GetComponentInChildren<Text>().text = names[i];
    //        btns[i] = lastbtn;
    //    }
    //}

    //public static void Delete()
    //{
    //    for (int i = 0; i < btns.Length; i++)
    //        Destroy(btns[i]);
    //    btns = null;
    //}
    //void Start()
    //{
    //    GUI.Button = new Rect(10, 10, 150, 100);

    //}


    void Update()
    {

    }
}
