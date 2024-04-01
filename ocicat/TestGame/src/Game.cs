﻿using ocicat;
using ocicat.Audio;
using ocicat.Graphics;	

namespace TestGame;

public class InGame : GameState
{
	public override void Draw()
	{
		Game.Renderer.DrawCircle(new Vector2(400, 300), 128, 64, Color.Yellow);
	}
}

public static class Program
{
	public static void Main(string[] args)
	{
		Game.Create("Test game", 800, 600);
		
		Sound sound = new Sound(Game.AudioEngine, "erro.ogg");
		Game.AudioEngine.PlaySound(sound);
		
		Game.ClearColor = Color.CreateFloat(0.2f, 0.2f, 0.2f, 1);
		Game.GameState = new InGame();
		
		Game.Run();
	}
}