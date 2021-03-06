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
    public class Surface : Element
    {
        protected readonly Shape _shape;
        protected readonly Curve _curve;

        public Surface(int id, Vector3Pair p, Transform3 transform, Curve curve, Shape shape) : base(id, p, transform)
        {
            this._curve = curve;
            this._shape = shape;
        }

        public Shape get_shape()
        {
            return _shape;
        }

        public Curve get_curve()
        {
            return _curve;
        }

        public Renderer.Style get_style()
        {
            return Renderer.Style.StyleSurface;
        }

        public void get_pattern(ConsumerVector3 f,
            Distribution d, bool unobstructed)
        {
            PatternConsumer de = (v2d) => { f(new Vector3(v2d.x(), v2d.y(), _curve.sagitta(v2d))); };

            // get distribution from shape
            _shape.get_pattern(de, d, unobstructed);
        }

        public override Vector3Pair get_bounding_box()
        {
            Vector2Pair sb = _shape.get_bounding_box();

            // FIXME we assume curve is symmetric here
            double z = 0;
            double ms = _curve.sagitta(new Vector2(_shape.max_radius()));
            if (Double.IsNaN(ms))
            {
                Console.WriteLine("Invalid sagitta at " + _shape.max_radius());
                return null;
            }

            if (z > ms)
            {
                double temp = z;
                z = ms;
                ms = temp;
            }

            return new Vector3Pair(new Vector3(sb.v0.x(), sb.v0.y(), z),
                new Vector3(sb.v1.x(), sb.v1.y(), ms));
        }


        public override string ToString()
        {
            return base.ToString() +
                   ", shape=" + _shape +
                   ", curve=" + _curve;
        }

        public new class Builder : Element.Builder
        {
            protected Shape _shape;
            protected Curve _curve;


            public override Surface.Builder position(Vector3Pair position)
            {
                return (Builder) base.position(position);
            }


            public override Element build()
            {
                return new Surface(_id, _position, _transform, _curve, _shape);
            }

            public virtual Builder shape(Shape shape)
            {
                this._shape = shape;
                return this;
            }

            public virtual Builder curve(Curve curve)
            {
                this._curve = curve;
                return this;
            }
        }
    }
}