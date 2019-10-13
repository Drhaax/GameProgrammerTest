using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PipePool
{
	private GameObject[] pipes;
	private int poolSize = 4;
	private float pipeMinOffset = -8;
	private float pipeMaxOffset = -2;
	private float spawnXPos = 3;
	private float spawnRate = 1.8f;
	private float timeLastSpawned = 0;
	private float pipeMoveSpeed = -3f;
	private bool gameRunning = false;
	private int currentPipeIndex = 0;
	private float outOfScreenXPos;

	public void InitializePool()
	{
		pipes = new GameObject[poolSize];
		var pipePrefab = Resources.Load<GameObject>("Prefabs/pipesPrefab");
		if(pipePrefab == null)
		{
			Debug.LogError("Pipe prefab was null");
			return;
		}

		for(int i = 0; i < poolSize; i++)
		{
			var pipe = GameObject.Instantiate(pipePrefab);
			pipe.SetActive(false);
			pipes[i] = pipe;
		}

		var camera = Camera.main;
		var halfHeight = camera.orthographicSize;
		var halfWidth = camera.aspect * halfHeight;
		var sr = pipes[0].GetComponentInChildren<SpriteRenderer>();

		if(sr == null)
		{
			Debug.LogError("Pipe was missing spriteRenderer!");
			return;
		}

		outOfScreenXPos = -halfWidth - sr.sprite.bounds.size.x;
	}

	public void StartGame()
	{
		if(pipes == null)
		{
			Debug.LogError("Pipe pool empty, did you remember to initialize pool?");
			return;
		}
		for(int i = 0; i < poolSize; i++)
		{
			pipes[i].SetActive(false);
		}
		timeLastSpawned = spawnRate;
		gameRunning = true;
	}

	public void GameOver()
	{
		gameRunning = false;
	}

	public void Tick()
	{
		if(!gameRunning)
		{
			return;
		}

		timeLastSpawned += Time.deltaTime;
		UpdatePipesPosition();
		if(timeLastSpawned >= spawnRate)
		{
			timeLastSpawned = 0;
			SpawnPipe();
			currentPipeIndex++;
			if(currentPipeIndex >= poolSize)
			{
				currentPipeIndex = 0;
			}
		}
	}

	public Pipe GetClosestPipe(Vector3 pos)
	{
		GameObject closest = null;
		float closestDistance = 0;
		foreach(var pipe in pipes.Where(p => p.activeInHierarchy))
		{
			var newDistance = Vector3.Distance(pos, pipe.transform.position);

			if(closest == null)
			{
				closest = pipe;
				closestDistance = newDistance;
				continue;
			}

			if(newDistance < closestDistance)
			{
				closestDistance = newDistance;
				closest = pipe;
			}
		}

		if(closest == null)
		{
			return null;
		}

		var closestPipe = closest.GetComponent<Pipe>();
		return closestPipe;
	}

	private void UpdatePipesPosition()
	{
		foreach(var pipe in pipes)
		{
			if(pipe.activeSelf)
			{
				if(pipe.transform.position.x < outOfScreenXPos)
				{
					pipe.SetActive(false);
					continue;
				}
				pipe.transform.position += new Vector3(pipeMoveSpeed, 0, 0) * Time.deltaTime;
			}
		}
	}

	private void SpawnPipe()
	{
		var spawnYPos = Random.Range(pipeMinOffset, pipeMaxOffset);
		pipes[currentPipeIndex].transform.position = new Vector2(spawnXPos, spawnYPos);
		pipes[currentPipeIndex].GetComponent<Pipe>().ResetPipe();
		pipes[currentPipeIndex].SetActive(true);

	}
}
