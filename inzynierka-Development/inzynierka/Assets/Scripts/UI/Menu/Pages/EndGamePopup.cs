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
	///  Class that implements end game popup logic
	/// </summary>
    public class EndGamePopup : UIMenu
    {
        #region Serialized Fields
		
        [field: SerializeField]
        private TMPro.TMP_Text EnemysCount { get; set; }
		
        [field: SerializeField]
        private TMPro.TMP_Text FloorsCount { get; set; }

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
        #region methods
	
		/// <summary>
		/// methode that assigns actions methods
		/// </summary>
		protected override void SubscribeEvents()
		{
			StopTime(true);
			this.MainMenu.onClick.AddListener(MainMenuAction);
			this.Restart.onClick.AddListener(RestartAction);
			this.BeatenEnemys.RelayNewBeatenEnemysCount += UpdateEnemysScore;
			this.BeatenFloors.RelayNewBeatenFloorsCount += UpdateFloorsCount;
			PointerToggle.ToggleState?.Invoke(false);
			Cursor.visible = true;
		}
		
		/// <summary>
		/// methode that unassigned actions methods
		/// </summary>
		protected override void UnsubscribeEvents()
		{
			Cursor.visible = false;
			StopTime(false);
			this.MainMenu.onClick.RemoveListener(MainMenuAction);
			this.Restart.onClick.RemoveListener(RestartAction);
			this.BeatenEnemys.RelayNewBeatenEnemysCount -= UpdateEnemysScore;
			this.BeatenFloors.RelayNewBeatenFloorsCount -= UpdateFloorsCount;
			PointerToggle.ToggleState?.Invoke(true);
		}
		
		/// <summary>
		/// methode that stops time
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
			Debug.Log("");
			this.ActiveSceneSwitch.SwitchCurrentScene?.Invoke(SceneType.Menu);
		}

		/// <summary>
		/// method that restarts player game attempt
		/// </summary>
		private void RestartAction()
		{
			Debug.Log("");
			ContinueAction();
			this.ActiveSceneSwitch.RestartGameMap?.Invoke();
		}
		
        #endregion



    }
}
