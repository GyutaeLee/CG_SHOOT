using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    private float m_DeltaTime = 0.0f;

    private void Update()
    {
        m_DeltaTime += (Time.unscaledDeltaTime - m_DeltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        int width = Screen.width;
        int height = Screen.height;

        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 0, width, height * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = 20;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = m_DeltaTime * 1000.0f;
        float fps = 1.0f / m_DeltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}