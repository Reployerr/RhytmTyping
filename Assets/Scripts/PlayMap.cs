using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMap : MonoBehaviour
{
	[SerializeField] private float _delay;
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private GameObject notePrefab;
	[SerializeField] private Transform spawnPoint;
	[SerializeField] private Transform targetPosition; // Целевая точка для нот
	[SerializeField] private float noteTravelTime = 2.0f; // Время, за которое нота проходит путь

	private string _selectedMap = "";
	private AudioClip _mapSong;
	private SongData _songData;
	private List<NotesData> _notes = new List<NotesData>();

	public void InitializeMap(string map, SongData songData)
	{

		_selectedMap = map;
		_songData = songData;
		_notes = _songData.notes;
		_mapSong = Resources.Load<AudioClip>($"Songs/{_selectedMap}/{_songData.audioFile}");
		StartCoroutine(SongStartDelay(_delay));

		Debug.Log($"Загружено {_notes.Count} нот");


	}

	private void StartMap()
	{
		_audioSource.clip = _mapSong;
		_audioSource.Play();

		StartCoroutine(SpawnNotes());
	}

	private IEnumerator SpawnNotes()
	{
		foreach (var note in _notes)
		{
			float spawnTime = note.time - noteTravelTime; // Момент, когда нота должна появиться
			if (spawnTime < 0) spawnTime = 0; // Защита от появления "в прошлом"

			yield return new WaitForSeconds(spawnTime);

			Debug.Log($"Спавним ноту за {noteTravelTime} секунд до момента {note.time}");

			GameObject spawnedNote = Instantiate(notePrefab, spawnPoint);
			NoteMover mover = spawnedNote.GetComponent<NoteMover>();
			if (mover != null)
			{
				mover.Initialize(targetPosition.position, note.time - Time.timeSinceLevelLoad);
			}
		}
	}

	IEnumerator SongStartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartMap();
    }
}
