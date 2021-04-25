using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DotNetDxc;

namespace uTinyRipper.Classes
{
	public struct ComputeShaderKernel : IAssetReadable
	{
		public void Read(AssetReader reader)
		{
			name = reader.ReadString();
			// TODO cbs
			reader.ReadInt32();
			// TODO textures
			reader.ReadInt32();
			// TODO builtinSamplers
			reader.ReadInt32();
			// TODO inBuffers
			reader.ReadInt32();
			m_outBuffers = reader.ReadAssetArray<ComputeShaderResource>();
			code = reader.ReadByteArray();
			disassemblyTxt = D3DCompiler.D3DCompiler.DisassembleToText(code);
			threadGroupSize = reader.ReadInt32Array();
		}



		public string name { get; private set; }
		public byte[] code { get; private set; }
		public string disassemblyTxt { get; private set; }
		public int[] threadGroupSize { get; private set; }

		public IReadOnlyList<ComputeShaderResource> OutBuffers => m_outBuffers;

		private ComputeShaderResource[] m_outBuffers;
	}
}
