using System.Collections;
using TMPro;
using UnityEngine;

public class EnvMapAnimator : MonoBehaviour
{
	#region Private Fields

	private Material m_material;
	private TMP_Text m_textMeshPro;

	#endregion

	//private Vector3 TranslationSpeeds;
	public Vector3 RotationSpeeds;
	#region Unity Callbacks

	private void Awake()
	{
		//Debug.Log("Awake() on Script called.");
		this.m_textMeshPro = this.GetComponent<TMP_Text>();
		this.m_material = this.m_textMeshPro.fontSharedMaterial;
	}

	// Use this for initialization
	private IEnumerator Start()
	{
		Matrix4x4 matrix = new Matrix4x4();

		while (true)
		{
			//matrix.SetTRS(new Vector3 (Time.time * TranslationSpeeds.x, Time.time * TranslationSpeeds.y, Time.time * TranslationSpeeds.z), Quaternion.Euler(Time.time * RotationSpeeds.x, Time.time * RotationSpeeds.y , Time.time * RotationSpeeds.z), Vector3.one);
			matrix.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * this.RotationSpeeds.x, Time.time * this.RotationSpeeds.y, Time.time * this.RotationSpeeds.z), Vector3.one);

			this.m_material.SetMatrix("_EnvMatrix", matrix);

			yield return null;
		}
	}

	#endregion
}
