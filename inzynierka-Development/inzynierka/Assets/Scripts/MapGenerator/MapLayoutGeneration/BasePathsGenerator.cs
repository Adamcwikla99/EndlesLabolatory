using System;
using System.Collections.Generic;
using System.Linq;
using Structures.Enums;
using Structures.Map;
using Structures.Map.Room;
using UnityEngine;

namespace MapGenerator.MapLayoutGeneration
{
	/// <summary>
	///  Class responsible for generating base floor room paths
	/// </summary>
	public class BasePathsGenerator : RoomAdder
	{
		#region Delegates

		public delegate bool RemoveCondition(int count);

		#endregion
		#region Enums

		private enum GenerationFaze { Initial = 0, Middle = 1, End = 2 }

		#endregion
		#region Constants

		private const int GENERATION_SAFETY_LIMITER = 5000;
		private const int LIMIT_NODES_DIRECTIONS_CHANCE = 25;
		private const int MIN_NODES_TO_LIMIT_COUNT = 1;
		private const int MAX_NODES_TO_LIMIT_COUNT = 3;
		private const int MAX_POSIBLE_MOVES_COUNT = 3;
		private const int LAST_DIRECTION_NODE_NUMERICAL_VALUE = 3;

		#endregion
		#region Private Fields

		private FloorRoomsNodes roomsNodes;
		private int gridCount;
		private float mapFilment;

		#endregion
		#region Constructors

		/// <summary>
		/// constructor setting generation parameters
		/// </summary>
		/// <param name="rows">map</param>
		/// <param name="middle"> middle cord</param>
		/// <param name="allCordsAroundMiddle"> list of cords around middle</param>
		/// <param name="floorDimensions">map dimensions</param>
		/// <param name="generationParameters"> generation parameters</param>
		/// <param name="roomsNodes">spawned rooms</param>
		public BasePathsGenerator(ref Row[]           rows, CordsXY middle, CordsXY[] allCordsAroundMiddle, FloorDimensions floorDimensions, MapGenerationParameters generationParameters,
								  ref FloorRoomsNodes roomsNodes) : base(allCordsAroundMiddle, 0, generationParameters, middle, rows, floorDimensions)
		{
			this.roomsNodes = roomsNodes;
			this.mapFilment = 0;
			this.gridCount = floorDimensions.columnsAmount * floorDimensions.rowsAmount;
		}

		#endregion
		#region Public Methods

		/// <summary>
		/// methode responsible for generating paths consistent of enemy rooms
		/// </summary>
		public void GenerateBasePaths()
		{
			float[] boundarys = this.generationParameters.generationStagesBoundary;
			int[] roomCountBoundarys = new int[3]
			{
				(int)(this.gridCount * boundarys[(int)GenerationFaze.Initial]), (int)(this.gridCount * boundarys[(int)GenerationFaze.Middle]), (int)(this.gridCount * boundarys[(int)GenerationFaze.End])
			};

			this.roomsNodes.AllCords.Add(this.middle);
			List<int> list = LimitNodesDirections();
			GenerationLoop(list, roomCountBoundarys);
		}

		#endregion
		#region Private Methods

		private bool DelegateConditionZero(int count) => count == 0;
		private bool DelegateConditionOne(int  count) => count == 0 || count == 1;

		/// <summary>
		/// methode that limit direction that generation can start form (start room is generation starting point)
		/// </summary>
		/// <returns></returns>
		private List<int> LimitNodesDirections() => Tools.GetRandomNumberFromRange(0, 100) > LIMIT_NODES_DIRECTIONS_CHANCE ? SelectNodesToLimit() : new List<int>();

		/// <summary>
		/// methode that defines what directions should be skipped
		/// </summary>
		/// <returns></returns>
		private List<int> SelectNodesToLimit()
		{
			int limitCount = Tools.GetRandomNumberFromRange(MIN_NODES_TO_LIMIT_COUNT, MAX_NODES_TO_LIMIT_COUNT);
			List<int> limitList = new List<int>();

			return ChoseNodeToLimit(limitList, limitCount);
		}

		/// <summary>
		///  methode that chooses nodes to limit form all nodes
		/// </summary>
		/// <returns></returns>
		private List<int> ChoseNodeToLimit(List<int> limitList, int limitCount)
		{
			for (int i = 0; i < limitCount; i++)
			{
				IndividualNodeToLimitSelection(ref limitList, ref i);
			}

			return limitList;
		}

		/// <summary>
		/// methode that removes individual node direction form generation process
		/// </summary>
		private void IndividualNodeToLimitSelection(ref List<int> limitList, ref int i)
		{
			int dirToLimit = Tools.GetRandomNumberFromRange(0, LAST_DIRECTION_NODE_NUMERICAL_VALUE);

			if (limitList.Contains(dirToLimit))
			{
				i--;

				return;
			}

			limitList.Add(dirToLimit);
		}

		/// <summary>
		/// method responsible for generating base paths room by room till all conditions are met
		/// </summary>
		private void GenerationLoop(List<int> list, int[] roomCountBoundarys)
		{
			int potentialBreaker = 0;

			while (ContinueGeneration())
			{
				if (GenerationIteration(list, roomCountBoundarys, ref potentialBreaker) == false)
				{
					break;
				}
			}
		}

		/// <summary>
		/// methode that checks if generation should be continued
		/// </summary>
		/// <returns></returns>
		private bool ContinueGeneration()
		{
			this.mapFilment = this.roomsNodes.AllCords.Count / this.gridCount;

			return this.generationParameters.mapFillPercentage * this.gridCount > this.roomsNodes.AllCords.Count;
		}

		/// <summary>
		/// methode responsible for single generation iteration
		/// </summary>
		/// <returns></returns>
		private bool GenerationIteration(List<int> list, int[] roomCountBoundarys, ref int potentialBreaker)
		{
			Delegator del = null;
			// make a list of generation methods?
			del = SelectGenerationMethode(GetGenerationStage(roomCountBoundarys), this.InitialFindAndAddCord, this.MiddleFindAndAddCord, this.EndFindAndAddCord);
			DelegateCaller(del, list);

			return this.SafetyLoopBreak(ref potentialBreaker, this.gridCount) != true;
		}

		/// <summary>
		/// methode responsible for setting generation faze
		/// </summary>
		/// <returns></returns>
		private GenerationFaze GetGenerationStage(int[] boundarys) => this.roomsNodes.AllCords.Count switch
		{
			{ } when this.roomsNodes.AllCords.Count < boundarys[0] => GenerationFaze.Initial,
			{ } when this.roomsNodes.AllCords.Count < boundarys[1] => GenerationFaze.Middle,
			_ => GenerationFaze.End
		};

		/// <summary>
		/// methode that returns delegator containing generation methode base on generation faze
		/// </summary>
		/// <returns></returns>
		private Delegator SelectGenerationMethode(GenerationFaze faze, Delegator initialFindAndAddCord, Delegator middleFindAndAddCord, Delegator endFindAndAddCord) => faze switch
		{
			GenerationFaze.Initial => initialFindAndAddCord,
			GenerationFaze.Middle => middleFindAndAddCord,
			GenerationFaze.End => endFindAndAddCord,
			_ => throw new NotImplementedException()
		};

		/// <summary>
		/// methode responsible for finding potential enemy room cords in initial generation faze
		/// </summary>
		private void InitialFindAndAddCord(ref List<CordsXY> directionList, List<CordsXY> possibleMoves)
		{
			if (CheckPossibleMovesPresence(ref directionList, ref possibleMoves) == false)
			{
				return;
			}

			CordsXY temp = SelectCordToAdd(possibleMoves);
			TryToAddCord(temp, ref directionList);
		}

		/// <summary>
		/// methode responsible for finding potential enemy room cords in middle generation faze
		/// </summary>
		private void MiddleFindAndAddCord(ref List<CordsXY> directionList, List<CordsXY> possibleMoves)
		{
			CordsXY temp;
			if (TrySelectNewCord(out temp, ref directionList, possibleMoves, MAX_POSIBLE_MOVES_COUNT - 1) == false)
			{
				return;
			}

			TryToAddCord(temp, ref directionList);
		}

		/// <summary>
		/// methode responsible for finding potential enemy room cords in end generation faze
		/// </summary>

		private void EndFindAndAddCord(ref List<CordsXY> directionList, List<CordsXY> possibleMoves)
		{
			CordsXY temp;
			if (TrySelectNewCord(out temp, ref directionList, possibleMoves, MAX_POSIBLE_MOVES_COUNT) == false)
			{
				return;
			}

			TryToAddCord(temp, ref directionList);
		}

		/// <summary>
		/// methode that check potential cord
		/// </summary>
		/// <returns></returns>
		private bool TrySelectNewCord(out CordsXY temp, ref List<CordsXY> directionList, List<CordsXY> possibleMoves, int potentialNehbiours)
		{
			temp = new CordsXY(-1, -1);

			return CheckPossibleMovesPresence(ref directionList, ref possibleMoves) && CheckGenerationCondition(out temp, possibleMoves, potentialNehbiours);
		}

		/// <summary>
		/// methode that check if selected cord has any possible generation cord
		/// </summary>
		/// <returns></returns>
		private bool CheckPossibleMovesPresence(ref List<CordsXY> possibleMoves, ref List<CordsXY> directionList)
		{
			RemoveCondition conditionToCheck = DelegateConditionZero;
			FilterMovesCondition(ref possibleMoves, conditionToCheck);

			return possibleMoves.Count != 0;
		}

		/// <summary>
		/// methode that removes possible moves not meeting generation conditiones
		/// </summary>
		private void FilterMovesCondition(ref List<CordsXY> possibleMoves, RemoveCondition conditionToCheck)
		{
			for (int i = possibleMoves.Count - 1; i >= 0; i--)
			{
				IndividualCordCheck(ref possibleMoves, conditionToCheck, i);
			}
		}

		/// <summary>
		/// methode that checks if individual cord is valid generation cord
		/// </summary>
		private void IndividualCordCheck(ref List<CordsXY> possibleMoves, RemoveCondition conditionToCheck, int i)
		{
			int potentialMovesCount = 0;
			CordsXY[] possibleCordsMoves = new CordsXY[4];

			FindPotentialCordMoves(ref possibleCordsMoves, ref potentialMovesCount, possibleMoves[i]);
			if (conditionToCheck(potentialMovesCount))
			{
				possibleMoves.RemoveAt(i);
			}
		}

		/// <summary>
		/// methode that check if generation conditions are met
		/// </summary>
		/// <returns></returns>
		private bool CheckGenerationCondition(out CordsXY temp, List<CordsXY> possibleMoves, int condition)
		{
			temp = SelectCordToAdd(possibleMoves);
			int tempInt = EmptyNeighbourCount(temp);

			return tempInt >= condition;
		}

		/// <summary>
		/// methode that select new generation cord form potential generation cords list
		/// </summary>
		/// <returns></returns>
		private CordsXY SelectCordToAdd(List<CordsXY> possibleMoves)
		{
			CordsXY[] possibleMovesAroundCord = new CordsXY[4];
			int potentialMovesCount = 0;

			int randCord = Tools.GetRandomNumberFromRange(0, possibleMoves.Count - 1);
			FindPotentialCordMoves(ref possibleMovesAroundCord, ref potentialMovesCount, possibleMoves[randCord]);
			int randMove = Tools.GetRandomNumberFromRange(0, potentialMovesCount - 1);

			return possibleMovesAroundCord[randMove];
		}

		/// <summary>
		/// methode that stops generation if it takes to loong
		/// </summary>
		private bool SafetyLoopBreak(ref int potentialBreaker, int gridCount)
		{
			potentialBreaker++;
			if (potentialBreaker > gridCount + GENERATION_SAFETY_LIMITER)
			{
				Debug.Log("Safety break");

				return true;
			}

			return false;
		}

		/// <summary>
		/// methode that calls methode stored in delegator
		/// </summary>
		/// <param name="delegated"></param>
		/// <param name="directionLimiter"></param>
		private void DelegateCaller(Delegator delegated, List<int> directionLimiter)
		{
			// change magic numbers to direction enum?
			List<int> directionsList = new List<int>
			{
				0,
				1,
				2,
				3
			};

			if (directionLimiter.Count != 0)
			{
				directionsList = directionsList.Except(directionLimiter).ToList();
			}

			int randomNumber = Tools.GetRandomNumberFromRange(0, directionsList.Count - 1);
			TryAddToSelectedNode(randomNumber, delegated);
		}
		
		/// <summary>
		/// methode that trys to add selected node to generated paths 
		/// </summary>
		private void TryAddToSelectedNode(int randomNumber, Delegator delegated)
		{
			switch (randomNumber)
			{
				case 0 :
					if (AddFirstNodeCorde(this.middle.x + 1, this.middle.y, ref this.roomsNodes.southNode, ref this.roomsNodes.AllCords, ref this.rows) == false)
					{
						AddNodeCorde(ref this.roomsNodes.southNode, delegated);
					}

					break;
				case 1 :
					if (AddFirstNodeCorde(this.middle.x, this.middle.y - 1, ref this.roomsNodes.westNode, ref this.roomsNodes.AllCords, ref this.rows) == false)
					{
						AddNodeCorde(ref this.roomsNodes.westNode, delegated);
					}

					break;
				case 2 :
					if (AddFirstNodeCorde(this.middle.x - 1, this.middle.y, ref this.roomsNodes.northNode, ref this.roomsNodes.AllCords, ref this.rows) == false)
					{
						AddNodeCorde(ref this.roomsNodes.northNode, delegated);
					}

					break;
				case 3 :
					if (AddFirstNodeCorde(this.middle.x, this.middle.y + 1, ref this.roomsNodes.eastNode, ref this.roomsNodes.AllCords, ref this.rows) == false)
					{
						AddNodeCorde(ref this.roomsNodes.eastNode, delegated);
					}

					break;
			}
		}

		/// <summary>
		/// methode that adds first cord to given node direction 
		/// </summary>
		private bool AddFirstNodeCorde(int cordX, int cordY, ref List<CordsXY> individualNode, ref List<CordsXY> allNodes, ref Row[] rows)
		{
			if (individualNode.Count == 0)
			{
				AddPointToNode(cordX, cordY, ref individualNode, ref allNodes, ref rows);

				return true;
			}

			return false;
		}

		/// <summary>
		/// methode that adds cord to given node direction 
		/// </summary>
		private void AddNodeCorde(ref List<CordsXY> individualNode, Delegator delegatedMethode)
		{
			List<CordsXY> possibleMoves = this.GetAllGoodMoves(individualNode);

			if (possibleMoves == null || possibleMoves.Count == 0)
			{
				return;
			}

			delegatedMethode(ref individualNode, possibleMoves);
		}

		/// <summary>
		/// methode that tries to add cord to direction node
		/// </summary>
		private void TryToAddCord(CordsXY selectedCord, ref List<CordsXY> directionList)
		{
			if (AroundMiddleCondition(selectedCord.x, selectedCord.y) == false)
			{
				return;
			}

			directionList.Add(selectedCord);
			this.roomsNodes.AllCords.Add(selectedCord);
			this.rows[selectedCord.x].SetSelectedColumnType(selectedCord.y, RoomType.NormalRoom);
		}

		/// <summary>
		/// methode that returns all potential cords that meets generation condition
		/// </summary>
		/// <returns></returns>
		private List<CordsXY> GetAllGoodMoves(List<CordsXY> direction)
		{
			List<CordsXY> possibleMoves = new List<CordsXY>();
			FindAllPotentialMoves(direction, ref possibleMoves);

			if (possibleMoves.Count == 0)
			{
				return null;
			}

			IEnumerable<CordsXY> distinctList = possibleMoves.Distinct(new RoomPositionComparer());
			possibleMoves = distinctList.ToList();

			return possibleMoves;

		}

		/// <summary>
		/// methode that looks for all points that mee generation conditiones
		/// </summary>
		private void FindAllPotentialMoves(List<CordsXY> node, ref List<CordsXY> possibleMoves)
		{
			foreach (CordsXY cord in node)
			{
				PotentialMoveCheck(ref possibleMoves, cord);
			}
		}

		/// <summary>
		/// methode that check if given cords are correct and adds them to list
		/// </summary>
		private void PotentialMoveCheck(ref List<CordsXY> possibleMoves, CordsXY cord)
		{
			CordsXY[] posibleMoves = new CordsXY[4];
			int potentialMovesCount = 0;
			FindPotentialCordMoves(ref posibleMoves, ref potentialMovesCount, cord);

			if (potentialMovesCount == 0)
			{
				return;
			}

			possibleMoves.Add(new CordsXY(cord.x, cord.y));
		}

		#endregion
	}
}
