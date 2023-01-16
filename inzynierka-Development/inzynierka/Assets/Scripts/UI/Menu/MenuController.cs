using System;
using Events.Menu;
using Events.UI;
using Structures.Enums;
using UI.Menu.Pages;
using UI.Popup;
using UnityEngine;

namespace UI.Menu
{
	/// <summary>
	/// Class responsible for displaying showing and hiding user interface popup
	/// </summary>
	public class MenuController : MonoBehaviour
	{
		#region Private Propertiesields
		
		[field: SerializeField]
		private SerializableDictionary<MenuType, UIMenu> MenuPrefabs { get; set; } = new SerializableDictionary<MenuType, UIMenu>();

		[field: SerializeField]
		private SwitchMenu SwitchActiveUI { get; set; }

		[field: SerializeField]
		private UIMenu CurrentActiveMenu { get; set; }
		
		[field: SerializeField]
		private ShopPopup PopupManagement { get; set; }
		
		[field: SerializeField]
		private ShopItemPopup ItemPopup { get; set; }
	
		#endregion
		#region variables

		private bool isGameFinished = false;

		#endregion
		#region Unity Callbacks

		private void Start()
		{
			this.LoadUIMenu(MenuType.MainMenu);
		}
		private void OnEnable()
		{
			EnableEvents();
		}

		private void OnDisable()
		{
			DisableEvents();
		}
		
		#endregion
		#region Private Methods

		/// <summary>
		/// method responsible for assigning event actions
		/// </summary>
		private void EnableEvents()
		{
			this.SwitchActiveUI.switchMenu += LoadUIMenu;
			this.PopupManagement.PopPopup += PopPopup;
			this.PopupManagement.TurnOffPopup += TurnOffPopup;
		}

		/// <summary>
		/// methode responsible for removing assigment of event actions
		/// </summary>
		private void DisableEvents()
		{
			this.SwitchActiveUI.switchMenu -= LoadUIMenu;
			this.PopupManagement.PopPopup -= PopPopup;
			this.PopupManagement.TurnOffPopup -= TurnOffPopup;
		}

		/// <summary>
		/// methode responsible for process of showing and hiding selected user interface popup
		/// </summary>
		/// <param name="toLoad">user interface popup to show</param>
		private void LoadUIMenu(MenuType toLoad)
		{
			if (this.isGameFinished == true)
			{
				return;
			}

			if (toLoad == MenuType.EndGameMenu)
			{
				this.isGameFinished = true;
			}
			
			UIMenu displayedMenu = this.MenuPrefabs.dictionary[toLoad];
			if (displayedMenu == this.CurrentActiveMenu)
			{
				TurnOffPopup(displayedMenu);
				return;
			}

			ChangeUI(displayedMenu);
		}

		/// <summary>
		/// methode responsible for hiding currently displaying user interface popup
		/// </summary>
		/// <param name="displayedMenu"> currently active user interface popup</param>
		private void TurnOffPopup(UIMenu displayedMenu)
		{
			this.CurrentActiveMenu = null;
			displayedMenu.gameObject.SetActive(false);
		}

		/// <summary>
		/// methode responsible for showing selected user interface popup
		/// </summary>
		/// <param name="displayedMenu">currently active user interface popup</param>
		private void ChangeUI(UIMenu displayedMenu )
		{
			if (this.CurrentActiveMenu != null)
			{
				this.CurrentActiveMenu.gameObject.SetActive(false);
			}
			
			this.CurrentActiveMenu = displayedMenu;
			this.CurrentActiveMenu.gameObject.SetActive(true);
		}

		/// <summary>
		/// methode that deactivates shop price popup
		/// </summary>
		private void TurnOffPopup()
		{
			this.ItemPopup.gameObject.SetActive(false);
		}
		
		/// <summary>
		/// methode that show shop price popup
		/// </summary>
		/// <param name="type">type of shop item</param>
		/// <param name="price">price of shop item</param>
		/// <param name="value">value or amount of bought item</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>

		private void PopPopup(ShopItemType type, float price, float value)
		{
			this.ItemPopup.gameObject.SetActive(true);
			string label = string.Empty;
			switch (type)
			{
				case ShopItemType.BuyHp :
					label = "health";
					break;
				case ShopItemType.BuyMaxHp :
					label = "max health";
					break;
				case ShopItemType.BuyArrows :
					label = "arrows";
					break;
				case ShopItemType.BuyGrenades :
					label = "Grenades";
					break;
				case ShopItemType.BuyRockets :
					label = "Rockets";
					break;
				default :
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
			this.ItemPopup.SetNewValues(label, price.ToString(),value.ToString());
		}
		
		#endregion
	}
}
