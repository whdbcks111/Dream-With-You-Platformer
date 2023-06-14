using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    private StoryButton _story;
    private Image _storySprite;
    private Image _stageSprite;
    private Button _stageButton;

    private int _stageNum;

    private void Awake()
    {
        _story = GetComponentInChildren<StoryButton>();
        _storySprite = _story is not null ? _story.GetComponent<Image>() : null;
        _stageSprite = GetComponent<Image>();
        _stageButton = GetComponent<Button>();

        _stageNum = _story.StoryNum;

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
                bool flag = true;

                for(int i = 1; i < 7; ++i)
                {
                    if (PlayerPrefs.GetInt("CollectedPictureCount" + i, 0) < 3)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag) MarkAsChallengeStage();
                else MarkAsLockedStage();
            }
            else MarkAsChallengeStage();
        }
        else if (PlayerPrefs.GetInt("ClearedStage") >= _stageNum) MarkAsClearedStage();
        else MarkAsLockedStage();
    }

    public void MarkAsClearedStage()
    {
        _stageSprite.sprite = Resources.Load<Sprite>("Stage/StageCleared");
        _stageButton.onClick.AddListener(OnClickStageEnter);
    }

    public void MarkAsChallengeStage()
    {
        _stageSprite.sprite = Resources.Load<Sprite>("Stage/StageChallenge");
        _stageButton.onClick.AddListener(OnClickStageEnter);
    }

    public void MarkAsLockedStage()
    {
        _stageSprite.sprite = Resources.Load<Sprite>("Stage/StageLocked");
        _stageButton.onClick.AddListener(OnClickStageLocked);
    }

    public void OnClickStageEnter()
    {
        StageEnterManager.Instance.EnterStage(_stageNum);
    }

    public void OnClickStageLocked()
    {
        _story.WarnText.text = "개방되지 않은 스테이지입니다.";
        _story.Warn();
    }
}
