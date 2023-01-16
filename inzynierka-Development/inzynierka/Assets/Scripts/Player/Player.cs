using System;
using Events.Menu;
using Events.Player;
using Events.UI;
using FMODUnity;
using Interface;
using ScenesManager;
using Structures;
using Structures.Enums;
using Structures.Map.Room;
using Structures.Wrapper;
using UnityEngine;

namespace Player
{
    /// <summary>
    ///  class responsible for controlling player behaviors and statistics
    /// </summary>
    public class Player : MonoBehaviour, IDamage
    {
        #region properties

        [field: SerializeField]
        public bool IsInClearedRoom { get; private set; }
        
        [field: SerializeField]
        public CordsXY PlayerRoomPosition { get; private set; }
        
        [field: SerializeField]
        private ContentDurabilityStats DurabilityStats { get; set; }
        
        [field: SerializeField]
        private PlayerSpeedEvents PlayerSpeedController { get; set; }

        [field: SerializeField]
        private float Money { get; set; } = 0;

        [field: SerializeField]
        private PlayerStatsEvents PlayerStats { get; set; }

        [field: SerializeField]
        private float IndividualParticleDamage { get; set; } = 5f;
        
        [field: SerializeField]
        private DisplayedUIStats StatsChangeEvents { get; set; }
        
        [field: SerializeField]
        private StudioEventEmitter PlayerDeath { get; set; }
        
        [field: SerializeField]
        private SwitchMenu SwitchActiveUI { get; set; }
        
        [field: SerializeField]
        private GameProgressManager ProgressManager { get; set; }
        
        [field: SerializeField]
        private PlayerObjectGetter PlayerObject { get; set; }
        
        #endregion
        #region unityCallbacks

        private void Start()
        {
            StatsChangeEvents.InitialDurabilitySetup?.Invoke(this.DurabilityStats.currentHealth,this.DurabilityStats.currentHealth, this.Money);
        }

        private void OnEnable()
        {
            this.PlayerStats.AddStat += AddStat;
            this.PlayerStats.GetStat += GetStat;
            this.PlayerObject.GetPlayerObject += ReturnPlayerObject;
        }

        private void OnDisable()
        {
            this.PlayerStats.AddStat -= AddStat;
            this.PlayerStats.GetStat -= GetStat;
            this.PlayerObject.GetPlayerObject -= ReturnPlayerObject;
        }
        
        private void OnParticleCollision(GameObject other)
        {
            TakeDamage(IndividualParticleDamage);
        }

        #endregion
        #region methods

        /// <summary>
        /// methode that returns player game object
        /// </summary>
        /// <returns></returns>
        private GameObject ReturnPlayerObject()
        {
            return this.gameObject;
        }

        /// <summary>
        /// methode that sets if player is in cleared room
        /// </summary>
        /// <param name="newState"></param>
        public void SetIsInClearedRoom(bool newState)
        {
            this.IsInClearedRoom = newState;
        }

        /// <summary>
        /// methode that sets player cell map position
        /// </summary>
        public void SetPlayerRoomPosition(CordsXY newPosition)
        {
            this.PlayerRoomPosition = newPosition;
        }
        
        /// <summary>
        /// methode that applies damage to player
        /// </summary>
        public void TakeDamage(float damageValue)
        {
            if (this.DurabilityStats.canBeDestroyed == false || DeflectProjectile() == true)
            {
                return;
            }

            this.DurabilityStats.currentHealth -= damageValue;
            if (this.DurabilityStats.currentHealth <= 0f)
            {
                this.PlayerDeath.Play();
                this.SwitchActiveUI.switchMenu?.Invoke(MenuType.EndGameMenu);
                this.ProgressManager.RelayNewPauseData();
            }
            
            this.StatsChangeEvents.Hp?.Invoke(this.DurabilityStats.currentHealth);
        }
        
        /// <summary>
        /// methode that informs if player can deflect projectile
        /// </summary>
        /// <returns></returns>
        public bool DeflectProjectile() => false;
        
        /// <summary>
        /// methode that changes one of players stats
        /// </summary>
        private void AddStat(PlayerStats statType, float value)
        {
            switch (statType)
            {
                case Structures.Enums.PlayerStats.Health :
                    AddHealthStat(value);
                    break;
                case Structures.Enums.PlayerStats.Money :
                    this.Money += value;
                    this.StatsChangeEvents.Money?.Invoke(this.Money);
                    break;
                case Structures.Enums.PlayerStats.MaxHealth :
                    this.DurabilityStats.maxHealth += value;
                    this.StatsChangeEvents.MaxHp?.Invoke(this.DurabilityStats.maxHealth);
                    break;
                default :
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }

            StatsChangeEvents.Money?.Invoke(this.Money);
        }

        /// <summary>
        /// methode that adds health stat
        /// </summary>
        private void AddHealthStat(float value)
        {
            if (this.DurabilityStats.currentHealth + value > this.DurabilityStats.maxHealth)
            {
                this.DurabilityStats.currentHealth = this.DurabilityStats.maxHealth;
                this.StatsChangeEvents.Hp?.Invoke(this.DurabilityStats.currentHealth);
                return;
            }
                    
            this.DurabilityStats.currentHealth += value;
            this.StatsChangeEvents.Hp?.Invoke(this.DurabilityStats.currentHealth);
        }
        
        /// <summary>
        /// methode that reruns one of player stats
        /// </summary>
        private float GetStat(PlayerStats statType)
        {
            return statType switch
            {
                Structures.Enums.PlayerStats.Health => this.DurabilityStats.currentHealth,
                Structures.Enums.PlayerStats.Money => this.Money,
                Structures.Enums.PlayerStats.MaxHealth => this.DurabilityStats.maxHealth,
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };
        }
        
        #endregion
    }
}
