using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    StoryButton _story;
    Image _storySprite;
    Image _stageSprite;
    Button _stageButton;

    int _stageNum;
    bool _stage8Active;

    private void Awake()
    {
        _story = GetComponentInChildren<StoryButton>();
        _storySprite = _story?.gameObject.GetComponent<Image>();
        _stageSprite = GetComponent<Image>();
        _stageButton = GetComponent<Button>();

        _stageNum = _story._storyNum;

        if (!PlayerPrefs.HasKey("CollectedPictureCount" + _stageNum))
        {
            PlayerPrefs.SetInt("CollectedPictureCount" + _stageNum, 0);
        }
        if (!PlayerPrefs.HasKey("ClearedStage"))
        {
            PlayerPrefs.SetInt("ClearedStage", 0);
        }

    }

    void Start()
    {
        if (_stageNum != 8)
        {
            _storySprite.sprite = Resources.Load<Sprite>("Picture/Picture" + PlayerPrefs.GetInt("CollectedPictureCount" + _stageNum));
        }
        if (PlayerPrefs.GetInt("ClearedStage") + 1 == _stageNum)
        {
            if (_stageNum == 8)
            {
                _stage8Active = true;

                if (PlayerPrefs.GetInt("CollectedPictureCount1") != 3)
                {
                    _stage8Active = false;
                }
                if (PlayerPrefs.GetInt("CollectedPictureCount2") != 3)
                {
                    _stage8Active = false;
                }
                if (PlayerPrefs.GetInt("CollectedPictureCount3") != 3)
                {
                    _stage8Active = false;
                }
                if (PlayerPrefs.GetInt("CollectedPictureCount4") != 3)
                {
                    _stage8Active = false;
                }
                if (PlayerPrefs.GetInt("CollectedPictureCount5") != 3)
                {
                    _stage8Active = false;
                }
                if (PlayerPrefs.GetInt("CollectedPictureCount6") != 3)
                {
                    _stage8Active = false;
                }
                if (PlayerPrefs.GetInt("CollectedPictureCount7") != 3)
                {
                    _stage8Active = false;
                }

                if (_stage8Active)
                {
                    _stageSprite.sprite = Resources.Load<Sprite>("Stage/StageChallenge");
                    _stageButton.onClick.AddListener(OnClickStageEnter);
                }
                else
                {
                    StageLocked();
                }
            }
            else
            {
                _stageSprite.sprite = Resources.Load<Sprite>("Stage/StageChallenge");
                _stageButton.onClick.AddListener(OnClickStageEnter);
            }
        }
        else if (PlayerPrefs.GetInt("ClearedStage") >= _stageNum)
        {
            _stageSprite.sprite = Resources.Load<Sprite>("Stage/StageCleared");
            _stageButton.onClick.AddListener(OnClickStageEnter);
        }
        else
        {
            StageLocked();
        }
    }

    public void StageLocked()
    {
        _stageSprite.sprite = Resources.Load<Sprite>("Stage/StageLocked");
        _stageButton.onClick.AddListener(OnClickStageLocked);
    }

    public void OnClickStageEnter()
    {
        PlayerPrefs.SetInt("EnterStage", _stageNum);
        SceneManager.LoadScene("InGameScene");
    }

    public void OnClickStageLocked()
    {
        _story._warnText.text = "개방되지 않은 스테이지입니다.";
        _story.WarnText();
    }
}
