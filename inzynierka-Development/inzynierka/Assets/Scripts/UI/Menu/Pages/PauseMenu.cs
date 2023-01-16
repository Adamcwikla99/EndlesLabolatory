using Events.Game;
using Events.Scene;
using Events.UI;
using ScenesManager;
using Structures.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu.Pages
{
	/// <summary>
	///  Class that implements pause menu logic
	/// </summary>
	public class PauseMenu : UIMenu
	{
		#region Serialized Fields
		
		[field: SerializeField]
		private TMPro.TMP_Text EnemysCount { get; set; }
		
		[field: SerializeField]
		private TMPro.TMP_Text FloorsCount { get; set; }

		[field: SerializeField]
		private Button Continue { get; set; }

		[field: SerializeField]
		private Button MainMenu { get; set; }

		[field: SerializeField]
		private Button Restart { get; set; }
		
		[field: SerializeField]
		private UpdateBeatenEnemys BeatenEnemys { get; set; }

		[field: SerializeField]
		private UpdateBeatenFloors BeatenFloors { get; set; }
		
		[field: SerializeField]
		private SwitchScene ActiveSceneSwitch { get; set; } 
		
		[field: SerializeField]
		private TogglePlayerPointer PointerToggle { get; set; }
		
		#endregion
		#region Protected Methods

		/// <summary>
		/// methode that assigns actions methods
		/// </summary>
		protected override void SubscribeEvents()
		{
			this.Continue.onClick.AddListener(this.ContinueAction);
			this.MainMenu.onClick.AddListener(this.MainMenuAction);
			this.Restart.onClick.AddListener(this.RestartAction);
			this.BeatenEnemys.RelayNewBeatenEnemysCount += UpdateEnemysScore;
			this.BeatenFloors.RelayNewBeatenFloorsCount += UpdateFloorsCount;
			StopTime(true);
			PointerToggle.ToggleState?.Invoke(false);
			Cursor.visible = true;
		}

		/// <summary>
		/// methode that unassigned actions methods
		/// </summary>
		protected override void UnsubscribeEvents()
		{
			Cursor.visible = false;
			this.Continue.onClick.RemoveListener(this.ContinueAction);
			this.MainMenu.onClick.RemoveListener(this.MainMenuAction);
			this.Restart.onClick.RemoveListener(this.RestartAction);
			this.BeatenEnemys.RelayNewBeatenEnemysCount -= UpdateEnemysScore;
			this.BeatenFloors.RelayNewBeatenFloorsCount -= UpdateFloorsCount;
			StopTime(false);
			PointerToggle.ToggleState?.Invoke(true);
		}

		#endregion
		#region Private Methods

		/// <summary>
		/// methode that stops game time
		/// </summary>
		private void StopTime(bool state)
		{
			if (state == true)
			{
				Time.timeScale = 0f;
				return;
			}
			
			Time.timeScale = 1f;
		}
		
		/// <summary>
		/// methode that updates displayed beaten enemy score
		/// </summary>
		private void UpdateEnemysScore(int value)
		{
			this.EnemysCount.text = value.ToString();
		}

		/// <summary>
		/// methode that updates displayed beaten floors score
		/// </summary>
		private void UpdateFloorsCount(int value)
		{
			this.FloorsCount.text = value.ToString();
		}

		/// <summary>
		/// methode that closes popup
		/// </summary>
		private void ContinueAction()
		{
			this.gameObject.SetActive(false);
		}

		/// <summary>
		/// methode that redirect player to main menu
		/// </summary>
		private void MainMenuAction()
		{
			ContinueAction();
			this.ActiveSceneSwitch.SwitchCurrentScene?.Invoke(SceneType.Menu);
		}

		/// <summary>
		/// method that restarts player game attempt
		/// </summary>
		private void RestartAction()
		{
			ContinueAction();
			this.ActiveSceneSwitch.RestartGameMap?.Invoke();
		}

		#endregion
	}
}
