using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SongSelection : MonoBehaviour
{
    [SerializeField] private PlayMap _game;
    [SerializeField] private GameObject _selectionUI;
    [SerializeField] private GameObject _playUI;
    [SerializeField] private AudioSource previewAudioSource;
    [SerializeField] private Image coverImage;
    [SerializeField] private Button startGameButton;

    private string selectedSongFolder = "";
    private SongData _songData;

    [SerializeField] SelectionUI_View UI;

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
            UI.ShowSongsList(songData, jsonFile);
        }
    }
    public void SelectSong(string songFolder, SongData songData, GameObject button)
    {
        selectedSongFolder = songFolder;
        _songData = songData;

        Sprite coverSprite = Resources.Load<Sprite>($"Songs/{selectedSongFolder}/{_songData.coverImage}");
        if (coverSprite != null)
        {
            coverImage.sprite = coverSprite;
            UI.ChangePreviewBackground(coverImage);
        }

        AudioClip clip = Resources.Load<AudioClip>($"Songs/{selectedSongFolder}/{_songData.audioFile}");
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
            UI.ShowHideSelectionUI(_selectionUI, _playUI);

            Debug.Log($"playing map: {selectedSongFolder}");
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
    public float time;
}
