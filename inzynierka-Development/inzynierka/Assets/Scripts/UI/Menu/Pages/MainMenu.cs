using Events.Menu;
using Events.Scene;
using ScenesManager;
using Structures.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Pages
{
	/// <summary>
	///  Class that implements main menu logic
	/// </summary>
	public class MainMenu : UIMenu
	{

		[field: SerializeField]
		private Button StartGame { get; set; }

		[field: SerializeField]
		private Button Options { get; set; }

		[field: SerializeField]
		private Button Credits { get; set; }

		[field: SerializeField]
		private Button Exit { get; set; }

		[field: SerializeField]
		private SwitchScene SwitchCurrentScene { get; set; }

		[field: SerializeField]
		private SwitchMenu SwitchCurrentMenu { get; set; }
	
		[field: SerializeField]
		private GameObject EventSystem { get; set; }
	
		#region Protected Methods

		/// <summary>
		/// methode that assigns actions methods
		/// </summary>
		protected override void SubscribeEvents()
		{
			this.StartGame.onClick.AddListener(this.StartAction);
			this.Options.onClick.AddListener(this.OptionsAction);
			this.Credits.onClick.AddListener(this.CreditsAction);
			this.Exit.onClick.AddListener(this.ExitAction);
		}

		/// <summary>
		/// methode that unassigned actions methods
		/// </summary>
		protected override void UnsubscribeEvents()
		{
			this.StartGame.onClick.RemoveListener(this.StartAction);
			this.Options.onClick.RemoveListener(this.OptionsAction);
			this.Credits.onClick.RemoveListener(this.CreditsAction);
			this.Exit.onClick.RemoveListener(this.ExitAction);
		}

		#endregion
		#region Private Methods

		/// <summary>
		/// methode that initializes game attempt start
		/// </summary>
		private void StartAction()
		{
			this.SwitchCurrentScene.SwitchCurrentScene?.Invoke(SceneType.Basic);
			this.EventSystem.SetActive(false);
			this.gameObject.SetActive(false);
		}
		

		/// <summary>
		/// methode that redirect player to options UI panel
		/// </summary>
		private void OptionsAction()
		{
			this.SwitchCurrentMenu.switchMenu?.Invoke(MenuType.OptionsMenu);
		}

		/// <summary>
		/// methode that redirect player to credits UI panel
		/// </summary>
		private void CreditsAction()
		{
			this.SwitchCurrentMenu.switchMenu?.Invoke(MenuType.CreditsMenu);
		}

		/// <summary>
		/// methode that closes aplication
		/// </summary>
		private void ExitAction()
		{
			Application.Quit();
		}

		#endregion
	}
}
