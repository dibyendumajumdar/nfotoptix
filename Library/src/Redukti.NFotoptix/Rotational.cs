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


    public abstract class Rotational : CurveBase
    {

        /**
         * Get curve sagitta (z) at specified distance from origin.
         * A derived class need only implement this method, in which case
         * derivatives will be computed numerically. Derived classes may of course
         * provide analytic implementations of derivatives.
         *
         * @param s distance from curve origin (0, 0), s = sqrt(x^2 + y^2).
         */
        public abstract double sagitta(double s);

        override public double sagitta(Vector2 xy)
        {
            return sagitta(xy.len());
        }

        public virtual double derivative(double r)
        {
            return rotational_derivative(r);
        }

        protected double rotational_derivative(double r)
        {
            DerivFunction df = (x) => this.sagitta(x);
            DerivResult result = Derivatives.central_derivative(df, r, 1e-4);
            return result.result;
        }

        override public Vector2 derivative(Vector2 xy)
        {
            return rotational_derivative(xy);
        }

        protected Vector2 rotational_derivative(Vector2 xy)
        {
            double r = xy.len();
            if (r == 0)
                return Vector2.vector2_0;
            double p = derivative(r);
            return xy.times(p / r);
        }

        override public Vector3 normal(Vector3 point)
        {
            return rotational_normal(point);
        }

        protected Vector3 rotational_normal(Vector3 point)
        {
            double r = Math.Sqrt(MathUtils.square(point.x()) + MathUtils.square(point.y()));
            if (r == 0)
                return new Vector3(0, 0, -1);
            else
            {
                double p = derivative(r);
                return new Vector3(point.x() * p / r, point.y() * p / r, -1.0).normalize();
            }
        }
    }

}