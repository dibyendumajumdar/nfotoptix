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


public class OpticalSurface : Surface {
    protected MaterialBase[] _mat = new MaterialBase[2];

    public OpticalSurface(int id,
                          Vector3Pair p,
                          Transform3 transform,
                          Curve curve,
                          Shape shape,
                          MaterialBase left,
                          MaterialBase right) : base(id, p, transform, curve, shape){
        _mat[0] = left;
        _mat[1] = right;
    }

    public MaterialBase get_material(int i) {
        return _mat[i];
    }

    private string toString(MaterialBase mat)
    {
        return mat == null ? "none" : mat.ToString();
    }

    public override string ToString() {
        return "OpticalSurface{" +
                base.ToString() +
                ", left material=" + toString(_mat[0]) +
                ", right material=" + toString(_mat[1]) +
                '}';
    }

    public new class Builder : Surface.Builder {
        protected MaterialBase _left = Air.air;
        protected MaterialBase _right = Air.air;

        
        public override OpticalSurface.Builder position(Vector3Pair position) {
            return (OpticalSurface.Builder) base.position(position);
        }

        public override OpticalSurface.Builder shape(Shape shape) {
            return (OpticalSurface.Builder) base.shape(shape);
        }

        public override OpticalSurface.Builder curve(Curve curve) {
            return (OpticalSurface.Builder) base.curve(curve);
        }

        public virtual OpticalSurface.Builder leftMaterial(MaterialBase left) {
            this._left = left;
            return this;
        }

        public virtual OpticalSurface.Builder rightMaterial(MaterialBase right) {
            this._right = right;
            return this;
        }

        public override OpticalSurface build() {
            return new OpticalSurface(_id, _position, _transform, _curve, _shape, _left, _right);
        }

    }
}

}