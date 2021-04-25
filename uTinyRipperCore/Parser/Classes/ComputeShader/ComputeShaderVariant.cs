using System.Collections.Generic;

namespace uTinyRipper.Classes
{
	public struct ComputeShaderVariant : IAssetReadable
	{
		public void Read(AssetReader reader)
		{
			targetRenderer = reader.ReadInt32();
			targetLevel = reader.ReadInt32();
			m_kernels = reader.ReadAssetArray<ComputeShaderKernel>();
			// TODO constantBuffers
			reader.ReadInt32();
			reader.ReadBoolean();
		}
		
		public int targetRenderer { get; private set; }
		public int targetLevel { get; private set; }

		public IReadOnlyList<ComputeShaderKernel> Kernels => m_kernels;
		
		private ComputeShaderKernel[] m_kernels;
	}
}
