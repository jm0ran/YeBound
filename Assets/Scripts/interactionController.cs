using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionController : MonoBehaviour
{
    public float interactionRange; //Want to implement later
    public bool talks;
    public bool item;
    public List<string> dia; //Creates a list for the Dialogue
    public GameObject UI;
    
    void Awake(){
        UI = GameObject.FindWithTag("UI");
    }


    void renderSpeech(){
        foreach (string line in dia){
            Debug.Log(line);
        }
        UI.SetActive(true);
        //This will be what creates and renders the dialougue, want to seperate the speech function

        //Steps:

        //Enter a locked state where player cannot make any actions
        //Intend to take a list with strings of text split up for dialouge differences and cycle through waiting for the next keyprompt to continue
        //After cycling through speech, check if the interactable object has an item and if so give it to the player
        //Reenable player movement



    }


    void printHi(GameObject player){
        gameObject.SetActive(false);
        player.SendMessage("lockPlayer", true);
        renderSpeech();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
