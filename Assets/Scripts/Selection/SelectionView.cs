using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectionView : MonoBehaviour
{
	public abstract void ChangePreviewBackground(Image image);
	public abstract void ShowHideSelectionUI(GameObject selectionUI, GameObject playUI);
	public abstract void ShowSongsList(SongData songData, TextAsset jsonFile);
}
