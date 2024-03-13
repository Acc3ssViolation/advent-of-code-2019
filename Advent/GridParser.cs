using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    internal class GridParser
    {
        public List<int> ParseColumns(int startColumn, string row)
        {
            var colCount = 0;
            var colStart = 0;
            var result = new List<int>();
            var wasWhitespace = false;
            for (var i = 0; i < row.Length; i++)
            {
                var isWhitespace = char.IsWhiteSpace(row[i]);
                if (wasWhitespace && !isWhitespace)
                {
                    // Found start of column
                    colStart = i;
                }
                else if (isWhitespace && !wasWhitespace)
                {
                    // Found end of column
                    colCount++;
                    if (colCount > startColumn)
                    {
                        var str = row.AsSpan(colStart, i - colStart);
                        result.Add(int.Parse(str));
                    }
                }
                wasWhitespace = isWhitespace;
            }

            if (!wasWhitespace)
            {
                var str = row.AsSpan(colStart);
                result.Add(int.Parse(str));
            }

            return result;
        }
    }
}
