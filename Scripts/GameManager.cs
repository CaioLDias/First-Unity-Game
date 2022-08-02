using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hazardPrefab;
    [SerializeField]
    private int maxHazardsToSpawn = 3;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreText;
    [SerializeField]
    private Image backgroundMenu;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject mainVCam;
    [SerializeField]
    private GameObject zoomVCam;
    [SerializeField]
    private GameObject gameOverMenu;

    private int highScore;
    private int score;
    private float time;
    private Coroutine hazardsCoroutine;
    
    private bool gameOver;

    private static GameManager instance;
    private const string HighScorePreferenceKey = "HighScore";


    public static GameManager Instance => instance;
    public int HighScore => highScore;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        highScore = PlayerPrefs.GetInt(HighScorePreferenceKey);
        Debug.Log(highScore);
    }

    private void OnEnable()
    {
        player.SetActive(true);

        zoomVCam.SetActive(false);
        mainVCam.SetActive(true);

        gameOver = false;
        scoreText.text = "0";
        score = 0;
        time = 0;

        hazardsCoroutine = StartCoroutine(SpawnHazards());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 0)
            {
                Resume();
            }

            if(Time.timeScale == 1)
            {
                Pause();
            }
        }
        
        if(gameOver)
        {
            return;
        }
        
        time += Time.deltaTime;


        if (time >= 1f)
        {
            score ++;
            scoreText.text = score.ToString();

            time = 0;
        }
        
    }

    private void Pause()
    {
        LeanTween.value(0, 1, 0.5f).setOnUpdate(SetTimeScale).setIgnoreTimeScale(true);
        backgroundMenu.gameObject.SetActive(true);
    }

    private void Resume()
    {
        LeanTween.value(0, 1, 0.5f).setOnUpdate(SetTimeScale).setIgnoreTimeScale(true);
        backgroundMenu.gameObject.SetActive(false);
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = 0.02f * value;
    }

    private IEnumerator SpawnHazards()
    {
        var hazardToSpawn = Random.Range(1, maxHazardsToSpawn);

        for (int i = 0; i < hazardToSpawn; i++)
        {
            var x = Random.Range(-7, 7);
            var drag = Random.Range(0f, 2f);
            var hazard = Instantiate(hazardPrefab, new Vector3(x, 10, -1), Quaternion.identity);
            hazard.GetComponent<Rigidbody>().drag = drag;
        }

        yield return new WaitForSeconds(1f);

        yield return SpawnHazards();
    }

    public void GameOver()
    {
        StopCoroutine(hazardsCoroutine);
        gameOver = true;

        if(Time.timeScale < 1)
        {
            Resume();
        }

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScorePreferenceKey, highScore);
            Debug.Log(highScore);
        }

        mainVCam.SetActive(false);
        zoomVCam.SetActive(true);

        gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    } 

}
