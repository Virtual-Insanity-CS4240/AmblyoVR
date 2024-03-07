using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ChickenType m_ChickenType;

    public void AssignChickenTypeToProjectile(ChickenType chickenType)
    {
        m_ChickenType = chickenType;
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        switch (tag)
        {
            case "Pot":
                // TODO: Some particle effects and sounds
                if (m_ChickenType == ChickenType.White)
                    PlayerInventory.CookedWhiteChicken(1);
                else if (m_ChickenType == ChickenType.Black)
                    PlayerInventory.CookedBlackChicken(1);
                break;            
        }
    }
}
