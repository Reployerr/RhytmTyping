using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : MonoBehaviour
{
	public PlayMap playmap;

	public float startX;
	public float endX;
	public float removeLineX;
	public float beat;

	private void Start()
	{


	}

	public void Initialize(PlayMap playmap, float startX, float endX, float removeLineX, float posY, float beat)
	{
		this.playmap = playmap;
		this.startX = startX;
		this.endX = endX;
		this.removeLineX = removeLineX;
		this.beat = beat;

		transform.position = new Vector2(startX, posY);
	}

	 void Update()
	{
		transform.position = new Vector2(startX + (endX - startX) * (1f - (beat - playmap.songposition / playmap.secondsPerBeat) / playmap.BeatsShownOnScreen), transform.position.y);

		// Remove itself when out of the screen (remove line).
		if (transform.position.x > removeLineX)
		{
			Destroy(gameObject);
		}
	}
}
