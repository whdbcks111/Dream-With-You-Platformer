using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageEnterManager : MonoBehaviour
{
    public static StageEnterManager Instance;

    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _progressText;

    private void Awake()
    {
        Instance = this;
        _loadingPanel.SetActive(false);
    }

    public void EnterStage(int num)
    {
        StartCoroutine(EnterSceneRoutine(num));

    }

    private IEnumerator EnterSceneRoutine(int num)
    {
        PlayerPrefs.SetInt("EnterStage", num);
        _loadingPanel.SetActive(true);
        var oper = SceneManager.LoadSceneAsync("InGameScene");
        while(!oper.isDone)
        {
            var progress = oper.progress;
            _progressBar.fillAmount = progress;
            _progressText.SetText(string.Format("{0:0}%", progress * 100));
            yield return null;
        }
    }
}
