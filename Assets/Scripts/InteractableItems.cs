﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour
{
    public List<InteractableObject> useableItemList;
    public Dictionary<string, string> examineDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> takeDictionary = new Dictionary<string, string>();
    Dictionary<string, ActionResponse> useDictionary = new Dictionary<string, ActionResponse>();

    [HideInInspector] public List<string> nounsInRoom = new List<string>();
    List<string> nounsInInventory = new List<string>();
    GameController controller;

    void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public string GetObjectsNotInInventory(Room currentRoom, int i)
    {
        InteractableObject interactableInRoom = currentRoom.interacatbleObjectsInRoom[i];

        if (!nounsInInventory.Contains(interactableInRoom.noun))
        {
            nounsInRoom.Add(interactableInRoom.noun);
            return interactableInRoom.description;
        }

        return null;
    }

    public void ClearCollections()
    {
        examineDictionary.Clear();
        takeDictionary.Clear();
        nounsInRoom.Clear();
    }

    public Dictionary<string,string> Take (string[] seperatedInputWords)
    {
        string noun = seperatedInputWords[1];

        if (nounsInRoom.Contains(noun))
        {
            nounsInInventory.Add(noun);
            AddActionResponseToUseDictionary();
            nounsInRoom.Remove(noun);
            return takeDictionary;
        }
        else
        {
            controller.LogStringWithReturn("There is no " + noun + " here to take");
            return null;
        }

    }

    public void DisplayInventory()
    {
        controller.LogStringWithReturn("You look your backpack, you have :");

        for(int i = 0; i < nounsInInventory.Count; i++)
        {
            controller.LogStringWithReturn(nounsInInventory[i]);
        }
    }

    public void AddActionResponseToUseDictionary()
    {
        for(int i = 0; i < nounsInInventory.Count; i++)
        {
            string noun = nounsInInventory[i];
            InteractableObject interatableObjectInventory = GetInteractableObjectFromUsableList(noun);
            if(interatableObjectInventory == null)
            {
                continue;
            }
                 
            for(int j = 0; j < interatableObjectInventory.interaction.Length; j++)
            {
                Interaction interaction = interatableObjectInventory.interaction[j];

                if (interaction.actionResponse == null)
                    continue;

                if (useDictionary.ContainsKey(noun) == false)
                {
                    useDictionary.Add(noun, interaction.actionResponse);
                }
            }
        }
    }

    InteractableObject GetInteractableObjectFromUsableList(string noun)
    {
        for(int i = 0; i < useableItemList.Count; i++)
        {
            if (useableItemList[i].noun == noun)
            {
                return useableItemList[i];
            }
        }
        return null;

    }

    public void UseItem(string[] separatedInputWords)
    {
        string nounToUse = separatedInputWords[1];

        if (nounsInInventory.Contains(nounToUse))
        {
            if (useDictionary.ContainsKey(nounToUse))
            {
                bool actionResult = useDictionary[nounToUse].DoActionResponse(controller);
                if (!actionResult)
                {
                    controller.LogStringWithReturn("Hmm. Nothing happen");
                }
            }
            else
            {
                controller.LogStringWithReturn("You can't use " + nounToUse);
            }
        }
        else
        {
            controller.LogStringWithReturn("There is no " + nounToUse + "in your inventory to use");
        }
    }
}
