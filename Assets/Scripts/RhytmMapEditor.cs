using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class RhythmMapEditor : MonoBehaviour
{
    [Header("BaseVariables")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider timelineSlider;
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Transform notesContainer;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private TMP_Text TimeLineText;

    [Header("NoteTimeLine")]
    [SerializeField] private RectTransform noteTimeline; // Линия таймлайна
    [SerializeField] private RectTransform noteContainer; // Контейнер для нот
    [SerializeField] private GameObject noteMarkerPrefab; // Префаб кружочка

    private List<float> notes = new List<float>();
    private bool isRecording = false;
    private float timelineScrollSpeed = 100f;

    private float lastNotePosition = 0f; // Переменная для отслеживания позиции предыдущей ноты
    [SerializeField] private float noteSpacing = 1f; // Расстояние между нотами

    private void Start()
    {
        playButton.onClick.AddListener(PlayPauseSong);
        saveButton.onClick.AddListener(SaveNotes);
        timelineSlider.onValueChanged.AddListener(UpdateTimeline);
    }

    private void Update()
    {
        TimeLineText.text = audioSource.time.ToString();

        if (audioSource.isPlaying)
        {
            timelineSlider.value = audioSource.time / audioSource.clip.length;
            ScrollTimeline();
        }

        if (isRecording && Input.anyKeyDown)
        {
            float noteTime = audioSource.time * 1000;
            notes.Add(noteTime);
            Debug.Log($"Нота добавлена: {noteTime} мс");
            SpawnNoteMarker(noteTime);
            SpawnNote(noteTime);
        }
    }

    public void PlayPauseSong()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause(); // Ставит на паузу
            isRecording = false;
        }
        else
        {
            audioSource.Play(); // Возобновляет с текущего места
            isRecording = true;
        }
    }

    public void StopSong()
    {
        audioSource.Stop();
        isRecording = false;
    }

    private void UpdateTimeline(float value)
    {
        audioSource.time = value * audioSource.clip.length;
    }

    private void SpawnNote(float time)
    {
        // Если это не первая нота, добавляем смещение от предыдущей
        float spawnPosition = lastNotePosition + noteSpacing;

        GameObject note = Instantiate(notePrefab, notesContainer);
        note.GetComponentInChildren<Text>().text = time.ToString("F0") + " ms";

        // Сохраняем текущую позицию ноты для следующей
        lastNotePosition = spawnPosition;

        // Примените spawnPosition, если нужно, для позиционирования ноты в пространстве
        note.GetComponent<RectTransform>().anchoredPosition = new Vector2(spawnPosition, note.GetComponent<RectTransform>().anchoredPosition.y);
    }

    public void SaveNotes()
    {
        string path = Path.Combine(Application.persistentDataPath, "notes.json");
        File.WriteAllText(path, JsonUtility.ToJson(new NoteData(notes)));
        Debug.Log("Сохранено: " + path);
    }

    private void SpawnNoteMarker(float noteTime)
    {
        Debug.Log("SpawnNoteMarker вызвался");
        float timelineWidth = noteTimeline.rect.width;
        float songDurationMs = audioSource.clip.length * 1000;
        float normalizedTime = noteTime / songDurationMs;
        float positionX = Mathf.Lerp(0, timelineWidth, normalizedTime);

        GameObject noteMarker = Instantiate(noteMarkerPrefab, noteContainer);
        noteMarker.GetComponent<RectTransform>().anchoredPosition = new Vector2(positionX, 0);
    }

    private void ScrollTimeline()
    {
        float timelineWidth = noteTimeline.rect.width;
        float songDurationMs = audioSource.clip.length * 1000;
        float normalizedTime = audioSource.time * 1000 / songDurationMs;
        float newX = Mathf.Lerp(0, timelineWidth, normalizedTime);
        noteContainer.anchoredPosition = new Vector2(-newX, noteContainer.anchoredPosition.y);
    }
}

[System.Serializable]
public class NoteData
{
    public List<float> notes;
    public NoteData(List<float> notes)
    {
        this.notes = notes;
    }
}
