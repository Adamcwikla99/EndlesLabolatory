using UnityEngine;

namespace TMPro.Examples
{

	public class ObjectSpin : MonoBehaviour
	{
#pragma warning disable 0414

		public float SpinSpeed = 5;
		public int RotationRange = 15;
		private Transform m_transform;

		private float m_time;
		private Vector3 m_prevPOS;
		private Vector3 m_initial_Rotation;
		private Vector3 m_initial_Position;
		private Color32 m_lightColor;
		private int frames;

		public enum MotionType { Rotation, BackAndForth, Translation }
		public MotionType Motion;

		private void Awake()
		{
			this.m_transform = this.transform;
			this.m_initial_Rotation = this.m_transform.rotation.eulerAngles;
			this.m_initial_Position = this.m_transform.position;

			Light light = this.GetComponent<Light>();
			this.m_lightColor = light != null ? light.color : Color.black;
		}

		// Update is called once per frame
		private void Update()
		{
			if (this.Motion == MotionType.Rotation)
			{
				this.m_transform.Rotate(0, this.SpinSpeed * Time.deltaTime, 0);
			}
			else if (this.Motion == MotionType.BackAndForth)
			{
				this.m_time += this.SpinSpeed * Time.deltaTime;
				this.m_transform.rotation = Quaternion.Euler(this.m_initial_Rotation.x, Mathf.Sin(this.m_time) * this.RotationRange + this.m_initial_Rotation.y, this.m_initial_Rotation.z);
			}
			else
			{
				this.m_time += this.SpinSpeed * Time.deltaTime;

				float x = 15 * Mathf.Cos(this.m_time * .95f);
				float y = 10; // *Mathf.Sin(m_time * 1f) * Mathf.Cos(m_time * 1f);
				float z = 0f; // *Mathf.Sin(m_time * .9f);    

				this.m_transform.position = this.m_initial_Position + new Vector3(x, z, y);

				// Drawing light patterns because they can be cool looking.
				//if (frames > 2)
				//    Debug.DrawLine(m_transform.position, m_prevPOS, m_lightColor, 100f);

				this.m_prevPOS = this.m_transform.position;
				this.frames += 1;
			}
		}
	}
}
