using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace TMPro.Examples
{

	public class Benchmark03 : MonoBehaviour
	{
		#region Enums

		public enum BenchmarkType { TMP_SDF_MOBILE = 0, TMP_SDF__MOBILE_SSD = 1, TMP_SDF = 2, TMP_BITMAP_MOBILE = 3, TEXTMESH_BITMAP = 4 }

		#endregion
		public BenchmarkType Benchmark;

		public int NumberOfSamples = 100;

		public Font SourceFont;
		#region Unity Callbacks

		private void Awake()
		{

		}

		private void Start()
		{
			TMP_FontAsset fontAsset = null;

			// Create Dynamic Font Asset for the given font file.
			switch (this.Benchmark)
			{
				case BenchmarkType.TMP_SDF_MOBILE :
					fontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SDFAA, 256, 256);

					break;
				case BenchmarkType.TMP_SDF__MOBILE_SSD :
					fontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SDFAA, 256, 256);
					fontAsset.material.shader = Shader.Find("TextMeshPro/Mobile/Distance Field SSD");

					break;
				case BenchmarkType.TMP_SDF :
					fontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SDFAA, 256, 256);
					fontAsset.material.shader = Shader.Find("TextMeshPro/Distance Field");

					break;
				case BenchmarkType.TMP_BITMAP_MOBILE :
					fontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SMOOTH, 256, 256);

					break;
			}

			for (int i = 0; i < this.NumberOfSamples; i++)
			{
				switch (this.Benchmark)
				{
					case BenchmarkType.TMP_SDF_MOBILE :
					case BenchmarkType.TMP_SDF__MOBILE_SSD :
					case BenchmarkType.TMP_SDF :
					case BenchmarkType.TMP_BITMAP_MOBILE :
						{
							GameObject go = new GameObject();
							go.transform.position = new Vector3(0, 1.2f, 0);

							TextMeshPro textComponent = go.AddComponent<TextMeshPro>();
							textComponent.font = fontAsset;
							textComponent.fontSize = 128;
							textComponent.text = "@";
							textComponent.alignment = TextAlignmentOptions.Center;
							textComponent.color = new Color32(255, 255, 0, 255);

							if (this.Benchmark == BenchmarkType.TMP_BITMAP_MOBILE)
							{
								textComponent.fontSize = 132;
							}
						}

						break;
					case BenchmarkType.TEXTMESH_BITMAP :
						{
							GameObject go = new GameObject();
							go.transform.position = new Vector3(0, 1.2f, 0);

							TextMesh textMesh = go.AddComponent<TextMesh>();
							textMesh.GetComponent<Renderer>().sharedMaterial = this.SourceFont.material;
							textMesh.font = this.SourceFont;
							textMesh.anchor = TextAnchor.MiddleCenter;
							textMesh.fontSize = 130;

							textMesh.color = new Color32(255, 255, 0, 255);
							textMesh.text = "@";
						}

						break;
				}
			}
		}

		#endregion
	}
}
