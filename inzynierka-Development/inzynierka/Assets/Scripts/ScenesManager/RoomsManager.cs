using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Events;
using Events.FileManagment;
using Events.Room;
using Structures.Enums;
using Structures.Map.Room;
using UnityEngine;

namespace ScenesManager
{
	/// <summary>
	///  Class that provides room template data and objects
	/// </summary>
	public class RoomsManager : MonoBehaviour
	{
		#region Private Propertiesields
		[field: SerializeField]
		private SaveSystem SaveEvents { get; set; }

		[field: SerializeField]
		private RoomEvents IndividualRoomEvents { get; set; }
		#endregion
		#region Private Fields

		private int roomSize;
		private List<RoomGridCell[,]> roomCellList;
		private List<RoomCellLayout> allTemplates;
		private List<Dictionary<RoomGridCell, List<RoomZones>>> RoomTemplatesDictionary;

		#endregion
		#region Unity Callbacks

		private void Start() => this.Init();

		private void OnEnable()
		{
			EnableEvents();
		}

		private void OnDisable()
		{
			DisableEvents();
		}
		
		#endregion
		#region Public Methods

		/// <summary>
		/// methode responsible for finding all zones in room template
		/// </summary>
		public void FindAllZones()
		{
			
			Parallel.For(0, this.RoomTemplatesDictionary.Count, i =>
																{
																	this.FindZones(this.roomSize, this.roomCellList[i], this.RoomTemplatesDictionary[i]);
																});
		}

		/// <summary>
		/// methode responsible for finding individual room template zones
		/// </summary>
		public void FindZones(int lengthCount, RoomGridCell[,] currentRoomGridCell, Dictionary<RoomGridCell, List<RoomZones>> currentRoomZonesDictionary)
		{
			this.FindType(RoomGridCell.Wall, lengthCount, currentRoomGridCell, currentRoomZonesDictionary);
			this.FindType(RoomGridCell.Enemy, lengthCount, currentRoomGridCell, currentRoomZonesDictionary);
		}

		/// <summary>
		/// methode responsible for finding zones of given type
		/// </summary>
		public void FindType(RoomGridCell toFind, int lengthCount, RoomGridCell[,] currentRoomGridCell, Dictionary<RoomGridCell, List<RoomZones>> currentRoomZonesDictionary)
		{
			for (int x = 0; x < this.roomSize; x++)
			{
				this.IndividualRowAction(x, toFind, lengthCount, currentRoomGridCell, currentRoomZonesDictionary);
			}
		}
		
		#endregion
		#region Private Methods
		
		/// <summary>
		/// methode that enables template zones relaying methods
		/// </summary>
		private void EnableEvents()
		{
			this.IndividualRoomEvents.GetRoomGridTemplate += GetProcessedRoomTemplate;
			this.IndividualRoomEvents.GetRandomRoomGridTemplate += GetRandomProcessedRoomTemplate;
		}

		/// <summary>
		/// methode that disbales template zones relaying methods
		/// </summary>
		private void DisableEvents()
		{
			this.IndividualRoomEvents.GetRoomGridTemplate -= GetProcessedRoomTemplate;
			this.IndividualRoomEvents.GetRandomRoomGridTemplate -= GetRandomProcessedRoomTemplate;
		}

		/// <summary>
		/// methode that check individual room template rows 
		/// </summary>
		private void IndividualRowAction(int x, RoomGridCell toFind, int lengthCount, RoomGridCell[,] currentRoomGridCell, Dictionary<RoomGridCell, List<RoomZones>> currentRoomZonesDictionary)
		{
			for (int y = 0; y < this.roomSize; y++)
			{
				IndividualCellCheck(x, y, toFind, lengthCount, currentRoomGridCell, currentRoomZonesDictionary);
			}
		}
		
		/// <summary>
		/// methode that returns random processed room template  
		/// </summary>
		private Dictionary<RoomGridCell, List<RoomZones>> GetRandomProcessedRoomTemplate()
		{
			return GetProcessedRoomTemplate(Tools.GetRandomNumberFromRange(0, this.RoomTemplatesDictionary.Count-1));
		}
		
		/// <summary>
		/// method that returns processed room templates
		/// </summary>
		private Dictionary<RoomGridCell, List<RoomZones>> GetProcessedRoomTemplate(int i)
		{
			Dictionary<RoomGridCell, List<RoomZones>> dictonaryDeepCopy = new Dictionary<RoomGridCell, List<RoomZones>>();
			foreach (KeyValuePair<RoomGridCell, List<RoomZones>> individualZone in this.RoomTemplatesDictionary[i])
			{
				dictonaryDeepCopy.Add(individualZone.Key, new List<RoomZones>());
				dictonaryDeepCopy[individualZone.Key] = individualZone.Value.ConvertAll(element => new RoomZones(element));
			}

			return dictonaryDeepCopy;
		}

		/// <summary>
		/// methode that initializes class lists and loads file datas
		/// </summary>
		private void Init()
		{
			LoadData();
			InitDictionary();
			FindAllZones();
		}

		/// <summary>
		/// methode that initializes class dictonary
		/// </summary>
		private void InitDictionary()
		{
			foreach (RoomCellLayout x in this.allTemplates)
			{
				this.RoomTemplatesDictionary.Add(new Dictionary<RoomGridCell, List<RoomZones>>());
				this.RoomTemplatesDictionary[^1].Add(RoomGridCell.Wall, new List<RoomZones>());
				this.RoomTemplatesDictionary[^1].Add(RoomGridCell.Enemy, new List<RoomZones>());
			}
		}

		/// <summary>
		/// methode that loads files data form saving manager
		/// </summary>
		private void LoadData()
		{
			this.allTemplates = this.SaveEvents.GetAllRoomTemplates?.Invoke();
			this.RoomTemplatesDictionary = new List<Dictionary<RoomGridCell, List<RoomZones>>>();
			this.roomCellList = new List<RoomGridCell[,]>();
			foreach (RoomCellLayout x in this.allTemplates)
			{
				this.roomCellList.Add(x.forLoad);
			}

			this.roomSize = this.roomCellList[0].GetLength(0);
		}

		/// <summary>
		/// methode that check individual room template cell 
		/// </summary>
		private void IndividualCellCheck(int x, int y, RoomGridCell toFind, int lengthCount, RoomGridCell[,] currentRoomGridCell, Dictionary<RoomGridCell, List<RoomZones>> currentRoomZonesDictionary)
		{
			if (currentRoomGridCell[x, y] != toFind)
			{
				return;
			}

			if (this.NoZoneExists(x, y, toFind, currentRoomZonesDictionary))
			{
				return;
			}

			this.AddToOrCreateZone(x, y, toFind, currentRoomZonesDictionary);

		}

		/// <summary>
		/// methode that adds new zone 
		/// </summary>
		private bool NoZoneExists(int x, int y, RoomGridCell toFind, Dictionary<RoomGridCell, List<RoomZones>> currentRoomZonesDictionary)
		{
			if (currentRoomZonesDictionary[toFind].Count != 0)
			{
				return false;
			}

			currentRoomZonesDictionary[toFind].Add(new RoomZones(toFind, this.roomSize));
			currentRoomZonesDictionary[toFind][0].AddNewCord(new CordsXY(x, y));

			return true;
		}

		/// <summary>
		/// methode that adds cord to existing zone
		/// </summary>
		private void AddToOrCreateZone(int x, int y, RoomGridCell toFind, Dictionary<RoomGridCell, List<RoomZones>> currentRoomZonesDictionary)
		{
			List<int> nearbyZones = new List<int>();
			List<RoomZones> zonesInRoom = currentRoomZonesDictionary[toFind];
			bool foundExistingZone = LookForPointAlignedZones(x, y, ref nearbyZones, zonesInRoom);

			if (foundExistingZone)
			{
				PointAligntToZeneOperations(x, y, toFind, nearbyZones, zonesInRoom, currentRoomZonesDictionary);

				return;
			}

			this.CreateNewZone(x, y, toFind, currentRoomZonesDictionary);
		}

		/// <summary>
		/// methode that looks for existing zones aligned to zone 
		/// </summary>
		private bool LookForPointAlignedZones(int x, int y, ref List<int> nearbyZones, List<RoomZones> temp)
		{
			bool toReturn = false;

			for (int i = 0; i < temp.Count; i++)
			{
				if (temp[i].ZoneContainsCord(new CordsXY(x - 1, y)) || temp[i].ZoneContainsCord(new CordsXY(x, y - 1)) || temp[i].ZoneContainsCord(new CordsXY(x + 1, y)) ||
					temp[i].ZoneContainsCord(new CordsXY(x, y + 1)))
				{
					nearbyZones.Add(i);
					toReturn = true;
				}
			}

			return toReturn;
		}

		/// <summary>
		/// methode that checks if point is alligned to two zones 
		/// </summary>
		private void PointAligntToZeneOperations(int                                       x, int y, RoomGridCell toFind, List<int> nearbyZones, List<RoomZones> zonesInRoom,
												 Dictionary<RoomGridCell, List<RoomZones>> currentRoomZonesDictionary)
		{
			if (nearbyZones.Count == 1)
			{
				this.AddToExistingZone(x, y, nearbyZones, ref zonesInRoom);

				return;
			}

			this.AddAndJoinZones(x, y, toFind, nearbyZones, ref zonesInRoom, currentRoomZonesDictionary);
		}

		/// <summary>
		/// methode that adds point to existing zone
		/// </summary>
		private void AddToExistingZone(int x, int y, List<int> nearbyZones, ref List<RoomZones> temp) => temp[nearbyZones[0]].AddNewCord(new CordsXY(x, y));

		/// <summary>
		/// methode that joins existing zones
		/// </summary>
		private void AddAndJoinZones(int x, int y, RoomGridCell toFind, List<int> nearbyZones, ref List<RoomZones> zonesInRoom, Dictionary<RoomGridCell, List<RoomZones>> currentRoomZonesDictionary)
		{
			zonesInRoom[nearbyZones[0]].AddNewCord(new CordsXY(x, y));
			for (int i = nearbyZones.Count - 1; i > 0; i--)
			{
				zonesInRoom[nearbyZones[0]].JoinZoneCords(zonesInRoom[nearbyZones[i]]);
				currentRoomZonesDictionary[toFind].RemoveAt(nearbyZones[i]);
			}
		}
		
		/// <summary>
		/// methode that creates new zone
		/// </summary>
		private void CreateNewZone(int x, int y, RoomGridCell toFind, Dictionary<RoomGridCell, List<RoomZones>> currentRoomZonesDictionary)
		{
			currentRoomZonesDictionary[toFind].Add(new RoomZones(toFind, this.roomSize));
			currentRoomZonesDictionary[toFind][^1].AddNewCord(new CordsXY(x, y));
		}

		#endregion
	}
}
