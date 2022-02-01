using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;

public class EventController : MonoBehaviour
{
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private Text mainText;
    private Text optionOne;
    private Text optionTwo;
    private Text optionOneUnderline;
    private Text optionTwoUnderline;

    private Image weed;
    private Image wine;
    private Image burger;

    private bool clickReady = true;

    public int dialogue_stage = 0;

    private int numCollected;
    public int numFuckups = 0;
    public bool infected = false;


    // Start is called before the first frame update
    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        weed = GameObject.Find("Weed").GetComponent<Image>();
        weed.enabled = false;
        wine = GameObject.Find("Wine").GetComponent<Image>();
        wine.enabled = false;
        burger = GameObject.Find("Burger").GetComponent<Image>();
        burger.enabled = false;
        mainText = GameObject.Find("MainText").GetComponent<Text>();
        optionOne = GameObject.Find("Choice1").GetComponent<Text>();
        optionTwo = GameObject.Find("Choice2").GetComponent<Text>();
        optionOneUnderline = GameObject.Find("Choice1Line").GetComponent<Text>();
        optionTwoUnderline = GameObject.Find("Choice2Line").GetComponent<Text>();
        next_dialogue(0);
    }

    // Update is called once per frame
    void Update()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);
        if (results.Count == 0) {
            optionOneUnderline.text = "";
            optionTwoUnderline.text = "";
        }
        foreach(RaycastResult swatch in results)
        {
            if (swatch.gameObject == optionOne.gameObject){
                optionOneUnderline.text = new string('_', optionOne.text.Length / 2);
                if (Input.GetMouseButtonDown(0) && clickReady) {
                    clickReady = false;
                    dialogue_stage += 1;
                    next_dialogue(0);
                }
            } else {
                optionOneUnderline.text = "";
            }

            if (swatch.gameObject == optionTwo.gameObject){
                optionTwoUnderline.text = new string('_', optionTwo.text.Length / 2);
                if (Input.GetMouseButtonDown(0) && clickReady) {
                    clickReady = false;
                    dialogue_stage += 1;
                    next_dialogue(1);
                }
            } else {
                optionTwoUnderline.text = "";
            }
            
        }

        if (Input.GetMouseButtonUp(0)) {
            clickReady = true;
        }
    }

    public void next_dialogue(int option){
        if (dialogue_stage == 0) {
            mainText.text = "It is currently Tuesday the 22nd of August, around 3pm.";
            optionOne.text = "It's a beautiful day!";
            optionTwo.text = "Just another day on the dole.";
        }

        if (dialogue_stage == 1) {
            mainText.text = "They just announced a new covid case.";
            optionOne.text = "Well fuck.";
            optionTwo.text = "";
        }

        if (dialogue_stage == 2) {
            mainText.text = "You need to check your supplies. You rush to your largely empty fridge.";
            optionOne.text = "We're all out of beer!";
            optionTwo.text = "There's like, no wine in here.";
        }

        if (dialogue_stage == 3) {
            if (option == 0) {
                mainText.text = "You knew you shouldn't have offered your flatmate 'one or two' from the 12 pack.";
                optionTwo.text = "So we need more beer, what else?";
            } else if (option == 1) {
                mainText.text = "Just one, half empty, depressed looking bottle of $7 cleanskin chardonnay.";
                optionTwo.text = "So we need more wine, what else?";
            }
            optionOne.text = "My life is a mess.";
        }

        if (dialogue_stage == 4) {
            if (option == 0) {
                mainText.text = "Yeah it is, it's probably all the pot you smoke. Speaking of which, you're running low on that too. You can buy some just around the corner";
            } else if (option == 1) {
                mainText.text = "Weed, obviously. Luckily the guy who can sort you out is just around the corner.";

            }
            optionOne.text = "Okay, weed and booze, got it.";
            optionTwo.text = "";
        }

        if (dialogue_stage == 5) {
            mainText.text = "Oh and remember, people are sick out there, you're not going to be able to tell which ones. Maintain social distancing! That's two meters, minimum. Are you ready?";
            optionOne.text = "I could also go for a burger.";
            optionTwo.text = "";
        }

        if (dialogue_stage == 6) {
            mainText.text = "Good thinking! Just walk into the big green circles to buy your items then return home, and remember, keep a two meter distance from everyone!";
            optionOne.text = "Got it.";
            optionTwo.text = "This is a bad idea.";
        }

        if (dialogue_stage == 7) {
            begin_gameplay();
            mainText.text = "";
            optionOne.text = "";
            optionTwo.text = "";
        }

        if (dialogue_stage == 69) {
            Camera camera_a = GameObject.Find("UICamera").GetComponent<Camera>();
            camera_a.enabled = true;
            mainText.text = "Welcome back!";
            optionOne.text = "Thank you.";
            optionTwo.text = "It's terrible out there!";
        }

        if (dialogue_stage == 70) {
            mainText.text = string.Format("You broke social distancing rules {0} times.", numFuckups);
            if (numFuckups > 0){
                optionOne.text = "Oops.";
            } else {
                optionOne.text = "Bloomfield would be proud!";
            }
            optionTwo.text = "";
        }

        if (dialogue_stage == 71) {
            if (infected) {
                mainText.text = "One of those people were sick, and now you probably are too.";
                optionOne.text = "That's not good.";
                optionTwo.text = "AHHHHHHHHH";
            } else if (numFuckups > 0) {
                mainText.text = "Despite that, you didn't pass by anyone who was sick.";
                optionOne.text = "Thank God.";
                optionTwo.text = "";
            } else {
                mainText.text = "It was a nice walk, and you're glad to be home.";
                optionOne.text = "I feel fit as a fiddle.";
                optionTwo.text = "";
            }
        }

        if (dialogue_stage == 72) {
            if (infected) {
                if (numCollected == 3) {
                    mainText.text = "You collected all three items, not that it's going to do you much good now.";
                    optionOne.text = "Ending C";
                    optionTwo.text = "Game Over.";
                }else if (numCollected > 0) {
                    mainText.text = "You didn't even get everything you wanted. Was it worth it?";
                    optionOne.text = "Ending D";
                    optionTwo.text = "Game Over.";
                } else if (numCollected == 0){
                    mainText.text = "You left the house for just a minute, and now you're sick. That fucking sucks.";
                    optionOne.text = "Ending E";
                    optionTwo.text = "Game Over.";
                }
            } else {
                if (numCollected == 3) {
                    mainText.text = "You collected all three items, time to sit back, relax and enjoy another lockdown!";
                    optionOne.text = "Ending A";
                    optionTwo.text = "Game Over.";
                }else if (numCollected > 0) {
                    mainText.text = "You didn't get everything you wanted. I'm sure you can enjoy lockdown without them, maybe you could get into baking.";
                    optionOne.text = "Ending B";
                    optionTwo.text = "Game Over.";
                } else if (numCollected == 0){
                    mainText.text = "You followed the government's health advice and decided against running to the shops and panic buying. You are a true national hero!";
                    optionOne.text = "Ending S";
                    optionTwo.text = "Game Over.";
                }
            }
        }
        if (dialogue_stage == 73){
            Application.Quit();
        }
    }

    void begin_gameplay() {
        Debug.Log("begin");
        Camera camera_a = GameObject.Find("UICamera").GetComponent<Camera>();
        camera_a.enabled = false;

        SC_TPSController player = FindObjectOfType<SC_TPSController>();
        player.Activate();

        Wander[] wanderers = FindObjectsOfType<Wander>();
        foreach (Wander wanderer in wanderers) {
            int number = Random.Range(0, 9);
            if (number == 0){
                wanderer.infected = true;
            } else {
                wanderer.infected = false;
            }
            wanderer.Activate();
            wanderer.maxHeadingChange = 180;
        }
    }

    public void collected(int item) {
        if (item == 0) {
            wine.enabled = true;
        } else if (item == 1) {
            weed.enabled = true;
        } else if (item == 2) {
            burger.enabled = true;
        }
        numCollected += 1;
    }

   
}


