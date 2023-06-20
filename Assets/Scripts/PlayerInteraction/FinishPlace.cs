using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlace : MonoBehaviour
{
    StageClear _stageClear;

    private void Awake()
    {
        _stageClear = FindObjectOfType<StageClear>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameCleared();
        }
    }

    void GameCleared()
    {
        SoundManager.Instance.Play("Clear");
        Time.timeScale = 0;
        _stageClear.StageCleared();
    }
}
