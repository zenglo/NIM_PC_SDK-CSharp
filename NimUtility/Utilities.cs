using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NimUtility
{
    public static class Utilities
    {
        public static string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
