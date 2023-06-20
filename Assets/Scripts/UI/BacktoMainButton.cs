using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BacktoMainButton : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickBacktoMain);
    }

    public void OnClickBacktoMain()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
