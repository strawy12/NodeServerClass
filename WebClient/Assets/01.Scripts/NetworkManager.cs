using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class UserData
{
    public int id;
    public string name;
}

public class Datas
{
    public string name;
    public List<UserData> users;
}

public class NetworkManager : MonoBehaviour
{
    private bool _isLoading = false;
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (_isLoading) return;
            _isLoading = true;
            Debug.Log("LoadingData from Server");
            StartCoroutine(GetDataFromServer());
        }
    }

    private IEnumerator GetDataFromServer()
    {
        //CRUD Create, Read, Update, Delete

        UnityWebRequest webReq = UnityWebRequest.Get("http://localhost:50000");

        yield return webReq.SendWebRequest();

        //ConnectionError, DeataProcessingError, ProtocolError
        if (webReq.result == UnityWebRequest.Result.Success)
        {
            string msg = webReq.downloadHandler.text;
            Datas datas = JsonUtility.FromJson<Datas>(msg);

            foreach(var data in datas.users)
            {
                Debug.Log($"id:{data.id}, name:{data.name}");
            }
        }


        _isLoading = false;
        Debug.Log("Loading Complete");
    }
}
