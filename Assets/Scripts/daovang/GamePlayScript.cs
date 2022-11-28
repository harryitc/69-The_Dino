using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GamePlayScript : MonoBehaviour
{
	public Text timeText;
	public Text scoreText;
	public int score;
	public int time;
	public GameObject menuEndGame;


	public GameObject[] levelsVang;
	public int level;
	public bool endgame = false;
	// Use this for initialization
	public void Start ()
	{
		Application.targetFrameRate= 60;
		startGame ();
		level = 0;
		this.StartCoroutine ("Do");
	}

	public IEnumerator Do ()
	{
		bool add = true;
		while (add) {
			yield return new WaitForSeconds (1);
			if (time > 0) {
				time--;
			}
			if (time <= 0 && !endgame) {
				endGame ();
//				StopCoroutine("Do");
			}
		}
	}
	public void endGame ()
	{
		endgame = true;
		menuEndGame.SetActive (true);
		level++;
	}
    void startGame ()
	{



		menuEndGame.SetActive(false);
		endgame = false;
		time = 60;
		score = 0;
		for (int i = 0; i < levelsVang.Length; i++)
		{
			if (level == i)
			{
				levelsVang[i].SetActive(true);
			}
			else
			{
				levelsVang[i].SetActive(false);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		timeText.text = "Time: " + time.ToString ();
		scoreText.text = "Score: " + score.ToString ();
	}

	public void replay ()
	{
		SceneManager.LoadScene("GamePlay");
	}
}
