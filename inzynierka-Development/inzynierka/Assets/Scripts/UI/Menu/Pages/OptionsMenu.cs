using Events.Menu;
using Structures.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Pages
{
	/// <summary>
	///  Class that implements options menu logic
	/// </summary>
	public class OptionsMenu : UIMenu
	{
		#region Serialized Fields

		[field: SerializeField]
		private Button Return { get; set; }
	
		[field: SerializeField]
		private Slider MasterSlider { get; set; }

		[field: SerializeField]
		private Slider MusicsSlider { get; set; }

		[field: SerializeField]
		private Slider EffectsSlider { get; set; }

		[field: SerializeField]
		private SwitchMenu SwitchCurrentMenu { get; set; }

	
		#endregion
		#region Variables

		private FMOD.Studio.Bus master;
		private FMOD.Studio.Bus effects;
		private FMOD.Studio.Bus musics;

		#endregion
		#region UnityCallbacks

		private void Awake()
		{
			this.master = FMODUnity.RuntimeManager.GetBus("bus:/");
			this.musics = FMODUnity.RuntimeManager.GetBus("bus:/Master/Soundtracks");
			this.effects = FMODUnity.RuntimeManager.GetBus("bus:/Master/Effects");
		}

		#endregion
		#region Protected Methods
	
		/// <summary>
		/// methode that assigns actions methods
		/// </summary>
		protected override void SubscribeEvents()
		{
			this.Return.onClick.AddListener(this.ReturnAction);
			this.MasterSlider.onValueChanged.AddListener(x => this.master.setVolume(x));
			this.MusicsSlider.onValueChanged.AddListener(x => this.musics.setVolume(x));
			this.EffectsSlider.onValueChanged.AddListener(x => this.effects.setVolume(x));
		
		} 

		/// <summary>
		/// methode that unassigned actions methods
		/// </summary>
		protected override void UnsubscribeEvents()
		{
			this.Return.onClick.RemoveListener(this.ReturnAction);
			this.MasterSlider.onValueChanged.RemoveAllListeners();
			this.MusicsSlider.onValueChanged.RemoveAllListeners();
			this.EffectsSlider.onValueChanged.RemoveAllListeners();
		} 

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
