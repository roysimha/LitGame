using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private int CollectedScore=0;
	private int globalScore=0;
	 private float timer=0;
	// Use this for initialization
	void Start ()
	{
	}
	public void addScore()
	{
		CollectedScore++;
		globalScore += 500;
		Debug.Log ("add Score");
	}
	public int getScore()
	{
		return CollectedScore;
	}
	void Update ()
	{
		timer += Time.deltaTime;
	}
}
