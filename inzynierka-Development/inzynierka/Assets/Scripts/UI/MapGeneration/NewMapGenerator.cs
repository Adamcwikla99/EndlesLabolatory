using MapGenerator.MapLayoutGeneration;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MapGeneration
{
	/// <summary>
	///     Class used in inspector for generating new map. It allows to easily check if generation parameters change was good
	///     or not
	/// </summary>
	public class NewMapGenerator : MonoBehaviour
	{
		#region Private Properties

		[field: SerializeField]
		private Button GenerateButton { get; set; }

		[field: SerializeField]
		private MapLayout Layout { get; set; }

		#endregion
		#region Unity Callbacks

		private void Start() => this.GenerateButton.onClick.AddListener(this.GenerateAcction);

		private void OnDestroy() => this.GenerateButton.onClick.RemoveListener(this.GenerateAcction);

		#endregion
		#region Private Methods

		/// <summary>
		/// methode that generates new floor map
		/// </summary>
		private void GenerateAcction() => this.Layout.GenerateNewMap();

		#endregion
	}
}
