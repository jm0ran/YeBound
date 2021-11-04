using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioTestController : MonoBehaviour
{
    public AudioSource testAudio;
    public GameObject testObj;
    public beatMap mainMap;

    //User Defined Functions
    void visualizeNote(){
        if(mainMap.map.Count > 0 && testAudio.time > mainMap.map[0].time){
            Debug.Log(mainMap.map.Count);
            mainMap.map.RemoveAt(0);
            testObj.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(ExecuteAfterTime());
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        //Creates and starts audio
        testAudio = gameObject.GetComponent<AudioSource>();
        testAudio.Play();

        //Imports the beatMap's json file which holds the information on each note
        mainMap = new beatMap();
        mainMap.readBeatMap("Runaway.json");
    }

    // Update is called once per frame
    void Update()
    {
        visualizeNote();
        if(Input.GetKey("p")){
            testAudio.Pause();
        }else if(Input.GetKey("o")){
            testAudio.UnPause();
        }
        if(Input.GetKeyDown("space")){
            Debug.Log(testAudio.time);
        }
    }
    void FixedUpdate(){
        //Debug.Log(testAudio.time);
    }

    IEnumerator ExecuteAfterTime(){
        yield return new WaitForSeconds(0.2f);
        testObj.GetComponent<SpriteRenderer>().enabled = true;
    }
}