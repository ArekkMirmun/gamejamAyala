using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeChangeScript : MonoBehaviour
{
    //In this script, we manage the time of the day in the game
    //We start at 12:00 and we go through the day until 24:00
    //We use a float to keep track of the time
    //We want the level to take 4 minutes to go from 12:00 to 24:00
    
    public static TimeChangeScript Instance;
    public TextMeshProUGUI timeText;
    public float time;
    public float timeSpeed = 1f;
    public float hour;
    public float minutes;
    public bool isPaused = false;

    public GameObject midnightIntro;
    public GameObject bossPrefab;

    public GameObject battleHUD;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        time = 12.00f;
        minutes = 0;
        //Resume time when the game starts
        ResumeTime();
        //Sets time scale
        Time.timeScale = 1;
    }
    
    // Update is called once per frame
    //if time reaches 24:00, we pause the timer

    void Update()
    {
        if (!isPaused)
        {
            //rotate the directional light around the level to simulate the time of the day
            RenderSettings.sun.transform.Rotate(Vector3.right * (Time.deltaTime * timeSpeed / 6));
            
            time += Time.deltaTime * timeSpeed / 60;
            hour = Mathf.FloorToInt(time);
            minutes = Mathf.FloorToInt((time - hour) * 60);
            //format minutes to always have 2 digits
            string minutesString = minutes.ToString("00");
            timeText.text = hour + ":" + minutesString;
            if (time >= 24)
            {
                isPaused = true;
                time = 0;
                StartCoroutine(MidnightEvent());
            }
        }
    }

    private IEnumerator MidnightEvent()
    {
        BattleSystem.Instance.PlayBossMusic();
        //Do something when it's midnight
        print("It's midnight");
        midnightIntro.SetActive(true);
        yield return new WaitForSeconds(12f);
        //Start the boss fight
        
        PlayerInfo.Instance.EnemyEncountered(bossPrefab);
        yield return new WaitForSeconds(2f);
        midnightIntro.SetActive(false);
    } 
    
    
    public void PauseTime()
    {
        isPaused = true;
    }
    
    public void ResumeTime()
    {
        isPaused = false;
    }
    
}
