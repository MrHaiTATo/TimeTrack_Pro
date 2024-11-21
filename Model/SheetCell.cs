using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TimeTrack_Pro.Model
{
    public struct SheetCell
    {
        public string? Text { get; set; }
        public Color Color { get; set; }

        public SheetCell()
        {
            Color = Color.Empty;
        }
    }
}
