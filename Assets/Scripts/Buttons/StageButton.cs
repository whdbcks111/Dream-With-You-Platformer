using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        _story = GetComponentInChildren<StoryButton>();
        _storySprite = _story.gameObject.GetComponent<Image>();
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
        _storySprite.sprite = Resources.Load<Sprite>("Picture/Picture" + PlayerPrefs.GetInt("CollectedPictureCount" + _stageNum));
        if (PlayerPrefs.GetInt("ClearedStage") + 1 == _stageNum)
        {
            _stageSprite.sprite = Resources.Load<Sprite>("Stage/StageChallenge");
            _stageButton.onClick.AddListener(OnClickStageEnter);
        }
        else if (PlayerPrefs.GetInt("ClearedStage") >= _stageNum)
        {
            _stageSprite.sprite = Resources.Load<Sprite>("Stage/StageCleared");
            _stageButton.onClick.AddListener(OnClickStageEnter);
        }
        else
        {
            _stageSprite.sprite = Resources.Load<Sprite>("Stage/StageLocked");
            _stageButton.onClick.AddListener(OnClickStageLocked);
        }
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
