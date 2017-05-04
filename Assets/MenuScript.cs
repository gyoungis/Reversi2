using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public GameObject mainCamera;
    public Button Easiest;
    public Button Easier;
    public Button Easy;
    public Button Hard;
    public Button Harder;
    public Button Hardest;
    public Text title;
    public int level;
    private GameManager gameMan;
	// Use this for initialization
	void Start () {
        title = title.GetComponent<Text>();
        Easiest = Easiest.GetComponent<Button>();
        Easier = Easier.GetComponent<Button>();
        Easy = Easy.GetComponent<Button>();
        Hard = Hard.GetComponent<Button>();
        Harder = Harder.GetComponent<Button>();
        Hardest = Hardest.GetComponent<Button>();
        GameObject manager = GameObject.Find("GameManager");
        gameMan = manager.GetComponent<GameManager>();
    }

    public void DisableText()
    {
        title.GetComponent<Text>().enabled = false;
        Easiest.GetComponent<Text>().enabled = false;
        Easier.GetComponent<Text>().enabled = false;
        Easy.GetComponent<Text>().enabled = false;
        Hardest.GetComponent<Text>().enabled = false;
        Harder.GetComponent<Text>().enabled = false;
        Hard.GetComponent<Text>().enabled = false;

        Easiest.GetComponent<Button>().interactable = false;
        Easier.GetComponent<Button>().interactable = false;
        Easy.GetComponent<Button>().interactable = false;
        Hardest.GetComponent<Button>().interactable = false;
        Harder.GetComponent<Button>().interactable = false;
        Hard.GetComponent<Button>().interactable = false;
    }
	
	public void EasiestPress()
    {
        DisableText();
        mainCamera.transform.position = new Vector3(4, 10, 4);
        gameMan.difficulty = 3;
        gameMan.started = true;

    }

    public void EasierPress()
    {
        DisableText();
        mainCamera.transform.position = new Vector3(4, 10, 4);
        gameMan.difficulty = 4;
        gameMan.started = true;
    }

    public void EasyPress()
    {
        DisableText();
        mainCamera.transform.position = new Vector3(4, 10, 4);
        gameMan.difficulty = 5;
        gameMan.started = true;
    }

    public void HardPress()
    {
        DisableText();
        mainCamera.transform.position = new Vector3(4, 10, 4);
        gameMan.difficulty = 6;
        gameMan.started = true;
    }

    public void HarderPress()
    {
        DisableText();
        mainCamera.transform.position = new Vector3(4, 10, 4);
        gameMan.difficulty = 7;
        gameMan.started = true;
    }

    public void HardestPress()
    {
        DisableText();
        mainCamera.transform.position = new Vector3(4, 10, 4);
        gameMan.difficulty = 8;
        gameMan.started = true;
    }
}
