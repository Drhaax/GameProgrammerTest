using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBounds : MonoBehaviour
{
	public SpriteRenderer PipeSr;
	public SpriteRenderer PipeTopSr;

	public bool IsCollidingWith(Bounds bird)
	{
		return PipeSr.bounds.Intersects(bird) || PipeTopSr.bounds.Intersects(bird);
	}

}
