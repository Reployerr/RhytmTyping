using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionUI_View : SelectionView
{
	[SerializeField] private Transform songListContainer;
	[SerializeField] private AudioSource previewAudioSource;
	[SerializeField] private Image coverImage;
	[SerializeField] private Button startGameButton;

	private bool uiIsHide = false;
	public override void ChangePreviewAudio(AudioClip clip)
	{
		previewAudioSource.clip = clip;
		previewAudioSource.Play();
	}

	public override void ChangePreviewBackground(Image bg)
	{
		coverImage = bg;
	}

	public override void ShowHideSelectionUI(GameObject selectionUI)
	{
		uiIsHide = !uiIsHide;

		if (!uiIsHide)
		{
			selectionUI.SetActive(uiIsHide);
		}
	}
}
