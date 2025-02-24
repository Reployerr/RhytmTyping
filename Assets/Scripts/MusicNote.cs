using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicNote : MonoBehaviour
{
	public PlayMap playmap;

	public float startX;
	public float endX;
	public float removeLineX;
	public float beat;
	public string key;

	[SerializeField] TMP_Text noteKeyText;
	public void Initialize(PlayMap playmap, float startX, float endX, float removeLineX, float posY, float beat, string key)
	{
		this.playmap = playmap;
		this.startX = startX;
		this.endX = endX;
		this.removeLineX = removeLineX;
		this.beat = beat;
		this.key = key;

		transform.position = new Vector2(startX, posY);

		noteKeyText.text = key;
	}

	 void Update()
	{
		//transform.position = Vector2.Lerp(startX, endX, (playmap.BeatsShownOnScreen - (beat - playmap.songposition)) / playmap.BeatsShownOnScreen);
		transform.position = new Vector2(startX + (endX - startX) * (1f - (beat - playmap.songposition / playmap.secondsPerBeat) / playmap.BeatsShownOnScreen), transform.position.y);

		if (transform.position.x < removeLineX)
		{
			DestroyNote();
		}
	}

	public void DestroyNote()
	{
		Destroy(gameObject);
	}
}
