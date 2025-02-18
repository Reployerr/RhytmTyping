using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayMap : MonoBehaviour
{
	[SerializeField] private float _delay;
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private GameObject musicNotePrefab;

	private string _selectedMap = "";
	private AudioClip _mapSong;
	private SongData _songData;
	private List<NotesData> _notes = new List<NotesData>();
	private bool songStarted = false;

	public float startPosX;
	public float finishPosX;
	public float removeLine;
	public float posY;
	public float tolerationOffset;
	public float songOffset;

	public float secondsPerBeat;
	public float BeatsShownOnScreen = 4f;
	private Queue<MusicNote> notesOnScreen;

	public float[] track;

	private float dsptimesong;
	public float songposition;

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

			// Instantiate a new music note. (Search "Object Pooling" for more information if you wish to minimize the delay when instantiating game objects.)
			// We don't care about the position and rotation because we will set them later in MusicNote.Initialize(...).
			MusicNote musicNote = ((GameObject)Instantiate(musicNotePrefab, Vector2.zero, Quaternion.identity)).GetComponent<MusicNote>();

			musicNote.Initialize(this, startPosX, finishPosX, removeLine, posY, track[indexOfNextNote]);

			// The note is push into the queue for reference.
			notesOnScreen.Enqueue(musicNote);

			// Update the next index.
			indexOfNextNote++;
		}

		if (notesOnScreen.Count > 0)
		{
			MusicNote currNote = notesOnScreen.Peek();

			if (currNote.transform.position.x >= finishPosX + tolerationOffset)
			{

				notesOnScreen.Dequeue();

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
			// Get the front note.
			MusicNote frontNote = notesOnScreen.Peek();

			// Distance from the note to the finish line.
			float offset = Mathf.Abs(frontNote.gameObject.transform.position.x - finishPosX);

			//hit logic
			///
			/*
			 * if (offset <= tolerationOffset) 
			{
				// Change color to green to indicate a "HIT".
				frontNote.ChangeColor(true);

				statusText.text = "HIT!";
				
				// Remove the reference. (Now the next note moves to the front of the queue.)
				notesOnScreen.Dequeue();
			}
			 */
		}
	}
	public void InitializeMap(string map, SongData songData)
	{
		

		_selectedMap = map;
		_songData = songData;
		_notes = _songData.notes;
		_mapSong = Resources.Load<AudioClip>($"Songs/{_selectedMap}/{_songData.audioFile}");

		// ����������� ������ ��� � ������ ������� (float)
		track = _notes.ConvertAll(note => note.time).ToArray();
	}


	private void StartMap()
	{
		songStarted = true; 

		dsptimesong = (float)AudioSettings.dspTime;

		_audioSource.clip = _mapSong;
		_audioSource.Play();

		foreach (var note in _notes)
		{
			Debug.Log($"������� ���� {note.time}");
		}
	}

}