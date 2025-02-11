using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SongSelection : MonoBehaviour
{

    [SerializeField] private PlayMap _game;
    [SerializeField] private GameObject songButtonPrefab;
    [SerializeField] private Transform songListContainer;
    [SerializeField] private AudioSource previewAudioSource;
    [SerializeField] private Image coverImage;
    [SerializeField] private Button startGameButton;

    private string selectedSongFolder = "";
    private SongData _songData;

    private void Start()
    {
        LoadSongs();
        startGameButton.interactable = false;
    }

    private void LoadSongs()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Songs");

        foreach (var jsonFile in jsonFiles)
        {
            SongData songData = JsonUtility.FromJson<SongData>(jsonFile.text);
            if (songData == null || string.IsNullOrEmpty(songData.songName))
            {
                continue;
            }

            GameObject button = Instantiate(songButtonPrefab, songListContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"{songData.artist} - {songData.songName}";
            button.GetComponent<Button>().onClick.AddListener(() => SelectSong(jsonFile.name, songData, button));
        }
    }

    private void SelectSong(string songFolder, SongData songData, GameObject button)
    {
        selectedSongFolder = songFolder;
        _songData = songData;

        Sprite coverSprite = Resources.Load<Sprite>($"Songs/{songFolder}/{songData.coverImage}");
        if (coverSprite != null)
        {
            coverImage.sprite = coverSprite;
            //coverImage.SetNativeSize();
        }

        AudioClip clip = Resources.Load<AudioClip>($"Songs/{songFolder}/{songData.audioFile}");
        if (clip != null)
        {
            previewAudioSource.clip = clip;
            previewAudioSource.Play();
        }

        startGameButton.interactable = true;
    }

    public void StartGame()
    {
        if (!string.IsNullOrEmpty(selectedSongFolder))
        {
            previewAudioSource.Stop();
            Debug.Log($"Начинаем игру с картой: {selectedSongFolder}");
            _game.InitializeMap(selectedSongFolder, _songData);
        }
    }
}

[System.Serializable]
public class SongData
{
    public string songName;
    public string artist;
    public string audioFile;
    public string coverImage;
    public List<NotesData> notes;
}

[System.Serializable]
public class NotesData
{
    public float time; // Время нажатия на ноту
}
