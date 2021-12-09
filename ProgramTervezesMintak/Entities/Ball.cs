﻿using ProgramTervezesMintak.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramTervezesMintak.Entities
{
    public class Ball: Toy
    {
        public SolidBrush BallBrush { get; private set; }

        public Ball(Color kivantszin)
        {
            BallBrush = SolidBrush(kivantszin);
        }

        protected override void DrawImage (Graphics g)
        {
            g.FillEllipse(BallBrush, 0, 0, Width, Height);
        }
        
    }
}
