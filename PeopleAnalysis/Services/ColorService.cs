using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PeopleAnalysis.Services
{
    public class ColorService
    {
        private readonly Random random = new Random(Environment.TickCount);
        public Color GetRandomColor() => Color.FromArgb(1, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        public List<string> ColorHexList(int count) => Enumerable.Repeat(new Color(), count).Select(x => GetRandomColor().ToHEX()).ToList();
    }

    public static class ColorExtension
    {
        public static string ToHEX(this Color color) => $@"""#{color.R.ToString("X2")}{color.G.ToString("X2")}{color.B.ToString("X2")}""";
    }
}
