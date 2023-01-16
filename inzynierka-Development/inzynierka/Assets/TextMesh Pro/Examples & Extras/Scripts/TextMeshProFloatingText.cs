using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{

	public class TextMeshProFloatingText : MonoBehaviour
	{
		#region Private Fields

		//private int m_frame = 0;

		private static readonly WaitForEndOfFrame k_WaitForEndOfFrame = new WaitForEndOfFrame();
		private static readonly WaitForSeconds[] k_WaitForSecondsRandom =
		{
			new WaitForSeconds(0.05f), new WaitForSeconds(0.1f), new WaitForSeconds(0.15f), new WaitForSeconds(0.2f), new WaitForSeconds(0.25f), new WaitForSeconds(0.3f), new WaitForSeconds(0.35f),
			new WaitForSeconds(0.4f), new WaitForSeconds(0.45f), new WaitForSeconds(0.5f), new WaitForSeconds(0.55f), new WaitForSeconds(0.6f), new WaitForSeconds(0.65f), new WaitForSeconds(0.7f),
			new WaitForSeconds(0.75f), new WaitForSeconds(0.8f), new WaitForSeconds(0.85f), new WaitForSeconds(0.9f), new WaitForSeconds(0.95f), new WaitForSeconds(1.0f)
		};
		private Vector3 lastPOS = Vector3.zero;
		private Quaternion lastRotation = Quaternion.identity;
		private Transform m_cameraTransform;

		private GameObject m_floatingText;
		private Transform m_floatingText_Transform;
		private TextMesh m_textMesh;
		private TextMeshPro m_textMeshPro;

		private Transform m_transform;

		#endregion
		public bool IsTextObjectScaleStatic;

		public int SpawnType;
		public Font TheFont;
		#region Unity Callbacks

		private void Awake()
		{
			this.m_transform = this.transform;
			this.m_floatingText = new GameObject(this.name + " floating text");

			// Reference to Transform is lost when TMP component is added since it replaces it by a RectTransform.
			//m_floatingText_Transform = m_floatingText.transform;
			//m_floatingText_Transform.position = m_transform.position + new Vector3(0, 15f, 0);

			this.m_cameraTransform = Camera.main.transform;
		}

		private void Start()
		{
			if (this.SpawnType == 0)
			{
				// TextMesh Pro Implementation
				this.m_textMeshPro = this.m_floatingText.AddComponent<TextMeshPro>();
				this.m_textMeshPro.rectTransform.sizeDelta = new Vector2(3, 3);

				this.m_floatingText_Transform = this.m_floatingText.transform;
				this.m_floatingText_Transform.position = this.m_transform.position + new Vector3(0, 15f, 0);

				//m_textMeshPro.fontAsset = Resources.Load("Fonts & Materials/JOKERMAN SDF", typeof(TextMeshProFont)) as TextMeshProFont; // User should only provide a string to the resource.
				//m_textMeshPro.fontSharedMaterial = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(Material)) as Material;

				this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				this.m_textMeshPro.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
				this.m_textMeshPro.fontSize = 24;
				//m_textMeshPro.enableExtraPadding = true;
				//m_textMeshPro.enableShadows = false;
				this.m_textMeshPro.enableKerning = false;
				this.m_textMeshPro.text = string.Empty;
				this.m_textMeshPro.isTextObjectScaleStatic = this.IsTextObjectScaleStatic;

				_ = this.StartCoroutine(this.DisplayTextMeshProFloatingText());
			}
			else if (this.SpawnType == 1)
			{
				//Debug.Log("Spawning TextMesh Objects.");

				this.m_floatingText_Transform = this.m_floatingText.transform;
				this.m_floatingText_Transform.position = this.m_transform.position + new Vector3(0, 15f, 0);

				this.m_textMesh = this.m_floatingText.AddComponent<TextMesh>();
				this.m_textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
				this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
				this.m_textMesh.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
				this.m_textMesh.anchor = TextAnchor.LowerCenter;
				this.m_textMesh.fontSize = 24;

				_ = this.StartCoroutine(this.DisplayTextMeshFloatingText());
			}
			else if (this.SpawnType == 2)
			{

			}

		}

		#endregion
		#region Public Methods

		//void Update()
		//{
		//    if (SpawnType == 0)
		//    {
		//        m_textMeshPro.SetText("{0}", m_frame);
		//    }
		//    else
		//    {
		//        m_textMesh.text = m_frame.ToString();
		//    }
		//    m_frame = (m_frame + 1) % 1000;

		//}

		public IEnumerator DisplayTextMeshProFloatingText()
		{
			float CountDuration = 2.0f;                   // How long is the countdown alive.
			float starting_Count = Random.Range(5f, 20f); // At what number is the counter starting at.
			float current_Count = starting_Count;

			Vector3 start_pos = this.m_floatingText_Transform.position;
			Color32 start_color = this.m_textMeshPro.color;
			float alpha = 255;
			float fadeDuration = 3 / starting_Count * CountDuration;

			while (current_Count > 0)
			{
				current_Count -= Time.deltaTime / CountDuration * starting_Count;

				if (current_Count <= 3)
				{
					//Debug.Log("Fading Counter ... " + current_Count.ToString("f2"));
					alpha = Mathf.Clamp(alpha - Time.deltaTime / fadeDuration * 255, 0, 255);
				}

				int int_counter = (int)current_Count;
				this.m_textMeshPro.text = int_counter.ToString();
				//m_textMeshPro.SetText("{0}", (int)current_Count);

				this.m_textMeshPro.color = new Color32(start_color.r, start_color.g, start_color.b, (byte)alpha);

				// Move the floating text upward each update
				this.m_floatingText_Transform.position += new Vector3(0, starting_Count * Time.deltaTime, 0);

				// Align floating text perpendicular to Camera.
				if (!this.lastPOS.Compare(this.m_cameraTransform.position, 1000) || !this.lastRotation.Compare(this.m_cameraTransform.rotation, 1000))
				{
					this.lastPOS = this.m_cameraTransform.position;
					this.lastRotation = this.m_cameraTransform.rotation;
					this.m_floatingText_Transform.rotation = this.lastRotation;
					Vector3 dir = this.m_transform.position - this.lastPOS;
					this.m_transform.forward = new Vector3(dir.x, 0, dir.z);
				}

				yield return k_WaitForEndOfFrame;
			}

			//Debug.Log("Done Counting down.");

			yield return k_WaitForSecondsRandom[Random.Range(0, 19)];

			this.m_floatingText_Transform.position = start_pos;

			_ = this.StartCoroutine(this.DisplayTextMeshProFloatingText());
		}

		public IEnumerator DisplayTextMeshFloatingText()
		{
			float CountDuration = 2.0f;                   // How long is the countdown alive.
			float starting_Count = Random.Range(5f, 20f); // At what number is the counter starting at.
			float current_Count = starting_Count;

			Vector3 start_pos = this.m_floatingText_Transform.position;
			Color32 start_color = this.m_textMesh.color;
			float alpha = 255;
			float fadeDuration = 3 / starting_Count * CountDuration;

			while (current_Count > 0)
			{
				current_Count -= Time.deltaTime / CountDuration * starting_Count;

				if (current_Count <= 3)
				{
					//Debug.Log("Fading Counter ... " + current_Count.ToString("f2"));
					alpha = Mathf.Clamp(alpha - Time.deltaTime / fadeDuration * 255, 0, 255);
				}

				int int_counter = (int)current_Count;
				this.m_textMesh.text = int_counter.ToString();
				//Debug.Log("Current Count:" + current_Count.ToString("f2"));

				this.m_textMesh.color = new Color32(start_color.r, start_color.g, start_color.b, (byte)alpha);

				// Move the floating text upward each update
				this.m_floatingText_Transform.position += new Vector3(0, starting_Count * Time.deltaTime, 0);

				// Align floating text perpendicular to Camera.
				if (!this.lastPOS.Compare(this.m_cameraTransform.position, 1000) || !this.lastRotation.Compare(this.m_cameraTransform.rotation, 1000))
				{
					this.lastPOS = this.m_cameraTransform.position;
					this.lastRotation = this.m_cameraTransform.rotation;
					this.m_floatingText_Transform.rotation = this.lastRotation;
					Vector3 dir = this.m_transform.position - this.lastPOS;
					this.m_transform.forward = new Vector3(dir.x, 0, dir.z);
				}

				yield return k_WaitForEndOfFrame;
			}

			//Debug.Log("Done Counting down.");

			yield return k_WaitForSecondsRandom[Random.Range(0, 20)];

			this.m_floatingText_Transform.position = start_pos;

			_ = this.StartCoroutine(this.DisplayTextMeshFloatingText());
		}

		#endregion
	}
}
