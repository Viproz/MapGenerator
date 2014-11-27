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
		float[,,] splatmap;
		int heightFactor = 35000;

		System.Drawing.Color nullcolor = System.Drawing.Color.FromArgb(0,0,0,0);

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
			splatmap = (float[,,])typeof(TerrainGenerator).GetField("splatmap", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(terrainGen);

			Logger.Log(heightmap.GetLength(0).ToString() + ":" + heightmap.GetLength(1).ToString());

			//Color32[,] colors = ((TerrainMath)typeof(TerrainGenerator).GetField("terrainMath", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(terrainGen)).colormap;

			//Logger.LogError(colors.GetLength(0).ToString() + colors.GetLength(1).ToString());

			System.Drawing.Color color = nullcolor;
			System.Drawing.Color zero = nullcolor;
			System.Drawing.Color one = nullcolor;
			System.Drawing.Color two = nullcolor;
			System.Drawing.Color three = nullcolor;
			System.Drawing.Color four = nullcolor;
			System.Drawing.Color five = nullcolor;
			System.Drawing.Color six = nullcolor;
			System.Drawing.Color seven = nullcolor;
			System.Drawing.Color eight = nullcolor;

			Bitmap image = new Bitmap(heightmap.GetLength(0), heightmap.GetLength(1));
			for(int i = 0 ; i < heightmap.GetLength(0) ; i++) {
				for(int j = 0 ; j < heightmap.GetLength(1) ; j++) {
					if (heightmap[i, j] < 0.5) {
						image.SetPixel(i, j, Color.FromArgb(0, 20, 185));
						continue;
					}

					if (splatmap [i, j, 0] != 0)
						zero = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splatmap[i, j, 0] + 1) * 127)), System.Drawing.Color.LightGray);
					else
						zero = nullcolor;
					if (splatmap [i, j, 1] != 0)
						one = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splatmap[i, j, 1] + 1) * 127)), System.Drawing.Color.LightGreen);
					else
						one = nullcolor;
					if (splatmap [i, j, 2] != 0)
						two = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splatmap[i, j, 2] + 1) * 127)), System.Drawing.Color.SandyBrown);
					else
						two = nullcolor;
					if (splatmap [i, j, 3] != 0)
						three = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splatmap[i, j, 3] + 1) * 127)), System.Drawing.Color.SaddleBrown);
					else
						three = nullcolor;
					if (splatmap [i, j, 4] != 0)
						four = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splatmap[i, j, 4] + 1) * 127)), System.Drawing.Color.ForestGreen);
					else
						four = nullcolor;
					if (splatmap [i, j, 5] != 0)
						five = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splatmap[i, j, 5] + 1) * 127)), System.Drawing.Color.GhostWhite);
					else
						five = nullcolor;
					if (splatmap [i, j, 6] != 0)
						six = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splatmap[i, j, 6] + 1) * 127)), System.Drawing.Color.White);
					else
						six = nullcolor;
					if (splatmap [i, j, 7] != 0)
						seven = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splatmap[i, j, 7] + 1) * 127)), System.Drawing.Color.Red);
					else
						seven = nullcolor;
					if (splatmap [i, j, 8] != 0)
						eight = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splatmap[i, j, 8] + 1) * 127)), System.Drawing.Color.Blue);
					else
						eight = nullcolor;
					System.Drawing.Color[] colors = new System.Drawing.Color[]{ zero, one, two, three, four, five, six, seven, eight };
					color = CombineColors (colors);
					color = avrgColor(color, Color.FromArgb(Math.Min(getDiffLevel(i, j), 255), 10, 20));

					//image.SetPixel(i, j, Color.FromArgb((int)((heightmap[i, j] - 0.5) * 510), 255 - (int)((heightmap[i, j] - 0.5) * 510), 20));
					image.SetPixel(i, j, color);
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

		public System.Drawing.Color CombineColors(System.Drawing.Color[] colors)
		{
			System.Drawing.Color result = nullcolor;
			foreach (System.Drawing.Color color in colors) {
				result = avrgColor(color, result);
			}
			return result;
		}

		public System.Drawing.Color avrgColor(System.Drawing.Color c1, System.Drawing.Color c2)
		{
			if (c1.Equals (nullcolor))
				return c2;
			if (c2.Equals (nullcolor))
				return c1;
			return System.Drawing.Color.FromArgb(avrg(c1.A, c2.A), avrg(c1.R, c2.R), avrg(c1.G, c2.G), avrg(c1.B, c2.B));
		}

		public int avrg(byte a, byte b)
		{
			return (System.Convert.ToInt32(a) + System.Convert.ToInt32(b)) / 2;
		}
	}
}

