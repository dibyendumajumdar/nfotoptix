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

namespace Redukti.Nfotopix
{
    public class OpticalSystem : Container
    {
        protected List<Element> _elements;
        protected Transform3Cache _transform3Cache;
        protected bool _keep_aspect = true;


        public List<Element> elements()
        {
            return _elements;
        }

        public OpticalSystem(List<Element> elements, Transform3Cache transform3Cache)
        {
            this._elements = elements;
            this._transform3Cache = transform3Cache;
        }

        public Element getElement(int pos)
        {
            if (pos >= 0 && pos < _elements.Count)
            {
                return _elements[pos];
            }

            return null;
        }

        public Group getGroup(int pos)
        {
            if (pos >= 0 && pos < _elements.Count && _elements[pos] is Group)
            {
                return (Group) _elements[pos];
            }

            return null;
        }

        public Vector3 getPosition(Element e)
        {
            return _transform3Cache.local_2_global_transform(e.id()).transform(Vector3.vector3_0);
        }

        public Vector3Pair get_bounding_box()
        {
            return Element.get_bounding_box(_elements);
        }

        public Transform3 get_transform(Element from, Element to)
        {
            return _transform3Cache.transform_cache_update(from.id(), to.id());
        }

        public Transform3 get_global_transform(Element e)
        {
            return _transform3Cache.local_2_global_transform(e.id());
        }

//    Transform3 get_local_transform(Element e) {
//        return transform3Cache.getGlobal2LocalTransform(e.id());
//    }


        public override string ToString()
        {
            return "OpticalSystem{" +
                   "elements={" + string.Join(",", _elements) +
                   "}, transform3Cache=" + _transform3Cache +
                   ", keep_aspect=" + _keep_aspect +
                   '}';
        }

        public class Builder
        {
            protected List<Element.Builder> _elements = new List<Element.Builder>();
            protected Transform3Cache _transform3Cache;

            public Builder add(Element.Builder element)
            {
                this._elements.Add(element);
                return this;
            }

            public OpticalSystem build()
            {
                generateIds();
                Transform3Cache transform3Cache = setCoordinates();
                List<Element> elements = buildElements();
                OpticalSystem system = new OpticalSystem(elements, transform3Cache);
                foreach (Element e in system.elements())
                {
                    e.set_system(system);
                }

                return system;
            }

            private List<Element> buildElements()
            {
                List<Element> els = new();
                foreach (Element.Builder e in _elements)
                {
                    els.Add(e.build());
                }

                return els;
            }

            private Transform3Cache setCoordinates()
            {
                _transform3Cache = new Transform3Cache();
                foreach (Element.Builder e in _elements)
                {
                    e.compute_global_transforms(_transform3Cache);
                }

                return _transform3Cache;
            }

            private void generateIds()
            {
                AtomicInteger id = new AtomicInteger(0);
                foreach (Element.Builder e in _elements)
                {
                    e.setId(id);
                }
            }

            /**
         * Sets element position using global coordinate system
         * Needs a prior call to build so we have the transformations needed
         */
            public OpticalSystem updatePosition(Element.Builder e, Vector3 v)
            {
                // FIXME
                if (_transform3Cache == null)
                    throw new InvalidOperationException("build() must be called prior to updating position");
                if (e.parent() != null)
                {
                    e.localPosition(_transform3Cache.global_2_local_transform(e.parent().id()).transform(v));
                }
                else
                {
                    e.localPosition(v);
                }

                return build();
            }
        }
    }
}