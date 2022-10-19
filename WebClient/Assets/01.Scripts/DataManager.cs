using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst;
    public const string URL = "http://localhost:50000";
    public int user_id = 1;

    private void Awake()
    {
        if(Inst != null)
        {
            Debug.LogError("두개의 DataManager가 존재함");
            Destroy(gameObject);
            return;
        }
        Inst = this;
    }

    public void SaveData(string json, string uri, Action<string, bool> Callback)
    {
        StartCoroutine(SendData(json, uri, Callback));
    }

    public void SaveData(WWWForm form, string uri, Action<string, bool> Callback)
    {
        StartCoroutine(SendData("", uri, Callback, form));
    }
    private IEnumerator SendData(string json, string uri, Action<string, bool> Callback, WWWForm form = null)
    {
        if(form == null)
        {
            form = new WWWForm();
            form.AddField("user_id", user_id);
            form.AddField("json", json);
        }

        UnityWebRequest req = UnityWebRequest.Post(URL + uri, form);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Callback?.Invoke(req.downloadHandler.text, true);
        }

        else
        {
            Callback?.Invoke(req.error, false);
        }
    }

    public void LoadData(string uri, Action<string, bool> Callback)
    {
        StartCoroutine(LoadCoroutine(uri, Callback));
    }

    IEnumerator LoadCoroutine(string uri, Action<string, bool> Callback)
    {
        UnityWebRequest req = UnityWebRequest.Get($"{URL}{uri}");
        yield return req.SendWebRequest();

        if(req.result == UnityWebRequest.Result.Success)
        {
            Callback?.Invoke(req.downloadHandler.text, true);
        }

        else
        {
            Callback?.Invoke(req.error, false);
        }
    }

}
