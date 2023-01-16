namespace Structures.Map
{
	/// <summary>
	///  Data wrapper for map generation parameters
	/// </summary>
	public struct MapGenerationParameters
	{
		#region Public Variables

		public float[] generationStagesBoundary;

		public int goodMoveChance;
		public float mapFillPercentage;
		public float maxNodePercentage;
		public int maxRoomsAroundCenter;
		public float minimalBossDistanceThreshold;
		public float minimalShopDistanceThreshold;	

		#endregion
		#region Constructors

		/// <summary>
		/// constructor that allows for manual setting of all map generation parameters 
		/// </summary>
		public MapGenerationParameters(int goodMoveChance,       float   minimalBossDistanceThreshold, float minimalShopDistanceThreshold, float maxNodePercentage, float mapFillPercentage,
									   int maxRoomsAroundCenter, float[] boundarys)
		{
			this.goodMoveChance = goodMoveChance;
			this.minimalBossDistanceThreshold = minimalBossDistanceThreshold;
			this.minimalShopDistanceThreshold = minimalShopDistanceThreshold;
			this.maxNodePercentage = maxNodePercentage;
			this.mapFillPercentage = mapFillPercentage;
			this.maxRoomsAroundCenter = maxRoomsAroundCenter;
			this.generationStagesBoundary = boundarys;
		}

		#endregion
		#region Public Methods

		/// <summary>
		/// method that allows for manual setting of all map generation parameters 
		/// </summary>
		public void AdjustGenerationSettings(int goodMove, float boosDistance, float shopDistance, float maxNodeProcantage, float mapFillProcantage, int maxRoomsAroundCenter, float[] boundarys)
		{
			this.goodMoveChance = goodMove;
			this.minimalBossDistanceThreshold = boosDistance;
			this.minimalShopDistanceThreshold = shopDistance;
			this.maxNodePercentage = maxNodeProcantage;
			this.mapFillPercentage = mapFillProcantage;
			this.maxRoomsAroundCenter = maxRoomsAroundCenter;
			this.generationStagesBoundary = boundarys;
		}

		/// <summary>
		/// method that sets default map generation parameters 
		/// </summary>
		public void SetDefault()
		{
			this.goodMoveChance = 75;
			this.minimalBossDistanceThreshold = 0.4f;
			this.minimalShopDistanceThreshold = 0.2f;
			this.mapFillPercentage = 0.30f;
			this.maxRoomsAroundCenter = 4;
			this.generationStagesBoundary = new float[3]
			{
				0.05f, 0.15f, this.mapFillPercentage
			};
		}

		#endregion
	}
}
