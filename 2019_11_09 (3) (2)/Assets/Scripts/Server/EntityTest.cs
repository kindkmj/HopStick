using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

public class EntityTest : MonoBehaviourPunCallbacks
{
    public virtual IEnumerator FindPlayerNickName(string fileAddress, string getValue1, string setValue1)
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

            }
        }
    }
}
