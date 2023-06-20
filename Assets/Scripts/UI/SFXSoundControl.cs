using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXSoundControl : MonoBehaviour
{
    Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        _slider.value = SoundManager.Instance.VolumeMultiplier;

    }
    void Update()
    {
        
    }

    public void OnSFXChange()
    {
        SoundManager.Instance.VolumeMultiplier = _slider.value;
    }
}
