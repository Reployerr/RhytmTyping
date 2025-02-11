using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMap : MonoBehaviour
{
	[SerializeField] private float _delay;
	[SerializeField] private AudioSource _audioSource;

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

		foreach (var note in _notes)
		{
			Debug.Log($"Нота появится в {note.time} секундах");
		}
	}

	 IEnumerator SongStartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartMap();
    }
}
