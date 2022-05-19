using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class playButtonScript : MonoBehaviour
{
    public void changeToGameScene()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
