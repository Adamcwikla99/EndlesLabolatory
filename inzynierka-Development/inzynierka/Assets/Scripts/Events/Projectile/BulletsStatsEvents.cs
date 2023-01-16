using UnityEngine;

namespace Events.Projectile
{
	/// <summary>
	///  Class responsible for relaying manipulating bullet stats events
	/// </summary>
	[CreateAssetMenu(fileName = "BulletsStatsEvents", menuName = "Projectile/BulletsStatsEvents")]
	public class BulletsStatsEvents : ScriptableObject
	{
		public System.Action<float> IncreaseSpeed;
		public System.Action<float> IncreaseDamage;
		public System.Action<float> DecreaseReload;
		
	}
}
