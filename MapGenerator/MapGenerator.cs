using System;
using System.Text;
using System.IO;
using Pluton;
using System.Drawing;
using System.Reflection;
using UnityEngine;

namespace MapGenerator
{
	public class MapGenerator : CSharpPlugin
	{
		public System.Drawing.Color nullcolor = System.Drawing.Color.FromArgb(0,0,0,0);

		public void On_PluginInit()
		{
			ServerConsoleCommands.Register("gen")
				.setCallback(genCmd);
		}

		public void genCmd(string[] args)
		{
			TerrainGenerator terrainGen = SingletonComponent<TerrainGenerator>.Instance;

			float[,,] splat = (float[,,])typeof(TerrainGenerator).GetField("splatmap", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(terrainGen);

			Bitmap pic = new Bitmap(2049, 2049);
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
			Debug.Log(splat.GetLength(0) + ":" + splat.GetLength(1) + " -> " + splat.GetLength(2));
			for (int i = 0; i < splat.GetLength(0); i++) {
				for (int j = 0; j < splat.GetLength(1); j++) {
					if (splat [i, j, 0] != 0)
						zero = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splat[i, j, 0] + 1) * 127)), System.Drawing.Color.LightGray);
					else
						zero = nullcolor;
					if (splat [i, j, 1] != 0)
						one = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splat[i, j, 1] + 1) * 127)), System.Drawing.Color.LightGreen);
					else
						one = nullcolor;
					if (splat [i, j, 2] != 0)
						two = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splat[i, j, 2] + 1) * 127)), System.Drawing.Color.SandyBrown);
					else
						two = nullcolor;
					if (splat [i, j, 3] != 0)
						three = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splat[i, j, 3] + 1) * 127)), System.Drawing.Color.SaddleBrown);
					else
						three = nullcolor;
					if (splat [i, j, 4] != 0)
						four = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splat[i, j, 4] + 1) * 127)), System.Drawing.Color.ForestGreen);
					else
						four = nullcolor;
					if (splat [i, j, 5] != 0)
						five = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splat[i, j, 5] + 1) * 127)), System.Drawing.Color.GhostWhite);
					else
						five = nullcolor;
					if (splat [i, j, 6] != 0)
						six = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splat[i, j, 6] + 1) * 127)), System.Drawing.Color.White);
					else
						six = nullcolor;
					if (splat [i, j, 7] != 0)
						seven = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splat[i, j, 7] + 1) * 127)), System.Drawing.Color.Red);
					else
						seven = nullcolor;
					if (splat [i, j, 8] != 0)
						eight = System.Drawing.Color.FromArgb((int)Math.Floor((decimal)((splat[i, j, 8] + 1) * 127)), System.Drawing.Color.Blue);
					else
						eight = nullcolor;
					System.Drawing.Color[] colors = new System.Drawing.Color[]{ zero, one, two, three, four, five, six, seven, eight };
					color = CombineColors (colors);
					pic.SetPixel(i, j, color);
				}
			}
			string path = Path.Combine (Util.GetRootFolder (), "map_" + server.seed + ".png");
			if (File.Exists(path))
				File.Delete(path);
			pic.Save(path, System.Drawing.Imaging.ImageFormat.Png);
		}

		public System.Drawing.Color CombineColors(System.Drawing.Color[] colors)
		{
			System.Drawing.Color result = nullcolor;
			foreach (System.Drawing.Color color in colors) {
				result = AvrgColor(color, result);
			}
			return result;
		}

		public System.Drawing.Color AvrgColor(System.Drawing.Color c1, System.Drawing.Color c2)
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

