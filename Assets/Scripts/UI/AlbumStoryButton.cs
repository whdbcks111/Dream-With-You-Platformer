using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StoryButton), typeof(Image))]
public class AlbumStoryButton : MonoBehaviour
{
    private StoryButton _storyButton;
    private Image _image;

    private void Awake()
    {
        _storyButton = GetComponent<StoryButton>();
        _image = GetComponent<Image>();

        var stageCleared = PlayerPrefs.GetInt("CollectedPictureCount" + _storyButton.StoryNum) >= 3;

        _image.sprite = Resources.Load<Sprite>(stageCleared ? "AlbumStory/Photo" : "AlbumStory/PhotoLocked");
    }
}
