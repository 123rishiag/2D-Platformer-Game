using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour
{
    private Button button;
    public string buttonType;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        if (buttonType == "Level")
        {
            int index = transform.GetSiblingIndex();
            SceneManager.LoadScene(index + 1);
        }
        else if (buttonType == "Back")
        {
            SceneManager.LoadScene(0);
        }
    }
}
