using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryButton : CanvasOnOff
{
    [SerializeField] public int StoryNum;
    [SerializeField] public TextMeshProUGUI WarnText;
    
    private Image _storyImage;
    private Sprite _sprite;
    private IEnumerator _warnRoutine;

    protected override void Awake()
    {
        if (StoryNum != 8)
        {
            base.Awake();
        }
        _storyImage = GameObject.Find("CutScene").GetComponent<Image>();
        _sprite = Resources.Load<Sprite>("Story/StoryCutScene"+StoryNum);
    }

    protected override void Start()
    {
        if (StoryNum != 8)
        {
            base.Start();
        }
        _button?.onClick.AddListener(OnClickStoryButton);
        WarnText.text = "";

        if (PlayerPrefs.GetInt("CollectedPictureCount" + StoryNum) != 3)
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
        WarnText.text = "사진 조각이 부족합니다.";
        Warn();
    }

    public void Warn() 
    {
        if (_warnRoutine != null) StopCoroutine(_warnRoutine);
        WarnText.color = new Color(WarnText.color.r, WarnText.color.b, WarnText.color.g, 1);
        _warnRoutine = WarnRoutine();
        StartCoroutine(_warnRoutine);
    }

    IEnumerator WarnRoutine()
    {
        var col = WarnText.color;
        while(WarnText.color.a > 0)
        {
            yield return null;
            col.a -= Time.deltaTime;
            WarnText.color = col;
        }
    }
}
