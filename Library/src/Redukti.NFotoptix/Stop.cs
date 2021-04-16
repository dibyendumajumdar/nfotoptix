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

public class Stop : Surface {

    double _external_radius;

    public Stop(int id, Vector3Pair p, Transform3 transform, Curve curve, Shape shape): base(id, p, transform, curve, shape) {
        _external_radius = shape.max_radius () * 2.0;
    }

    
    public override string ToString() {
        return "Stop{" +
                base.ToString() +
                ", external_radius=" + _external_radius +
                '}';
    }

    public double get_external_radius() {
        return _external_radius;
    }

    public new class Builder : Surface.Builder {
        
        public override Stop.Builder position(Vector3Pair position) {
            return (Stop.Builder) base.position(position);
        }

        public override Stop.Builder shape(Shape shape) {
            return (Stop.Builder) base.shape(shape);
        }

        public override Stop.Builder curve(Curve curve) {
            return (Stop.Builder) base.curve(curve);
        }

        public override Stop build() {
            return new Stop(_id, _position, _transform, _curve, _shape);
        }
    }
}

}