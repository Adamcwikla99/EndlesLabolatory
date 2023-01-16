using System;

namespace CustomExtensions
{

	public static class Extensions
	{
		#region Public Methods

		/// <summary>
		///     method that returns next enum type
		/// </summary>
		/// <typeparam name="T"> type of enum</typeparam>
		/// <param name="src">enum class object to operate on </param>
		/// <returns>next enum next value</returns>
		/// <exception cref="System.ArgumentException"> exception that is thrown if operating type isin't enum</exception>
		public static T GetNext<T>(this T src) where T : struct, Enum
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
			}

			T[] Arr = (T[])Enum.GetValues(src.GetType());
			int j = Array.IndexOf(Arr, src) + 1;

			return Arr.Length == j ? Arr[0] : Arr[j];
		}

		/// <summary>
		///     method that returns previous enum type
		/// </summary>
		/// <typeparam name="T"> type of enum</typeparam>
		/// <param name="src">enum class object to operate on </param>
		/// <returns>previous enum value</returns>
		/// <exception cref="System.ArgumentException"> exception that is thrown if operating type isin't enum</exception>
		public static T GetPrevious<T>(this T src) where T : struct, Enum
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
			}

			T[] Arr = (T[])Enum.GetValues(src.GetType());
			int j = Array.IndexOf(Arr, src) - 1;

			return 0 > j ? Arr[^1] : Arr[j];
		}

		/// <summary>
		///     method that changes variable enum type to next defined one
		/// </summary>
		/// <typeparam name="T"> type of enum</typeparam>
		/// <param name="src">enum class object to operate on </param>
		/// <exception cref="System.ArgumentException"> exception that is thrown if operating type isin't enum</exception>
		public static void Next<T>(ref this T src) where T : struct, Enum
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
			}

			T[] Arr = (T[])Enum.GetValues(src.GetType());
			int j = Array.IndexOf(Arr, src) + 1;
			src = Arr.Length == j ? Arr[0] : Arr[j];
		}

		/// <summary>
		///     method that changes variable enum type to previous defined one
		/// </summary>
		/// <typeparam name="T"> type of enum</typeparam>
		/// <param name="src">enum class object to operate on </param>
		/// <exception cref="System.ArgumentException"> exception that is thrown if operating type isin't enum</exception>
		public static void Previous<T>(ref this T src) where T : struct, Enum
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
			}

			T[] Arr = (T[])Enum.GetValues(src.GetType());
			int j = Array.IndexOf(Arr, src) - 1;
			src = 0 > j ? Arr[^1] : Arr[j];
		}

		#endregion
	}
}
