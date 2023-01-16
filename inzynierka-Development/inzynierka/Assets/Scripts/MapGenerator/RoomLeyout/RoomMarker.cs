using Structures.Enums;
using UnityEngine;

namespace MapGenerator.RoomLeyout
{
	/// <summary>
	/// Class to mark grid cell and its current type in template editor scene
	/// </summary>
	public class RoomMarker : MonoBehaviour
	{
		#region Private Fields

		private RoomGridCell cell;
		private Renderer renderer;

		#endregion
		#region Unity Callbacks

		private void Start() => Init();

		#endregion
		#region Public Methods

		/// <summary>
		/// methode that changes cell marker type
		/// </summary>
		public void ChangeType(RoomGridCell newType, Color color)
		{
			this.cell = newType;
			this.renderer.material.color = color;
		}

		#endregion
		#region Private Methods

		/// <summary>
		/// methode that initialize marker settings
		/// </summary>
		private void Init()
		{
			this.renderer = this.gameObject.GetComponent<Renderer>();
			this.renderer.material.color = Color.white;
			this.cell = RoomGridCell.Empty;
		}

		#endregion
	}
}
