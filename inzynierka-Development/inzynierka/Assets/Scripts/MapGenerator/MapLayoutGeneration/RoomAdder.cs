using System.Collections.Generic;
using Structures.Enums;
using Structures.Map;
using Structures.Map.Room;

namespace MapGenerator.MapLayoutGeneration
{
	/// <summary>
	///  Class for inheritance purposes - it serves as an interface that provides universally needed variables and methods
	/// </summary>
	public class RoomAdder
	{
		#region Delegates

		public delegate void Delegator(ref List<CordsXY> nodeToAdd, List<CordsXY> possibleMoves);

		public delegate bool InMapBoundarys(int baseValue);

		#endregion
		#region Protected Variables

		protected int pointsAroundMiddleCount;
		protected CordsXY[] allCordsAroundMiddle;
		protected CordsXY middle;
		protected Row[] rows;
		protected MapGenerationParameters generationParameters;
		protected FloorDimensions floorDimensions;

		#endregion
		#region Constructors

		/// <summary>
		/// constructor setting generation parameters
		/// </summary>
		/// <param name="allCordsAroundMiddle"> allowed filed cells next start room</param>
		/// <param name="pointsAroundMiddleCount">allowed filed cells around start room</param>
		/// <param name="generationParameters"> generation parameters</param>
		/// <param name="middle"> middle points cords</param>
		/// <param name="rows"> amount of map rows</param>
		/// <param name="floorDimensions">floor dimensions</param>
		public RoomAdder(CordsXY[] allCordsAroundMiddle, int pointsAroundMiddleCount, MapGenerationParameters generationParameters, CordsXY middle, Row[] rows, FloorDimensions floorDimensions)
		{
			this.allCordsAroundMiddle = allCordsAroundMiddle;
			this.pointsAroundMiddleCount = pointsAroundMiddleCount;
			this.generationParameters = generationParameters;
			this.middle = middle;
			this.rows = rows;
			this.floorDimensions = floorDimensions;
		}

		#endregion
		#region Protected Methods

		/// <summary>
		/// methode that check if value is in lower map boundarys
		/// </summary>
		/// <param name="baseValue"> base value</param>
		/// <returns>true if value is in lower map boundarys</returns>
		protected bool IsInLowerMapBoundarys(int baseValue) => baseValue - 1 >= 0;

		/// <summary>
		/// methode that check if value is in upper map boundarys
		/// </summary>
		/// <param name="baseValue"> base value</param>
		/// <returns>true if value is in upper map boundarys</returns>
		protected bool IsInUpperMapBoundarys(int baseValue) => baseValue + 1 < this.floorDimensions.rowsAmount;

		/// <summary>
		/// methode that adds point to node
		/// </summary>
		/// <param name="cordX">x coordinate</param>
		/// <param name="cordY">y coordinate</param>
		/// <param name="individualNode">room nodes</param>
		/// <param name="allNodes"> all room nodes</param>
		/// <param name="rows">map rows</param>
		protected void AddPointToNode(int cordX, int cordY, ref List<CordsXY> individualNode, ref List<CordsXY> allNodes, ref Row[] rows)
		{
			if (AroundMiddleCondition(cordX, cordY) == false)
			{
				return;
			}

			AddPoint(cordX, cordY, ref individualNode, ref allNodes, ref rows);
		}

		/// <summary>
		/// methode that adds point to individual node
		/// </summary>
		/// <param name="cordX">x coordinate</param>
		/// <param name="cordY">y coordinate</param>
		/// <param name="individualNode">room nodes</param>
		/// <param name="allNodes"> all room nodes</param>
		/// <param name="rows">map rows</param>
		protected void AddPoint(int cordX, int cordY, ref List<CordsXY> individualNode, ref List<CordsXY> allNodes, ref Row[] rows)
		{
			individualNode.Add(new CordsXY(cordX, cordY));
			rows[cordX].SetSelectedColumnType(cordY, RoomType.NormalRoom);
			allNodes.Add(new CordsXY(cordX, cordY));
		}

		/// <summary>
		/// methode that check map filament around middle point
		/// </summary>
		/// <param name="cordX"></param>
		/// <param name="cordY"></param>
		/// <returns>returns true if can add next point around middle</returns>
		protected bool AroundMiddleCondition(int cordX, int cordY)
		{
			for (int i = 0; i < 7; i++)
			{
				if (IsPointAroundMiddle(this.allCordsAroundMiddle[i], cordX, cordY))
				{
					return CheckIfCanAddNextPoint();
				}
			}

			return true;
		}

		/// <summary>
		/// methode that point is next to midle point
		/// </summary>
		/// <param name="cordAroundMiddle"></param>
		/// <param name="xToCheck"></param>
		/// <param name="yToCheck"></param>
		/// <returns>true if point is next to middle point</returns>
		protected bool IsPointAroundMiddle(CordsXY cordAroundMiddle, int xToCheck, int yToCheck) => cordAroundMiddle.x == xToCheck && cordAroundMiddle.y == yToCheck;

		/// <summary>
		/// methode that checks if generator can add next point
		/// </summary>
		/// <returns>true if generator can add next point</returns>
		protected bool CheckIfCanAddNextPoint()
		{
			if (this.pointsAroundMiddleCount < this.generationParameters.maxRoomsAroundCenter)
			{
				this.pointsAroundMiddleCount++;

				return true;
			}

			return false;
		}

		/// <summary>
		/// methode that count empty cells around point
		/// </summary>
		/// <param name="cordToCheck"> point coordinates</param>
		/// <returns> number of empty point around middle</returns>
		protected int EmptyNeighbourCount(CordsXY cordToCheck)
		{
			CordsXY[] possibleMoves = new CordsXY[4];
			int potentialMovesCount = 0;
			FindPotentialCordMoves(ref possibleMoves, ref potentialMovesCount, cordToCheck);

			return potentialMovesCount;
		}

		/// <summary>
		/// methode that finds all potential new cords around given point
		/// </summary>
		/// <param name="possibleMoves"> array to fill with potential cords</param>
		/// <param name="potentialMovesCount">potential cords size</param>
		/// <param name="generationStart"> point to check potential moves for</param>
		protected void FindPotentialCordMoves(ref CordsXY[] possibleMoves, ref int potentialMovesCount, CordsXY generationStart)
		{
			CheckPotentialCord(generationStart, ref possibleMoves, ref potentialMovesCount, new CordsXY(-1, 0), IsInLowerMapBoundarys, generationStart.x);
			CheckPotentialCord(generationStart, ref possibleMoves, ref potentialMovesCount, new CordsXY(0, -1), IsInLowerMapBoundarys, generationStart.y);
			CheckPotentialCord(generationStart, ref possibleMoves, ref potentialMovesCount, new CordsXY(1, 0), IsInUpperMapBoundarys, generationStart.x);
			CheckPotentialCord(generationStart, ref possibleMoves, ref potentialMovesCount, new CordsXY(0, 1), IsInUpperMapBoundarys, generationStart.y);
		}

		/// <summary>
		/// methode that check if can add cord in given direction
		/// </summary>
		/// <param name="currentCord"> point to check potential moves for</param>
		/// <param name="possibleMoves"> array to fill with potential cords</param>
		/// <param name="potentialMovesCount">potential cords size</param>
		/// <param name="offset"> point to check direction</param>
		/// <param name="condition"> safety condition</param>
		/// <param name="conditionValue">cord direction to check</param>
		protected void CheckPotentialCord(CordsXY currentCord, ref CordsXY[] possibleMoves, ref int potentialMovesCount, CordsXY offset, InMapBoundarys condition, int conditionValue)
		{
			if (condition(conditionValue) && IsCordEmpty(currentCord, offset))
			{
				possibleMoves[potentialMovesCount] = new CordsXY(currentCord.x + offset.x, currentCord.y + offset.y);
				potentialMovesCount++;
			}
		}

		/// <summary>
		/// methode that check if given cord doesn't have assigned room
		/// </summary>
		/// <param name="currentCord"> point to check potential moves for</param>
		/// <param name="offset"> point to check direction</param>
		/// <returns>true if cord is empty cell</returns>
		protected bool IsCordEmpty(CordsXY currentCord, CordsXY offset) => this.rows[currentCord.x + offset.x].GetRowColumns()[currentCord.y + offset.y] == RoomType.Nothing;

		/// <summary>
		/// methode that removes cord form cords list
		/// </summary>
		/// <param name="cordsList">list of cords</param>
		/// <param name="cord">cord to remove</param>
		protected void RemoveCordFromList(ref List<CordsXY> cordsList, CordsXY cord)
		{
			for (int i = cordsList.Count - 1; i >= 0; i--)
			{
				if (cordsList[i].x == cord.x && cordsList[i].y == cord.y)
				{
					cordsList.RemoveAt(i);
				}
			}
		}

		#endregion
	}
}
