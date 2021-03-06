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
    public class Image : Surface
    {
        public Image(int id, Vector3Pair p, Transform3 transform, Curve curve, Shape shape) : base(id, p, transform,
            curve, shape)
        {
        }


        public override string ToString()
        {
            return "Image{" + base.ToString() + "}";
        }

        public new class Builder : Surface.Builder
        {
            public override Image.Builder position(Vector3Pair position)
            {
                return (Image.Builder) base.position(position);
            }

            public override Image.Builder shape(Shape shape)
            {
                return (Image.Builder) base.shape(shape);
            }

            public override Image.Builder curve(Curve curve)
            {
                return (Image.Builder) base.curve(curve);
            }

            public override Image build()
            {
                return new Image(_id, _position, _transform, _curve, _shape);
            }
        }
    }
}