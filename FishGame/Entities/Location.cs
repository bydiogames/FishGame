using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.Entities
{
    [Flags]
    public enum Location
    {
        None = 0,
        Pond = 1,
        River = 2,
        Ocean = 4,
    }

    public static class LocationUtils
    {
        private static readonly string[] ReadableLocationNames = new string[]
        {
            "Pond",
            "River",
            "Ocean"
        };

        public static string GetName(this Location location)
        {
            return ReadableLocationNames[BitOperations.Log2((uint)location)];
        }
    }
}
