using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct MsgVO
{
    public bool success;
    public string msg;
    public string token;
}

public class LoginImage : MonoBehaviour
{
    private InputField _accountField;
    private InputField _passField;

    private Button _loginBtn;

    private void Awake()
    {
        _accountField = transform.Find("AccountInputField").GetComponent<InputField>();
        _passField = transform.Find("PassInputField").GetComponent<InputField>();

        _loginBtn = transform.Find("LoginBtn").GetComponent<Button>();
        _loginBtn.onClick.AddListener(() =>
        {
            WWWForm form = new WWWForm();
            form.AddField("account", _accountField.text);
            form.AddField("pass", _passField.text);
            form.AddField("deviceID", SystemInfo.deviceUniqueIdentifier);

            DataManager.Inst.SaveData(form, "/login", (json, success) =>
            {
                Debug.Log(json);
                MsgVO msg = JsonUtility.FromJson<MsgVO>(json);

                if(msg.success)
                {
                    PlayerPrefs.SetString("token", msg.token);
                }

                else
                {
                    Debug.LogError("아이디와 비밀번호 불일치");
                }
            });

        });
    }
}
