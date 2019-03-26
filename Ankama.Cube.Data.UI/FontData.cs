using Ankama.AssetManagement.AssetReferences;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Data.UI
{
	[Serializable]
	public class FontData
	{
		[UsedImplicitly]
		[SerializeField]
		private FontLanguage m_fontLanguage;

		[UsedImplicitly]
		[SerializeField]
		private AssetReference m_fontAsset;

		[UsedImplicitly]
		[SerializeField]
		private AssetReference m_styleMaterial;

		[UsedImplicitly]
		[SerializeField]
		[Range(8f, 300f)]
		private float m_defaultFontSize = 32f;

		[UsedImplicitly]
		[SerializeField]
		[Range(8f, 300f)]
		private float m_mobileFontSize = 32f;

		[UsedImplicitly]
		[SerializeField]
		private float m_characterSpacing;

		[UsedImplicitly]
		[SerializeField]
		private float m_wordSpacing;

		[UsedImplicitly]
		[SerializeField]
		private float m_lineSpacing;

		[UsedImplicitly]
		[SerializeField]
		private float m_paragraphSpacing;

		public AssetReference fontAssetReference => m_fontAsset;

		public AssetReference styleMaterialReference => m_styleMaterial;

		public float fontSize => m_defaultFontSize;

		public float characterSpacing => m_characterSpacing;

		public float wordSpacing => m_wordSpacing;

		public float lineSpacing => m_lineSpacing;

		public float paragraphSpacing => m_paragraphSpacing;
	}
}
