using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct RegisterVO
{
    public string account;
    public string name;
    public string pass;
}

public class RegisterImage : MonoBehaviour
{
    private InputField _accountField;
    private InputField _nameField;
    private InputField _passField;

    private Button _registerBtn;


    private void Awake()
    {
        _accountField = transform.Find("AccountInputField").GetComponent<InputField>();
        _nameField = transform.Find("NameInputField").GetComponent<InputField>();
        _passField = transform.Find("PassInputField").GetComponent<InputField>();

        _registerBtn = transform.Find("RegisterBtn").GetComponent<Button>();
        _registerBtn.onClick.AddListener(() =>
        {
            WWWForm form = new WWWForm();
            form.AddField("account", _accountField.text);
            form.AddField("name", _nameField.text);
            form.AddField("pass", _passField.text);

            DataManager.Inst.SaveData(form, "/user", (json, success) =>
            {
                Debug.Log(json);
            });

        });
    }
}
