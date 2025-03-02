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
	[SerializeField] private ScoreUI ScoreUI;
	[SerializeField] private float ScoreX;
	[SerializeField] private float ScoreCombo;

	[Header("Notes parameters")]
	[SerializeField] private float startPosX;
	[SerializeField] private float finishPosX;
	[SerializeField] private float removeLine;
	[SerializeField] private float posY;
	[SerializeField] private float greenOffset;
	[SerializeField] private float yellowOffset;
	[SerializeField] private float redOffset;
	[SerializeField] private float songOffset;

	public float secondsPerBeat;
	public float BeatsShownOnScreen = 4f;
	private Queue<MusicNote> notesOnScreen;

	private string _selectedMap = "";
	private AudioClip _mapSong;
	private List<NotesData> _notes = new List<NotesData>();
	private bool songStarted = false;

	private float dsptimesong;
	private int indexOfNextNote;
	private List<string> availableKeys;

	private SongData _songData;
	private ScoreManager _scoreManager;


	private void Start()
	{
		_scoreManager = new ScoreManager();
		notesOnScreen = new Queue<MusicNote>();

		indexOfNextNote = 0;
		if (songOffset > 0)
		{
			startDelay = songOffset;
		}
	}
    private void OnEnable()
    {
        songStarted = false;
	}
    private void Update()
	{

		if (Input.anyKeyDown)
		{
			PlayerInputted();
		}

		if (!songStarted) return;

		if(songStarted == true)
        {
			SpawnNote();
		}

	}
	private void PlayerInputted()
	{
		string pressedKey = Input.inputString.ToUpper();

		if (notesOnScreen.Count > 0)
		{
			
			MusicNote frontNote = notesOnScreen.Peek();

			float offset = Mathf.Abs(frontNote.gameObject.transform.position.x - finishPosX);

			if (pressedKey == frontNote.key)
			{
				bool hit = false;

				if (offset <= greenOffset)
				{
					Debug.Log("HIT! (GREEN)");
					_scoreManager.AddScore(Mathf.RoundToInt(100 * ScoreX)); 
					hit = true;
				}
				else if (offset <= yellowOffset)
				{
					Debug.Log("YELLOW HIT!");
					_scoreManager.AddScore(Mathf.RoundToInt(50 * ScoreX));
					hit = true;
				}
				else if (offset <= redOffset)
				{
					Debug.Log("RED HIT!");
					_scoreManager.AddScore(Mathf.RoundToInt(20 * ScoreX));
					hit = true;
				}

				if (hit)
				{
					hitSound.Play();
					notesOnScreen.Dequeue();
					frontNote.DestroyNote();
				}
			}

		}
	}
	public void InitializeMap(string map, SongData songData, List<string> keys, float scoreMultiplier)
	{
		_selectedMap = map;
		_songData = songData;
		_notes = _songData.notes;
		_mapSong = Resources.Load<AudioClip>($"Songs/{_selectedMap}/{_songData.audioFile}");
		_scoreManager.LoadScore(_selectedMap);
		availableKeys = keys;

		ScoreX = scoreMultiplier;

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
		StartCoroutine(WaitForMusicEnd());
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

			if (currNote.transform.position.x <= finishPosX - greenOffset)
			{
				Debug.Log("Miss!");
				notesOnScreen.Dequeue();
			}

		}
	}

	public void TestSave()
    {
		Debug.Log("Music stop!");

		songStarted = false;
		_audioSource.Stop();
		songposition = 0;

		int finalScore = _scoreManager.CurrentScore;
		_scoreManager.SaveScore(_selectedMap);
		Leaderboard.Instance.SaveScore(finalScore, _selectedMap);
		ScoreUI.ShowFinalResult(finalScore.ToString());
		//_scoreManager.ResetScore();
	}

	IEnumerator WaitForMusicEnd()
	{
		yield return new WaitUntil(() => !_audioSource.isPlaying);

		Debug.Log("Музыка закончилась!");

		songStarted = false;
		_audioSource.Stop();
		songposition = 0;

		int finalScore = _scoreManager.CurrentScore;
		_scoreManager.SaveScore(_selectedMap);
		Leaderboard.Instance.SaveScore(finalScore, _selectedMap);
		ScoreUI.ShowFinalResult(finalScore.ToString());

	}

}