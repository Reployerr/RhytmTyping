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

    [SerializeField] private TMP_Dropdown difficultyDropdown;

    private string selectedSongFolder = "";
    private SongData _songData;
    private List<string> selectedKeys;

    [SerializeField] private SelectionUI_View UI;
    [SerializeField] private Leaderboard leaderboard;

    private void Start()
    {
        LoadSongs();
        startGameButton.interactable = false;

        difficultyDropdown.ClearOptions();
        difficultyDropdown.AddOptions(new List<string> { "Easy", "Medium", "Hard", "Pro" });
        difficultyDropdown.value = 0;
    }

    public void LoadSongs()
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

        Leaderboard.Instance.DisplayScores(songFolder);

        startGameButton.interactable = true;
    }
    public void StartGame()
    {
        if (!string.IsNullOrEmpty(selectedSongFolder))
        {
            previewAudioSource.Stop();
            UI.ShowHideSelectionUI(_selectionUI, _playUI);

            string difficultyFile = GetDifficultyFile();
            selectedKeys = LoadKeysFromJson(difficultyFile);

            Debug.Log($"playing map: {selectedSongFolder}");
            _game.InitializeMap(selectedSongFolder, _songData, selectedKeys);
            _game.InputForStart();
        }
    }

    private string GetDifficultyFile()
    {
        switch (difficultyDropdown.value)
        {
            case 0: return "easy_keys";
            case 1: return "medium_keys";
            case 2: return "hard_keys";
            case 3: return "pro_keys";
            default: return "easy_keys";
        }
    }

    private List<string> LoadKeysFromJson(string fileName) //
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Keys/{fileName}");
        if (jsonFile != null)
        {
            KeysData keysData = JsonUtility.FromJson<KeysData>(jsonFile.text);
            return keysData.keys;
        }
        return new List<string> { "A", "S", "D", "F" };
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

[System.Serializable]
public class KeysData
{
    public List<string> keys;
}