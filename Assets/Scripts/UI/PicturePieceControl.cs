using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicturePieceControl : MonoBehaviour
{
    PicturePiece[] _picturePieces;
    Image _image;

    public int _collectedPicture;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("EnterStage") == 8)
        {
            Destroy(gameObject);
        }
        _picturePieces = FindObjectsOfType<PicturePiece>();
        _image = GetComponent<Image>();
        _collectedPicture = 0;
    }

    void Start()
    {
        if (_picturePieces.Length != 3)
        {
            Debug.LogError("���� ���� ���� �̴� : " + _picturePieces.Length + "��");
        }

        _image.sprite = Resources.Load<Sprite>("Picture/Picture" + _collectedPicture);
    }

    public void PictureCollected()
    {
        _collectedPicture++;
        _image.sprite = Resources.Load<Sprite>("Picture/Picture" + _collectedPicture);
    }
}
