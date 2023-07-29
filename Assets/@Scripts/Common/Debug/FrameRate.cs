using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    private float deltaTime = 0f;
    
    void Start()
    {
        if (fpsText == null)
        {
            fpsText = GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        if (fpsText != null)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            fpsText.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        }
    }
}
