using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextConsoleSimulator : MonoBehaviour
	{
		#region Private Fields

		private bool hasTextChanged;
		private TMP_Text m_TextComponent;

		#endregion
		#region Unity Callbacks

		private void Awake() => this.m_TextComponent = this.gameObject.GetComponent<TMP_Text>();

		private void Start() => this.StartCoroutine(this.RevealCharacters(this.m_TextComponent)); //StartCoroutine(RevealWords(m_TextComponent));

		private void OnEnable() =>
			// Subscribe to event fired when text object has been regenerated.
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(this.ON_TEXT_CHANGED);

		private void OnDisable() => TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(this.ON_TEXT_CHANGED);

		#endregion
		#region Private Methods

		// Event received when the text object has changed.
		private void ON_TEXT_CHANGED(Object obj) => this.hasTextChanged = true;

		/// <summary>
		///     Method revealing the text one character at a time.
		/// </summary>
		/// <returns></returns>
		private IEnumerator RevealCharacters(TMP_Text textComponent)
		{
			textComponent.ForceMeshUpdate();

			TMP_TextInfo textInfo = textComponent.textInfo;

			int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
			int visibleCount = 0;

			while (true)
			{
				if (this.hasTextChanged)
				{
					totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
					this.hasTextChanged = false;
				}

				if (visibleCount > totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1.0f);
					visibleCount = 0;
				}

				textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

				visibleCount += 1;

				yield return null;
			}
		}

		/// <summary>
		///     Method revealing the text one word at a time.
		/// </summary>
		/// <returns></returns>
		private IEnumerator RevealWords(TMP_Text textComponent)
		{
			textComponent.ForceMeshUpdate();

			int totalWordCount = textComponent.textInfo.wordCount;
			int totalVisibleCharacters = textComponent.textInfo.characterCount; // Get # of Visible Character in text object
			int counter = 0;
			int visibleCount = 0;

			while (true)
			{
				int currentWord = counter % (totalWordCount + 1);

				// Get last character index for the current word.
				if (currentWord == 0) // Display no words.
				{
					visibleCount = 0;
				}
				else if (currentWord < totalWordCount) // Display all other words with the exception of the last one.
				{
					visibleCount = textComponent.textInfo.wordInfo[currentWord - 1].lastCharacterIndex + 1;
				}
				else if (currentWord == totalWordCount) // Display last word and all remaining characters.
				{
					visibleCount = totalVisibleCharacters;
				}

				textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

				// Once the last character has been revealed, wait 1.0 second and start over.
				if (visibleCount >= totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1.0f);
				}

				counter += 1;

				yield return new WaitForSeconds(0.1f);
			}
		}

		#endregion
	}
}
