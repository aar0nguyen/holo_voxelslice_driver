using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class API_request : MonoBehaviour
{

    string url = "https://www.example.com";

    private void Start()
    {
        StartCoroutine(url);
    }

    IEnumerator GetRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log(response);
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }

    }
}
