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


public class Surface : Element {

    readonly Shape shape;
    readonly Curve curve;

    public Surface(int id, Vector3Pair p, Transform3 transform, Curve curve, Shape shape) {
        super(id, p, transform);
        this.curve = curve;
        this.shape = shape;
    }

    public Shape get_shape() {
        return shape;
    }

    public Curve get_curve() {
        return curve;
    }

    public Renderer.Style get_style() {
        return Renderer.Style.StyleSurface;
    }

    public void get_pattern(Consumer<Vector3> f,
                            Distribution d, bool unobstructed) {
        Consumer<Vector2> de = (v2d) -> {
            f.accept(new Vector3(v2d.x(), v2d.y(), curve.sagitta(v2d)));
        };

        // get distribution from shape
        shape.get_pattern(de, d, unobstructed);
    }

    public Vector3Pair get_bounding_box() {
        Vector2Pair sb = shape.get_bounding_box();

        // FIXME we assume curve is symmetric here
        double z = 0;
        double ms = curve.sagitta(new Vector2(shape.max_radius()));
        if (Double.isNaN(ms)) {
            System.err.println("Invalid sagitta at " + shape.max_radius());
            return null;
        }

        if (z > ms) {
            double temp = z;
            z = ms;
            ms = temp;
        }
        return new Vector3Pair(new Vector3(sb.v0.x(), sb.v0.y(), z),
                new Vector3(sb.v1.x(), sb.v1.y(), ms));
    }

    
    public string toString() {
        return super.toString() +
                ", shape=" + shape +
                ", curve=" + curve;
    }

    public static class Builder extends Element.Builder {
        Shape shape;
        Curve curve;

        
        public Surface.Builder position(Vector3Pair position) {
            return (Builder) super.position(position);
        }

        
        public Element build() {
            return new Surface(id, position, transform, curve, shape);
        }

        public Builder shape(Shape shape) {
            this.shape = shape;
            return this;
        }

        public Builder curve(Curve curve) {
            this.curve = curve;
            return this;
        }
    }
}

}