/*
The software is ported from Goptical, hence is licensed under the GPL.
Copyright (c) 2021 Dibyendu Majumdar

Original GNU Optical License and Authors are as follows:

      The Goptical library is free software; you can redistribute it
      and/or modify it under the terms of the GNU General Public
      License as published by the Free Software Foundation; either
      version 3 of the License, or (at your option) any later version.

      The Goptical library is distributed in the hope that it will be
      useful, but WITHOUT ANY WARRANTY; without even the implied
      warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
      See the GNU General Public License for more details.

      You should have received a copy of the GNU General Public
      License along with the Goptical library; if not, write to the
      Free Software Foundation, Inc., 59 Temple Place, Suite 330,
      Boston, MA 02111-1307 USA

      Copyright (C) 2010-2011 Free Software Foundation, Inc
      Author: Alexandre Becoulet
 */


namespace Redukti.Nfotopix
{
    public class Rgb
    {
        public readonly double r;
        public readonly double g;
        public readonly double b;
        public readonly double a;

        public Rgb(double red, double green, double blue, double alpha)
        {
            this.r = red;
            this.g = green;
            this.b = blue;
            this.a = alpha;
        }

        public Rgb negate()
        {
            return new Rgb(1.0 - r, 1.0 - g, 1.0 - b, a);
        }

        public static readonly Rgb rgb_black = new Rgb(0.0f, 0.0f, 0.0f, 1.0f);
        public static readonly Rgb rgb_red = new Rgb(1.0f, 0.0f, 0.0f, 1.0f);
        public static readonly Rgb rgb_green = new Rgb(0.0f, 1.0f, 0.0f, 1.0f);
        public static readonly Rgb rgb_blue = new Rgb(0.0f, 0.0f, 1.0f, 1.0f);
        public static readonly Rgb rgb_yellow = new Rgb(1.0f, 1.0f, 0.0f, 1.0f);
        public static readonly Rgb rgb_cyan = new Rgb(0.0f, 1.0f, 1.0f, 1.0f);
        public static readonly Rgb rgb_magenta = new Rgb(1.0f, 0.0f, 1.0f, 1.0f);
        public static readonly Rgb rgb_gray = new Rgb(0.5f, 0.5f, 0.5f, 1.0f);
        public static readonly Rgb rgb_white = new Rgb(1.0f, 1.0f, 1.0f, 1.0f);
    }
}