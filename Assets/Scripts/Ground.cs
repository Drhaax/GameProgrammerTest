using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
	public float Speed;
	private float width;
	private bool gameRunning;
	private float originalPos;
	GameObject[] ground;
	
	void Start()
	{
		originalPos = transform.position.x;

		var worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		var worldScreenWidth =	worldScreenHeight / Screen.height * Screen.width;
		width = worldScreenWidth * 2;
		var tile = Resources.Load<GameObject>("Prefabs/groundTile");
		var sr = tile.GetComponent<SpriteRenderer>();
		var tileWidth = sr.sprite.bounds.size.x;
		var tileHeight = sr.sprite.bounds.extents.y;
		var neededTiles = Mathf.RoundToInt((worldScreenWidth / tileWidth) * 2);
		var groundStartPos = -(worldScreenWidth / 2)-tileWidth/2;
		ground = new GameObject[neededTiles];
		
		for(int i = 1; i <= neededTiles; i++) {
			var go = Instantiate(tile,this.transform);
			var tileXPos = groundStartPos + (tileWidth)*i;
			var tileYPos = -worldScreenHeight/2 + tileHeight;
			go.transform.position = new Vector3(tileXPos,tileYPos ,0);
			ground[i - 1] = go;
		}
		
	}

	void Update()
	{
		if(!gameRunning)
		{
			return;
		}

		if(transform.position.x > -(width / 2))
		{
			var newX = transform.position.x - Speed * Time.deltaTime;
			transform.position = new Vector3(newX, 0, 0);
		}
		else
		{
			ResetGroundPos();
		}
	}

	private void ResetGroundPos()
	{
		transform.position = new Vector3(originalPos, 0, 0);
	}

	public void StartToMoveGround()
	{
		gameRunning = true;
	}

	public void GameOver()
	{
		gameRunning = false;
	}

	public float GetGroundHeight()
	{
		var block = transform.GetChild(0);
		var sr = block.GetComponent<SpriteRenderer>();

		if(sr == null)
		{
			Debug.LogError("Sprite renderer not found! " + gameObject.name);
			return 0;
		}
		return block.transform.position.y;
	}
}
