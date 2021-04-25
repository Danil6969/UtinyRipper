using System;
using System.Collections.Generic;
using System.Text;
using uTinyRipper.Converters;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes
{
	public sealed class ComputeShader : TextAsset
	{
		public ComputeShader(AssetInfo assetInfo) :
			base(assetInfo)
		{
		}

		public override void Read(AssetReader reader)
		{
			ReadNamedObject(reader);
			m_variants = reader.ReadAssetArray<ComputeShaderVariant>();
			reader.AlignStream();
			string text = "";
			for (int i = 0; i < m_variants.Length; i++)
			{
				text += "Variant " + i + "\n";
				for (int j = 0; j < m_variants[i].Kernels.Count; j++)
					text += "Kernel " + j + "\n" + m_variants[i].Kernels[j].disassemblyTxt + "\n";
				text += "\n";
			}
			Script = Encoding.UTF8.GetBytes(text);
		}
		
		protected override YAMLMappingNode ExportYAMLRoot(IExportContainer container)
		{
			throw new NotSupportedException();
		}

		public IReadOnlyList<ComputeShaderVariant> Variants => m_variants;
		
		private ComputeShaderVariant[] m_variants;
	}
}
