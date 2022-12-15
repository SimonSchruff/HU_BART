using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SaveMoneyToServer : MonoBehaviour
{
    [SerializeField] string URL = "https://marki.fun/HUBart/write.php";

    private void Start()
    {
        saveMoneyOnServer(111);
    }
    public void saveMoneyOnServer(int _amountMoney)
    {
        StartCoroutine(PostData(_amountMoney));
    }
    
    private IEnumerator PostData(int _amount)
    {
        Debug.Log("set 111");
        using (UnityWebRequest webRequest = UnityWebRequest.Post(URL + "?" + _amount, ""))
        {
            // Hopefully allow for https transfer
            webRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");
            webRequest.certificateHandler = new BybassHTTPSCertificate();

            yield return webRequest.SendWebRequest();
            Debug.Log("set 111 success?");

            //switch (webRequest.result)
            //{
            //    //case UnityWebRequest.Result.ConnectionError:
            //    //    StartCoroutine(retrySendData());
            //    //    break;
            //    //case UnityWebRequest.Result.DataProcessingError:
            //    //    StartCoroutine(retrySendData());
            //    //    Debug.LogError("PostDataRequest Error: " + webRequest.error);
            //    //    break;
            //    //case UnityWebRequest.Result.ProtocolError:
            //    //    StartCoroutine(retrySendData());
            //    //    Debug.LogError("PostDataRequest HTTP Error: " + webRequest.error);
            //    //    break;
            //    //case UnityWebRequest.Result.Success:
            //    //    Debug.Log("PostDataRequest Response: " + webRequest.downloadHandler.text);
            //    //    break;
            //}
        }
    }
}
