﻿using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(CharacterMotor))]
	[RequireComponent(typeof(PlayerStats))]
	public class PlayerController : MonoBehaviour
	{
		private const string ANIMATOR_MOVE_VERT_STATE = "MoveVert";
		private const string ANIMATOR_MOVE_HORZ_STATE = "MoveHorz";
		private const string ANIMATOR_START_MOVE_UP_STATE = "StartMoveUp";
		private const string ANIMATOR_START_MOVE_DOWN_STATE = "StartMoveDown";
		private const string ANIMATOR_START_MOVE_RIGHT_STATE = "StartMoveRight";
		private const string ANIMATOR_START_MOVE_LEFT_STATE = "StartMoveLeft";

		[SerializeField]
		private Animator m_animator;

		private CharacterMotor m_motor;
		private Level m_currentLevel;
		private int m_lastHDir;
		private int m_lastVDir;

		private void Start()
		{
			m_motor = GetComponent<CharacterMotor>();
			m_motor.Speed = PlayerPrefs.GetFloat(PlayerPrefsKeys.PLAYER_SPEED, GlobalConstants.MIN_PLAYER_SPEED);
			m_currentLevel = LevelManager.GetLoadedLevel();
			m_lastHDir = 0;
			m_lastVDir = 0;
			NotificationCenter.AddObserver(gameObject, Notifications.ON_GAME_LEVEL_COMPLETE);

			PlayerStats playerStats = GetComponent<PlayerStats>();
			playerStats.MaxBombs = PlayerPrefs.GetInt(PlayerPrefsKeys.BOMB_COUNT, GlobalConstants.MIN_BOMB_COUNT);
			playerStats.BombRange = PlayerPrefs.GetInt(PlayerPrefsKeys.BOMB_RANGE, GlobalConstants.MIN_BOMB_RANGE);
		}

		private void OnDestroy()
		{
			if(NotificationCenter.Exists)
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_GAME_LEVEL_COMPLETE);
		}

		private void Update()
		{
			if(m_motor.IsAtDestination && m_currentLevel != null)
			{
				HandleLocomotionInput();
				HandleActionInput();
			}
		}

		private void HandleLocomotionInput()
		{
			Vector2 tilePos = m_currentLevel.WorldToTile(transform.position);
			int h = 0, v = 0;

			if(Input.GetButton("MoveUp"))
				v = 1;
			else if(Input.GetButton("MoveDown"))
				v = -1;
			else if(Input.GetButton("MoveRight"))
				h = 1;
			else if(Input.GetButton("MoveLeft"))
				h = -1;

			if(v > 0) 
			{
				Tile tile = m_currentLevel.GetAt((int)tilePos.x, (int)tilePos.y - 1);
				if(tile != null && !tile.IsSolid)
				{
					m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x, (int)tilePos.y - 1, transform.position.z));
					if(v != m_lastVDir)
						m_animator.SetTrigger(ANIMATOR_START_MOVE_UP_STATE);
				}
				else
				{
					v = 0;
				}
			}
			else if(v < 0) 
			{
				Tile tile = m_currentLevel.GetAt((int)tilePos.x, (int)tilePos.y + 1);
				if(tile != null && !tile.IsSolid)
				{
					m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x, (int)tilePos.y + 1, transform.position.z));
					if(v != m_lastVDir)
						m_animator.SetTrigger(ANIMATOR_START_MOVE_DOWN_STATE);
				}
				else
				{
					v = 0;
				}
			}
			else if(h > 0) 
			{
				Tile tile = m_currentLevel.GetAt((int)tilePos.x + 1, (int)tilePos.y);
				if(tile != null && !tile.IsSolid)
				{
					m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x + 1, (int)tilePos.y, transform.position.z));
					if(h != m_lastHDir)
						m_animator.SetTrigger(ANIMATOR_START_MOVE_RIGHT_STATE);
				}
				else
				{
					h = 0;
				}
			}
			else if(h < 0) 
			{
				Tile tile = m_currentLevel.GetAt((int)tilePos.x - 1, (int)tilePos.y);
				if(tile != null && !tile.IsSolid)
				{
					m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x - 1, (int)tilePos.y, transform.position.z));
					if(h != m_lastHDir)
						m_animator.SetTrigger(ANIMATOR_START_MOVE_LEFT_STATE);
				}
				else
				{
					h = 0;
				}
			}

			m_lastHDir = h;
			m_lastVDir = v;

			m_animator.SetInteger(ANIMATOR_MOVE_HORZ_STATE, h);
			m_animator.SetInteger(ANIMATOR_MOVE_VERT_STATE, v);
		}

		private void HandleActionInput()
		{
			if(Input.GetButtonDown("DropBomb"))
				SendMessage("DropBomb");
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.tag == Tags.Flame)
				LevelManager.ReloadCurrentLevel();
		}

		private void OnGameLevelComplete()
		{
			PlayerStats playerStats = GetComponent<PlayerStats>();
			PlayerPrefs.SetInt(PlayerPrefsKeys.BOMB_COUNT, playerStats.MaxBombs);
			PlayerPrefs.SetInt(PlayerPrefsKeys.BOMB_RANGE, playerStats.BombRange);
			PlayerPrefs.SetFloat(PlayerPrefsKeys.PLAYER_SPEED, m_motor.Speed);
		}
	}
}