using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    public GameObject panelConfig;
    string playPrefTimeMatch = "tempoPartida";
    string playPrefTimeSpawnEnemy = "tempoSpawn";

    public Slider sliderTimeMatch;
    public TextMeshProUGUI textTimeMatch;
    public Slider sliderTimeSpawn;
    public TextMeshProUGUI textTimeSpawn;

    void Start(){
        GetTimeMatch();
        GetTimeSpawn();
    }

    public void LoadUnloadConfig(){
        panelConfig.SetActive(!panelConfig.activeSelf);
        
    }

    public void LoadGamePlay(){
        SceneManager.LoadScene(1);
    }

    public void SetTimeMatch(){
        float optionSelected = sliderTimeMatch.value;
        PlayerPrefs.SetFloat(playPrefTimeMatch, optionSelected);
        textTimeMatch.text = optionSelected.ToString();
        Debug.Log("Set Time Match: " + optionSelected);
    }

    public void SetTimeSpawn(){
        float optionSelected = sliderTimeSpawn.value;
        PlayerPrefs.SetFloat(playPrefTimeSpawnEnemy, optionSelected);
        textTimeSpawn.text = optionSelected.ToString();
        Debug.Log("Set Time Match: " + sliderTimeSpawn.value);
    }
    public void GetTimeMatch(){
        float timeSelected = PlayerPrefs.GetFloat(playPrefTimeMatch, 60);
        sliderTimeMatch.value = timeSelected;
        textTimeMatch.text = timeSelected.ToString();
        Debug.Log("Get Time Match: " + timeSelected);
    }

    public void GetTimeSpawn(){
        sliderTimeSpawn.value = PlayerPrefs.GetFloat(playPrefTimeSpawnEnemy, 15);
        textTimeSpawn.text = sliderTimeSpawn.value.ToString();
        Debug.Log("Get Time Spawn: " + sliderTimeSpawn);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void OpenClosePanel(GameObject panel){
        panel.SetActive(!panel.activeSelf);
    }

}
