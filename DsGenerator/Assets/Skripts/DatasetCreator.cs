using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatasetCreator : MonoBehaviour
{
    [SerializeField] private float waitForSeconds = 1;
    [SerializeField] private string dsSaveFolder;
    [SerializeField] private string datasetsFolder;

    private int pictureNum = 0;

    private void Start()
    {
        dsSaveFolder = Application.dataPath + dsSaveFolder;
        string[] dirs = System.IO.Directory.GetDirectories(dsSaveFolder);
        
        if (dirs.Length == 0)
        {
            
        }
        else
        {
            
        }
        System.IO.Directory.CreateDirectory(dsSaveFolder +"/Dataset_0");

        StartCoroutine(MainLoop());
    }

    private IEnumerator MainLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitForSeconds);
            StartCoroutine(TakeScreenshot());
            pictureNum++;
        }
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Datasets/Dataset_1/img_name_" + pictureNum + ".png", byteArray);
    }
}
