using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossSceneManagement : MonoBehaviour
{
    [HideInInspector]public Database database;
    [HideInInspector]public PointHolder pointHolder;
    private static int total = 0;
    [SerializeField] private string previousSceneName;
    [HideInInspector]public int previousBattleSceneLevel;

    private void Awake()
    {
        if (total == 0)
        {
            total++;
            DontDestroyOnLoad(gameObject);

            database = transform.GetChild(0).GetComponent<Database>();
            database.SetUp();
            pointHolder = transform.GetChild(1).GetComponent<PointHolder>();
            pointHolder.SetUp();
        }
        else
        {
            Destroy(gameObject);
        }


    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetObjectActivation(scene.name);
    }

    public void LoadScene(string sceneName)
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void SetObjectActivation(string sceneName)
    {
        switch (sceneName)
        {
            case "BigMap":
                database.gameObject.SetActive(false);
                pointHolder.gameObject.SetActive(true);
                if (previousSceneName == "BattleScene")
                {
                    pointHolder.ExpandLevel();
                }
                pointHolder.Initialize();
                break;
            case "BattleScene":
                database.gameObject.SetActive(true);
                database.Initialize();
                pointHolder.gameObject.SetActive(false);
                break;
            case "Shop":
                pointHolder.gameObject.SetActive(false);
                database.gameObject.SetActive(true);
                break;
        }
    }
}
