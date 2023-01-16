using Events.Menu;
using Structures.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Pages
{
	/// <summary>
	///  Class that implements credits menu logic
	/// </summary>
	public class CreditsMenu : UIMenu
	{
		#region Serialized Fields

		[field: SerializeField]
		private Button Return;
	
		[field: SerializeField]
		private SwitchMenu SwitchCurrentMenu { get; set; }

		#endregion
		#region Protected Methods

		/// <summary>
		/// methode that assigns actions methods
		/// </summary>
		protected override void SubscribeEvents() => this.Return.onClick.AddListener(this.ReturnAction);

		/// <summary>
		/// methode that unassigned actions methods
		/// </summary>
		protected override void UnsubscribeEvents() => this.Return.onClick.RemoveListener(this.ReturnAction);

		#endregion
		#region Private Methods

		/// <summary>
		/// methode that redirects player to main menu
		/// </summary>
		private void ReturnAction()
		{
			this.SwitchCurrentMenu.switchMenu?.Invoke(MenuType.MainMenu);
		}

		#endregion
	}
}
