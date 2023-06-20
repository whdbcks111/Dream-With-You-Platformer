using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToStageButton : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickBacktoStage);
    }

    public void OnClickBacktoStage()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
