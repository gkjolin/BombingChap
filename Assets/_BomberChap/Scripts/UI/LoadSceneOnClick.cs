﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(Button))]
	public class LoadSceneOnClick : MonoBehaviour 
	{
		[SerializeField]
		private string m_sceneName;

		[SerializeField]
		private float m_delay;

		[SerializeField]
		private bool m_ignoreTimescale;

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
			StartCoroutine(LoadScene());
		}

		private IEnumerator LoadScene()
		{
			if(m_ignoreTimescale)
			{
				float f = 0.0f;
				while(f < m_delay)
				{
					f += Time.unscaledDeltaTime;
					yield return null;
				}
			}
			else
			{
				yield return new WaitForSeconds(m_delay);
			}
			Application.LoadLevel(m_sceneName);
		}
	}
}