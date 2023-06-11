using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicturePiece : MonoBehaviour
{
    PicturePieceControl _pictureControl;

    private void Awake()
    {
        _pictureControl = FindObjectOfType<PicturePieceControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _pictureControl.PictureCollected();
            Destroy(gameObject);
        }
    }
}
