using Structures.Map.Room;

namespace Structures.Map
{
	/// <summary>
	///  Class that implements floor initialization logic
	/// </summary>
	public class FloorInicjalizator
	{
		#region Public Methods

		/// <summary>
		/// methode responsible for initialization of floor layout cells 
		/// </summary>
		public void InitDimensions(int rowsAmount, int columnsAmount, ref FloorDimensions floorDimensions, ref Row[] rows)
		{
			floorDimensions = new FloorDimensions(rowsAmount, columnsAmount);
			rows = new Row[rowsAmount];

			for (int i = 0; i < floorDimensions.rowsAmount; i++)
			{
				rows[i] = new Row(floorDimensions.columnsAmount);
			}
		}

		/// <summary>
		/// methode responsible for initialization of middle cord value and grid count value
		/// </summary>
		public void InitCordsData(FloorDimensions floorDimensions, ref FloorEssentialRooms floorEssentialRooms, ref CordsXY midle, ref int gridCount)
		{
			floorEssentialRooms = new FloorEssentialRooms();
			floorEssentialRooms.InitAssigned();

			midle = new CordsXY(floorDimensions.rowsAmount / 2, floorDimensions.columnsAmount / 2);
			gridCount = floorDimensions.columnsAmount * floorDimensions.rowsAmount;
		}

		/// <summary>
		/// method responsible for setting generation randomness parameters 
		/// </summary>
		public void InitRandomnessVariables(ref MapGenerationParameters generationParameters, ref FloorRoomsNodes roomsNodes)
		{
			generationParameters = new MapGenerationParameters();
			generationParameters.SetDefault();
			roomsNodes = new FloorRoomsNodes();
			roomsNodes.SetEmpty();
		}

		/// <summary>
		/// methode responsible for setting points around middle
		/// </summary>
		public void InitPointsAroundMiddle(ref int pointsAroundMidleCount, ref CordsXY[] allCordsAroundMidle, CordsXY midle)
		{
			pointsAroundMidleCount = 0;
			allCordsAroundMidle = new CordsXY[8];
			int i = 0;

			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					if (x == 1 && y == 1)
					{
						continue;
					}

					allCordsAroundMidle[i] = new CordsXY(midle.x - 1 + x, midle.y - 1 + y);
					i++;
				}
			}
		}

		#endregion
	}
}
