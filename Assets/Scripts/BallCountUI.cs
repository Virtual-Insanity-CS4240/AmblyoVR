using UnityEngine;
using UnityEngine.UI;

public class BallCountUI : MonoBehaviour
{
    public Image ballIcon;
    public Text ballCountText;
    public Sprite[] ballIcons;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

        private void UpdateUI()
    {
        ballIcon.sprite = ballIcons[(int)PlayerInventory.equippedBallColor];
        ballCountText.text = PlayerInventory.ballCount.ToString();
    }
}