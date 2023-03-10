using UnityEngine;

namespace TMPro.Examples
{

	public class SimpleScript : MonoBehaviour
	{
		#region Private Fields

		private float m_frame;

		private TextMeshPro m_textMeshPro;

		#endregion
		#region Constants

		//private TMP_FontAsset m_FontAsset;

		private const string label = "The <#0050FF>count is: </color>{0:2}";

		#endregion
		#region Unity Callbacks

		private void Start()
		{
			// Add new TextMesh Pro Component
			this.m_textMeshPro = this.gameObject.AddComponent<TextMeshPro>();

			this.m_textMeshPro.autoSizeTextContainer = true;

			// Load the Font Asset to be used.
			//m_FontAsset = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
			//m_textMeshPro.font = m_FontAsset;

			// Assign Material to TextMesh Pro Component
			//m_textMeshPro.fontSharedMaterial = Resources.Load("Fonts & Materials/LiberationSans SDF - Bevel", typeof(Material)) as Material;
			//m_textMeshPro.fontSharedMaterial.EnableKeyword("BEVEL_ON");

			// Set various font settings.
			this.m_textMeshPro.fontSize = 48;

			this.m_textMeshPro.alignment = TextAlignmentOptions.Center;

			//m_textMeshPro.anchorDampening = true; // Has been deprecated but under consideration for re-implementation.
			//m_textMeshPro.enableAutoSizing = true;

			//m_textMeshPro.characterSpacing = 0.2f;
			//m_textMeshPro.wordSpacing = 0.1f;

			//m_textMeshPro.enableCulling = true;
			this.m_textMeshPro.enableWordWrapping = false;

			//textMeshPro.fontColor = new Color32(255, 255, 255, 255);
		}

		private void Update()
		{
			this.m_textMeshPro.SetText(label, this.m_frame % 1000);
			this.m_frame += 1 * Time.deltaTime;
		}

		#endregion
	}
}
