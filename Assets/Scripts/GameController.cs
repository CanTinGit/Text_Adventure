using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text displayText;
    public InputAction[] inputActions;

    [HideInInspector] public RoomNavigation roomNavigation;
    [HideInInspector] public List<string> interactionDescriptionInRoom = new List<string>();
    [HideInInspector] public InteractableItems interactableItems;
    List<string> actionLog = new List<string>();

	// Use this for initialization
	void Awake ()
    {
        interactableItems = GetComponent<InteractableItems>();
        roomNavigation = GetComponent<RoomNavigation>();
	}

    void Start()
    {
        DisplayRoomText();
        DisplayLoggedText();
    }
	
    public void DisplayLoggedText()
    {
        string logAsText = string.Join("\n", actionLog.ToArray());

        displayText.text = logAsText;
    }

    public void DisplayRoomText()
    {
        ClearCollectionsForNewRoom();
        UnpackRoom();

        string joinedInteractionDescription = string.Join("\n", interactionDescriptionInRoom.ToArray());
        string combinedText = roomNavigation.currentRoom.description + "\n" + joinedInteractionDescription;

        LogStringWithReturn(combinedText);
    }
    
    void ClearCollectionsForNewRoom()
    {
        interactableItems.ClearCollections();
        interactionDescriptionInRoom.Clear();
        roomNavigation.ClearExits();
    }

    void UnpackRoom()
    {
        roomNavigation.UnpackExitsInRoom();
        PrepareObjectsInRoom(roomNavigation.currentRoom);
    }

    void PrepareObjectsInRoom(Room currentRoom)
    {
        for(int i = 0; i < currentRoom.interacatbleObjectsInRoom.Length; i++)
        {
            string itemDescriptionNotInInventory = interactableItems.GetObjectsNotInInventory(currentRoom, i);
            if(itemDescriptionNotInInventory != null)
            {
                interactionDescriptionInRoom.Add(itemDescriptionNotInInventory);
            }

            InteractableObject interactableObjectInRoom = currentRoom.interacatbleObjectsInRoom[i];

            for(int j = 0; j < interactableObjectInRoom.interaction.Length; j++)
            {
                Interaction interaction = interactableObjectInRoom.interaction[j];
                if(interaction.inputAction.keyWord == "examine")
                {
                    interactableItems.examineDictionary.Add(interactableObjectInRoom.noun, interaction.textResponse);
                }

                if (interaction.inputAction.keyWord == "take")
                {
                    interactableItems.takeDictionary.Add(interactableObjectInRoom.noun, interaction.textResponse);
                }
                //interactableItems.examineDictionary.Add(interactableObjectInRoom.interaction[j].inputAction.ToString(), interactableObjectInRoom.interaction[i].textResponse);
            }
            
        }
    }

    public string TestVerbDictionaryWithNoun(Dictionary<string,string> verbDictionary, string verb, string noun)
    {
        if (verbDictionary.ContainsKey(noun))
        {
            return verbDictionary[noun];
        }

        return "You can't" + verb + " " + noun;
    }

    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
    }
	// Update is called once per frame
	void Update () {
		
	}
}
