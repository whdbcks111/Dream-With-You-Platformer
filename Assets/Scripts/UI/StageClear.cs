using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageClear : MonoBehaviour
{
    CanvasGroup _canvas;
    Button _backToStageSelectButton;
    Text _collectedPictureCount;
    Image _collectedPictureCountImage;

    PicturePieceControl _pictureControl;

    int _currentStage;

    private void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
        _backToStageSelectButton = GetComponentInChildren<Button>();
        _collectedPictureCount = GameObject.Find("PicturePieceText").GetComponent<Text>();
        _collectedPictureCountImage = GameObject.Find("PicturePieceImage").GetComponent<Image>();
        _pictureControl = FindObjectOfType<PicturePieceControl>();
        _currentStage = PlayerPrefs.GetInt("EnterStage");
    }

    void Start()
    {
        OffCanvas();
        _backToStageSelectButton.onClick.AddListener(OnClickBackToStageSelect);
    }

    public void StageCleared()
    {
        OnCanvas();
        _collectedPictureCount.text = _pictureControl._collectedPicture.ToString() + " / 3";
        _collectedPictureCountImage.sprite = Resources.Load<Sprite>("Picture/Picture" + _pictureControl._collectedPicture);
    }

    public void OnClickBackToStageSelect()
    {
        Time.timeScale = 1;
        if (PlayerPrefs.GetInt("ClearedStage") < _currentStage)
        {
            PlayerPrefs.SetInt("ClearedStage", _currentStage);
        }
        if (PlayerPrefs.GetInt("CollectedPictureCount"+_currentStage) < _pictureControl._collectedPicture)
        {
            PlayerPrefs.SetInt("CollectedPictureCount" + _currentStage, _pictureControl._collectedPicture);
        }

        SceneManager.LoadScene("StageSelectScene");
    }

    public void CanvasControl()
    {
        if (_canvas.alpha == 1)
        {
            OffCanvas();
        }
        else
        {
            OnCanvas();
        }
    }
    void OnCanvas()
    {
        _canvas.alpha = 1;
        _canvas.interactable = true;
        _canvas.blocksRaycasts = true;
    }
    void OffCanvas()
    {
        _canvas.alpha = 0;
        _canvas.interactable = false;
        _canvas.blocksRaycasts = false;
    }
}
