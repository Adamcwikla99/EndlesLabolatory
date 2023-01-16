using System;
using System.Collections.Generic;
using System.Linq;
using Events.Player;
using Events.Projectile;
using Events.UI;
using FMODUnity;
using Projectaile;
using Structures;
using Structures.Enums;
using Structures.Wrapper;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    /// <summary>
    ///  class responsible for management of player ammunition and fire action
    /// </summary>
    public class PlayerWeapon : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private Camera PlayerCamera { get; set; }

        [field: SerializeField]
        private ProjectileGetter GetProjectileEvents { get; set; }

        [field: SerializeField]
        private PlayerFireEvents FireEvents { get; set; }
        
        [field: SerializeField]
        private PlayerChangeWeaponEvents ChangeWeaponEvents { get; set; }
        
        // TODO: Make a scriptable object with it
        [field: SerializeField]
        private List<ProjectileFireDelay> FireDelays { get; set; }
        
        [field: SerializeField]
        private List<Projectile> ProjectileStats { get; set; }

        [field: SerializeField]
        private int CurrentWeaponIndex { get; set; } = 0; 
        
        [field: SerializeField]
        private AmmunitionAdder AmmunitionManager { get; set; }
        
        [field: SerializeField]
        private List<AmmunitionCount> CurrentAmmunitionCounts { get; set; }

        [field: SerializeField]
        private BulletBonusStats BulletStats { get; set; }
        
        [field: SerializeField]
        private BulletsStatsEvents BulletEvents { get; set; }
        
        [field: SerializeField]
        private DisplayedUIStats StatsChangeEvents { get; set; }
        
        [field: SerializeField]
        private StudioEventEmitter BulletEmitter { get; set; }
        
        [field: SerializeField]
        private StudioEventEmitter ArrowEmitter { get; set; }
        
        [field: SerializeField]
        private StudioEventEmitter GrandeEmitter { get; set; }
        
        [field: SerializeField]
        private StudioEventEmitter RocketEmitter { get; set; }
        
        [field: SerializeField]
        private StudioEventEmitter NoAmmunition { get; set; }

        #endregion
        #region variables

        private Dictionary<Structures.Enums.ProjectileType, TimeSinceFired> fireController = new Dictionary<ProjectileType, TimeSinceFired>();
        private const float MINIMAL_RELOAD_TIME = 0.001f;

        #endregion
        #region unityCallbacks

        private void Start()
        {
            Init();
        }

        private void FixedUpdate()
        {
            ManageWeaponColdown();
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
        /// class initialization methode
        /// </summary>
        private void Init()
        {
            StatsChangeEvents.InitialAmmunitionSetup?.Invoke(this.CurrentAmmunitionCounts[1].count, this.CurrentAmmunitionCounts[2].count, this.CurrentAmmunitionCounts[3].count);
            foreach (ProjectileFireDelay delay in FireDelays)
            {
                fireController.Add(delay.projectileType, new TimeSinceFired());
            }
        }

        /// <summary>
        /// methode that manages weapon coldown
        /// </summary>
        private void ManageWeaponColdown()
        {
            foreach (KeyValuePair<ProjectileType, TimeSinceFired> projectileType in fireController)
            {
                IndividualProjectileTypeCheck(projectileType);
            }
        }

        /// <summary>
        /// methode that check if weapon type can be fired 
        /// </summary>
        private void IndividualProjectileTypeCheck(KeyValuePair<ProjectileType, TimeSinceFired> projectileType)
        {
            if (projectileType.Value.fired == false)
            {
                return;
            }

            projectileType.Value.timeSinceLastFired += Time.fixedDeltaTime;
            if (projectileType.Value.timeSinceLastFired < FireDelays.First(x => x.projectileType == projectileType.Key).delay)
            {
                return;
            }

            projectileType.Value.fired = false;
            projectileType.Value.timeSinceLastFired = 0;
        }
        
        /// <summary>
        /// methode that trys to fire weapon
        /// </summary>
        private void TryFire()
        {
            ProjectileType currentType = FireDelays[this.CurrentWeaponIndex].projectileType;
            if (fireController[currentType].fired == true)
            {
                return;
            }

            if (CurrentAmmunitionCounts[this.CurrentWeaponIndex].count == 0)
            {
                if (this.NoAmmunition.IsPlaying() == true)
                {
                    return;
                }
                
                this.NoAmmunition.Play();
                return;
            }

            FireType(currentType);
        }

        /// <summary>
        /// methode that fires weapon type
        /// </summary>
        private void FireType(Structures.Enums.ProjectileType currentType)
        {
            float weaponBonusSpeed, weaponBonusDamage;
            
            Projectile toFire = GetProjectileEvents.GetProjectile?.Invoke(currentType);
            AddBonusStats(out weaponBonusSpeed,out weaponBonusDamage, toFire);
            toFire!.Fire(this.PlayerCamera.transform.rotation, this.transform.position, this.PlayerCamera.transform.forward,0);
            fireController[FireDelays[this.CurrentWeaponIndex].projectileType].fired = true;
            ConsumeAmmunition();
            UpdateUI(currentType);
            PlaySound(currentType);

        }

        /// <summary>
        /// methode that plays fire sound
        /// </summary>
        private void PlaySound(Structures.Enums.ProjectileType currentType)
        {
            switch (currentType)
            {
                case ProjectileType.Enemy :
                    throw new ArgumentOutOfRangeException(nameof(currentType), currentType, null);
                case ProjectileType.Bullet :
                    this.BulletEmitter.Play();
                    break;
                case ProjectileType.Arrow :
                    this.ArrowEmitter.Play();
                    break;
                default :
                    break;
            }
        }
        
        /// <summary>
        /// methode that adds weapon stats value
        /// </summary>
        private void AddBonusStats(out float weaponBonusSpeed, out float weaponBonusDamage, Projectile toFire )
        {
            weaponBonusSpeed = 0;
            weaponBonusDamage = 0;
            if (toFire.ThisType != ProjectileType.Bullet)
            {
                return;
            }

            weaponBonusSpeed = this.BulletStats.bulletBonusSpeed;
            weaponBonusDamage = this.BulletStats.bulletBonusDamage;
        }
        
        /// <summary>
        /// methode that consumes ammunition
        /// </summary>
        private void ConsumeAmmunition()
        {
            if (CurrentAmmunitionCounts[this.CurrentWeaponIndex].consumable == false)
            {
                return;
            }

            CurrentAmmunitionCounts[this.CurrentWeaponIndex].count--;
        }
        
        /// <summary>
        /// methode that changes player currently used weapon 
        /// </summary>
        private void ChangePlayerWeapon(bool nextType)
        {
            if (nextType == false)
            {
                SetNextWeapon();
                return;
            }

            SetPreviousWeapon();
        }

        /// <summary>
        /// methode that changes weapon type to next type
        /// </summary>
        private void SetNextWeapon()
        {
            if (this.CurrentWeaponIndex+1 > FireDelays.Count-1)
            {
                this.CurrentWeaponIndex = 0;
                return;
            }

            this.CurrentWeaponIndex++;
        }

        /// <summary>
        /// methode that changes weapon type to previous type
        /// </summary>
        private void SetPreviousWeapon()
        {
            if (this.CurrentWeaponIndex-1 < 0)
            {
                this.CurrentWeaponIndex = FireDelays.Count-1;
                return;
            }

            this.CurrentWeaponIndex--;
        }
        
        /// <summary>
        /// methode that increases bullet damage
        /// </summary>
        private void IncreaseBulletDamage(float bonus)
        {
            this.BulletStats.bulletBonusDamage += bonus;
        }
        
        /// <summary>
        /// methode that increases bullet speed
        /// </summary>
        private void IncreaseBulletSpeed(float bonus)
        {
            this.BulletStats.bulletBonusSpeed += bonus;
        }

        /// <summary>
        /// methode that decreases bullet reload time
        /// </summary>
        private void DecreaseBulletReload(float bonus)
        {
            if (this.BulletStats.bulletBonusReloadTime - bonus < 0)
            {
                this.BulletStats.bulletBonusReloadTime = MINIMAL_RELOAD_TIME;
                this.FireDelays.First(x => x.projectileType == ProjectileType.Bullet).delay = MINIMAL_RELOAD_TIME;                
            }
            
            this.BulletStats.bulletBonusReloadTime -= bonus;
            this.FireDelays.First(x => x.projectileType == ProjectileType.Bullet).delay -= bonus;
        }

        /// <summary>
        /// methode that enables events
        /// </summary>
        private void EnableEvents()
        {
            this.FireEvents.PlayerFire += TryFire;
            this.ChangeWeaponEvents.ChangeWeaponType += ChangePlayerWeapon;
            this.AmmunitionManager.AddAmmunition += AddAmmunition;
            this.BulletEvents.DecreaseReload += DecreaseBulletReload;
            this.BulletEvents.IncreaseDamage += IncreaseBulletDamage;
            this.BulletEvents.IncreaseSpeed += IncreaseBulletSpeed;
        }

        /// <summary>
        /// methode that disables events
        /// </summary>
        private void DisableEvents()
        {
            this.FireEvents.PlayerFire -= TryFire;
            this.ChangeWeaponEvents.ChangeWeaponType -= ChangePlayerWeapon;
            this.AmmunitionManager.AddAmmunition -= AddAmmunition;
            this.BulletEvents.DecreaseReload -= DecreaseBulletReload;
            this.BulletEvents.IncreaseDamage -= IncreaseBulletDamage;
            this.BulletEvents.IncreaseSpeed -= IncreaseBulletSpeed;
        }
        
        /// <summary>
        /// methode that adds ammunition of type
        /// </summary>
        private void AddAmmunition(ProjectileType type, int amount)
        {
            CurrentAmmunitionCounts.First(x => x.type == type).count += amount;
            UpdateUI(type);
        }

        /// <summary>
        /// methode that updates ui stats
        /// </summary>
        private void UpdateUI(ProjectileType type)
        {
            switch (type)
            {
                case ProjectileType.Enemy :
                    break;
                case ProjectileType.Bullet :
                    break;
                case ProjectileType.Arrow :
                    StatsChangeEvents.SetArrows?.Invoke(this.CurrentAmmunitionCounts[1].count);
                    break;
                case ProjectileType.Granade :
                    StatsChangeEvents.SetGranades?.Invoke( this.CurrentAmmunitionCounts[2].count);
                    break;
                case ProjectileType.Rocket :
                    StatsChangeEvents.SetRockets?.Invoke(this.CurrentAmmunitionCounts[3].count);
                    break;
                default :
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        #endregion

    }
}
