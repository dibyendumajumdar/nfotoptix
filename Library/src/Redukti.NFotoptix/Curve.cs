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

    public interface Curve
    {
        /** Get curve sagitta (z) at specified point */
        public double sagitta(Vector2 xy);

        /** Get curve dz/dx and dx/dy partial derivatives (gradient) at specified
         * point */
        public Vector2 derivative(Vector2 xy);

        /** Get intersection point between curve and 3d ray. Return
         false if no intersection occurred. ray must have a position vector and
         direction vector (cosines). */
        public Vector3 intersect(Vector3Pair ray);

        /** Get normal to curve surface at specified point. */
        public Vector3 normal(Vector3 point);
    }

}