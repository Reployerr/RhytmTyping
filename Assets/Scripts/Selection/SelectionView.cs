using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectionView : MonoBehaviour
{
	public abstract void ChangePreviewBackground(Image bg);
	public abstract void ChangePreviewAudio(AudioClip clip);
	public abstract void ShowHideSelectionUI(GameObject selectionUI);
}
