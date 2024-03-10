using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image bulletImage;
    public Sprite whiteSprite;
    public Sprite blackSprite;
    public Sprite emptySprite;
    [SerializeField] private GameObject LoadedGunYellow;
    [SerializeField] private GameObject LoadedGunBlack;
    [SerializeField] private GameObject UnloadedGun;
    private GameObject currentGun;


    private void Start()
    {
        currentGun = UnloadedGun;
        LoadedGunBlack.SetActive(false);
        LoadedGunYellow.SetActive(false);
        UpdateMagazine();
    }

    void Update()
    {
        UpdateMagazine();
    }

    void UpdateMagazine()
    {
        // Queue<ChickenType> sampleChickenQueue = new Queue<ChickenType>();
        // sampleChickenQueue.Enqueue(ChickenType.White);
        // sampleChickenQueue.Enqueue(ChickenType.Black);
        // sampleChickenQueue.Enqueue(ChickenType.White);
        // sampleChickenQueue.Enqueue(ChickenType.White);

        // int chickenCount = sampleChickenQueue.Count;
        // int chickenCount = PlayerInventory.Instance.m_ChickenQueue.Count;
        // for (int i = 0; i < bulletImages.Count; i++)
        // {
        //     if (i < chickenCount)
        //     {
        //         // ChickenType chicken = sampleChickenQueue.Dequeue();
        //         ChickenType chicken = PlayerInventory.Instance.m_ChickenQueue.Dequeue();
        //         Sprite sourceImage = chicken == ChickenType.White ? whiteSprite : blackSprite;
        //         bulletImages[i].sprite = sourceImage;
        //     }
        //     else
        //     {
        //         bulletImages[i].sprite = emptySprite;
        //     }
        // }

        //Queue<ChickenType> chickenQueue = PlayerInventory.GetChickenQueue?.Invoke();
        //ChickenType firstChicken = chickenQueue.Count > 0 ? chickenQueue.Peek() : ChickenType.None;
        //bulletImage.sprite = firstChicken == ChickenType.White ? whiteSprite : firstChicken == ChickenType.Black ? blackSprite : emptySprite;
        //GameObject newGun = firstChicken == ChickenType.White ? LoadedGunYellow : firstChicken == ChickenType.Black ? LoadedGunBlack : UnloadedGun;
        //if (newGun != currentGun)
        //{
        //    currentGun.SetActive(false);
        //    newGun.SetActive(true);
        //    currentGun = newGun;
        //    print("Changed gun!");
        //}
    }
}