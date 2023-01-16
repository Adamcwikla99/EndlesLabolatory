using System.Collections.Generic;
using Structures.Enums;
using Structures.Map;
using UnityEngine;

namespace MapGenerator.MapLayoutGeneration
{
	/// <summary>
	///  Class responsible for spawning visual representation of generated map
	/// </summary>
	public class VisualizeMap : MonoBehaviour
	{
		#region Serialized Fields

		[field: SerializeField]
		private GameObject NormalRoom;

		[field: SerializeField]
		private GameObject ShopRoom;
		
		[field: SerializeField]
		private GameObject ItemRoom;

		[field: SerializeField]
		private GameObject BossRoom;

		[field: SerializeField]
		private GameObject StartRoom;

		[field: SerializeField]
		private GameObject GenerationStart;

		[field: SerializeField]
		private float SeparationSpace = 5f;

		[field: SerializeField]
		private float CubeSize = 2f;

		#endregion
		#region Private Variables

		private float currentX;
		private float currentZ;
		private List<GameObject> spawnedMapRooms = new List<GameObject>();

		#endregion
		#region Public Methods

		/// <summary>
		/// methode that initialize visualization process
		/// </summary>
		/// <param name="rows"> map to display</param>
		/// <param name="floorDimensions">map dimensions </param>
		public void Visualize(Row[] rows, FloorDimensions floorDimensions)
		{
			ScriptPreparation();
			IndividualRowOperations(rows, floorDimensions);
		}

		#endregion
		#region Private Methods

		/// <summary>
		/// methode responsible for reset of variables
		/// </summary>
		private void ScriptPreparation()
		{
			this.currentX = 0;
			this.currentZ = 0;
			this.spawnedMapRooms = Tools.ClearAndDestroy(this.spawnedMapRooms);
		}

		/// <summary>
		/// methode that initialize visualization process
		/// </summary>
		/// <param name="rows"> map to display</param>
		/// <param name="floorDimensions">map dimensions </param>
		private void IndividualRowOperations(Row[] rows, FloorDimensions floorDimensions)
		{
			for (int x = 0; x < floorDimensions.rowsAmount; x++)
			{
				GenerateRowColumns(rows, x, floorDimensions.columnsAmount);
				this.currentX += this.SeparationSpace + this.CubeSize;
			}
		}

		/// <summary>
		/// generates rows columns
		/// </summary>
		/// <param name="rows"> map to display</param>
		/// <param name="x"> process row index</param>
		/// <param name="columnsAmount"> amount of columns</param>
		private void GenerateRowColumns(Row[] rows, int x, int columnsAmount)
		{
			this.currentZ = 0;
			RoomType[] tempRow = rows[x].GetRowColumns();

			for (int y = 0; y < columnsAmount; y++)
			{
				CaseSpawner(tempRow[y]);
				this.currentZ += this.SeparationSpace + this.CubeSize;
			}
		}

		/// <summary>
		///  methode responsible for generating correct room type
		/// </summary>
		/// <param name="caseOfSwitch"> room type</param>
		private void CaseSpawner(RoomType caseOfSwitch)
		{
			switch (caseOfSwitch)
			{
				case RoomType.Nothing :
					break;
				case RoomType.StartRoom :
					GenerateAndAddToList(this.StartRoom, new Vector3(this.currentX, 0, this.currentZ));

					break;
				case RoomType.ShopRoom :
					GenerateAndAddToList(this.ShopRoom, new Vector3(this.currentX, 0, this.currentZ));

					break;
				case RoomType.BossRoom :
					GenerateAndAddToList(this.BossRoom, new Vector3(this.currentX, 0, this.currentZ));

					break;
				case RoomType.NormalRoom :
					GenerateAndAddToList(this.NormalRoom, new Vector3(this.currentX, 0, this.currentZ));

					break;
				case RoomType.ItemRoom :
					GenerateAndAddToList(this.ItemRoom, new Vector3(this.currentX, 0, this.currentZ));
					break;
				case RoomType.NoNeighbor :
					break;
			}
		}

		/// <summary>
		/// methode responsible for storing and generating viusalization
		/// </summary>
		/// <param name="toSpawn"></param>
		/// <param name="offset"></param>
		private void GenerateAndAddToList(GameObject toSpawn, Vector3 offset)
		{
			GameObject spawned = Instantiate(toSpawn, this.GenerationStart.transform);
			spawned.transform.localPosition += offset;
			this.spawnedMapRooms.Add(spawned);
		}

		#endregion
	}
}
