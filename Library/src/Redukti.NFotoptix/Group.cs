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

using System.Collections.Generic;
using System.Text;

namespace Redukti.Nfotopix {

public class Group : Element,  Container {
    private readonly List<Element> _elements;

    public Group(int id, Vector3Pair p, Transform3 transform3, List<Element> elements): base(id, p, transform3)
        {
        
        this._elements = elements;
    }

    
    public List<Element> elements() {
        return _elements;
    }

    public Element getElement(int pos) {
        if (pos >= 0 && pos < _elements.Count) {
            return _elements[pos];
        }
        return null;
    }

    public Group getGroup(int pos) {
        if (pos >= 0 && pos < _elements.Count && _elements[pos] is Group) {
            return (Group)_elements[pos];
        }
        return null;
    }

    public Surface getSurface(int pos) {
        if (pos >= 0 && pos < _elements.Count  && _elements[pos] is Surface) {
            return (Surface) _elements[pos];
        }
        return null;
    }

    public override void set_system(OpticalSystem system) {
        this._system = system;
        foreach (Element e in elements()) {
            e.set_system(system);
        }
    }

    public override Vector3Pair get_bounding_box ()
    {
        return Element.get_bounding_box(_elements);
    }

    
    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append("id=" + _id +
                ", position=" + _position +
                ", transform=" + _transform)
                .Append('\n');
        foreach (Element e in elements()) {
            sb.Append('\t').Append(e.ToString()).Append('\n');
        }
        return sb.ToString();
    }

    public class Builder: Element.Builder {
        protected List<Element.Builder> _elements = new List<Element.Builder>();

        public override Group.Builder position(Vector3Pair position) {
            return (Builder) base.position(position);
        }

        public Group.Builder add(Element.Builder element) {
            this._elements.Add(element);
            element.parent(this);
            return this;
        }

        public Group.Builder setId(AtomicInteger id) {
            this._id = id.incrementAndGet();
            foreach (Element.Builder e in _elements) {
                e.setId(id);
            }
            return this;
        }

        
        public override void compute_global_transforms(Transform3Cache tcache) {
            base.compute_global_transforms(tcache);
            foreach (Element.Builder e in _elements) {
                e.compute_global_transforms(tcache);
            }
        }

        
        public override Element build() {
            return new Group(_id, _position, _transform, getElements());
        }

        protected List<Element> getElements() {
            List<Element> myels = new List<Element>();
            foreach (Element.Builder e in _elements) {
                myels.Add(e.build());
            }
            return myels;
        }
    }
}

}