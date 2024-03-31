using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : SimpleSingleton<GameplayManager>
{
    private HashSet<string> clearedRooms = new HashSet<string>();
    private string[] roomNames = { "LivingRoom", "Kitchen1", "KitchenBoss", "Armoury1", "Armoury2", "ArmouryBoss", "Library1", "Library2", "Library3", "Library4", "LibraryBoss" };

    public static VoidEvent StartGame;
    public static StringEvent CompleteRoom;

    private void OnEnable()
    {
        StartGame += HandleStartGame;
        CompleteRoom += HandleCompleteRoom;
    }

    private void OnDisable()
    {
        StartGame -= HandleStartGame;
        CompleteRoom -= HandleCompleteRoom;
    }

    private void HandleStartGame()
    {
        foreach (string roomName in roomNames)
        {
            clearedRooms.Add(roomName);
        }

        // Refill the player's inventory
        PlayerInventory.ChangeBallCount(20);
    }

    private void HandleCompleteRoom(string roomName)
    {
        if (clearedRooms.Contains(roomName))
        {
            clearedRooms.Remove(roomName);
            
            // Check if Game is complete (i.e. no more rooms left to clear)
            if (clearedRooms.Count == 0 )
            {
                EndGame();
            }
        }
        else
        {
            Debug.LogError("Room Name not found: " + roomName);
        }
    }

    private void EndGame()
    {
        // TODO: Something
    }
}
