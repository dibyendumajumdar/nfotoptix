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


public class MirrorSurface : OpticalSurface {

    public MirrorSurface(int id, Vector3Pair p, Transform3 transform, Curve curve, Shape shape, MaterialBase left, MaterialBase right): base(id, p, transform, curve, shape, left, right) {
    }

    public new class Builder : OpticalSurface.Builder {

        bool _light_from_left = true;

        public Builder(bool _light_from_left) {
            this._light_from_left = _light_from_left;
            this._left = Mirror.mirror;
            this._right = null; /* none */
        }

        
        public override MirrorSurface.Builder position(Vector3Pair position) {
            return (MirrorSurface.Builder) base.position(position);
        }

        
        public override MirrorSurface.Builder shape(Shape shape) {
            return (MirrorSurface.Builder) base.shape(shape);
        }

        
        public override MirrorSurface.Builder curve(Curve curve) {
            return (MirrorSurface.Builder) base.curve(curve);
        }

        
        public override MirrorSurface.Builder leftMaterial(MaterialBase left) {
            return (MirrorSurface.Builder)base.leftMaterial(left);
        }
        public MirrorSurface.Builder metal(MaterialBase left) {
            return (MirrorSurface.Builder)base.leftMaterial(left);
        }

        
        public override MirrorSurface.Builder rightMaterial(MaterialBase right) {
            return (MirrorSurface.Builder) base.rightMaterial(right);
        }
        public MirrorSurface.Builder air(MaterialBase right) {
            return (MirrorSurface.Builder) base.rightMaterial(right);
        }

        public MirrorSurface.Builder light_from_left() {
            this._light_from_left = true;
            return this;
        }

        public MirrorSurface.Builder light_from_right() {
            this._light_from_left = false;
            return this;
        }

        MaterialBase metal() {
            return this._left;
        }
        MaterialBase air() {
            return this._right;
        }

        
        public override OpticalSurface build() {
            return new MirrorSurface(_id, _position, _transform, _curve, _shape,
                    _light_from_left ?  air() : metal(),
                    _light_from_left ? metal() : air() );
        }
    }
}

}