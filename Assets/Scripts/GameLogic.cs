using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
	public Ground Ground;
	public GameObject BirdPrefab;
	public Button Button;
	public TextMeshProUGUI Text;
	public float BirdSpawnXOffset;

	private int score;
	private Bird bird;
	private PipePool pipePool;
	private bool gameRunning;
	private float birdSpawnXPos;
	
	void Start()
	{
		if(BirdPrefab == null)
		{
			Debug.LogError("Bird prefab was null");
			return;
		}
		var birdGO = Instantiate(BirdPrefab);
		bird = birdGO.GetComponent<Bird>();
		var camera = Camera.main;
		var halfHeight = camera.orthographicSize;
		birdSpawnXPos = -(camera.aspect * halfHeight) + BirdSpawnXOffset;

		pipePool = new PipePool();
		Button.onClick.AddListener(StartGame);
		pipePool.InitializePool();
	}

	void Update()
	{
		Button.gameObject.SetActive(!gameRunning);
		Text.text = "Score: " + score.ToString();
		
		if(!gameRunning)
		{
			return;
		}

		pipePool.Tick();
		ShouldGameEnd(bird.GetBirdBottomPosition() < Ground.GetGroundHeight());

		if(Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			bird.Flap();
		}
		
		var closestPipe = pipePool.GetClosestPipe(bird.transform.position);

		if(closestPipe != null && closestPipe.DidBirdPass(bird.transform.position))
		{
			score++;
		}
		
		ShouldGameEnd(closestPipe != null && closestPipe.IsCollidingWith(bird.transform));
	}

	private void ShouldGameEnd(bool endGame)
	{
		if(endGame)
		{
			GameOver();
		}
	}

	private void GameOver()
	{
		gameRunning = false;
		Ground.GameOver();
		bird.GameOver();
		pipePool.GameOver();
	}

	private void StartGame()
	{
		bird.StartGame(new Vector3(birdSpawnXPos,0,0));
		Ground.StartToMoveGround();
		pipePool.StartGame();
		score = 0;
		gameRunning = true;
	}

}
