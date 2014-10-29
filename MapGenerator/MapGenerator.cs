using System;
using Pluton;
using System.Drawing;
using System.Reflection;
//using UnityEngine;

namespace MapGenerator
{
	public class MapGenerator : CSharpPlugin
	{
		public void On_PluginInit()
		{
			Logger.LogError("putain ca marche");
			ServerConsoleCommands.Register("gen")
				.setCallback(genCmd);
		}

		public void genCmd(string[] args)
		{
			TerrainGenerator terrainGen = SingletonComponent<TerrainGenerator>.Instance;

			float[,] heightmap = (float[,])typeof(TerrainGenerator).GetField("heightmap", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(terrainGen);
			Logger.Log(heightmap.GetLength(0).ToString() + ":" + heightmap.GetLength(1).ToString());

			//Color32[,] colors = ((TerrainMath)typeof(TerrainGenerator).GetField("terrainMath", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(terrainGen)).colormap;

			//Logger.LogError(colors.GetLength(0).ToString() + colors.GetLength(1).ToString());

			Bitmap image = new Bitmap(2048, 2048);
			for(int i = 0 ; i < 2049 ; i++) {
				if (i == 2048)
					continue;
				for(int j = 0 ; j < 2049 ; j++) {
					if (j == 2048)
						continue;
					if (i == 1500 && j % 100 == 0)
						Logger.Log(Math.Min(Math.Abs(heightmap[i, j] - heightmap[i, j + 1]) * 35000, 255).ToString());
					if(heightmap[i, j] < 0.5)
						image.SetPixel(i, j, Color.FromArgb(0, 20, 185));
					else
						//image.SetPixel(i, j, Color.FromArgb((int)((heightmap[i, j] - 0.5) * 510), 255 - (int)((heightmap[i, j] - 0.5) * 510), 20));
						image.SetPixel(i, j, Color.FromArgb((int)(Math.Min(Math.Abs(heightmap[i, j] - heightmap[i, j + 1]) * 35000, 255)), 10, 20));
					//image.SetPixel(i, j, System.Drawing.Color.FromArgb(colors[i,j].r, colors[i,j].g, colors[i,j].b));
				}
			}
			image.Save(Plugin.ValidateRelativePath("img.png"));
		}
	}
}

