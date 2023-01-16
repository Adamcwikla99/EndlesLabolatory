using Events.Game;
using Events.UI;
using Structures;
using Structures.Wrapper;
using Unity.VisualScripting;
using UnityEngine;

namespace ScenesManager
{
    /// <summary>
    ///  Class that implements attempt progression logic
    /// </summary>
    public class GameProgressManager : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private GameScore ManageGameScore { get; set; }
        
        [field: SerializeField]
        private EnemyFloorBoost ManageEnemyFloorBoost { get; set; }
        
        [field: SerializeField]
        private int BeatenFloors { get; set; }
        
        [field: SerializeField]
        private int BeatenEnemys { get; set; }
        
        [field: SerializeField]
        private float MaxHpScale { get; set; }

        [field: SerializeField]
        private float ProjectilePowerScale { get; set; }
        
        [field: SerializeField]
        private float ProjectileSpeedScale { get; set; }
        
        [field: SerializeField]
        private UpdateBeatenEnemys UIBeatenEnemys { get; set; }
        
        [field: SerializeField]
        private UpdateBeatenFloors UIBeatenFloors { get; set; }

        #endregion

        #region variables

        private EntityBoost currentEntityBoost = new EntityBoost();
        
        #endregion

        #region unityCallbacks

        private void Start()
        {
            this.currentEntityBoost.CalculateNewBoost(this.BeatenFloors, this.MaxHpScale, this.ProjectilePowerScale, this.ProjectileSpeedScale);
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
        /// methode that sends new attempt statistic to UI windows
        /// </summary>
        public void RelayNewPauseData()
        {
            this.UIBeatenFloors.RelayNewBeatenFloorsCount?.Invoke(this.BeatenFloors);
            this.UIBeatenEnemys.RelayNewBeatenEnemysCount?.Invoke(this.BeatenEnemys);
        }
        
        /// <summary>
        /// methode that enables enemys power up and attempt statistic relying methods 
        /// </summary>
        private void EnableEvents()
        {
            this.ManageGameScore.IncreaseBeatenEnemysCount += IncreaseBeatenEnemys;
            this.ManageGameScore.IncreaseBeatenFloorsCount += IncreaseBeatenFloors;
            this.ManageGameScore.GetBeatenFloors += () => this.BeatenFloors;
            this.ManageEnemyFloorBoost.GetEntityPowerUp += EntityPowerUp;
        }

        /// <summary>
        /// methode that disables enemys power up and attempt statistic relying methods 
        /// </summary>
        private void DisableEvents()
        {
            this.ManageGameScore.IncreaseBeatenEnemysCount -= IncreaseBeatenEnemys;
            this.ManageGameScore.IncreaseBeatenFloorsCount -= IncreaseBeatenFloors;
            this.ManageGameScore.GetBeatenFloors -= () => this.BeatenFloors;
            this.ManageEnemyFloorBoost.GetEntityPowerUp -= EntityPowerUp;
        }

        /// <summary>
        /// method that incises floor beaten count
        /// </summary>
        private void IncreaseBeatenFloors()
        {
            this.BeatenFloors += 1;
            this.currentEntityBoost.CalculateNewBoost(this.BeatenFloors, this.MaxHpScale, this.ProjectilePowerScale, this.ProjectileSpeedScale);
            this.UIBeatenFloors.RelayNewBeatenFloorsCount?.Invoke(this.BeatenFloors);
        }
        
        /// <summary>
        /// method that incises enemys beaten count
        /// </summary>
        private void IncreaseBeatenEnemys()
        {
            this.BeatenEnemys += 1;
            this.UIBeatenEnemys.RelayNewBeatenEnemysCount?.Invoke(this.BeatenEnemys);
        }
        
        /// <summary>
        /// methode that power up enemys
        /// </summary>
        /// <returns></returns>
        private EntityBoost EntityPowerUp()
        {
            return this.currentEntityBoost;
        }
        
        #endregion



    }
}
