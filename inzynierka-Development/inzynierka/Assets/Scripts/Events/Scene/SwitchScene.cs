using System;
using ScenesManager;
using Structures.Enums;
using UnityEngine;

namespace Events.Scene
{
	/// <summary>
	///  Class responsible for relaying change scene events
	/// </summary>
	[CreateAssetMenu(fileName = "SwitchSceneEvent", menuName = "GameEvents/SwitchSceneEvent")]
	public class SwitchScene : ScriptableObject
	{
		public Action<SceneType> SwitchCurrentScene;
		public Action RestartGameMap;
	}
}
