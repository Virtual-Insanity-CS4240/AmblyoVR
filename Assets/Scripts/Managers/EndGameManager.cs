using System.Collections;
using System.Collections.Generic;
using Delegates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : SimpleSingleton<EndGameManager>
{
    [SerializeField] private int roomCount = 10;
    private int completedRooms = 0;
    public static VoidEvent AddRoomComplete;

    private void OnEnable()
    {
        AddRoomComplete += HandleAddRoomComplete;
    }

    private void OnDisable()
    {
        AddRoomComplete -= HandleAddRoomComplete;
    }
    
    private void HandleAddRoomComplete()
    {
        completedRooms++;
        if (completedRooms >= roomCount)
        {
            Debug.Log("All rooms completed!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
