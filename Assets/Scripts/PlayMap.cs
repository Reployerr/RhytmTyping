using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayMap : MonoBehaviour
{
	public float songposition;
	public float[] track;

	[Header("Basic refs")]
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private AudioSource hitSound;
	[SerializeField] private GameObject musicNotePrefab;

	[Header("Notes parameters")]
	[SerializeField] private float startPosX;
	[SerializeField] private float finishPosX;
	[SerializeField] private float removeLine;
	[SerializeField] private float posY;
	[SerializeField] private float tolerationOffset;
	[SerializeField] private float songOffset;
	public float secondsPerBeat;
	public float BeatsShownOnScreen = 4f;
	private Queue<MusicNote> notesOnScreen;

	private string _selectedMap = "";
	private AudioClip _mapSong;
	private SongData _songData;
	private List<NotesData> _notes = new List<NotesData>();
	private bool songStarted = false;

	private float dsptimesong;
	private int indexOfNextNote;

	private void Start()
	{
		notesOnScreen = new Queue<MusicNote>();
		indexOfNextNote = 0;
	}

	private void Update()
	{

		if (Input.GetKeyDown(KeyCode.Space))
		{
			PlayerInputted();
		}

		if (!songStarted) return;

		songposition = (float)(AudioSettings.dspTime - dsptimesong - songOffset);
		float beatToShow = songposition / secondsPerBeat + BeatsShownOnScreen;


		if (indexOfNextNote < track.Length && track[indexOfNextNote] < beatToShow)
		{
			Debug.Log("kek");

			MusicNote musicNote = ((GameObject)Instantiate(musicNotePrefab, Vector2.zero, Quaternion.identity)).GetComponent<MusicNote>();

			musicNote.Initialize(this, startPosX, finishPosX, removeLine, posY, track[indexOfNextNote]);

			notesOnScreen.Enqueue(musicNote);

			indexOfNextNote++;
		}

		if (notesOnScreen.Count > 0)
		{
			MusicNote currNote = notesOnScreen.Peek();

			if (currNote.transform.position.x >= finishPosX + tolerationOffset)
			{
				notesOnScreen.Dequeue();
				Debug.Log("Miss!");
			}

		}

	}
	private void PlayerInputted()
	{
		if (!songStarted)
		{
			songStarted = true;
			StartMap();
			return;
		}

		if (notesOnScreen.Count > 0)
		{
			MusicNote frontNote = notesOnScreen.Peek();

			float offset = Mathf.Abs(frontNote.gameObject.transform.position.x - finishPosX);

			if (offset <= tolerationOffset)
			{
				Debug.Log("HIT!");
				hitSound.Play();
				notesOnScreen.Dequeue();
				frontNote.DestroyNote();
			}

			else
			{
				Debug.Log("Miss");
			}

		}
	}
	public void InitializeMap(string map, SongData songData)
	{
		_selectedMap = map;
		_songData = songData;
		_notes = _songData.notes;
		_mapSong = Resources.Load<AudioClip>($"Songs/{_selectedMap}/{_songData.audioFile}");

		_audioSource.clip = _mapSong;
		track = _notes.ConvertAll(note => note.time).ToArray();
	}


	private void StartMap()
	{
		dsptimesong = (float)AudioSettings.dspTime;
		_audioSource.Play();
	}
}