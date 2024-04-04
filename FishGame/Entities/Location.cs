using System;
using System.Collections.Generic;
using System.Linq;
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
}
