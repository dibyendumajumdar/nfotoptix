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

namespace Redukti.Nfotopix
{


    public abstract class Round : ShapeBase
    {

        static Random random = new Random();

        bool hole;

        public abstract double get_xy_ratio();

        public abstract double get_external_xradius();

        public abstract double get_internal_xradius();

        public Round(bool hole)
        {
            this.hole = hole;
        }

        public override void get_pattern(PatternConsumer f,
                                Distribution d,
                                bool unobstructed)
        {
            const double epsilon = 1e-8;
            double xyr = 1.0 / get_xy_ratio();
            double tr = get_external_xradius() * d.get_scaling();
            bool obstructed = hole && !unobstructed;
            double hr = obstructed
                    ? get_internal_xradius() * (2.0 - d.get_scaling())
                    : 0.0;
            int rdens = (int)Math.Floor((double)d.get_radial_density()
                    - (d.get_radial_density() * (hr / tr)));
            rdens = Math.Max(1, rdens);
            double step = (tr - hr) / rdens;

            Pattern p = d.get_pattern();

            switch (p)
            {
                case Pattern.MeridionalDist:
                    {

                        if (!obstructed)
                            f(Vector2.vector2_0);

                        double bound = obstructed ? hr - epsilon : epsilon;

                        for (double r = tr; r > bound; r -= step)
                        {
                            f(new Vector2(0, r * xyr));
                            f(new Vector2(0, -r * xyr));
                        }
                    }
                    break;

                case Pattern.SagittalDist:
                    {

                        if (!obstructed)
                            f(Vector2.vector2_0);

                        double bound = obstructed ? hr - epsilon : epsilon;

                        for (double r = tr; r > bound; r -= step)
                        {
                            f(new Vector2(r, 0));
                            f(new Vector2(-r, 0));
                        }
                    }
                    break;

                case Pattern.CrossDist:
                    {

                        if (!obstructed)
                            f(Vector2.vector2_0);

                        double bound = obstructed ? hr - epsilon : epsilon;

                        for (double r = tr; r > bound; r -= step)
                        {
                            f(new Vector2(0, r * xyr));
                            f(new Vector2(r, 0));
                            f(new Vector2(0, -r * xyr));
                            f(new Vector2(-r, 0));
                        }
                    }
                    break;

                case Pattern.RandomDist:
                    {
                        if (!obstructed)
                            f(Vector2.vector2_0);

                        double bound = obstructed ? hr - epsilon : epsilon;

                        double tr1 = tr / 20.0;
                        for (double r = tr1; r > bound; r -= step)
                        {
                            double astep = (Math.PI / 3) / Math.Ceiling(r / step);
                            // angle
                            for (double a = 0; a < 2 * Math.PI - epsilon; a += astep)
                            {
                                Vector2 v = new Vector2(Math.Sin(a) * r + (random.NextDouble() - .5) * step,
                                        Math.Cos(a) * r * xyr + (random.NextDouble() - .5) * step);
                                double h = MathUtils.Hypot(v.x(), v.y() / xyr);
                                if (h < tr && (h > hr || unobstructed))
                                    f(v);
                            }
                        }
                    }
                    break;

                case Pattern.DefaultDist:
                case Pattern.HexaPolarDist:
                    {

                        if (!obstructed)
                            f(Vector2.vector2_0);

                        double bound = obstructed ? hr - epsilon : epsilon;

                        for (double r = tr; r > bound; r -= step)
                        {
                            double astep = (Math.PI / 3) / Math.Ceiling(r / step);

                            for (double a = 0; a < 2 * Math.PI - epsilon; a += astep)
                                f(new Vector2(Math.Sin(a) * r, Math.Cos(a) * r * xyr));
                        }
                    }
                    break;

                default:
                    {
                        PatternConsumer f2 = (Vector2 v) =>
                        {
                            // unobstructed pattern must be inside external
                            // radius
                            if (MathUtils.square(v.x())
                                    + MathUtils.square(v.y() / xyr)
                                    < MathUtils.square(tr))
                                f(v);
                        };
                        base.get_pattern(f2, d, unobstructed);
                        break;
                    }
            }
        }


        public override int get_contour_count()
        {
            return hole ? 2 : 1;
        }

        double get_radial_step(double resolution)
        {
            double xyr = 1.0 / get_xy_ratio();
            double width = xyr <= 1.0
                    ? get_external_xradius() - get_internal_xradius()
                    : get_external_xradius() * xyr
                    - get_internal_xradius() * xyr;

            if (resolution < width / 30.0)
                resolution = width / 30.0;

            if (resolution > width)
                resolution = width;

            if (hole && resolution > get_internal_xradius())
                resolution = get_internal_xradius();

            return (get_external_xradius() - get_internal_xradius())
                    / Math.Ceiling(width / resolution);
        }


        public override void get_contour(int contour,
                                PatternConsumer f,
                                double resolution)
        {
            const double epsilon = 1e-8;
            double xyr = 1.0 / get_xy_ratio();
            double r;

            if (hole && contour == 1)
                r = get_internal_xradius();
            else
                r = get_external_xradius();

            double astep1 = (Math.PI / 3.0) / Math.Round(r / get_radial_step(resolution));

            for (double a1 = 0; a1 < 2 * Math.PI - epsilon; a1 += astep1)
                f(new Vector2(Math.Cos(a1) * r, Math.Sin(a1) * r * xyr));
        }


        public override void get_triangles(ConsumerTriangle2 f, double resolution)
        {
            const double epsilon = 1e-8;
            double xyr = 1.0 / get_xy_ratio();
            double rstep = get_radial_step(resolution);

            double astep1;
            double r;

            if (!hole)
            {
                r = rstep;
                astep1 = Math.PI / 3.0;

                // central hexagon

                for (double a1 = 0; a1 < Math.PI - epsilon; a1 += astep1)
                {
                    Vector2 a = new Vector2(Math.Cos(a1) * rstep, Math.Sin(a1) * rstep * xyr);
                    Vector2 b = new Vector2(Math.Cos(a1 + astep1) * rstep,
                            Math.Sin(a1 + astep1) * rstep * xyr);
                    Vector2 z = Vector2.vector2_0;

                    f(new Triangle2(b, a, z));
                    f(new Triangle2(b.negate(), a.negate(), z));
                }
            }
            else
            {
                r = get_internal_xradius();
                astep1 = (Math.PI / 3.0) / Math.Round(r / rstep);
            }

            // hexapolar distributed triangles

            for (; r < get_external_xradius() - epsilon; r += rstep)
            {
                double astep2 = (Math.PI / 3.0) / Math.Round((r + rstep) / rstep);
                double a1 = 0, a2 = 0;

                while ((a1 < Math.PI - epsilon) || (a2 < Math.PI - epsilon))
                {
                    Vector2 a = new Vector2(Math.Cos(a1) * r, Math.Sin(a1) * r * xyr);
                    Vector2 b = new Vector2(Math.Cos(a2) * (r + rstep),
                            Math.Sin(a2) * (r + rstep) * xyr);
                    Vector2 c;

                    if (a1 + epsilon > a2)
                    {
                        a2 += astep2;
                        c = new Vector2(Math.Cos(a2) * (r + rstep),
                                Math.Sin(a2) * (r + rstep) * xyr);
                    }
                    else
                    {
                        a1 += astep1;
                        c = new Vector2(Math.Cos(a1) * r, Math.Sin(a1) * r * xyr);
                    }

                    f(new Triangle2(a, c, b));
                    f(new Triangle2(a.negate(), c.negate(), b.negate()));
                }

                astep1 = astep2;
            }
        }

    }

}