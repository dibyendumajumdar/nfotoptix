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
    public class Sphere : ConicBase
    {
        public Sphere(double roc) : base(roc, 0.0)
        {
        }


        override public double sagitta(double r)
        {
            double x = Math.Abs(_roc) - Math.Sqrt(MathUtils.square(_roc) - MathUtils.square(r));
            return _roc < 0 ? -x : x;
        }


        override public double derivative(double r)
        {
            return r / Math.Sqrt(MathUtils.square(_roc) - MathUtils.square(r));
        }

        /*

    ligne AB A + t * B
    sphere passant par C(Cx, 0, 0), rayon R

    d = Ax - R - Cx
    (Bz*t+Az)^2+(By*t+Ay)^2+(Bx*t+d)^2=R^2

    t=-(sqrt((Bz^2+By^2+Bx^2)*R^2+(-Bz^2-By^2)*d^2+(2*Az*Bx*Bz+2*Ay*Bx*By)
    *d-Ay^2*Bz^2+2*Ay*Az*By*Bz-Az^2*By^2+(-Az^2-Ay^2)*Bx^2)+Bx*d+Az*Bz+Ay*By)/(Bz^2+By^2+Bx^2),

    t= (sqrt((Bz^2+By^2+Bx^2)*R^2+(-Bz^2-By^2)*d^2+(2*Az*Bx*Bz+2*Ay*Bx*By)
    *d-Ay^2*Bz^2+2*Ay*Az*By*Bz-Az^2*By^2+(-Az^2-Ay^2)*Bx^2)-Bx*d-Az*Bz-Ay*By)/(Bz^2+By^2+Bx^2)

    */

        override public Vector3 intersect(Vector3Pair ray)
        {
            double ax = (ray.origin().x());
            double ay = (ray.origin().y());
            double az = (ray.origin().z());
            double bx = (ray.direction().x());
            double by = (ray.direction().y());
            double bz = (ray.direction().z());

            // double bz2_by2_bx2 = math::square(bx) + math::square(by) +
            // math::square(bx);
            // == 1.0

            double d = az - _roc;
            double ay_by = ay * by;
            double ax_bx = ax * bx;

            double s = +MathUtils.square(_roc) // * bz2_by2_bx2
                       + 2.0 * (ax_bx + ay_by) * bz * d + 2.0 * ax_bx * ay_by
                       - MathUtils.square(ay * bx) - MathUtils.square(ax * by)
                       - (MathUtils.square(bx) + MathUtils.square(by)) * MathUtils.square(d)
                       - (MathUtils.square(ax) + MathUtils.square(ay)) * MathUtils.square(bz);

            // no sphere/ray colision
            if (s < 0)
                return null;

            s = Math.Sqrt(s);

            // there are 2 possible sphere/line collision point, keep the right
            // one depending on ray direction
            if (_roc * bz > 0)
                s = -s;

            double t = (s - (bz * d + ax_bx + ay_by)); // / bz2_by2_bx2;

            // do not collide if line intersection is before ray start position
            if (t <= 0)
                return null;

            // intersection point
            return ray.origin().plus(ray.direction().times(t));
        }


        override public Vector3 normal(Vector3 point)
        {
            // normalized vector to sphere center
            Vector3 normal = new Vector3(point.x(), point.y(), point.z() - _roc).normalize();
            if (_roc < 0)
                normal = normal.negate();
            return normal;
        }
    }
}