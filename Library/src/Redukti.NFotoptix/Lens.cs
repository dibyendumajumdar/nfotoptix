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
using System.Linq;

namespace Redukti.Nfotopix
{
    public class Lens : Group
    {
        public enum LensEdge
        {
            StraightEdge,
            SlopeEdge,
        }

        protected OpticalSystem _opticalSystem;
        protected List<OpticalSurface> _surfaces;
        protected readonly Stop _stop;

        public Lens(int id, Vector3Pair position, Transform3 transform, List<OpticalSurface> surfaces,
            List<Element> elementList, Stop stop)
            : base(id, position, transform, elementList)
        {
            this._stop = stop;
            this._surfaces = surfaces;
        }

        public Stop stop()
        {
            return _stop;
        }

        public List<OpticalSurface> surfaces()
        {
            return _surfaces;
        }


        public override void set_system(OpticalSystem system)
        {
            base.set_system(system);
            _stop.set_system(system);
        }


        public override string ToString()
        {
            return "Lens{" + base.ToString() + "}";
        }

        public new class Builder : Group.Builder
        {
            double _last_pos = 0;
            Medium _next_mat = Air.air;
            Stop.Builder _stop = null;


            public override Element build()
            {
                List<Element> elements = getElements();
                Stop stop = elements.OfType<Stop>().First();
                List<OpticalSurface> surfaces = elements
                    .OfType<OpticalSurface>().ToList();
                return new Lens(_id, _position, _transform, surfaces, elements, stop);
            }

            /**
         * Add an optical surface
         *
         * @param curvature curvature of the surface, 1/r
         * @param radius    the radius of the disk
         * @param thickness the thickness after this surface
         * @param glass     the material after this surface
         */
            public Lens.Builder add_surface(double curvature, double radius, double thickness, Medium glass)
            {
                Curve curve;
                if (curvature == 0.0)
                    curve = Flat.flat;
                else
                    curve = new Sphere(curvature);
                return add_surface(curve, new Disk(radius), thickness,
                    glass);
            }

            public Lens.Builder add_surface(double curvature, double radius, double thickness)
            {
                return add_surface(curvature, radius, thickness, null);
            }

            public Lens.Builder add_surface(Curve curve, Shape shape, double thickness, Medium glass)
            {
                if (glass == null)
                {
                    glass = Air.air;
                }

                OpticalSurface.Builder surface = new OpticalSurface.Builder()
                    .position(new Vector3Pair(new Vector3(0, 0, _last_pos), Vector3.vector3_001))
                    .curve(curve)
                    .shape(shape)
                    .leftMaterial(_next_mat)
                    .rightMaterial(glass);
                _next_mat = glass;
                _last_pos += thickness;
                add(surface);
                return this;
            }

            public Lens.Builder add_stop(double radius, double thickness)
            {
                return add_stop(new Disk(radius), thickness);
            }

            public Lens.Builder add_stop(Shape shape, double thickness)
            {
                if (_stop != null)
                    throw new InvalidOperationException("Can not add more than one stop per Lens");
                _stop = new Stop.Builder()
                    .position(new Vector3Pair(new Vector3(0, 0, _last_pos), Vector3.vector3_001))
                    .curve(Flat.flat)
                    .shape(shape);
                _last_pos += thickness;
                add(_stop);
                return this;
            }


            public override void compute_global_transforms(Transform3Cache tcache)
            {
                base.compute_global_transforms(tcache);
            }


            public override Lens.Builder position(Vector3Pair position)
            {
                return (Lens.Builder) base.position(position);
            }
        }
    }
}