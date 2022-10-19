using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

            DataManager.Inst.SaveData(form, "/login", (json, success) =>
            {
                Debug.Log(json);
            });

        });
    }
}
