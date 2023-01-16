using Events.Game;
using Events.Sound;
using Interface;
using Structures;
using Structures.Wrapper;
using UnityEngine;

namespace Room.RoomContent.Enemys
{
	/// <summary>
	///  Class that implements room entity logic
	/// </summary>
	public class RoomEntity : RoomContent, IDamage
	{
		#region properties

		[field: SerializeField]
		private float ParticleDamageToEntity { get; set; } = 5f;
		
		[field: SerializeField]
		protected EnemyFloorBoost ManageEnemyFloorBoost { get; set; }
		
		[field: SerializeField]
		protected GameScore PlayerGameScore { get; set; }
		
		[field: SerializeField]
		protected EnemyFired EnemyFireEffect { get; set; }
		
		#endregion
		#region Variables

		public ContentAttackStats attackStats;
		protected RoomEntity thisEntity;
		protected const float MAX_OUTLINE_INTENSITY = 0.5f;

		#endregion
		#region unityCallbacks

		protected void OnParticleCollision(GameObject other)
		{
			this.TakeDamage(this.ParticleDamageToEntity);
		}

		#endregion
		#region methodes
	
		/// <summary>
		/// methode that initializes room entity state
		/// </summary>
		protected override void Init()
		{
			base.Init();
			this.attackStats.projectileSpeed = 1f;
			this.attackStats.projectilePower = 5f;
		} 

		/// <summary>
		/// methode that applies damage to room entity
		/// </summary>
		public virtual void TakeDamage(float damageValue)
		{
			if (this.durabilityStats.canBeDestroyed == false || this.DeflectProjectile() == true)
			{
				return;
			}

			this.CheckIfSurvived(damageValue);
			this.AdjustOutline();
		}
		
		/// <summary>
		/// methode that plays desolve animation if desolve material was added
		/// </summary>
		public virtual void PlayDesolveAnimation(){}

		/// <summary>
		/// methode that check if entity survived taking damage 
		/// </summary>
		protected void CheckIfSurvived(float damageValue)
		{
			if (this.durabilityStats.currentHealth <0)
			{
				return;
			}
			
			this.durabilityStats.currentHealth -= damageValue;
			if (this.durabilityStats.currentHealth <= 0f)
			{
				this.SpawnReword();
				this.parentRoom.DestroyRoomEntity(this.thisEntity);
			}
		}

		/// <summary>
		/// methode that spawns reward for deafiting room entity
		/// </summary>
		protected virtual void SpawnReword()
		{
			this.PlayerGameScore.IncreaseBeatenEnemysCount?.Invoke();
		}

		/// <summary>
		/// methode that adjusts material outline visibility
		/// </summary>
		protected virtual void AdjustOutline(){}

		/// <summary>
		/// methode that calculates new outline intensity
		/// </summary>
		protected float CalculateNewOutlineIntensity()
		{
			return MAX_OUTLINE_INTENSITY - (MAX_OUTLINE_INTENSITY * (this.durabilityStats.currentHealth / this.durabilityStats.maxHealth));
		}
		
		#endregion
		#region InterfaceMethodes

		/// <summary>
		/// methode that determins if object can deflect projectiles
		/// </summary>
		public virtual bool DeflectProjectile()           => false;

		#endregion

	}
}
