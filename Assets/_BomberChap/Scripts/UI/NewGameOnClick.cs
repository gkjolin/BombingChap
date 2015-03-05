﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(Button))]
	public class NewGameOnClick : MonoBehaviour 
	{
		[SerializeField]
		private string m_singlePlayerSceneName;

		private Button m_button;
		
		private void Awake()
		{
			m_button = GetComponent<Button>();
			m_button.onClick.AddListener(HandleOnClick);
		}
		
		private void OnDestroy()
		{
			if(m_button != null)
				m_button.onClick.RemoveListener(HandleOnClick);
		}
		
		private void HandleOnClick()
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.GAME_LEVEL, 0);
			PlayerPrefs.SetInt(PlayerPrefsKeys.BOMB_COUNT, GlobalConstants.MIN_BOMB_COUNT);
			PlayerPrefs.SetInt(PlayerPrefsKeys.BOMB_RANGE, GlobalConstants.MIN_BOMB_RANGE);
			PlayerPrefs.SetFloat(PlayerPrefsKeys.PLAYER_SPEED, GlobalConstants.MIN_PLAYER_SPEED);

			Application.LoadLevel(m_singlePlayerSceneName);
		}
	}
}