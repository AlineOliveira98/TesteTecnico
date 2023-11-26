using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTime;
    [SerializeField] TextMeshProUGUI textPoints;
    [SerializeField] TextMeshProUGUI textPointsFinish;
    [SerializeField] GameObject panelFinish;
    [SerializeField] GameObject effectExplosion;
    [SerializeField] GameObject[] prefabEnemies;

    public List<GameObject> routes;
    public bool gameOver;

    PlayerController playerController;
    float timeValue;
    float spawnTimeValue;
    int points;
    bool generatingEnemy;
    

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        points = 0;
        timeValue = PlayerPrefs.GetFloat("tempoPartida", 60);
        spawnTimeValue = PlayerPrefs.GetFloat("tempoSpawn", 15);

        InvokeRepeating("SpawnEnemy", 1f, spawnTimeValue);
        generatingEnemy = true;
        gameOver = false;
    }

    
    void Update()
    {
        timeValue = timeValue > 0 ? timeValue -= Time.deltaTime : timeValue = 0;

        SetGameOver();        
        Cronometro(timeValue);

        if(!generatingEnemy && routes.Count != 0 && !gameOver){
            InvokeRepeating("SpawnEnemy", 1f, spawnTimeValue);
            generatingEnemy = true;
        }
    }

    void SetGameOver(){
        if(timeValue <= 0 || !playerController.isAlive){
            panelFinish.SetActive(true);
            CancelInvoke("SpawnEnemy");
            gameOver = true;
        }
    }

    void Cronometro(float timeToDisplay){
        if(timeToDisplay < 0)
            timeToDisplay = 0;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        textTime.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    public void SumPoint(){
        points += 1;
        textPoints.text = "Points: " + points;
        textPointsFinish.text = "Points: " + points;
    }

    public void RestartGame(){
        SceneManager.LoadScene(1);
    }

    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }

    public void GerarExplosao(Transform transformEffect){
        GameObject effect = Instantiate(effectExplosion, transformEffect.position, transformEffect.rotation);
        Destroy(effect, 0.35f);
    }

    void SpawnEnemy(){
        if(routes.Count == 0){
            CancelInvoke("SpawnEnemy");
            generatingEnemy = false;
            return;
        }

        Instantiate(prefabEnemies[Random.Range(0, prefabEnemies.Length)]);
    }

    public void OpenClosePanel(GameObject panel){
        panel.SetActive(!panel.activeSelf);
    }
}
