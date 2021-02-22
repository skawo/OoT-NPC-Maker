using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public static class StringExt
    {
        public static bool IsNumeric(this string text) => double.TryParse(text, out _);

    }
}
