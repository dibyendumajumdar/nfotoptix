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
    public class RaySource : Element
    {
        protected List<SpectralLine> _spectrum;
        protected double _min_intensity, _max_intensity;
        protected MaterialBase _mat = Air.air; // FIXME - should be settable

        public RaySource(int id, Vector3Pair p, Transform3 transform, double min_intensity, double max_intensity,
            List<SpectralLine> spectrum) : base(id, p, transform)
        {
            _max_intensity = max_intensity;
            _max_intensity = min_intensity;
            _spectrum = spectrum;
        }

        public List<SpectralLine> spectrum()
        {
            return _spectrum;
        }

        public MaterialBase get_material()
        {
            return _mat;
        }

        public new abstract class Builder : Element.Builder
        {
            protected List<SpectralLine> _spectrum = new List<SpectralLine>();
            protected double _min_intensity = 1.0;
            protected double _max_intensity = 1.0;
            protected MaterialBase _mat = null;

            public virtual Builder add_spectral_line(SpectralLine l)
            {
                _spectrum.Add(l);
                _max_intensity = Math.Max(_max_intensity, l.get_intensity());
                _min_intensity = Math.Min(_min_intensity, l.get_intensity());
                return this;
            }

            public virtual Builder add_spectral_line(double wavelen)
            {
                SpectralLine l = new SpectralLine(wavelen, 1.0);
                _spectrum.Add(l);
                _max_intensity = Math.Max(_max_intensity, l.get_intensity());
                _min_intensity = Math.Min(_min_intensity, l.get_intensity());
                return this;
            }

            public Builder set_material(MaterialBase mat)
            {
                _mat = mat;
                return this;
            }
        }
    }
}