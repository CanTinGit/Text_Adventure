using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Change to Room which can't be accessed by exit
[CreateAssetMenu(menuName = "TextAdventure/ActionResponse/ChangeRoom")]
public class ChangeRoomResponse : ActionResponse
{
    public Room roomToChangeTo;

    public override bool DoActionResponse(GameController controller)
    {
        if(controller.roomNavigation.currentRoom.roomName == requireString)
        {
            controller.roomNavigation.currentRoom = roomToChangeTo;
            controller.DisplayRoomText();
            return true;
        }

        return false;
    }
}
