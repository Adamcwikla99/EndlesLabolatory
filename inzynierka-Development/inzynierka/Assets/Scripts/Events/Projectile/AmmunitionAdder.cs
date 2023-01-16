using UnityEngine;

namespace Events.Projectile
{
	/// <summary>
	///  Class responsible for relaying adding ammunition of given type events
	/// </summary>
	[CreateAssetMenu(fileName = "AmmunitionAdder", menuName = "Projectile/AmmunitionAdder")]
	public class AmmunitionAdder : ScriptableObject
	{
		public System.Action<Structures.Enums.ProjectileType, int> AddAmmunition;

	}
}
