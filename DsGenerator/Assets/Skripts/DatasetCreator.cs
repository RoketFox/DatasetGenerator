using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Structs;


public class DatasetCreator : MonoBehaviour
{
    [SerializeField] private bool takeScreenshot = true;
    [SerializeField] private bool createJson = true;
    [SerializeField] private bool logging = false;
    [Space]
    [SerializeField] private float waitForSeconds = 1;
    [Space]
    [SerializeField] private string dsSaveFolder;
    [Tooltip("Choose name once for one save directory.")]
    [SerializeField] private string dsName = "Dataset";
    [SerializeField] private string imgName = "img_name";
    [SerializeField] private string jsonName = "BboxesData";
    [SerializeField] private Vector2 screenWH = new Vector2(640, 480);
    private GameObject[] Bboxes;
    private string dsFolder;
    private Camera cam;
    private int currImgNum = 0;
    private SaveDataObject saveData = new SaveDataObject();


    private void Start()
    {
        Bboxes = GameObject.FindGameObjectsWithTag("Bbox");
        cam = Camera.main;
        dsName = $"/{dsName}_";
        imgName = $"/{imgName}_";
        jsonName = $"/{jsonName}";

        CreateNewDsDirectory();
        StartCoroutine(MainLoop());
    }

    private IEnumerator MainLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitForSeconds);
            if (createJson) GetBbox();
            if (takeScreenshot) StartCoroutine(TakeScreenshot());
            currImgNum++;
        }
    }

    private void OnApplicationQuit()
    {
        SaveDataToJson();
    }

    private void CreateNewDsDirectory()
    {
        string[] dirs = System.IO.Directory.GetDirectories(dsSaveFolder);

        if (dirs.Length == 0)
        {
            dsName += "0";
            dsFolder = dsSaveFolder + dsName;
        }
        else
        {
            string lastDsPath = dirs[dirs.Length - 1];
            char[] lastDsPathChar = lastDsPath.ToCharArray();
            Array.Reverse(lastDsPathChar);
            string lastDsPathR = new string(lastDsPathChar);
            string lastDsNum = lastDsPath.Substring(lastDsPath.Length - lastDsPathR.IndexOf("_"));
            dsName = dsName + (int.Parse(lastDsNum) + 1);
            dsFolder = dsSaveFolder + dsName;
        }

        System.IO.Directory.CreateDirectory(dsFolder);
        if (logging) Debug.Log("Created new folder: " + dsFolder);
    }

    //{
    //"img" : {"class":class, "bbox":[[x1, y1], [x2, y2]]},
    //}

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
        System.IO.File.WriteAllBytes(dsFolder + imgName + currImgNum + ".png", byteArray);
        if (logging) Debug.Log("Created screenshot: " + currImgNum);
    }

    private void GetBbox()
    {
        foreach (GameObject bbox in Bboxes)
        {
            Vector2[] outCorners = new Vector2[2];
            Transform[] bboxCorners = bbox.GetComponentsInChildren<Transform>();
            uint numNotInView = 0;
            for(int i = 0; i < 2; i++)
            {
                Vector2 screenPos = cam.WorldToScreenPoint(bboxCorners[i].transform.position);
                screenPos.x = Mathf.Floor(Mathf.Clamp(screenPos.x, 0, screenWH.x));
                screenPos.y = Mathf.Floor(screenWH.y - Mathf.Clamp(screenPos.y, 0, screenWH.y));
                if(screenPos.x == 0 ||
                   screenPos.y == 0 ||
                   screenPos.x == screenWH.x ||
                   screenPos.y == screenWH.y )
                    numNotInView++;

                outCorners[i] = screenPos;
            }

            if (numNotInView == 2) continue;

            DataEntry newData = new DataEntry();
            newData.img_name = imgName.Substring(1) + currImgNum;
            newData.obstacle_type = bbox.GetComponent<Obstacle>().oblType.ToString();
            newData.bbox = outCorners;

            saveData.dataEntries.Add(newData);

            if (logging) Debug.Log(string.Concat(outCorners));
        }
    }

    private void SaveDataToJson()
    {
        string newJsonString = JsonUtility.ToJson(saveData);

        string jsonPath = dsFolder + jsonName + ".json";
        File.WriteAllText(jsonPath, newJsonString);
    }

    public class AddData
    {
    // Метод для добавления новой записи в файл JSON
        public void AddNewData()
        {
            string filePath = Application.persistentDataPath + "/gameData.json";

            if (!File.Exists(filePath))
            {
                SaveDataObject newData = new SaveDataObject();
                // newData.playerScore = 0;
                // newData.playerName = "";
                // newData.hasCompletedLevel = false;
                string jsonString = JsonUtility.ToJson(newData);
                File.WriteAllText(filePath, jsonString);
            }

            string fileContent = File.ReadAllText(filePath);
            SaveDataObject dataObject = JsonUtility.FromJson<SaveDataObject>(fileContent);

            // Создать новую запись
            DataEntry newDataEntry = new DataEntry();
            // newDataEntry.date = System.DateTime.Now.ToString();
            // newDataEntry.score = 100;
            // newDataEntry.level = "Level 1";

            // dataObject.dataEntries.Add(newDataEntry);

            string newJsonString = JsonUtility.ToJson(dataObject);

            File.WriteAllText(filePath, newJsonString);
        }
    }

    [System.Serializable]
    public class SaveDataObject
    {
        public List<DataEntry> dataEntries = new List<DataEntry>();
    }

    [System.Serializable]
    public class DataEntry
    {
        public string img_name;
        public string obstacle_type;
        public Vector2[] bbox = new Vector2[2];
    }
}
