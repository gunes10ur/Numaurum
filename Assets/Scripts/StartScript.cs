using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject howtoplay;
    public float nexttime;

    void Start()
    {
        howtoplay = GameObject.Find("howtoplay");
        howtoplay.active = false;

        float nexttime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nexttime) {
        	howtoplay.active = false;
        }
    }

    public void StartButton() {
    	SceneManager.LoadScene("GameScene");
    }

    public void HowToPlay() {
    	howtoplay.active = true;
    	nexttime = Time.time + 5.0f;

    }
}
