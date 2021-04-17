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
using System.Collections.Generic;

namespace Redukti.Nfotopix {

public abstract class Element {

    protected readonly int _id;
    protected readonly Vector3Pair _position;
    protected readonly Transform3 _transform;
    protected OpticalSystem _system;

    public Element(int id, Vector3Pair p, Transform3 transform) {
        this._id = id;
        this._position = p;
        this._transform = transform;
    }

    public virtual Vector3 local_position() {
        return this._transform.translation;
    }

    public int id() {
        return _id;
    }

    public virtual Vector3Pair get_bounding_box() {
        return new Vector3Pair(Vector3.vector3_0, Vector3.vector3_0);
    }

    public Transform3 get_transform() {
        return _transform;
    }

    public Transform3 get_transform_to(Element e) {
        return e != null
                ? _system.get_transform(this, e)
                : _system.get_global_transform(this);
    }

    public Transform3 get_global_transform ()
    {
        return _system.get_global_transform (this);
    }

//    public Transform3 get_local_transform ()
//    {
//        assert (_system != null);
//        return _system.get_local_transform (this);
//    }

    public virtual void set_system(OpticalSystem system) {
        this._system = system;
    }

    public virtual Vector3 get_position (Element e)
    {
        return _system.get_transform (this, e).transform (Vector3.vector3_0);
    }

    public virtual Vector3 get_position ()
    {
        return _system.get_global_transform (this).transform (Vector3.vector3_0);
    }

    
    public override string ToString() {
        return "id=" + _id +
                ", position=" + _position +
                ", transform=" + _transform;
    }

    public static Vector3Pair get_bounding_box(List<Element> elementList) {
        Vector3 a = new Vector3(Double.MaxValue);
        Vector3 b = new Vector3(Double.MinValue);

        foreach (Element e in elementList) {
            Vector3Pair bi = e.get_bounding_box();
            if (bi == null) // FIXME - this is a temp solution to failure
                continue;

            if (bi.v0 == bi.v1)
                continue;

            bi = e.get_transform().transform_pair(bi);

            for (int j = 0; j < 3; j++) {
                if (bi.v0.v(j) > bi.v1.v(j))
                    bi = Vector3Pair.swapElement(bi, j);

                if (bi.v0.v(j) < a.v(j))
                    a = a.v(j, bi.v0.v(j));

                if (bi.v1.v(j) > b.v(j))
                    b = b.v(j, bi.v1.v(j));
            }
        }
        return new Vector3Pair(a, b);
    }

    public abstract class Builder {
        protected int _id;
        protected Vector3Pair _position;
        protected Transform3 _transform;
        private Element.Builder _parent;

        public virtual Builder position(Vector3Pair position) {
            this._position = position;
            this._transform = new Transform3(position);
            return this;
        }

        public Builder localPosition(Vector3 v) {
            this._transform = new Transform3(v, this._transform.rotation_matrix, this._transform.use_rotation_matrix);
            return this;
        }

        public Builder parent(Element.Builder parent) {
            this._parent = parent;
            return this;
        }

        public Element.Builder parent()
        {
            return _parent;
        }

        public virtual Builder setId(AtomicInteger id) {
            this._id = id.incrementAndGet();
            return this;
        }

        public Builder rotate(double x, double y, double z) {
            this._transform = this._transform.rotate_axis_by_angles(new Vector3(x, y, z));
            return this;
        }

        public Transform3 transform() {
            return _transform;
        }

        public Element.Builder transform(Transform3 transform3) {
            this._transform = transform3;
            return this;
        }

        public int id() {
            return _id;
        }

        public virtual void compute_global_transforms(Transform3Cache tcache) {
            //System.err.println("Computing coordinate for " + this);

            Transform3 t = _transform; // local transform
            Element.Builder p = this._parent;
            while (p != null) {
                t = Transform3.compose(p._transform, t);
                p = p._parent;
            }
            tcache.put_local_2_global_transform(this._id, t);  // Local to global
            tcache.put_global_2_local_transform(this._id, t.inverse()); // Global to local
        }

        public abstract Element build();

        public override string ToString() {
            return this.GetType().Name + ",id=" + _id;
        }
    }
}

}