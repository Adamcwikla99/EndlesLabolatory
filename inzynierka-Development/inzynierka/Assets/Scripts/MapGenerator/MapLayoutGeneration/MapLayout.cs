using Structures.Enums;
using Structures.Map;
using Structures.Map.Room;
using UnityEngine;

namespace MapGenerator.MapLayoutGeneration
{
	/// <summary>
	///  Class responsible for generation of floor rooms layout
	/// </summary>
	public class MapLayout : MonoBehaviour
	{
		#region Public Properties

		[field: SerializeField]
		public float MinimalBossDistanceThreshold { get; private set; } = 0.7f;

		[field: SerializeField]
		public float MinimalShopDistanceThreshold { get; private set; } = 0.4f;

		[field: SerializeField]
		public CordsXY BossRoomCords { get; private set; }

		[field: SerializeField]
		public CordsXY ShopRoomCords { get; private set; }

		[field: SerializeField]
		public CordsXY StartRoomCords { get; private set; }

		#endregion
		#region Private Properties

		[field: SerializeField]
		private FloorRoomMap FloorGenerator { get; set; }

		[field: SerializeField]
		private VisualizeMap MapVisualizator { get; set; }

		#endregion
		#region Private Fields

		private RoomType[][] floorRooms;
		private CordsXY mapMiddle;
		private int middle;

		#endregion
		#region Unity Callbacks

		private void Start() => InitNewMap();

		#endregion
		#region Public Methods

		/// <summary>
		/// methode that generates new map
		/// </summary>
		public void GenerateNewMap() => InitNewMap();

		/// <summary>
		/// methode that sets minimal boss distance threshold
		/// </summary>
		/// <param name="value"> new value</param>
		public void SetMinimalBossDistanceThreshold(float value) => this.MinimalBossDistanceThreshold = value;

		/// <summary>
		/// methode that sets minimal shop distance threshold
		/// </summary>
		/// <param name="value"> new value</param>
		public void SetMinimalShopDistanceThreshold(float value) => this.MinimalShopDistanceThreshold = value;

		#endregion
		#region Private Methods

		private void InitNewMap()
		{
			this.FloorGenerator = new FloorRoomMap(11, 11);
			this.FloorGenerator.GenerateNewMap();
			this.MapVisualizator.Visualize(this.FloorGenerator.GetMap(), this.FloorGenerator.GetFloorDimensions());
		}

		#endregion
	}
}
