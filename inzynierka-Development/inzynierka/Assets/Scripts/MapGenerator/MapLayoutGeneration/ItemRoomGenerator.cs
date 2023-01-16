using Structures.Map;
using Structures.Map.Room;

namespace MapGenerator.MapLayoutGeneration
{
    /// <summary>
    ///  class responsible for generating item room on given floor
    /// </summary>
    public class ItemRoomGenerator : RoomAdder
    {
        #region properties

    

        #endregion
        #region variables

    

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
        /// <param name="distanceList"> map generation thresholds limits </param>
        public ItemRoomGenerator(CordsXY[] allCordsAroundMiddle, int pointsAroundMiddleCount, MapGenerationParameters generationParameters, CordsXY middle, Row[] rows, FloorDimensions floorDimensions) : base(allCordsAroundMiddle, pointsAroundMiddleCount, generationParameters, middle, rows, floorDimensions)
        {
        
        }
        
        #endregion
    }
}
