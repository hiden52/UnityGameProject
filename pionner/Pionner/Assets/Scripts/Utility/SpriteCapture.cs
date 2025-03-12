using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteCapture : MonoBehaviour
{
    public Camera renderCamera;

    [Header("3D Model to capture")]
    public GameObject target;

    [Header("Set True to Capture")]
    public bool Capture;

    void Start()
    {
        Capture = false;
    }

    private void Update()
    {
        if(Capture)
        {
            CaptureScreenshot();
            Capture = false;
        }
    }

    void CaptureScreenshot()
    {
        if(target == null)
        {
            Debug.LogWarning("Missing target model reference!");
            return;
        }
        Debug.Log("Start to Capture");
        renderCamera.clearFlags = CameraClearFlags.SolidColor;
        renderCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);

        // ���� ��� ����
        string screenshotPath = (Application.dataPath + "/Sprites/" + target.name + ".png");
        // ������ ������ ����
        if (!System.IO.Directory.Exists(Application.dataPath + "/Sprites"))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Sprites");
        }

        // ī�޶� ���� ��ũ���� ĸó
        RenderTexture renderTexture = new RenderTexture(512, 512, 24);  // ĸó�� �ػ� ����
        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();

        // �ؽ�ó�� ��ȯ
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenshot.Apply();

        // �̹��� ����
        Debug.Log("Save 2D Sprite " + target.name);
        byte[] bytes = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes(screenshotPath, bytes);

        // Clean up
        renderCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
    }
}
