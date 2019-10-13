using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScaler : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		var sr = GetComponent<SpriteRenderer>();
		if(sr == null)
		{
			return;
		}

		transform.localScale = Vector3.one;

		var width = sr.sprite.bounds.size.x;
		var height = sr.sprite.bounds.size.y;

		var worldScreenHeight = Camera.main.orthographicSize * 2.0;
		var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		this.transform.localScale = new Vector2((float)worldScreenWidth / width, (float)worldScreenHeight / height);

	}

}
