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
using System.Linq;

namespace Redukti.Nfotopix {


/**
 * Point image analysis base class
 */
public class AnalysisPointImage {
    public OpticalSystem _system;
    public RayTracer _tracer;
    public RayTraceParameters _params;
    public bool _processed_trace;
    /** Image plan that collect rays for analysis */
    public Image _image;
    /** The rays intercepted at image place */
    public List<TracedRay> _intercepts;
    public RayTraceResults _results;

    public AnalysisPointImage (OpticalSystem system)
    {
        _system = system;
        _tracer = new RayTracer();
        _processed_trace = false;
        _image = null;
        _intercepts = new List<TracedRay>();
        _params = new RayTraceParameters(system);
        _params.set_default_distribution (
                new Distribution (Pattern.HexaPolarDist, 20, 0.999));
        _params.get_default_distribution ().set_uniform_pattern ();
    }

    public void trace() {
        if (_processed_trace)
            return;

            _image = (Image)(from p in _params.get_sequence()
                       where p is Image
                       select p as Image).First();
        _results = _tracer.trace(_system, _params);
        _intercepts = _results.get_intercepted(_image);

        _processed_trace = true;
    }
}

}