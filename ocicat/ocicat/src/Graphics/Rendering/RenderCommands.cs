namespace ocicat.Graphics.Rendering;

public abstract class RenderCommands
{
	public abstract void Init();
	
	public abstract void SetClearColor(float r, float g, float b, float a);
	public abstract void ClearScreen();

	public abstract void DrawArrays(VertexArray vertexArray, int count);
	public abstract void DrawIndexed(VertexArray vertexArray);

	public abstract void ResizeViewport(int width, int height);
	
	public static RenderCommands Create(Renderer renderer)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.RenderCommands();
		}

		throw new ArgumentException("Invalid RenderingApi");
	}
}