using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;

public class HighscoreTable : MonoBehaviour
{

    public static HighscoreTable Instance { get; private set; }

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList1;

    private void Awake()
    {
        Instance = this;
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        //PlayerPrefs.DeleteKey("highscoreTable");
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores1 highscores1 = JsonUtility.FromJson<Highscores1>(jsonString);

        transform.Find("Reset").GetComponent<Button_UI>().ClickFunc = () =>
        {
            highscores1 = null; PlayerPrefs.SetString("highscoreTable", ""); PlayerPrefs.Save(); // Reset Scores
            SceneManager.LoadScene(0);
        };
        
        if (highscores1 == null)
        {
            // There's no stored table, initialize
            //Debug.Log("Initializing table with default values...");
            AddHighscoreEntry1(30, "MINH");
            AddHighscoreEntry1(40, "LINH");
            // Reload
            jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores1 = JsonUtility.FromJson<Highscores1>(jsonString);
        }

        RefreshHighscoreTable1();

        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    private void RefreshHighscoreTable1()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores1 highscores1 = JsonUtility.FromJson<Highscores1>(jsonString);

        // Sort entry list by Score
        for (int i = 0; i < highscores1.highscoreEntryList1.Count; i++)
        {
            for (int j = i + 1; j < highscores1.highscoreEntryList1.Count; j++)
            {
                if (highscores1.highscoreEntryList1[j].score < highscores1.highscoreEntryList1[i].score)
                {
                    // Swap
                    HighscoreEntry1 tmp = highscores1.highscoreEntryList1[i];
                    highscores1.highscoreEntryList1[i] = highscores1.highscoreEntryList1[j];
                    highscores1.highscoreEntryList1[j] = tmp;
                }
            }
        }

        if (highscoreEntryTransformList1 != null)
        {
            foreach (Transform highscoreEntryTransform1 in highscoreEntryTransformList1)
            {
                Destroy(highscoreEntryTransform1.gameObject);
            }
        }

        highscoreEntryTransformList1 = new List<Transform>();
        foreach (HighscoreEntry1 highscoreEntry1 in highscores1.highscoreEntryList1)
        {
            CreateHighscoreEntryTransform1(highscoreEntry1, entryContainer, highscoreEntryTransformList1);
        }
    }

    private void CreateHighscoreEntryTransform1(HighscoreEntry1 highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 20.5f;
        Transform entryTransform1 = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform1.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform1.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform1.Find("posText").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform1.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform1.Find("nameText").GetComponent<Text>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform1.Find("background").gameObject.SetActive(rank % 2 == 1);

        // Highlight First
        if (rank == 1)
        {
            entryTransform1.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform1.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform1.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        // Set tropy
        switch (rank)
        {
            default:
                entryTransform1.Find("trophy").gameObject.SetActive(false);
                break;
            case 1:
                entryTransform1.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("FFD200");
                break;
            case 2:
                entryTransform1.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("C6C6C6");
                break;
            case 3:
                entryTransform1.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("B76F56");
                break;

        }

        transformList.Add(entryTransform1);
    }

    public void AddHighscoreEntry1(int score, string name)
    {
        // Create HighscoreEntry
        HighscoreEntry1 highscoreEntry1 = new HighscoreEntry1 { score = score, name = name };

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores1 highscores = JsonUtility.FromJson<Highscores1>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            highscores = new Highscores1()
            {
                highscoreEntryList1 = new List<HighscoreEntry1>()
            };
        }

        // Add new entry to Highscores
        highscores.highscoreEntryList1.Add(highscoreEntry1);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

        RefreshHighscoreTable1();
    }

    private class Highscores1
    {
        public List<HighscoreEntry1> highscoreEntryList1;
    }

    /*
     * Represents a single High score entry
     * */
    [System.Serializable] private class HighscoreEntry1
    {
        public int score;
        public string name;
    }

}