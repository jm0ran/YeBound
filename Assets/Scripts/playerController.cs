using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public float moveSpeed = 3f; //Used to control the player's movement speed
    public bool turnLocked; //Condition can be used to block movement
    public Rigidbody2D rb; //Allows script access to objects rigid body
    public Animator animator; //Allows script access to animator
    public Vector2 movement;
    public string lastKey; //String to store the last key you pressed
    public float horizontalSpeed;
    public float verticalSpeed;
    public float totSpeed;

    //Used for interaction
    public GameObject currentInterObj;
    public bool locked = false;

    //Slighly different from start but also runs before the game starts
    void Awake(){
        animator = GetComponent<Animator>(); //Grabs the animator component from the object that this script is attached to
    }

    //Fixed refresh time seperate from framerate
    void FixedUpdate(){
        checkMovement();  
    }
    
    void Update(){
        checkActions();
    }

    //Will be used to lock player movement and actions during dialouge and the sorts
    void lockPlayer(bool state){
        locked = state;
        animator.SetFloat("horizontalSpeed", 0);
        animator.SetFloat("verticalSpeed", 0);
        animator.SetFloat("totSpeed", 0);
    }


    //Used to check actions for non movement related tasks
    void checkActions()
    {
        if(Input.GetKeyDown(KeyCode.E) && currentInterObj != null && !locked){ //Checks for keypress as well as that you are in range of an interactable object
            currentInterObj.SendMessage("startDia", gameObject);
        }
    }


    //Move function accepts different directions
    void checkMovement()
    {
        if(!locked)
        {
            //I want to be careful of this code here, dont want it to cause jitters, it should but idk
            movement.y = 0;
            movement.x = 0;

            //Heres the basic movement functions, very simple move controller, I want to lock into one direction, but am going to work with animator first
            
            //This is the entrance point for movement, the system is designed so you can only move in one direction at a time
            if(!turnLocked){
                if(Input.GetKey("up"))
                {
                    movement.y = 1;
                    lastKey = "up";
                }else if(Input.GetKey("down"))
                {
                    movement.y = -1;
                    lastKey = "down";
                }else if(Input.GetKey("left"))
                {
                    movement.x = -1;
                    lastKey = "left";
                }else if(Input.GetKey("right"))
                {
                    movement.x = 1;
                    lastKey = "right";
                }

                //Locks turning
                if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right")){
                    turnLocked = true;
                }
            }
            //Triggered once a key has been held down, just lets you continue in that general direction
            else{
                if(Input.GetKey(lastKey)){
                    if(lastKey == "up"){
                        movement.y = 1;
                    }else if(lastKey == "down"){
                        movement.y = -1;
                    }else if(lastKey == "left"){
                        movement.x = -1; 
                    }else if(lastKey == "right"){
                        movement.x = 1;
                    }
                
                }else{
                    turnLocked = false;
                }
            }

            //Below is where we define floats that the animation controller uses to manage states
            horizontalSpeed = movement.x * moveSpeed;
            verticalSpeed = movement.y * moveSpeed ;
            totSpeed = horizontalSpeed + verticalSpeed;
            if(!locked){
                animator.SetFloat("horizontalSpeed", horizontalSpeed);
                animator.SetFloat("verticalSpeed", verticalSpeed);
                animator.SetFloat("totSpeed", totSpeed);
            }
        
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    //Interaction Section ---------------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D other){ //Runs when there is a collision between a trigger and regular collider
        if (other.tag == "Interactable"){
            currentInterObj = other.gameObject; //Stores collided object assuming it is tagged "Interactable"
        }
    }
    void OnTriggerExit2D(Collider2D other){ //On exit with a trigger collision area
        if (other.tag == "Interactable" && other.gameObject == currentInterObj){
            currentInterObj = null; //Resets the object assuming it's equal to the current game object
        }
    }





}

