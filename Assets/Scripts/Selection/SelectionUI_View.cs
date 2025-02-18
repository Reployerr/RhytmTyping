using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionUI_View : SelectionView
{
	[SerializeField] private Image bgSprite;
	[SerializeField] private Button startGameButton;
	[SerializeField] private SongSelection _songSelection;
	[SerializeField] private Transform _songListContainer;
	[SerializeField] private GameObject _songBtnPrefab;

	[SerializeField] private SongData _songData;
	private TextAsset _songTextAsset;
	

	private bool uiIsHide = false;

    public override void ChangePreviewBackground(Image image)
	{
		this.bgSprite.sprite = image.sprite;
	}

	public override void ShowHideSelectionUI(GameObject selectionUI, GameObject playUI)
	{
			selectionUI.SetActive(false);
			playUI.SetActive(true);

	}

    public override void ShowSongsList(SongData songData, TextAsset jsonFile)
    {
		GameObject button = Instantiate(_songBtnPrefab, _songListContainer);
		button.GetComponentInChildren<TextMeshProUGUI>().text = $"{songData.artist} - {songData.songName}";
		button.GetComponent<Button>().onClick.AddListener(() => _songSelection.SelectSong(jsonFile.name, songData, _songBtnPrefab));
	}
}
