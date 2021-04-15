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


public class Disk : Round {

    double _radius;

    public Disk(double radius): base(false) {
        this._radius = radius;
    }

    
    override public double get_xy_ratio() {
        return 1.0;
    }


        override public double get_external_xradius() {
        return _radius;
    }


        override public double get_internal_xradius() {
        return 0;
    }


        override public bool inside(Vector2 point) {
        return (MathUtils.square(point.x()) + MathUtils.square(point.y())
                <= MathUtils.square(_radius));
    }


        override public double max_radius() {
        return _radius;
    }


        override public double min_radius() {
        return _radius;
    }


        override public double get_outter_radius(Vector2 dir) {
        return _radius;
    }


        override public Vector2Pair get_bounding_box() {
        Vector2 hs = new Vector2(_radius, _radius);
        return new Vector2Pair(hs.negate(), hs);
    }

    
    override public string ToString() {
        return "Disk{" +
                "radius=" + _radius +
                '}';
    }
}

}