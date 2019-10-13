using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
	bool gameRunning;
	bool flapping;
	float flapTime = 0.15f;
	float timer = 0;
	float spriteHeight;
	void Start()
	{
		var sr = GetComponent<SpriteRenderer>();
		if(sr == null)
		{
			Debug.LogError("Sprite renderer not found! " + gameObject.name);
			return;
		}

		spriteHeight = sr.sprite.bounds.size.y;
	}

	public void Flap()
	{
		flapping = true;
		timer = 0;
	}

	private void FixedUpdate()
	{
		if(!gameRunning)
		{
			return;
		}

		timer += Time.deltaTime;
		if(flapping)
		{
			if(timer >= flapTime)
			{
				timer = 0;
				flapping = false;
			}
			MoveBird(6f);

		}
		ApplyGravity(timer);

	}

	private void MoveBird(float heightChange)
	{
		transform.position += new Vector3(0, heightChange, 0) * Time.deltaTime;
	}

	private void ApplyGravity(float time)
	{
		MoveBird(-5f * (time * 2));
	}

	public void StartGame(Vector3 spawnPos)
	{
		gameRunning = true;
		transform.position = spawnPos;
	}

	public void GameOver()
	{
		gameRunning = false;
	}

	public float GetBirdBottomPosition()
	{
		return transform.position.y - spriteHeight;
	}
}
