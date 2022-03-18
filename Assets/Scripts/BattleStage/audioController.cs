using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{
//------------------------------------------------------------------------
//Main Variables Used in Scripts
    public static float songTime = 0;

    public static AudioSource mainSong;
    public GameObject arrowPrefab;
    public beatMap mainMap;
    public float delayStart;
    public float timeToTarget;
    public float noteSpawnX;
    private float noteSpawnY;
    public float noteTargetX;
    public float customStartTime = 0;

    private bool noteLocked = false;

//------------------------------------------------------------------------
//User Defined Functions
    void noteSpawner(){ //Tracks current progress in song and spawns new notes accordingly      
        songTime = mainSong.time;
        if(mainMap.map.Count > 0 && !noteLocked){
            if(mainMap.map[0].time - timeToTarget - customStartTime <= 0){
                if((Time.timeSinceLevelLoad + customStartTime) > (mainMap.map[0].time + delayStart - timeToTarget)){
                    newArrow(mainMap.map[0].time, mainMap.map[0].button);
                }
            }
            else if(!(mainMap.map[0].time - timeToTarget - customStartTime <= 0)){
                if((mainMap.map[0].time - timeToTarget) < mainSong.time){
                    newArrow(mainMap.map[0].time, mainMap.map[0].button);
                }
            }
            else{ //If it doesn't meet either conditions above remove the note bc something is prob wrong
                mainMap.map.RemoveAt(0);
            }
        }
    }

    void newArrow(float triggerTime, string button){ //This is my function that is going to instantiate my arrow
        mainMap.map.RemoveAt(0);
        GameObject newArrow = Instantiate(arrowPrefab); //This is what creates an arrow, this is also what I'll be doing with my script
        noteSpawnY = 0f; //Default in case beatmap is wrong
        switch(button){
            case "up":
                noteSpawnY = 1.5f;
                break;
            case "down":
                noteSpawnY = 0.5f;
                break;
            case "right":
                noteSpawnY = -0.5f;
                break;
            case "left":
                noteSpawnY = -1.5f;
                break;
        }
        
        newArrow.transform.position = new Vector3(noteSpawnX,noteSpawnY,0); //Want to change this based on note type
        noteController newArrowNC = newArrow.GetComponent<noteController>(); 
        newArrowNC.triggerTime = triggerTime;
        newArrowNC.button = button;
        newArrowNC.timeToTarget = timeToTarget;
        newArrowNC.trackDistance = noteSpawnX - noteTargetX;
        
    }

    IEnumerator startMusic(){
        yield return new WaitForSeconds(delayStart);
        //Starts audio
        mainSong.time = 0 + customStartTime; //Start at a custom start time if necessary
        mainSong.Play();
    }

    void pruneNotesFrom(float prunePoint){
        if(mainMap.map.Count > 0){
            while(mainMap.map[0].time <= prunePoint){
                mainMap.map.RemoveAt(0);
            }
        }
        
    }

    void isAction(bool state){ //Function that is going to clear notes on field, lock notespawner and iniate transition to action stage
        noteLocked = state;
    }



//------------------------------------------------------------------------
//Unity Defined functions
    // Start is called before the first frame update
    void Start()
    {
        mainSong = gameObject.GetComponent<AudioSource>();
        //Imports the beatMap's json file which holds the information on each note
        mainMap = new beatMap();
        mainMap.readBeatMap("Bring Me Down.json");
        pruneNotesFrom(customStartTime);
        StartCoroutine(startMusic());
       
    }

    // Update is called once per frame
    void Update()
    {
        noteSpawner();
        if(Input.GetKey("p")){
            mainSong.Pause();
        }else if(Input.GetKey("o")){
            mainSong.UnPause();
        }
    }
}