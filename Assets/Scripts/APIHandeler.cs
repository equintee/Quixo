using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class APIHandeler : MonoBehaviour
{
    private WebCamTexture webcamTexture;
    private Texture2D photo;
    public string localIPAdress;
    public Texture2D preCapturedImage;
    void Start()
    {
        webcamTexture = new WebCamTexture();
        GetComponent<Renderer>().material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            Debug.Log("asd");
            TakePhoto(true);
            this.enabled = false;
        }
            
    }
    public void TakePhoto(bool isImagePrecaptured)
    {
        if(isImagePrecaptured){
            photo = preCapturedImage;

        }else{
            photo = new Texture2D(webcamTexture.width, webcamTexture.height);
            photo.SetPixels(webcamTexture.GetPixels());
            photo.Apply();
        }
        byte[] imageBytes = photo.EncodeToPNG();
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageBytes, "image.png", "image/png");
        WWW www = new WWW("http://"+ localIPAdress + ":5000/upload_image", form);
        StartCoroutine(WaitForRequest(www));
    }

    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            /*Debug.Log("WWW received: " + www.text);
            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(www.text);
            List<int> data = JsonConvert.DeserializeObject<List<int>>(jsonData["data"].ToString());*/
            int[,] gameState = {{0,0,2,0,0},
                                {2,2,0,2,0},
                                {1,2,1,2,2},
                                {2,2,2,2,1},
                                {1,2,0,1,1}};

           /* for(int i=0;i<data.Count;i++)
                Debug.Log(data[i]);*/
            GamePlayDataHolder.Instance.SetSuggestion(gameState, new int[]{4,2}, 1);
            SceneManager.LoadScene("Gameplay");
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
