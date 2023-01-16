using System;
using System.Collections.Generic;
using System.Linq;
using Events.Floor;
using Events.Game;
using Events.Room;
using Room.RoomContent;
using Structures.Enums;
using Structures.Map.Room;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesManager
{
    /// <summary>
    ///  Class that manages generated floor
    /// </summary>
    public class FloorManager : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        public List<FloorRoom> GeneratedRooms { get; private set; } = new List<FloorRoom>();

        [field: SerializeField]
        private GateEvents IndividualGateEvents { get; set; }
        
        [field: SerializeField]
        private TunnelEvents IndividualTunnelEvents { get; set; }
        
        [field: SerializeField]
        private FloorEvents IndividualFloorEvents { get; set; }
        
        [field: SerializeField]
        private Player.Player GamePlayer { get; set; }
        
        [field: SerializeField]
        private Transform Middle { get; set; }
        
        [field: SerializeField]
        private CordsXY MiddleCord { get; set; }
        
        [field: SerializeField]
        private HostileRoomEvents HostileRoom { get; set; }

        
        #endregion
        #region variables

        public bool nextGeneration;

        #endregion
        #region unityCallbacks
        
        private void Update()
        {
            // TODO: only for testing, remove later
            if (this.nextGeneration)
            {
                this.nextGeneration = false;
                IndividualFloorEvents.GenerateNewMap?.Invoke();
            }
        }
        
        private void OnEnable()
        {
            EnableEvents();
        }

        private void OnDisable()
        {
            DisableEvents();
        }
        
        #endregion
        #region methods

        /// <summary>
        /// methode responsible for activation off all previously inactive room entity's
        /// </summary>
        private void AwakeRoomEntities(CordsXY position)
        {
            FloorRoom roomToModify = FindRoomWithCords(position);
            Structures.Enums.Direction movementDirection = GateToOpenDirection(GamePlayer.PlayerRoomPosition, position);

            if (roomToModify.RoomCleared == true && this.GamePlayer.IsInClearedRoom == true)
            {
                MoveBetweenClearedRooms(roomToModify, movementDirection, position);
                return;
            }

            if (this.GamePlayer.IsInClearedRoom == false)
            {
                return;
            }

            MoveBetweenNotClearedRooms(roomToModify, movementDirection, position);
        }

        /// <summary>
        /// methode that manages player movement between cleared rooms
        /// </summary>
        private void MoveBetweenClearedRooms(FloorRoom roomToModify, Structures.Enums.Direction movementDirection, CordsXY position)
        {
            ManageGate(roomToModify, movementDirection);
            ManageGate(FindRoomWithCords(GamePlayer.PlayerRoomPosition), GateToOpenDirection(position, GamePlayer.PlayerRoomPosition), true);
            GamePlayer.SetPlayerRoomPosition(position);
            ForceOpenAllGates(roomToModify);
        }

        /// <summary>
        /// methode that opens all room gates 
        /// </summary>
        private void ForceOpenAllGates(FloorRoom roomToModify)
        {
            roomToModify.ChangeWorkingGatesState(false);
        }
        
        /// <summary>
        /// methode that manages player movement between cleared and not cleared rooms
        /// </summary>
        private void MoveBetweenNotClearedRooms(FloorRoom roomToModify, Structures.Enums.Direction movementDirection, CordsXY position)
        {
            ManageGate(FindRoomWithCords(GamePlayer.PlayerRoomPosition), GateToOpenDirection( position, GamePlayer.PlayerRoomPosition), true);
            ManageGate(roomToModify, movementDirection);
            GamePlayer.SetIsInClearedRoom(false);
            GamePlayer.SetPlayerRoomPosition(position);
            RoomActivationSwitch(roomToModify, roomToModify.ThisRoomType);
        }

        /// <summary>
        /// methode that activates room entity's base on room type 
        /// </summary>
        private void RoomActivationSwitch(FloorRoom roomToModify, RoomType roomType)
        {
            switch (roomType)
            {
                case RoomType.BossRoom :
                    roomToModify.ActivateRoomEntitys();
                    roomToModify.RoomActive = true;
                    break;
                case RoomType.NormalRoom :
                    roomToModify.ActivateRoomEntitys();
                    roomToModify.RoomActive = true;
                    break;
                case RoomType.Nothing :
                    break;
                case RoomType.StartRoom :
                    break;
                case RoomType.ShopRoom :
                    break;
                case RoomType.ItemRoom :
                    break;
                case RoomType.NoNeighbor :
                    break;
                default :
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// methode responsible for opening correct gates 
        /// </summary>
        private void ManageGate(FloorRoom roomToModify, Structures.Enums.Direction movementDirection, bool closeGate = false)
        {
            switch (movementDirection)
            {
                case Structures.Enums.Direction.N :
                    roomToModify.ChangeSpecifiedWorkingGatesState(Structures.Enums.Direction.N, closeGate);
                    break;
                case Structures.Enums.Direction.W :
                    roomToModify.ChangeSpecifiedWorkingGatesState(Structures.Enums.Direction.W, closeGate);
                    break;
                case Structures.Enums.Direction.S :
                    roomToModify.ChangeSpecifiedWorkingGatesState(Structures.Enums.Direction.S, closeGate);
                    break;
                case Structures.Enums.Direction.E :
                    roomToModify.ChangeSpecifiedWorkingGatesState(Structures.Enums.Direction.E, closeGate);
                    break;
                default :
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// methode that returns information witch gate of aligned room should be opened 
        /// </summary>
        private Structures.Enums.Direction GateToOpenDirection(CordsXY oldPos, CordsXY newPos)
        {
            int xDif, yDif;
            xDif = oldPos.x - newPos.x;
            yDif = oldPos.y - newPos.y;

            if (xDif == -1)
            {
                return Structures.Enums.Direction.N;
            }
            
            if (xDif == 1)
            {
                return Structures.Enums.Direction.S;
            }

            return yDif == -1 ? Structures.Enums.Direction.W : Structures.Enums.Direction.E;

        }
        
        /// <summary>
        /// methode that removes all entity's - for future development
        /// </summary>
        private void RemoveAllEntities(CordsXY position)
        {
            
        }

        /// <summary>
        /// methode that acknowledges room clear 
        /// </summary>
        private void AcknowledgeRoomClear(CordsXY position)
        {
            FloorRoom roomToModify = FindRoomWithCords(position);
            roomToModify.SetRoomClearState(true);
            this.GamePlayer.SetIsInClearedRoom(true);
            roomToModify.ChangeWorkingGatesState(false);
        }

        /// <summary>
        /// methode that processes received floor map layout 
        /// </summary>
        private void ReceiveGeneratedMap(List<FloorRoom> generatedMap)
        {
            this.MiddleCord = new CordsXY(5, 5);
            this.GeneratedRooms = generatedMap;
            AdjustPlayerSettings();
            AdjustStartRoomGates();
        }

        /// <summary>
        /// methode that adjusts player position and information about whether he is in created room or not
        /// </summary>
        private void AdjustPlayerSettings()
        {
            Vector3 positionToSet = this.Middle.position;
            positionToSet.y += 1f;
            GamePlayer.gameObject.transform.position = positionToSet;
            GamePlayer.SetIsInClearedRoom(true);
            this.GamePlayer.SetPlayerRoomPosition(MiddleCord);
        }

        /// <summary>
        /// methode that adjusts start room gates state
        /// </summary>
        private void AdjustStartRoomGates()
        {
            FloorRoom roomToModify = FindRoomWithCords(MiddleCord);
            roomToModify.ChangeWorkingGatesState(false);
        }

        /// <summary>
        /// methode that returns room cord with selected cords
        /// </summary>
        private FloorRoom FindRoomWithCords(CordsXY roomCords)
        {
            return this.GeneratedRooms.FirstOrDefault(floorRoom => floorRoom.RoomCordsPosition.x == roomCords.x && floorRoom.RoomCordsPosition.y == roomCords.y);
        }
        
        /// <summary>
        /// methode that enables room activation, room state change and map generation events
        /// </summary>
        private void EnableEvents()
        {
            this.IndividualTunnelEvents.AwakeRoomEntity += AwakeRoomEntities;
            this.IndividualTunnelEvents.KillAllRoomEntity += RemoveAllEntities;
            this.IndividualFloorEvents.ReleyGeneratedMap += ReceiveGeneratedMap;
            this.HostileRoom.ClearedRoom += AcknowledgeRoomClear;
        }

        /// <summary>
        /// methode that disables room activation, room state change and map generation events
        /// </summary>
        private void DisableEvents()
        {
            this.IndividualTunnelEvents.AwakeRoomEntity -= AwakeRoomEntities;
            this.IndividualTunnelEvents.KillAllRoomEntity -= RemoveAllEntities;
            this.IndividualFloorEvents.ReleyGeneratedMap -= ReceiveGeneratedMap;
            this.HostileRoom.ClearedRoom -= AcknowledgeRoomClear;
        }
        
        #endregion
    }
}
