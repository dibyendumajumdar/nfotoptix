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


using System;

namespace Redukti.Nfotopix
{
    public class Vector2Pair
    {
        public readonly Vector2 v0;
        public readonly Vector2 v1;

        public readonly static Vector2Pair vector2_pair_00 = new Vector2Pair(Vector2.vector2_0, Vector2.vector2_0);

        public Vector2Pair(Vector2 v0, Vector2 b)
        {
            this.v0 = v0;
            this.v1 = b;
        }

        public bool isEquals(Vector2Pair other, double tolerance)
        {
            return v0.isEqual(other.v0, tolerance) && v1.isEqual(other.v1, tolerance);
        }

        public double ln_intersect_ln_scale(Vector2Pair line)
        {
            // based on
            // http://geometryalgorithms.com/Archive/algorithm_0104/algorithm_0104B.htm

            Vector2 w = v0.minus(line.v0);

            double d = v1.x() * line.v1.y() - v1.y() * line.v1.x();

            if (Math.Abs(d) < 1e-10)
                throw new InvalidOperationException("ln_intersect_ln_scale: lines are parallel");

            double s = (line.v1.x() * w.y() - line.v1.y() * w.x()) / d;

            return s;
        }

        public Vector2 ln_intersect_ln(Vector2Pair line)
        {
            return v0.plus(v1.times(ln_intersect_ln_scale(line)));
        }

        /**
         * Create a 2d vector pair and initialize vectors from
         * specified components of vectors from an other pair.
         */
        public static Vector2Pair from(Vector3Pair v, int c0, int c1)
        {
            return new Vector2Pair(Vector2.from(v.v0, c0, c1), Vector2.from(v.v1, c0, c1));
        }

        override public string ToString()
        {
            return "[" + v0.ToString() + "," + v1.ToString() + "]";
        }
    }
}