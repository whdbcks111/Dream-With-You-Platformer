using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageControl : MonoBehaviour
{
    [SerializeField] Text _text;

    int _currentStage;

    void Awake()
    {
        _currentStage = PlayerPrefs.GetInt("EnterStage");
        _text.text = "Stage " + _currentStage;
        Instantiate(Resources.Load("StagePrefabs/Stage" + _currentStage));
    }
}
