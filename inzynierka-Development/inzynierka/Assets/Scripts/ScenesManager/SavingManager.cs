using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Data;
using Events;
using Events.FileManagment;
using Newtonsoft.Json;
using UnityEngine;

namespace ScenesManager
{
	/// <summary>
	///  Class that implements saving and loading logic
	/// </summary>
	public class SavingManager : MonoBehaviour
	{
		#region Private Properties
		
		[field: SerializeField]
		private SaveSystem SaveEvents { get; set; }

		[field: SerializeField]
		private RoomTemplateNamList TemplateNamList { get; set; }

		#endregion
		#region Private Variables

		private string destination;
		private List<string> templatesPaths;
		private string tempPath;

		private List<RoomCellLayout> roomTemplates;

		#endregion
		#region Unity Callbacks

		private void Awake() => Init();

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
		/// methode that loads room templates form data files and finds theirs paths locations
		/// </summary>
		private void Init()
		{
			this.InitVariables();
			this.LoadAllTemplates();
		}

		/// <summary>
		/// methode that initializes files paths locations
		/// </summary>
		private void InitVariables()
		{
			this.destination = Application.persistentDataPath;
			this.tempPath = Application.dataPath + "/Data";
			this.roomTemplates = new List<RoomCellLayout>();
		}

		/// <summary>
		/// methode that returns file paths locations
		/// </summary>
		private void GetTemplatesLocations()
		{
			this.templatesPaths = new List<string>();

			DirectoryInfo dir = new DirectoryInfo(this.tempPath);
			FileInfo[] info = dir.GetFiles("*.json");

			foreach (FileInfo f in info)
			{
				this.templatesPaths.Add(f.ToString());
			}
		}

		/// <summary>
		/// methode that loads all data files data to lists
		/// </summary>
		private void LoadAllTemplates() => Parallel.For(0, 
														this.TemplateNamList.roomNameTemplateParis.Count, 
														i =>
																 {
																	 this.roomTemplates.Add(this.LoadRoomCellLayout(this.TemplateNamList
																		 .roomNameTemplateParis[i].templateFileName));
																 });

		/// <summary>
		/// methode that loads room cell layout form data file
		/// </summary>
		private RoomCellLayout LoadRoomCellLayout(string fileName)
		{
			new JsonTextReader(new StringReader(File.ReadAllText(this.tempPath + "/" + fileName)));
			string json = File.ReadAllText(this.tempPath + "/" + fileName);
			RoomCellSavedData deserialized = JsonConvert.DeserializeObject<RoomCellSavedData>(json);

			return new RoomCellLayout(deserialized.enumsToInt);
		}

		/// <summary>
		/// methode that saves room cell layout to file
		/// </summary>
		private void SaveRoomCellLayout(RoomCellLayout cellLayout, string fileName)
		{
			string toSave = JsonUtility.ToJson(cellLayout);
			File.WriteAllText(this.tempPath + "/" + fileName, toSave);
		}

		/// <summary>
		/// methode that returns all loaded files templates
		/// </summary>
		private List<RoomCellLayout> GetAllTemplates() => this.roomTemplates;
		
		/// <summary>
		/// methode that enables saving and loading events
		/// </summary>
		private void EnableEvents()
		{
			this.SaveEvents.SaveRoomLayout += SaveRoomCellLayout;
			this.SaveEvents.LoadRoomLayout += LoadRoomCellLayout;
			this.SaveEvents.GetAllRoomTemplates += GetAllTemplates;
			
		}

		/// <summary>
		/// methode that disbales saving and loading events
		/// </summary>
		private void DisableEvents()
		{
			this.SaveEvents.SaveRoomLayout -= SaveRoomCellLayout;
			this.SaveEvents.LoadRoomLayout -= LoadRoomCellLayout;
			this.SaveEvents.GetAllRoomTemplates -= GetAllTemplates;
		}

		#endregion
		#region Nested class
		public class RoomCellSavedData
		{
			public CellRowWrapper[] enumsToInt;
		}

		#endregion

	}
}
