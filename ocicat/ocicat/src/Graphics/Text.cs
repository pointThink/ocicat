using System.Runtime.InteropServices;
using System.Text;
using FreeTypeSharp.Native;
using ocicat.Graphics.Rendering;
using OpenTK.Mathematics;
namespace ocicat.Graphics;

/// <summary>
/// A single character from the font. 
/// </summary>
public class FontGlyph
{
	public FontGlyph(Texture texture, int sizeX, int sizeY, int bearingX, int bearingY, uint advance, uint fontSize)
	{
		Texture = texture;
		SizeX = sizeX;
		SizeY = sizeY;
		BearingX = bearingX;
		BearingY = bearingY;
		Advance = advance;
		FontSize = fontSize;
	}
	
	public readonly Texture Texture;

	public readonly int SizeX;
	public readonly int SizeY;
	public readonly int BearingX;
	public readonly int BearingY;
	public readonly uint Advance;
	public readonly uint FontSize;
}

public unsafe class Font
{
	public int SpaceSize = 10;
	
	private readonly Dictionary<uint, FontGlyph> _glyphs = new();
	public readonly uint FontSize;

	public Font(Renderer renderer, string path, uint size)
	{
		IntPtr freetype;
		FT.FT_Init_FreeType(out freetype);
		
		FT.FT_New_Face(freetype, path, 0, out IntPtr tempFace);
		FT_FaceRec* freetypeFace = (FT_FaceRec*)tempFace;
		
		FT.FT_Set_Pixel_Sizes((IntPtr)freetypeFace, 0, size);

		FT.FT_Library_SetLcdFilter(freetype, FT_LcdFilter.FT_LCD_FILTER_MAX);

		FontSize = size;
		
		// Load all glyphs
		for (uint ch = 0; ch < 128; ch++)
		{
			if (FT.FT_Load_Char((IntPtr)freetypeFace, ch, FT.FT_LOAD_RENDER) == FT_Error.FT_Err_Ok)
			{
				FT_Error error = FT.FT_Render_Glyph((IntPtr)freetypeFace->glyph, FT_Render_Mode.FT_RENDER_MODE_MAX);

				if (error != FT_Error.FT_Err_Ok)
				{
					Logging.Log(LogLevel.Error, $"Error loading font, error code {error}");
					continue;
				}

				if (freetypeFace->glyph->bitmap.buffer == IntPtr.Zero)
				{
					Logging.Log(LogLevel.Warning,
						$"_freetypeFace->glyph->bitmap.buffer for character {ch} is null but Freetype didn't report any errors");
					continue;
				}

				Vector2i charSize = new Vector2i((int)freetypeFace->glyph->bitmap.width,
					(int)freetypeFace->glyph->bitmap.rows);
				Vector2i bearing = new Vector2i((int)freetypeFace->glyph->bitmap_left,
					(int)freetypeFace->glyph->bitmap_top);
				uint advance = (uint)freetypeFace->glyph->metrics.horiAdvance >> 6;

				int dataSize = charSize.X * charSize.Y;

				byte[] managedArray = new byte[dataSize];
				Marshal.Copy(freetypeFace->glyph->bitmap.buffer, managedArray, 0, dataSize);
				Texture texture = Texture.Create(renderer, managedArray, charSize.X, charSize.Y, TextureFilter.Linear,
					1);

				_glyphs.Add(ch, new FontGlyph(texture, charSize.X, charSize.Y, bearing.X, bearing.Y, advance, size));
			}
		}

		FT.FT_Done_Face((IntPtr)freetypeFace);
		FT.FT_Done_Library(freetype);
	}

	/// <param name="glyph">The ASCII representation of the characters.</param>
	/// <returns>A glyph</returns>
	public FontGlyph GetGlyph(uint glyph)
	{
		return _glyphs[glyph];
	}

	public float GetStringSize(string text)
	{
		
		byte[] characters = Encoding.ASCII.GetBytes(text);
		Vector2 currentPosition = new Vector2();
		
		foreach (byte character in characters)
		{
			if (character == 32) // space
				currentPosition.X += SpaceSize;
			else if (character == 9) // tab
				currentPosition.X += SpaceSize * 4;
			else
			{
				FontGlyph glyph = GetGlyph(character);
				currentPosition.X += glyph.Advance;
			}
		}
		
		return currentPosition.X;
	}
}