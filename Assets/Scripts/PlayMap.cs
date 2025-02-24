using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayMap : MonoBehaviour
{
	public float songposition;
	public float[] track;


	[Header("Basic refs")]
	[SerializeField] private float startDelay = 1.3f;
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
	private List<string> availableKeys;

	private void Start()
	{
		notesOnScreen = new Queue<MusicNote>();
		indexOfNextNote = 0;

		if (songOffset > 0)
		{
			startDelay = songOffset;
		}

		
	}

	private void Update()
	{

		if (Input.anyKeyDown)
		{
			PlayerInputted();
		}

		if (!songStarted) return;

		SpawnNote();

	}
	private void PlayerInputted()
	{
		string pressedKey = Input.inputString.ToUpper();

		if (notesOnScreen.Count > 0)
		{
			
			MusicNote frontNote = notesOnScreen.Peek();

			float offset = Mathf.Abs(frontNote.gameObject.transform.position.x - finishPosX);

			if (offset <= tolerationOffset && pressedKey == frontNote.key)
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
	public void InitializeMap(string map, SongData songData, List<string> keys)
	{
		_selectedMap = map;
		_songData = songData;
		_notes = _songData.notes;
		_mapSong = Resources.Load<AudioClip>($"Songs/{_selectedMap}/{_songData.audioFile}");

		availableKeys = keys; // Загружаем список клавиш

		_audioSource.clip = _mapSong;
		track = _notes.ConvertAll(note => note.time).ToArray();
	}
	public void InputForStart()
	{
		if (!songStarted)
		{
			songStarted = true;
			StartMap();
			return;
		}
	}
	private void StartMap()
	{
		dsptimesong = (float)AudioSettings.dspTime;
		_audioSource.PlayScheduled(dsptimesong + startDelay);
	}

	private void SpawnNote()
    {
		songposition = (float)(AudioSettings.dspTime - dsptimesong - songOffset);

		float beatToShow = songposition / secondsPerBeat + BeatsShownOnScreen;


		if (indexOfNextNote < track.Length && track[indexOfNextNote] < beatToShow)
		{
			Debug.Log("kek");

			MusicNote musicNote = ((GameObject)Instantiate(musicNotePrefab, Vector2.zero, Quaternion.identity)).GetComponent<MusicNote>();

			string randomKey = availableKeys[UnityEngine.Random.Range(0, availableKeys.Count)];
			musicNote.Initialize(this, startPosX, finishPosX, removeLine, posY, track[indexOfNextNote], randomKey);

			notesOnScreen.Enqueue(musicNote);

			indexOfNextNote++;
		}

		if (notesOnScreen.Count > 0)
		{
			MusicNote currNote = notesOnScreen.Peek();

			if (currNote.transform.position.x <= finishPosX - tolerationOffset)
			{
				Debug.Log("Miss!");
				notesOnScreen.Dequeue();
			}

		}
	}
}