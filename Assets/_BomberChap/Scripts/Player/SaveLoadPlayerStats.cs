﻿using UnityEngine;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(CharacterMotor))]
	[RequireComponent(typeof(PlayerStats))]
	public class SaveLoadPlayerStats : MonoBehaviour 
	{
		private void Start()
		{
			PlayerStats playerStats = GetComponent<PlayerStats>();
			CharacterMotor motor = GetComponent<CharacterMotor>();

			playerStats.MaxBombs = PlayerPrefs.GetInt(PlayerPrefsKeys.BOMB_COUNT, GlobalConstants.MIN_BOMB_COUNT);
			playerStats.BombRange = PlayerPrefs.GetInt(PlayerPrefsKeys.BOMB_RANGE, GlobalConstants.MIN_BOMB_RANGE);
			motor.Speed = PlayerPrefs.GetFloat(PlayerPrefsKeys.PLAYER_SPEED, GlobalConstants.MIN_PLAYER_SPEED);

			NotificationCenter.AddObserver(gameObject, Notifications.ON_GAME_LEVEL_COMPLETE);
		}

		private void OnDestroy()
		{
			if(NotificationCenter.Exists)
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_GAME_LEVEL_COMPLETE);
		}

		private void OnGameLevelComplete()
		{
			PlayerStats playerStats = GetComponent<PlayerStats>();
			CharacterMotor motor = GetComponent<CharacterMotor>();

			PlayerPrefs.SetInt(PlayerPrefsKeys.BOMB_COUNT, playerStats.MaxBombs);
			PlayerPrefs.SetInt(PlayerPrefsKeys.BOMB_RANGE, playerStats.BombRange);
			PlayerPrefs.SetFloat(PlayerPrefsKeys.PLAYER_SPEED, motor.Speed);
		}
	}
}