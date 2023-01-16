using UnityEngine;

namespace UI.Menu.Pages
{
	/// <summary>
	///  Class that implements basic ui methods logic
	/// </summary>
	public abstract class UIMenu : MonoBehaviour
	{
		#region Unity Callbacks

		/// <summary>
		/// methode that assures that event assignment methode will be always called
		/// </summary>
		protected void OnEnable()
		{
			this.SubscribeEvents();
		}

		/// <summary>
		/// methode that assures that event unassignment methode will be always called
		/// </summary>
		protected void OnDisable()
		{
			this.UnsubscribeEvents();
		}

		#endregion
		#region Protected Methods

		/// <summary>
		/// abstract methode that should contain assigment of action methods and initializing values
		/// </summary>
		protected abstract void SubscribeEvents();

		/// <summary>
		/// abstract methode that should contain assigment of action methods and initializing values
		/// </summary>
		protected abstract void UnsubscribeEvents();

		#endregion
	}
}
