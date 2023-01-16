using System.Collections.Generic;
using Animations.Enemys.BaseEnemy;
using Events.Drop;
using Interface;
using Shaders;
using Structures;
using Structures.Wrapper;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal.BaseEnemy
{
	/// <summary>
	///  Class that implements base enemy logic
	/// </summary>
	public class BaseEnemy : RoomEntity, IDrop
	{
		#region Properties

		[field: SerializeField]
		private List<EnemyMaterialAdder> ObjectMaterialAdders { get; set; } = new List<EnemyMaterialAdder>();

		[field: SerializeField]
		private BaseEnemyAnimation DesolveAnimation { get; set; }
		
		[field: SerializeField]
		private DropItem DropItemEvent { get; set; }
		
		[field: SerializeField]
		private BaseEnemyAttackAI EntityAttackStats { get; set; }
		
		#endregion
		#region Unity Callbacks

		new private void Start()
		{
			this.thisEntity = this.gameObject.GetComponent<BaseEnemy>();
			EntityBoost value = this.ManageEnemyFloorBoost.GetEntityPowerUp?.Invoke();
			this.PowerUp(value);
			this.EntityAttackStats.SetBonusStats(this.attackStats.projectilePower, this.attackStats.projectileSpeed);
		}

		#endregion
		#region methodes

		public override void PlayDesolveAnimation()
		{
			this.DesolveAnimation.PlayAnimation();
		}

		public void PlayFireSound()
		{
			this.EnemyFireEffect.PlayEnemyFireSound?.Invoke();
		}

		protected override void AdjustOutline()
		{
			this.ObjectMaterialAdders[0].ChangeOutlineIntensity(this.CalculateNewOutlineIntensity());
		}
		
		#endregion

		/// <summary>
		/// methode that spawns reward for defeating enemy
		/// </summary>
		protected override void SpawnReword()
		{
			this.DropEnemyReward();
		}

		/// <summary>
		/// methode that powers up base enemy gameobject
		/// </summary>
		private void PowerUp(EntityBoost boostValue)
		{
			this.durabilityStats.maxHealth += boostValue.BonusMaxHP;
			this.durabilityStats.currentHealth = this.durabilityStats.maxHealth;
			this.attackStats.projectilePower += boostValue.BonusProjectilePower;
			this.attackStats.projectileSpeed += boostValue.BonusProjectileSpeed;
		}

		/// <summary>
		/// methode that drops received reword
		/// </summary>
		public void DropEnemyReward()
		{
			this.PlayerGameScore.IncreaseBeatenEnemysCount?.Invoke();
			this.DropItemEvent.DropLoot?.Invoke(this.transform.position);
		}
	}
	
}
