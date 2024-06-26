using OpenTK.Graphics.OpenGL4;

namespace ocicat.Graphics.Rendering.OpenGl;

public class VertexBuffer : Rendering.VertexBuffer
{
	private int _handle;
	
	public VertexBuffer(float[] data)
	{
		_handle = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
		GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		
		Logging.Log(LogLevel.Ocicat, $"Created OpenGL VertexBuffer at location {_handle}");
	}

	~VertexBuffer()
	{
		GL.DeleteBuffer(_handle);
		Logging.Log(LogLevel.Ocicat, $"Disposed OpenGL VertexArray with handle {_handle}");
	}

	public override void Bind()
	{
		GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
	}

	public override void Unbind()
	{
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	}
}

public class IndexBuffer : Rendering.IndexBuffer
{
	private int _handle;
	private uint _indexCount;
	
	public IndexBuffer(uint[] data)
	{
		_handle = GL.GenBuffer();
		_indexCount = (uint) data.Length;
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
		GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(uint), data, BufferUsageHint.StaticDraw);
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		
		Logging.Log(LogLevel.Ocicat, $"Created OpenGL IndexBuffer at location {_handle}");
	}

	~IndexBuffer()
	{
		GL.DeleteBuffer(_handle);
		Logging.Log(LogLevel.Ocicat, $"Disposed OpenGL IndexArray with handle {_handle}");
	}
	
	public override void Bind()
	{
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
	}

	public override void Unbind()
	{
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	}

	public override uint GetIndexCount()
	{
		return _indexCount;
	}
}