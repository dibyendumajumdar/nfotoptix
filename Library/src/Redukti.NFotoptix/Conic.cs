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
namespace Redukti.Nfotopix {

public class Conic : ConicBase {

    public Conic(double roc, double sc) {
        super(roc, sc);
    }

    public double derivative(double r) {
        // conical section differentiate (computed with Maxima)

        const double s2 = _sh * MathUtils.square(r);
        const double s3 = Math.sqrt(1 - s2 / MathUtils.square(_roc));
        const double s4
                = 2.0 / (_roc * (s3 + 1))
                + s2 / (MathUtils.square(_roc) * _roc * s3 * MathUtils.square(s3 + 1));
        return r * s4;
    }

    public double sagitta(double r) {
        return MathUtils.square(r)
                / (_roc
                * (Math.sqrt(1 - (_sh * MathUtils.square(r)) / MathUtils.square(_roc)) + 1));
    }

    
    public Vector3 intersect(Vector3Pair ray) {
        const double ax = ray.origin().x();
        const double ay = ray.origin().y();
        const double az = ray.origin().z();
        const double bx = ray.direction().x();
        const double by = ray.direction().y();
        const double bz = ray.direction().z();

  /*
    find intersection point between conical section and ray,
    telescope optics, page 266
  */
        double a = (_sh * MathUtils.square(bz) + MathUtils.square(by) + MathUtils.square(bx));
        double b = ((_sh * bz * az + by * ay + bx * ax) / _roc - bz) * 2.0;
        double c = (_sh * MathUtils.square(az) + MathUtils.square(ay) + MathUtils.square(ax))
                / _roc
                - 2.0 * az;

        double t;

        if (a == 0) {
            t = -c / b;
        } else {
            double d = MathUtils.square(b) - 4.0 * a * c / _roc;

            if (d < 0)
                return null; // no intersection

            double s = Math.sqrt(d);

            if (a * bz < 0)
                s = -s;

            if (_sh < 0)
                s = -s;

            t = (2 * c) / (s - b);
        }

        if (t <= 0) // ignore intersection if before ray origin
            return null;

        return ray.origin().plus(ray.direction().times(t));
    }

}

}