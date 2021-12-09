using ProgramTervezesMintak.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramTervezesMintak.Entities
{
    public class Present : Toy

    {
        public SolidBrush PresentBrush { get; private set; }
        public Present(Color kivantszin)
        {
            PresentBrush = SolidBrush(kivantszin);
        }
        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(PresentBrush, 0, 0, Width, Height);
        }
    }
}
