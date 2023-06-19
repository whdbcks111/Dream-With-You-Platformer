using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempStageControl : MonoBehaviour
{
    [SerializeField] int stage;

    void Awake()
    {
        PlayerPrefs.SetInt("EnterStage", stage);
    }
}
