using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMPro
{

	public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		#region Serialized Fields

		[SerializeField]
		private CharacterSelectionEvent m_OnCharacterSelection = new CharacterSelectionEvent();
		[SerializeField]
		private SpriteSelectionEvent m_OnSpriteSelection = new SpriteSelectionEvent();
		[SerializeField]
		private WordSelectionEvent m_OnWordSelection = new WordSelectionEvent();
		[SerializeField]
		private LineSelectionEvent m_OnLineSelection = new LineSelectionEvent();
		[SerializeField]
		private LinkSelectionEvent m_OnLinkSelection = new LinkSelectionEvent();

		#endregion
		#region Private Fields

		private Camera m_Camera;
		private Canvas m_Canvas;
		private int m_lastCharIndex = -1;
		private int m_lastLineIndex = -1;
		private int m_lastWordIndex = -1;

		private int m_selectedLink = -1;

		private TMP_Text m_TextComponent;

		#endregion
		#region Public Properties

		/// <summary>
		///     Event delegate triggered when pointer is over a character.
		/// </summary>
		public CharacterSelectionEvent onCharacterSelection {
			get { return this.m_OnCharacterSelection; }
			set { this.m_OnCharacterSelection = value; }
		}

		/// <summary>
		///     Event delegate triggered when pointer is over a sprite.
		/// </summary>
		public SpriteSelectionEvent onSpriteSelection {
			get { return this.m_OnSpriteSelection; }
			set { this.m_OnSpriteSelection = value; }
		}

		/// <summary>
		///     Event delegate triggered when pointer is over a word.
		/// </summary>
		public WordSelectionEvent onWordSelection {
			get { return this.m_OnWordSelection; }
			set { this.m_OnWordSelection = value; }
		}

		/// <summary>
		///     Event delegate triggered when pointer is over a line.
		/// </summary>
		public LineSelectionEvent onLineSelection {
			get { return this.m_OnLineSelection; }
			set { this.m_OnLineSelection = value; }
		}

		/// <summary>
		///     Event delegate triggered when pointer is over a link.
		/// </summary>
		public LinkSelectionEvent onLinkSelection {
			get { return this.m_OnLinkSelection; }
			set { this.m_OnLinkSelection = value; }
		}

		#endregion
		#region Unity Callbacks

		private void Awake()
		{
			// Get a reference to the text component.
			this.m_TextComponent = this.gameObject.GetComponent<TMP_Text>();

			// Get a reference to the camera rendering the text taking into consideration the text component type.
			if (this.m_TextComponent.GetType() == typeof(TextMeshProUGUI))
			{
				this.m_Canvas = this.gameObject.GetComponentInParent<Canvas>();

				if (this.m_Canvas != null)
				{
					this.m_Camera = this.m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : this.m_Canvas.worldCamera;
				}
			}
			else
			{
				this.m_Camera = Camera.main;
			}
		}

		private void LateUpdate()
		{
			if (TMP_TextUtilities.IsIntersectingRectTransform(this.m_TextComponent.rectTransform, Input.mousePosition, this.m_Camera))
			{
				#region Example of Character or Sprite Selection

				int charIndex = TMP_TextUtilities.FindIntersectingCharacter(this.m_TextComponent, Input.mousePosition, this.m_Camera, true);

				if (charIndex != -1 && charIndex != this.m_lastCharIndex)
				{
					this.m_lastCharIndex = charIndex;

					TMP_TextElementType elementType = this.m_TextComponent.textInfo.characterInfo[charIndex].elementType;

					// Send event to any event listeners depending on whether it is a character or sprite.
					if (elementType == TMP_TextElementType.Character)
					{
						this.SendOnCharacterSelection(this.m_TextComponent.textInfo.characterInfo[charIndex].character, charIndex);
					}
					else if (elementType == TMP_TextElementType.Sprite)
					{
						this.SendOnSpriteSelection(this.m_TextComponent.textInfo.characterInfo[charIndex].character, charIndex);
					}
				}

				#endregion

				#region Example of Word Selection

				// Check if Mouse intersects any words and if so assign a random color to that word.
				int wordIndex = TMP_TextUtilities.FindIntersectingWord(this.m_TextComponent, Input.mousePosition, this.m_Camera);

				if (wordIndex != -1 && wordIndex != this.m_lastWordIndex)
				{
					this.m_lastWordIndex = wordIndex;

					// Get the information about the selected word.
					TMP_WordInfo wInfo = this.m_TextComponent.textInfo.wordInfo[wordIndex];

					// Send the event to any listeners.
					this.SendOnWordSelection(wInfo.GetWord(), wInfo.firstCharacterIndex, wInfo.characterCount);
				}

				#endregion

				#region Example of Line Selection

				// Check if Mouse intersects any words and if so assign a random color to that word.
				int lineIndex = TMP_TextUtilities.FindIntersectingLine(this.m_TextComponent, Input.mousePosition, this.m_Camera);

				if (lineIndex != -1 && lineIndex != this.m_lastLineIndex)
				{
					this.m_lastLineIndex = lineIndex;

					// Get the information about the selected word.
					TMP_LineInfo lineInfo = this.m_TextComponent.textInfo.lineInfo[lineIndex];

					// Send the event to any listeners.
					char[] buffer = new char[lineInfo.characterCount];

					for (int i = 0; i < lineInfo.characterCount && i < this.m_TextComponent.textInfo.characterInfo.Length; i++)
					{
						buffer[i] = this.m_TextComponent.textInfo.characterInfo[i + lineInfo.firstCharacterIndex].character;
					}

					string lineText = new string(buffer);
					this.SendOnLineSelection(lineText, lineInfo.firstCharacterIndex, lineInfo.characterCount);
				}

				#endregion

				#region Example of Link Handling

				// Check if mouse intersects with any links.
				int linkIndex = TMP_TextUtilities.FindIntersectingLink(this.m_TextComponent, Input.mousePosition, this.m_Camera);

				// Handle new Link selection.
				if (linkIndex != -1 && linkIndex != this.m_selectedLink)
				{
					this.m_selectedLink = linkIndex;

					// Get information about the link.
					TMP_LinkInfo linkInfo = this.m_TextComponent.textInfo.linkInfo[linkIndex];

					// Send the event to any listeners.
					this.SendOnLinkSelection(linkInfo.GetLinkID(), linkInfo.GetLinkText(), linkIndex);
				}

				#endregion
			}
			else
			{
				// Reset all selections given we are hovering outside the text container bounds.
				this.m_selectedLink = -1;
				this.m_lastCharIndex = -1;
				this.m_lastWordIndex = -1;
				this.m_lastLineIndex = -1;
			}
		}

		#endregion
		#region Public Methods

		public void OnPointerEnter(PointerEventData eventData)
		{
			//Debug.Log("OnPointerEnter()");
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			//Debug.Log("OnPointerExit()");
		}

		#endregion
		#region Private Methods

		private void SendOnCharacterSelection(char character, int characterIndex)
		{
			if (this.onCharacterSelection != null)
			{
				this.onCharacterSelection.Invoke(character, characterIndex);
			}
		}

		private void SendOnSpriteSelection(char character, int characterIndex)
		{
			if (this.onSpriteSelection != null)
			{
				this.onSpriteSelection.Invoke(character, characterIndex);
			}
		}

		private void SendOnWordSelection(string word, int charIndex, int length)
		{
			if (this.onWordSelection != null)
			{
				this.onWordSelection.Invoke(word, charIndex, length);
			}
		}

		private void SendOnLineSelection(string line, int charIndex, int length)
		{
			if (this.onLineSelection != null)
			{
				this.onLineSelection.Invoke(line, charIndex, length);
			}
		}

		private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
		{
			if (this.onLinkSelection != null)
			{
				this.onLinkSelection.Invoke(linkID, linkText, linkIndex);
			}
		}

		#endregion

		[Serializable]
		public class CharacterSelectionEvent : UnityEvent<char, int>
		{
		}

		[Serializable]
		public class SpriteSelectionEvent : UnityEvent<char, int>
		{
		}

		[Serializable]
		public class WordSelectionEvent : UnityEvent<string, int, int>
		{
		}

		[Serializable]
		public class LineSelectionEvent : UnityEvent<string, int, int>
		{
		}

		[Serializable]
		public class LinkSelectionEvent : UnityEvent<string, string, int>
		{
		}
	}
}
