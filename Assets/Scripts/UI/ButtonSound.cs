using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SoundManager.Instance.Play("ButtonClick"));
    }
}
