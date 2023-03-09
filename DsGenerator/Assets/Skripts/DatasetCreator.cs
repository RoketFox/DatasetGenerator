using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatasetCreator : MonoBehaviour
{
    [SerializeField] private float waitForSeconds = 1;
    [SerializeField] private string dsSaveFolder;
    
    private string currDatasetFolder;

    private int pictureNum = 0;

    private void Start()
    {
        string[] dirs = System.IO.Directory.GetDirectories(dsSaveFolder);
        
        if (dirs.Length == 0)
        {
            currDatasetFolder = dsSaveFolder + "/Dataset_0";
        }
        else
        {
            string lastDs = dirs[dirs.Length - 1];
            string lastDsName = lastDs.Substring(lastDs.Length - 10);

            currDatasetFolder = dsSaveFolder + lastDsName.Substring(0, 9) + (Convert.ToInt32(lastDsName.Substring(9)) + 1);
        }

        Debug.Log("Created new folder: " + currDatasetFolder);
        System.IO.Directory.CreateDirectory(currDatasetFolder);

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
        System.IO.File.WriteAllBytes(currDatasetFolder + "/img_name_" + pictureNum + ".png", byteArray);
        Debug.Log("Created screenshot: " + pictureNum);
    }
}
