using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using uTinyRipper;
using uTinyRipper.Classes;
using uTinyRipper.Classes.Shaders;
using uTinyRipper.Converters;
using uTinyRipper.Converters.Shaders;
using uTinyRipper.Project;
using uTinyRipper.SerializedFiles;

using Object = uTinyRipper.Classes.Object;
using Version = uTinyRipper.Version;

namespace uTinyRipperGUI.Exporters
{
	public sealed class ShaderAssetExporter : IAssetExporter
	{
		public bool IsHandle(Object asset, ExportOptions options)
		{
			return true;
		}

		public bool Export(IExportContainer container, Object asset, string path)
		{
			Shader shader = (Shader)asset;
			using (Stream fileStream = FileUtils.CreateVirtualFile(path))
			{
				shader.ExportBinary(container, fileStream, ShaderExporterInstantiator);
			}

			if (shader.Blobs.Length != 1) return true;
			int index = path.LastIndexOf(".");
			string subPath = path.Substring(0,index);
			if (!DirectoryUtils.Exists(subPath))
			{
				DirectoryUtils.CreateVirtualDirectory(subPath);
			}

			ShaderSubProgram[] subprograms = shader.Blobs[0].SubPrograms;
			for (int i = 0; i < subprograms.Length; ++i)
			{
				string compiledPath = Path.Combine(subPath, $"Subprogram{i:D}.o");
				ShaderSubProgram subprogram = subprograms[i];
				byte[] bytes = subprogram.ProgramData;
				char[] chars = new char[bytes.Length];
				for (int j = 0; j < bytes.Length; ++j)
				{
					chars[j] = (char)bytes[j];
				}
				using (Stream fileStream = FileUtils.CreateVirtualFile(compiledPath))
				{
					using (StreamWriter writer = new InvariantStreamWriter(fileStream, new UTF8Encoding(false)))
					{
						writer.Write(chars);
					}
				}
			}
			return true;
		}

		public void Export(IExportContainer container, Object asset, string path, Action<IExportContainer, Object, string> callback)
		{
			Export(container, asset, path);
			callback?.Invoke(container, asset, path);
		}

		public bool Export(IExportContainer container, IEnumerable<Object> assets, string path)
		{
			throw new NotSupportedException();
		}

		public void Export(IExportContainer container, IEnumerable<Object> assets, string path, Action<IExportContainer, Object, string> callback)
		{
			throw new NotSupportedException();
		}

		public IExportCollection CreateCollection(VirtualSerializedFile virtualFile, Object asset)
		{
			return new AssetExportCollection(this, asset);
		}

		public AssetType ToExportType(Object asset)
		{
			ToUnknownExportType(asset.ClassID, out AssetType assetType);
			return assetType;
		}

		public bool ToUnknownExportType(ClassIDType classID, out AssetType assetType)
		{
			assetType = AssetType.Meta;
			return true;
		}

		private static ShaderTextExporter ShaderExporterInstantiator(Version version, GPUPlatform graphicApi)
		{
			switch (graphicApi)
			{
				case GPUPlatform.d3d9:
					return new ShaderDXExporter(graphicApi);

				case GPUPlatform.d3d11_9x:
				case GPUPlatform.d3d11:
					return new ShaderDXExporter(graphicApi);

				case GPUPlatform.vulkan:
					return new ShaderVulkanExporter();

				default:
					return Shader.DefaultShaderExporterInstantiator(version, graphicApi);
			}
		}

	}
}
