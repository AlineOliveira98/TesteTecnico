using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI textPoints;
    public TextMeshProUGUI textPointsFinish;
    public GameObject panelFinish;
    public GameObject effectExplosion;
    public GameObject[] prefabEnemies;
    public List<GameObject> rotas;
    public float timeValue;
    public float spawnTimeValue;
    public int points;
    bool gerandoInimigo;
    public bool gameOver;
    public PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        points = 0;
        timeValue = PlayerPrefs.GetFloat("tempoPartida", 60);
        spawnTimeValue = PlayerPrefs.GetFloat("tempoSpawn", 15);

        InvokeRepeating("SpawnEnemy", 1f, spawnTimeValue);
        gerandoInimigo = true;
        gameOver = false;
    }

    
    void Update()
    {
        timeValue = timeValue > 0 ? timeValue -= Time.deltaTime : timeValue = 0;

        SetGameOver();        
        Cronometro(timeValue);

        if(!gerandoInimigo && rotas.Count != 0 && !gameOver){
            InvokeRepeating("SpawnEnemy", 1f, spawnTimeValue);
            gerandoInimigo = true;
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
        if(rotas.Count == 0){
            CancelInvoke("SpawnEnemy");
            gerandoInimigo = false;
            return;
        }

        Instantiate(prefabEnemies[Random.Range(0, prefabEnemies.Length)]);
    }

    public void OpenClosePanel(GameObject panel){
        panel.SetActive(!panel.activeSelf);
    }
}
