using System;
using CustomExtensions;
using Data;
using Events;
using Events.FileManagment;
using Events.RoomEditor;
using Structures.Enums;
using Structures.Map.Room;
using TMPro;
using UnityEngine;



namespace MapGenerator.RoomLeyout
{
	/// <summary>
	///  class that allows to create in editor room entity spawning zones
	/// </summary>
	public class RoomGridEditorController : MonoBehaviour
	{
		#region Serialized Fields

		[field: SerializeField]
		private GameObject CameraObj;
		
		[field: SerializeField]
		private TMP_Text CurrentPaintMode { get; set; }

		[field: SerializeField]
		private PlayerEditor PlayerInput { get; set; }

		[field: SerializeField]
		private SaveSystem SaveEvents { get; set; }
		#endregion
		#region Public Properties

		[field: SerializeField]
		public Vector3 WorldPosition { get; set; }

		#endregion
		#region Private Fields

		private int indexOffset;
		private RoomMarker[,] roomMarkers;
		private RoomGridCell[,] roomCell;
		private RoomGridCell currentType = RoomGridCell.Empty;

		#endregion
		#region publick variabels

		public int columnLength, rowLength;
		public string fileName;
		public LayerMask layerMask;
		public bool loadChanges;
		public GameObject prefab;

		public bool saveChanges;
		public float x_Space, y_Space;

		public float x_Start, y_Start;
		

		#endregion
		#region Unity Callbacks

		private void Awake() => Init();

		private void OnEnable() => SubscribeToEvents();

		private void OnDisable() => UnSubscribeFromEvents();

		private void Start() => InitGrid();

		private void Update() => CheckForFileOperations();

		#endregion
		#region Private Methods

		/// <summary>
		/// methode that initializes the grid
		/// </summary>
		private void InitGrid()
		{
			int x = 0;
			int tempI = 0;

			for (int i = 0; i < this.columnLength * this.rowLength; i++)
			{
				SpawnMark(ref x, ref tempI, i);
			}

		}

		/// <summary>
		/// methode that spawns grid marked object
		/// </summary>
		private void SpawnMark(ref int x, ref int tempI, int i)
		{
			GameObject spawnedMark =
				Instantiate(this.prefab,
							new Vector3(this.transform.position.x + this.x_Start + this.x_Space * (i % this.columnLength), 6,
										this.transform.position.z + this.y_Start + -this.y_Space * (i / this.columnLength)), Quaternion.identity);

			spawnedMark.transform.SetParent(this.transform);
			AdjustGridLists(x, tempI, spawnedMark);
			MoveGridIterator(ref x, ref tempI, i);

		}

		/// <summary>
		/// methode that updates marker list grid
		/// </summary>
		/// <param name="x"></param>
		/// <param name="tempI"></param>
		/// <param name="spawnedMark"></param>
		private void AdjustGridLists(int x, int tempI, GameObject spawnedMark)
		{
			this.roomMarkers[x, tempI] = spawnedMark.GetComponent<RoomMarker>();
			this.roomCell[x, tempI] = RoomGridCell.Empty;
		}

		/// <summary>
		/// methode that updates grid cell that designer is looking at
		/// </summary>
		private void MoveGridIterator(ref int x, ref int tempI, int i)
		{
			if (tempI == this.columnLength - 1)
			{
				x++;
				tempI = 0;

				return;
			}

			tempI++;
		}

		/// <summary>
		/// methode that changes camera coom
		/// </summary>
		private void ChangeCameraZoom(float toModify) => this.CameraObj.transform.position += new Vector3(0, toModify, 0);

		/// <summary>
		/// methode that changes camera position
		/// </summary>
		/// <param name="toModify"></param>
		private void ChangeCameraPosition(Vector2 toModify)
		{
			Vector3 move = new Vector3(toModify.x, 0, toModify.y);
			this.CameraObj.transform.position += move;
		}

		/// <summary>
		/// methode that changes marker type
		/// </summary>
		/// <param name="hitData"></param>
		private void ChangeMarkerType(RaycastHit hitData)
		{
			this.WorldPosition = hitData.point;

			CordsXY cellCord = this.FindGridCellCord(hitData.point);
			this.roomMarkers[cellCord.y, cellCord.x].ChangeType(this.currentType, this.GetColor(this.currentType));
			this.roomCell[cellCord.y, cellCord.x] = this.currentType;
			Debug.Log("Selected cell " + cellCord.x + " " + cellCord.y);
		}

		/// <summary>
		/// methode that finds cell on cell grid
		/// </summary>
		private CordsXY FindGridCellCord(Vector3 pos)
		{
			int x, z;
			float tempX = pos.x * -1;
			x = Mathf.FloorToInt(tempX);
			float tempz = pos.z;
			z = Mathf.FloorToInt(tempz);

			x -= this.indexOffset;
			z -= this.indexOffset;

			x *= -1;
			z *= -1;

			return new CordsXY(x, z);
		}

		/// <summary>
		/// methode that initializes class values
		/// </summary>
		private void Init()
		{

			InitGridVariables();
			InitDisplayedText();
		}
		
		/// <summary>
		/// methode that initializes grid values
		/// </summary>
		private void InitGridVariables()
		{
			this.indexOffset = this.columnLength / 2;
			this.indexOffset--;
			this.roomCell = new RoomGridCell[this.columnLength, this.columnLength];
			this.roomMarkers = new RoomMarker[this.columnLength, this.columnLength];
		}

		
		/// <summary>
		/// methode that initializes displayed text
		/// </summary>
		private void InitDisplayedText() => this.CurrentPaintMode.text = this.currentType.ToString();

		/// <summary>
		/// methode that checks if save or load should be performed
		/// </summary>
		private void CheckForFileOperations()
		{
			if (this.saveChanges)
			{
				SaveData();
			}

			if (this.loadChanges)
			{
				LoadData();
			}
		}

		/// <summary>
		/// methode that saves template
		/// </summary>
		private void SaveData()
		{
			this.saveChanges = false;
			this.SaveEvents.SaveRoomLayout?.Invoke(new RoomCellLayout(this.roomCell), this.fileName);
		}

		/// <summary>
		/// methode that loads template
		/// </summary>
		private void LoadData()
		{
			this.loadChanges = false;
			RoomCellLayout loadedData = this.SaveEvents.LoadRoomLayout?.Invoke(this.fileName);
			this.roomCell = loadedData.forLoad;
			ProcessLoadedData();
		}

		/// <summary>
		/// methode that processes loaded template
		/// </summary>

		private void ProcessLoadedData()
		{
			for (int x = 0; x < this.columnLength; x++)
			{
				for (int y = 0; y < this.columnLength; y++)
				{
					this.roomMarkers[x, y].ChangeType(this.roomCell[x, y], this.GetColor(this.roomCell[x, y]));
				}
			}
		}

		/// <summary>
		/// methode that sets cell color base on cell type
		/// </summary>
		private Color GetColor(RoomGridCell cell) => cell switch
		{
			RoomGridCell.Empty => Color.white,
			RoomGridCell.Wall => Color.black,
			RoomGridCell.Enemy => Color.red,
			_ => throw new NotImplementedException()
		};

		/// <summary>
		/// methode that sets next cell type
		/// </summary>
		private void NextCellType()
		{
			this.currentType.Next();
			this.CurrentPaintMode.text = this.currentType.ToString();
		}

		/// <summary>
		/// methode that previous next cell type
		/// </summary>
		private void PreviousCellType()
		{
			this.currentType.Previous();
			this.CurrentPaintMode.text = this.currentType.ToString();
		}

		/// <summary>
		/// methode that subscribes to events
		/// </summary>
		private void UnSubscribeFromEvents()
		{
			this.PlayerInput.NextCell -= NextCellType;
			this.PlayerInput.PreviousCell -= PreviousCellType;
			this.PlayerInput.ReadMove -= ChangeCameraPosition;
			this.PlayerInput.ReadZoom -= ChangeCameraZoom;
			this.PlayerInput.ChangeMarkerType -= ChangeMarkerType;
		}

		/// <summary>
		/// methode that unsubscribes to events
		/// </summary>
		private void SubscribeToEvents()
		{
			this.PlayerInput.NextCell += NextCellType;
			this.PlayerInput.PreviousCell += PreviousCellType;
			this.PlayerInput.ReadMove += ChangeCameraPosition;
			this.PlayerInput.ReadZoom += ChangeCameraZoom;
			this.PlayerInput.ChangeMarkerType += ChangeMarkerType;
		}

		#endregion
	}

}
