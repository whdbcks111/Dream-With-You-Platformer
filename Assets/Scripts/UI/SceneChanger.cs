using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] string _sceneName;
    [SerializeField] KeyCode _triggerKey;

    private void Update()
    {
        if(Input.GetKeyDown(_triggerKey))
        {
            SceneManager.LoadSceneAsync(_sceneName);
        }
    }
}
