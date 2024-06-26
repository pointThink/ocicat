using ocicat.Graphics.Rendering;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
namespace ocicat.Graphics;
using ocicat.Input;

[Obsolete("Use GLFWWindow instead")]
public class OpenTKWindow : Window
{
	private GameWindow _tkWindow;
	private DateTime _frameBeginTime;
	
	public OpenTKWindow(string title, int width, int height, bool fullscreen = false, bool resizable = false, int samples = 0)
	{
		Logging.Log(LogLevel.Ocicat, $"Creating OpenTK window.\n\tTitle: {title}\n\tSize: {width}x{height}\n\tFullscreen: {fullscreen}\n\tResizable: {resizable}\n\tAA Samples: {samples}");
		
		NativeWindowSettings nativeWindowSettings = NativeWindowSettings.Default;
		nativeWindowSettings.APIVersion = new Version(4, 6);
		nativeWindowSettings.API = ContextAPI.OpenGL;
		nativeWindowSettings.NumberOfSamples = samples;
		
		GameWindowSettings gameWindowSettings = new();

		nativeWindowSettings.Title = title;
		nativeWindowSettings.ClientSize = new Vector2i(width, height);
		
		if (!resizable)
		{
			nativeWindowSettings.MinimumClientSize = new Vector2i(width, height);
			nativeWindowSettings.MaximumClientSize = new Vector2i(width, height);
		}
		
		if (fullscreen)
			nativeWindowSettings.WindowState = WindowState.Fullscreen;
		
		_tkWindow = new GameWindow(gameWindowSettings, nativeWindowSettings);
		
		_width = width;
		_height = height;
		
		DeltaTime = 0;

		AASamples = samples;

		_tkWindow.Resize += OnResizeEvent;
	}
	
	private void OnResizeEvent(ResizeEventArgs args)
	{
		if (OnResize != null)
			OnResize(args.Width, args.Height);

		_width = args.Width;
		_height = args.Height;
	}

	private int _width;
	private int _height;

	public override string Title
	{
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public override Image? Icon { get; set; }

	public override int Width
	{
		get => _width;
		set => throw new NotImplementedException();
	}

	public override int Height
	{
		get => _height;
		set => throw new NotImplementedException();
	}
	
	public override bool Resizable 
	{
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}
	
	public override bool Fullscreen
	{
		get => _tkWindow.IsFullscreen;
		set => throw new NotImplementedException();
	}
	
	public override Vector2 MousePosition
	{
		get => new Vector2(_tkWindow.MouseState.X, _height - _tkWindow.MouseState.Y);
		set => throw new NotImplementedException();
	}

	public override bool CursorVisible
	{
		get
		{
			return _tkWindow.Cursor != MouseCursor.Empty;
		}
		set
		{
			if (value)
				_tkWindow.Cursor = MouseCursor.Default;
			else
				_tkWindow.Cursor = MouseCursor.Empty;
		}
	}

	public override bool CursorLocked
	{
		get
		{
			return _tkWindow.CursorState != CursorState.Grabbed;
		}

		set
		{
			if (value)
				_tkWindow.CursorState = CursorState.Grabbed;
			else
				_tkWindow.CursorState = CursorState.Normal;
		}
	}

	public override void HandleEvents()
	{
		_frameBeginTime = DateTime.Now;
		_tkWindow.ProcessEvents(0);
	}

	public override void Present()
	{
		_tkWindow.SwapBuffers();
		DeltaTime = ( (float) (DateTime.Now - _frameBeginTime).TotalMilliseconds ) / 1000;
		Time += DeltaTime;
	}
	
	public override bool ShouldClose()
	{
		return _tkWindow.IsExiting;
	}

	public override bool IsKeyDown(Key key)
	{
		return _tkWindow.IsKeyDown((Keys) key);
	}
	
	public override bool IsKeyPressed(Key key)
	{
		return _tkWindow.IsKeyPressed((Keys) key);
	}
	
	public override bool IsMouseButtonDown(int button)
	{
		return _tkWindow.IsMouseButtonDown(MouseButton.Button1 + button);
	}
	
	public override bool IsMouseButtonPressed(int button)
	{
		return _tkWindow.IsMouseButtonPressed(MouseButton.Button1 + button);
	}

	public override GamePadState GetGamePadState(int gamepad)
	{
		throw new NotImplementedException();
	}
}