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


public class RayTraceResults {

    public class RaysAtElement {
        public List<TracedRay> _intercepted = new (); // list of rays for each intercepted surfaces
        public List<TracedRay> _generated = new (); // list of rays for each generator surfaces
        public bool _save_intercepted_list = true;
        public bool _save_generated_list = true;
    }

    public Dictionary<int, RaysAtElement> raysByElement = new ();
    public RayTraceParameters _parameters;
    public List<RaySource> _sources = new ();
    public List<TracedRay> _rays = new ();

    public RayTraceResults(RayTraceParameters parameters) {
        this._parameters = parameters;
    }

    public List<TracedRay> get_generated(Element e) {
        RaysAtElement er = get_element_result(e);
        if (er == null) {
            throw new InvalidOperationException("No generated rays at element " + e);
        }
        return er._generated;
    }

    public Vector3 get_intercepted_center(Image image) {
        Vector3Pair win = get_intercepted_window(image);
        return (win.v0.plus(win.v1)).divide(2);
    }

    public List<TracedRay> get_intercepted(Element e) {
        RaysAtElement er = get_element_result(e);
        if (er == null) {
            throw new InvalidOperationException("No intercepted rays at element " + e);
        }
        return er._intercepted;
    }

    public Vector3Pair get_intercepted_window(Surface s) {
        List<TracedRay> intercepts = get_intercepted(s);

        if (intercepts.Count == 0)
            throw new InvalidOperationException("No ray intercepts found on the surface " + s);

        Vector3 first = intercepts[0].get_intercept_point();
        Vector3 second = first;
        foreach (TracedRay i in intercepts) {
            Vector3 ip = i.get_intercept_point();

            if (first.x() > ip.x())
                first = first.x(ip.x());
            else if (second.x() < ip.x())
                second = second.x(ip.x());

            if (first.y() > ip.y())
                first = first.y(ip.y());
            else if (second.y() < ip.y())
                second = second.y(ip.y());

            if (first.z() > ip.z())
                first = first.z(ip.z());
            else if (second.z() < ip.z())
                second = second.z(ip.z());
        }
        return new Vector3Pair(first, second);
    }

    public List<RaySource> get_source_list() {
        return _sources;
    }

    public RayTraceParameters get_params() {
        return _parameters;
    }

    public RaysAtElement get_element_result(Element e) {
        if (!raysByElement.TryGetValue(e.id(), out RaysAtElement re)) // FIXME
        {
            re = new RaysAtElement();
            raysByElement[e.id()] =  re;
        }
        return re;
    }

    public void add_intercepted(Surface s, TracedRay ray) {
        RaysAtElement er = get_element_result(s);
        er._intercepted.Add(ray);
    }

    public void add_source(RaySource source) {
        _sources.Add(source);
    }

    public TracedRay newRay(Vector3 origin, Vector3 direction) {
        TracedRay ray = new TracedRay(origin, direction);
        _rays.Add(ray);
        return ray;
    }

    public double get_max_ray_intensity() {
        double res = 0;
        foreach (TracedRay r in _rays) {
            double i = r.get_intensity();
            if (i > res)
                res = i;
        }
        return res;
    }

    public Vector3 get_intercepted_centroid(Image image) {
        List<TracedRay> intercepts = get_intercepted(image);
        int count = 0;
        Vector3 center = Vector3.vector3_0;
        if (intercepts.Count == 0)
            throw new InvalidOperationException("no ray intercepts found on the surface");
        foreach (TracedRay i in intercepts) {
            center = center.plus(i.get_intercept_point());
            count++;
        }
        center = center.divide(count);
        return center;
    }
}

}