﻿using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class LoadMultiplayerWinScene : MonoBehaviour 
	{
		[SerializeField]
		private string m_sceneName;
		
		[SerializeField]
		private ScreenFader m_screenFader;
		
		private IEnumerator OnMultiPlayerMatchOver()
		{
			PauseManager.Pause();
			if(m_screenFader != null)
				yield return m_screenFader.FadeOut();
			else
				yield return null;
			
			Application.LoadLevel(m_sceneName);
		}
	}
}	