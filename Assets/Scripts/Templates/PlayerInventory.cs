using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChickenType
{
    White = 0,
    Black = 1,
    None = 2,
}

public class PlayerInventory : SimpleSingleton<PlayerInventory>
{
    public int m_WhiteChickenCount = 0;
    public int m_BlackChickenCount = 0;

    [SerializeField] private int m_QueueMaxSize = 5;
    public Queue<ChickenType> m_ChickenQueue = new Queue<ChickenType>();

    public static IntEvent CookedWhiteChicken;
    public static IntEvent CookedBlackChicken;
    public static ChickenEvent CollectChicken;
    public static ChickenReturnEvent ShootChicken;
    public delegate Queue<ChickenType> ChickenQueueEvent();
    public static ChickenQueueEvent GetChickenQueue;

    private void OnEnable()
    {
        CollectChicken += HandleCollectChicken;
        ShootChicken += HandleShootChicken;
        GetChickenQueue += HandleGetChickenQueue;

        CookedWhiteChicken += HandleCookedWhiteChicken;
        CookedBlackChicken += HandleCookedBlackChicken;
    }

    private void OnDisable()
    {
        CollectChicken -= HandleCollectChicken;
        ShootChicken -= HandleShootChicken;
        GetChickenQueue -= HandleGetChickenQueue;

        CookedWhiteChicken -= HandleCookedWhiteChicken;
        CookedBlackChicken -= HandleCookedBlackChicken;
    }

    private void HandleCollectChicken(ChickenType chicken)
    {
        if (m_ChickenQueue.Count == m_QueueMaxSize)
        {
            // TODO: Show some error
        }    
        else
        {
            m_ChickenQueue.Enqueue(chicken);
        }
    }

    private ChickenType HandleShootChicken()
    {
        return m_ChickenQueue.Dequeue();
    }

    private Queue<ChickenType> HandleGetChickenQueue()
    {
        return m_ChickenQueue;
    }

    private void HandleCookedWhiteChicken(int i)
    {
        m_WhiteChickenCount += i;
    }

    private void HandleCookedBlackChicken(int i)
    {
        m_BlackChickenCount += i;
    }
}
