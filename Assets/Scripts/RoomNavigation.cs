using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNavigation : MonoBehaviour {

    public Room currentRoom;
    GameController controller;
    Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();

    void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public void UnpackExitsInRoom()
    {
        for(int i = 0; i < currentRoom.exits.Length; i++)
        {
            exitDictionary.Add(currentRoom.exits[i].keyString, currentRoom.exits[i].valueRoom);
            controller.interactionDescriptionInRoom.Add(currentRoom.exits[i].exitDescription);
        }
    }

    public void AttemptToChangeRooms(string dictionaryNoun)
    {
        if (exitDictionary.ContainsKey(dictionaryNoun))
        {
            currentRoom = exitDictionary[dictionaryNoun];
            controller.LogStringWithReturn("You head off to the " + dictionaryNoun);
            controller.DisplayRoomText();
        }
        else
        {
            controller.LogStringWithReturn("There is no path to the " + dictionaryNoun);
        }
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }
}
