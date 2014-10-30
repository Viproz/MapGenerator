using System;
using Pluton;
using System.Drawing;
using System.Reflection;
//using UnityEngine;

namespace MapGenerator
{
	public class MapGenerator : CSharpPlugin
	{
		float[,] heightmap;
		int heightFactor = 35000;

		public void On_PluginInit()
		{
			Logger.LogError("putain ca marche");
			ServerConsoleCommands.Register("gen")
				.setCallback(genCmd);
		}

		public void genCmd(string[] args)
		{
			TerrainGenerator terrainGen = SingletonComponent<TerrainGenerator>.Instance;

			heightmap = (float[,])typeof(TerrainGenerator).GetField("heightmap", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(terrainGen);
			Logger.Log(heightmap.GetLength(0).ToString() + ":" + heightmap.GetLength(1).ToString());

			//Color32[,] colors = ((TerrainMath)typeof(TerrainGenerator).GetField("terrainMath", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(terrainGen)).colormap;

			//Logger.LogError(colors.GetLength(0).ToString() + colors.GetLength(1).ToString());

			Bitmap image = new Bitmap(2049, 2049);
			for(int i = 0 ; i < 2049 ; i++) {
				for(int j = 0 ; j < 2049 ; j++) {
					if (heightmap[i, j] < 0.5) {
						image.SetPixel(i, j, Color.FromArgb(0, 20, 185));
						continue;
					}

					//image.SetPixel(i, j, Color.FromArgb((int)((heightmap[i, j] - 0.5) * 510), 255 - (int)((heightmap[i, j] - 0.5) * 510), 20));
					image.SetPixel(i, j, Color.FromArgb(Math.Min(getDiffLevel(i, j), 255), 10, 20));
					//image.SetPixel(i, j, System.Drawing.Color.FromArgb(colors[i,j].r, colors[i,j].g, colors[i,j].b));
				}
			}
			image.Save(Plugin.ValidateRelativePath("imgmoy.png"));
		}

		public int getDiffLevel(int x, int y)
		{
			int values = 0;
			float tmp = 0;
			float pointHeight = heightmap[x, y];
			if(x != 0) {
				tmp += Math.Abs(heightmap[x - 1, y] - pointHeight) * (float)heightFactor;
				values++;
			}
			if(x != 2048) {
				tmp += Math.Abs(heightmap[x + 1, y] - pointHeight) * (float)heightFactor;
				values++;
			}
			if(y != 0) {
				tmp += Math.Abs(heightmap[x, y - 1] - pointHeight) * (float)heightFactor;
				values++;
			}
			if(y != 2048) {
				tmp += Math.Abs(heightmap[x, y + 1] - pointHeight) * (float)heightFactor;
				values++;
			}
			if(x == 1000)
				Logger.Log(tmp.ToString());
			int result = (int)(tmp / (float)values);
			if(x == 1000)
				Logger.Log(result.ToString());

			return result;
		}
	}
}

