using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryButton : CanvasOnOff
{
    [SerializeField] int _storyNum;
    Image _storyImage;
    Sprite _sprite;

    private protected override void Awake()
    {
        base.Awake();
        _storyImage = GameObject.Find("CutScene").GetComponent<Image>();
        _sprite = Resources.Load<Sprite>("Story/StoryCutScene"+_storyNum);
    }

    protected override void Start()
    {
        base.Start();
        _button.onClick.AddListener(OnClickStoryButton);

        if (PlayerPrefs.GetInt("CollectedPictureCount" + _storyNum) != 3)
        {
            _button.onClick.RemoveAllListeners();
        }
    }

    public void OnClickStoryButton()
    {
        _storyImage.sprite = _sprite;
    }
}
