using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] firstScreen;
    [SerializeField] private GameObject[] secondScreen;
    [SerializeField] private TeleportTrajectory movementScript;
    // Start is called before the first frame update
    void Start()
    {
        movementScript.enabled = false;
    }
    public void nextScreen()
    {
        foreach (GameObject screen in firstScreen)
        {
            screen.SetActive(false);
        }
        foreach (GameObject screen in secondScreen)
        {
            screen.SetActive(true);
        }
    }
    public void previousScreen()
    {
        foreach (GameObject screen in firstScreen)
        {
            screen.SetActive(true);
        }
        foreach (GameObject screen in secondScreen)
        {
            screen.SetActive(false);
        }
    }
    public void done()
    {
        movementScript.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
