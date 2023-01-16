using Interface;
using UnityEngine;

namespace Room.RoomContent.Obsticle
{
	/// <summary>
	///  Class that implements wall obstacle logic
	/// </summary>
	public class WallObstacle : RoomObstacle, IDamage
	{
		#region properties
		
		[field: SerializeField]
		private float ParticleDamageToObstacle { get; set; } = 10f;
		
		#endregion
		#region Unity Callbacks

		protected void OnParticleCollision(GameObject other)
		{
			TakeDamage(ParticleDamageToObstacle);
		}
		
		#endregion
		#region methods

		/// <summary>
		/// methode responsible for applying damage to wall obstacle object
		/// </summary>
		/// <param name="damageValue"></param>
		public void TakeDamage(float damageValue)
		{
		
			if (this.durabilityStats.canBeDestroyed == false || this.DeflectProjectile() == true)
			{
				return;
			}

			CheckIfSurvived(damageValue);
		}
		
		/// <summary>
		/// methode that determines if obstacle can take damage
		/// </summary>
		/// <returns></returns>
		public bool DeflectProjectile() => false;

		/// <summary>
		/// methode that checks if wall obstacle survived damage
		/// </summary>
		private void CheckIfSurvived(float damageValue)
		{
			if (this.durabilityStats.currentHealth <0)
			{
				return;
			}
			
			this.durabilityStats.currentHealth -= damageValue;
			if (this.durabilityStats.currentHealth <= 0f)
			{
				Destroy(this.gameObject);
			}
		}
		
		#endregion

		
	}
}
