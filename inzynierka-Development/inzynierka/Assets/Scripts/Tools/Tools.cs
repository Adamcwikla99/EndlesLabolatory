using System.Collections.Generic;
using Structures.Map.Room;
using UnityEngine;

/// <summary>
///  Class that implements custom tools that can be used in other projects
/// </summary>
public static class Tools
{
	#region Public Methods

	/// <summary>
	/// methode that returns random number form int range
	/// </summary>
	public static int GetRandomNumberFromRange(int lower, int upper) => Random.Range(lower, upper + 1);

	/// <summary>
	/// methode that gets random number form float range
	/// </summary>
	public static float GetRandomNumberFromRange(float lower, float upper) => Random.Range(lower, upper);

	/// <summary>
	/// methode that counts distance on two dimensional greed
	/// </summary>
	public static float DistanceCounter(CordsXY cordTo, CordsXY cordFrom, bool skipSquere = false)
	{
		int distX, distY;
		float resoult;
		distX = Mathf.Abs(cordFrom.x - cordTo.x);
		distY = Mathf.Abs(cordFrom.y - cordTo.y);
		resoult = Mathf.Pow(distX, 2) + Mathf.Pow(distY, 2);

		return skipSquere == false ? resoult : Mathf.Sqrt(resoult);
	}

	/// <summary>
	/// methode that cleats list and destroys it's game objects
	/// </summary>
	public static List<T> ClearAndDestroy<T>(List<T> objetcList) where T : Object
	{
		if (objetcList == null || objetcList.Count == 0)
		{
			return new List<T>();
		}

		if (objetcList is not List<GameObject>)
		{
			_ = objetcList.ConvertAll(x => new GameObject());
		}

		for (int i = objetcList.Count - 1; i >= 0; i--)
		{
			Object.Destroy(objetcList[i]);
			objetcList.RemoveAt(i);
		}

		return new List<T>();

	}

	#endregion
}
