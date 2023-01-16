using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Pages
{
	/// <summary>
	///  Class that implements endgame menu logic - not used
	/// </summary>
	public class EndGameMenu : UIMenu
	{
		#region Serialized Fields

		[field: SerializeField]
		private Button Restart;

		[field: SerializeField]
		private Button MainMenu;

		#endregion
		#region Protected Methods

		protected override void SubscribeEvents()
		{
			this.Restart.onClick.AddListener(this.RestartAction);
			this.MainMenu.onClick.AddListener(this.MainMenuAction);
		}

		protected override void UnsubscribeEvents()
		{
			this.Restart.onClick.RemoveListener(this.RestartAction);
			this.MainMenu.onClick.RemoveListener(this.MainMenuAction);
		}

		#endregion
		#region Private Methods

		private void RestartAction()
		{

		}

		private void MainMenuAction()
		{

		}

		#endregion
	}
}
