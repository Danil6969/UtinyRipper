namespace uTinyRipper.Classes
{
	public struct ComputeShaderResource : IAssetReadable
	{
		public void Read(AssetReader reader)
		{
			name = reader.ReadString();
			generatedName = reader.ReadString();
			bindPoint = reader.ReadInt32();
			//ComputeBufferCounter
			reader.ReadInt32();
			reader.ReadInt32();
		}
		
		public string name { get; private set; }
		public string generatedName { get; private set; }
		public int bindPoint { get; private set; }
	}
}
