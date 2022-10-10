using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Text = UnityEngine.UI.Text;


[Serializable]
public class ImageList
{
    public string text;
    public int count;
    public List<string> list;
}

public class NetworkManager : MonoBehaviour
{
    [SerializeField]
    private Image _targetImage;

    [SerializeField]
    private Button _imageButtonTemp;

    private bool _isLoading = false;

    private string SAVE_PATH = "";
    private const string SAVE_FILE = "/ImageFile.Json";

    [SerializeField]
    private ImageList _imageList;

    private void Start()
    {
       // GetImageList();
    }

    private void GetImage(string filename)
    {
        if (_isLoading) return;
        _isLoading = true;
        Debug.Log("LoadingData from Server"); 

        
        if (File.Exists($"{SAVE_PATH}/{filename}.png"))
        {
            Texture2D texture = new Texture2D(1, 1);
            byte[] textureData = File.ReadAllBytes($"{SAVE_PATH}/{filename}.png");
            texture.LoadImage(textureData);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            _targetImage.sprite = sprite;
            _targetImage.preserveAspect = true;

            _isLoading = false;
            return;
        }

        StartCoroutine(GatImageFromServer(filename));
    }

    private IEnumerator GatImageFromServer(string filename)
    {
        UnityWebRequest webReq = UnityWebRequestTexture.GetTexture($"http://localhost:50000/Image/{filename}");

        yield return webReq.SendWebRequest();

        if (webReq.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)webReq.downloadHandler).texture as Texture2D;

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            _targetImage.sprite = sprite;
            _targetImage.preserveAspect = true;

            // Png 파일 생성
            File.WriteAllBytes($"{SAVE_PATH}/{filename}.png", texture.EncodeToPNG());
        }

        _isLoading = false;
        Debug.Log("Loading Complete");
    }

    private void GetImageList()
    {
        SAVE_PATH = Application.dataPath + "/Datas";

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string stringJson = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            _imageList = JsonUtility.FromJson<ImageList>(stringJson);
            MakeButtons();
        }
        else
        {
            _isLoading = true;
            StartCoroutine(GatImageListServer());
        }
    }

    private void MakeButtons()
    {
        for (int i = 0; i < _imageList.count; i++)
        {
            Button button = Instantiate(_imageButtonTemp, _imageButtonTemp.transform.parent);
            button.name = $"{_imageList.list[i]}_Btn";
            string filename = _imageList.list[i];

            button.onClick.AddListener(() => GetImage(filename));
            Text btnText = button.transform.Find("Text").GetComponent<Text>();
            btnText.text = _imageList.list[i];

            button.gameObject.SetActive(true);
        }
    }

    private IEnumerator GatImageListServer()
    {
        UnityWebRequest webReq = UnityWebRequestTexture.GetTexture("http://localhost:50000/ImageList");

        yield return webReq.SendWebRequest();

        if (webReq.result == UnityWebRequest.Result.Success)
        {
            string msg = webReq.downloadHandler.text;
            _imageList = JsonUtility.FromJson<ImageList>(msg);
            MakeButtons();
            SaveJson();
        }

        _isLoading = false;
        Debug.Log("Loading Complete");
    }


    private void SaveJson()
    {
        string msg = JsonUtility.ToJson(_imageList,true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, msg, System.Text.Encoding.UTF8);
    }
}
