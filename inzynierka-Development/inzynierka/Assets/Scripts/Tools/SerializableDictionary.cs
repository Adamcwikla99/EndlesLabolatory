using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Class that implements serializable dictionary
/// </summary>
[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
	#region Serialized Fields

	[SerializeField] 
	private List<Element> dictionaryAsList = new List<Element>();

	#endregion
	#region Public Variables

	public Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

	#endregion
	#region Public Methods

	public void OnAfterDeserialize()
	{
		this.dictionary.Clear();

		foreach (Element element in this.dictionaryAsList)
		{
			try
			{
				this.dictionary.Add(element.key, element.value);
			}
			catch (ArgumentException) { }
		}
	}

	public void OnBeforeSerialize()
	{

		int countBefore = this.dictionaryAsList.Count;

		Element? last = countBefore != 0 ? this.dictionaryAsList[countBefore - 1] : null;

		this.dictionaryAsList.Clear();

		foreach (KeyValuePair<TKey, TValue> pair in this.dictionary)
		{
			this.dictionaryAsList.Add(new Element
			{
				key = pair.Key,
				value = pair.Value
			});
		}

		if (countBefore > this.dictionaryAsList.Count)
		{
			this.dictionaryAsList.Add(last.Value);
		}
	}

	#endregion
	#region Serializable

	[Serializable]
	private struct Element
	{
		public TKey key;
		public TValue value;
	}

	#endregion

}
