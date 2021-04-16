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


    public class Rectangle : ShapeBase
    {
        Vector2 _halfsize;

        public Rectangle(double sqwidth)
        {
            this._halfsize = new Vector2(sqwidth / 2.0, sqwidth / 2.0);
        }

        public Rectangle(double width, double height)
        {
            this._halfsize = new Vector2(width / 2.0, height / 2.0);
        }


        override public double max_radius()
        {
            return _halfsize.len();
        }


        override public double min_radius()
        {
            return Math.Min(_halfsize.x(), _halfsize.y());
        }


        override public bool inside(Vector2 point)
        {
            return (Math.Abs(point.x()) <= _halfsize.x()
                    && Math.Abs(point.y()) <= _halfsize.y());
        }


        public override void get_pattern(PatternConsumer f,
                                Distribution d, bool unobstructed)
        {
            const double epsilon = 1e-8;
            Vector2 hs = _halfsize.times(d.get_scaling());
            Vector2 step = hs.divide((double)(d.get_radial_density() / 2));

            switch (d.get_pattern())
            {
                case Pattern.MeridionalDist:
                    {
                        f(Vector2.vector2_0);

                        for (double y = step.y(); y < hs.y() + epsilon; y += step.y())
                        {
                            f(new Vector2(0, y));
                            f(new Vector2(0, -y));
                        }
                        break;
                    }

                case Pattern.SagittalDist:
                    {
                        f(Vector2.vector2_0);

                        for (double x = step.x(); x < hs.x() + epsilon; x += step.x())
                        {
                            f(new Vector2(x, 0));
                            f(new Vector2(-x, 0));
                        }
                        break;
                    }

                case Pattern.CrossDist:
                    {
                        f(Vector2.vector2_0);

                        for (double x = step.x(); x < hs.x() + epsilon; x += step.x())
                        {
                            f(new Vector2(x, 0));
                            f(new Vector2(-x, 0));
                        }

                        for (double y = step.y(); y < hs.y() + epsilon; y += step.y())
                        {
                            f(new Vector2(0, y));
                            f(new Vector2(0, -y));
                        }
                        break;
                    }

                case Pattern.DefaultDist:
                case Pattern.SquareDist:
                    {
                        double x, y;

                        f(Vector2.vector2_0);

                        for (x = step.x(); x < hs.x() + epsilon; x += step.x())
                            for (y = step.y(); y < hs.y() + epsilon; y += step.y())
                            {
                                f(new Vector2(x, y));
                                f(new Vector2(-x, y));
                                f(new Vector2(x, -y));
                                f(new Vector2(-x, -y));
                            }

                        for (x = step.x(); x < hs.x() + epsilon; x += step.x())
                        {
                            f(new Vector2(x, 0));
                            f(new Vector2(-x, 0));
                        }

                        for (y = step.y(); y < hs.y() + epsilon; y += step.y())
                        {
                            f(new Vector2(0, y));
                            f(new Vector2(0, -y));
                        }
                        break;
                    }

                default:
                    base.get_pattern(f, d, unobstructed);
                    break;
            }
        }


        override public Vector2Pair get_bounding_box()
        {
            return new Vector2Pair(_halfsize.negate(), _halfsize);
        }


        override public int get_contour_count()
        {
            return 1;
        }


        override public void get_contour(int contour, PatternConsumer f, double resolution)
        {
            const double epsilon = 1e-8;

            Vector2 step = get_step(resolution);

            double x, y;

            for (x = -_halfsize.x(); x < _halfsize.x() - epsilon; x += step.x())
                f(new Vector2(x, -_halfsize.y()));

            for (y = -_halfsize.y(); y < _halfsize.y() - epsilon; y += step.y())
                f(new Vector2(_halfsize.x(), y));

            for (x = _halfsize.x(); x > -_halfsize.x() + epsilon; x -= step.x())
                f(new Vector2(x, _halfsize.y()));

            for (y = _halfsize.y(); y > -_halfsize.y() + epsilon; y -= step.y())
                f(new Vector2(-_halfsize.x(), y));

        }


        override public void get_triangles(ConsumerTriangle2 f, double resolution)
        {
            const double epsilon = 1e-8;

            Vector2 step = get_step(resolution);

            for (double x = 0; x < _halfsize.x() - epsilon; x += step.x())
                for (double y = 0; y < _halfsize.y() - epsilon; y += step.y())
                {
                    Vector2 a = new Vector2(x, y);
                    Vector2 b = new Vector2(x + step.x(), y);
                    Vector2 c = new Vector2(x, y + step.y());
                    Vector2 d = new Vector2(x + step.x(), y + step.y());

                    f(new Triangle2(b, a, c));
                    f(new Triangle2(d, b, c));
                    f(new Triangle2(b.negate(), a.negate(), c.negate()));
                    f(new Triangle2(d.negate(), b.negate(), c.negate()));

                    a = a.x(-a.x());
                    b = b.x(-b.x());
                    c = c.x(-c.x());
                    d = d.x(-d.x());

                    f(new Triangle2(a, b, c));
                    f(new Triangle2(b, d, c));
                    f(new Triangle2(a.negate(), b.negate(), c.negate()));
                    f(new Triangle2(b.negate(), d.negate(), c.negate()));
                }
        }

        Vector2 get_step(double resolution)
        {
            double[] s = new double[2];
            for (int i = 0; i < 2; i++)
            {
                if (resolution > _halfsize.v(i))
                    s[i] = _halfsize.v(i);
                else
                    s[i] = _halfsize.v(i) / Math.Round(_halfsize.v(i) / resolution);
            }
            return new Vector2(s[0], s[1]);
        }


        override public double get_outter_radius(Vector2 dir)
        {
            Vector2 e
                    = (Math.Abs(dir.x() / dir.y()) < (_halfsize.x() / _halfsize.y()))
                    ? Vector2.vector2_10
                    : Vector2.vector2_01;

            return (new Vector2Pair(_halfsize, e)
                    .ln_intersect_ln(new Vector2Pair(Vector2.vector2_0, dir)))
                    .len();
        }
    }

}