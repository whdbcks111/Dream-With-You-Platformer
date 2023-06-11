using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryButton : CanvasOnOff
{
    [SerializeField] public int _storyNum;
    [SerializeField] public Text _warnText;
    Image _storyImage;
    Sprite _sprite;

    private protected override void Awake()
    {
        if (_storyNum != 8)
        {
            base.Awake();
        }
        _storyImage = GameObject.Find("CutScene").GetComponent<Image>();
        _sprite = Resources.Load<Sprite>("Story/StoryCutScene"+_storyNum);
    }

    protected override void Start()
    {
        if (_storyNum != 8)
        {
            base.Start();
        }
        _button?.onClick.AddListener(OnClickStoryButton);
        _warnText.text = "";

        if (PlayerPrefs.GetInt("CollectedPictureCount" + _storyNum) != 3)
        {
            //컷씬 잠금해제 안됨
            _button?.onClick.RemoveAllListeners();
            _button?.onClick.AddListener(OnClickStoryLocked);
        }
    }

    public void OnClickStoryButton()
    {

        _storyImage.sprite = _sprite;
    }

    public void OnClickStoryLocked()
    {
        _warnText.text = "사진 조각이 부족합니다.";
        WarnText();
    }

    public void WarnText()
    {
        _warnText.color = new Color(_warnText.color.r, _warnText.color.b, _warnText.color.g, 1);
        StartCoroutine(StoryLockedText());
    }

    IEnumerator StoryLockedText()
    {
        yield return new WaitForFixedUpdate();

        _warnText.color = new Color(_warnText.color.r, _warnText.color.b, _warnText.color.g, _warnText.color.a - Time.deltaTime);

        if (_warnText.color.a <= 0)
        {
            yield return null;
        }
        else
        {
            StartCoroutine(StoryLockedText());
        }
    }
}
