using System.Collections.Generic;

namespace Structures.Map.Room
{
	/// <summary>
	///  Class that implements enemy room zone logic and managment
	/// </summary>
	public class RoomEnemyZone
	{
		#region Public Variables

		public List<RoomEnemyZoneCell> cells;
		public int currentlyOccupied;
		public CordsXY lastSpawnLocation;		

		#endregion
		#region Constructors

		/// <summary>
		/// constructor responsible for initialization of enemy zone values
		/// </summary>
		public RoomEnemyZone()
		{
			this.cells = new List<RoomEnemyZoneCell>();
			this.currentlyOccupied = 0;
			this.lastSpawnLocation = new CordsXY(-1, -1);
		}

		#endregion
		#region Public Methods

		/// <summary>
		/// methode that checks if point can be added to enemy zone
		/// </summary>
		public bool TryAddEnemy(int dimensionX, int dimensionY)
		{
			int freeCellsLeft = this.cells.Count - this.currentlyOccupied;

			return freeCellsLeft >= dimensionX * dimensionY && this.AddedNewEnemy(dimensionX, dimensionY);
		}

		#endregion
		#region Private Methods

		/// <summary>
		/// methode that adds new enemy cell to enemy zone 
		/// </summary>
		private bool AddedNewEnemy(int dimensionX, int dimensionY)
		{
			List<CordsXY> potentialGenerationCords = this.FindPotentialGenerationCords(dimensionX, dimensionY);

			if (potentialGenerationCords.Count != 0)
			{
				this.GenerateElement(potentialGenerationCords, dimensionX, dimensionY);

				return true;
			}

			return false;
		}

		/// <summary>
		/// methode that returns list of potential cords where enemy party can be spawned
		/// </summary>
		private List<CordsXY> FindPotentialGenerationCords(int dimensionX, int dimensionY) => dimensionX == 1 && dimensionY == 1
			? this.BasePartySize(dimensionX, dimensionY)
			: this.FlexiblePartySize(dimensionX, dimensionY);

		/// <summary>
		/// methode that returns list of potential generation cords for basic party size
		/// </summary>
		private List<CordsXY> BasePartySize(int dimensionX, int dimensionY)
		{
			List<CordsXY> potentialGenerationCords = new List<CordsXY>();

			foreach (RoomEnemyZoneCell currentCord in this.cells)
			{
				if (currentCord.occupyed)
				{
					continue;
				}

				potentialGenerationCords.Add(currentCord.position);
			}

			return potentialGenerationCords.Count == 0 ? new List<CordsXY>() : potentialGenerationCords;
		}
		
		/// <summary>
		/// methode that returns list of potential generation cords for custom party size
		/// </summary>
		private List<CordsXY> FlexiblePartySize(int dimensionX, int dimensionY)
		{
			List<CordsXY> potentialGenerationCords = new List<CordsXY>();

			foreach (RoomEnemyZoneCell currentCord in this.cells)
			{
				if (this.CheckCordCorrectness(currentCord.position, dimensionX, dimensionY) == false)
				{
					continue;
				}

				potentialGenerationCords.Add(currentCord.position);
			}

			return potentialGenerationCords.Count == 0 ? new List<CordsXY>() : potentialGenerationCords;
		}

		/// <summary>
		/// methode that check if selected generation cord is correct 
		/// </summary>
		private bool CheckCordCorrectness(CordsXY cordToCheck, int dimensionX, int dimensionY)
		{
			List<CordsXY> zoneMustContainCords = new List<CordsXY>();

			for (int x = 0; x < dimensionX; x++)
			{
				for (int y = 0; y < dimensionY; y++)
				{
					zoneMustContainCords.Add(new CordsXY(cordToCheck.x + x, cordToCheck.y + y));
				}
			}

			return this.ZoneContainsAllCords(zoneMustContainCords);
		}

		/// <summary>
		/// methode that check if zone contains all cords required to span enemy party
		/// </summary>
		private bool ZoneContainsAllCords(List<CordsXY> zoneMustContainCords)
		{
			foreach (CordsXY cordToCheck in zoneMustContainCords)
			{
				if (this.IsCordInZone(cordToCheck) == false)
				{
					return false;
				}

			}

			return true;
		}

		/// <summary>
		/// methode that check if selected cord is enemy zone cord
		/// </summary>
		private bool IsCordInZone(CordsXY cordToCheck)
		{
			foreach (RoomEnemyZoneCell zoneCord in this.cells)
			{
				if (zoneCord.position.x != cordToCheck.x || zoneCord.position.y != cordToCheck.y)
				{
					continue;
				}

				if (zoneCord.occupyed)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// methode responsible for selecting generation cord from all potential party generation cords  
		/// </summary>
		private void GenerateElement(List<CordsXY> potentialGenerationCords, int dimensionX, int dimensionY)
		{
			int randomNumber = Tools.GetRandomNumberFromRange(0, potentialGenerationCords.Count - 1);
			CordsXY randomCord = potentialGenerationCords[randomNumber];
			this.lastSpawnLocation = randomCord;
			this.currentlyOccupied += dimensionX * dimensionY;

			for (int x = 0; x < dimensionX; x++)
			{
				for (int y = 0; y < dimensionY; y++)
				{
					this.SetCordOccupation(new CordsXY(randomCord.x + x, randomCord.y + y));
				}
			}
		}

		/// <summary>
		/// methode that sets information that selected cord if already occupied by enemy 
		/// </summary>
		private void SetCordOccupation(CordsXY cordToSet, bool newState = true)
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				if (this.cells[i].position.x == cordToSet.x && this.cells[i].position.y == cordToSet.y)
				{
					this.cells[i].occupyed = true;

					break;
				}
			}

		}

		#endregion
	}
}
