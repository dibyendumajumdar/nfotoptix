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
    public abstract class ShapeBase : Shape
    {
        static Random random = new Random();

        private void add_pattern_point(Vector2 v, bool unobstructed, Shape shape, PatternConsumer f)
        {
            if (unobstructed || shape.inside(v))
                f(v);
        }


        public virtual void get_pattern(PatternConsumer f,
            Distribution d,
            bool unobstructed)
        {
            get_base_pattern(f, d, unobstructed);
        }

        public void get_base_pattern(PatternConsumer f,
            Distribution d,
            bool unobstructed)
        {
            const double epsilon = 1e-8;
            // FIXME use bounding box instead of max radius
            double tr = max_radius() * d.get_scaling();
            double step = tr / d.get_radial_density();

            Pattern p = d.get_pattern();

            switch (p)
            {
                case Pattern.MeridionalDist:
                {
                    double r = tr;

                    add_pattern_point(Vector2.vector2_0, unobstructed, this, f);

                    for (int i = 0; i < d.get_radial_density(); i++)
                    {
                        add_pattern_point(new Vector2(0, r), unobstructed, this, f);
                        add_pattern_point(new Vector2(0, -r), unobstructed, this, f);
                        r -= step;
                    }

                    break;
                }

                case Pattern.SagittalDist:
                {
                    double r = tr;

                    add_pattern_point(new Vector2(0, 0), unobstructed, this, f);

                    for (int i = 0; i < d.get_radial_density(); i++)
                    {
                        add_pattern_point(new Vector2(r, 0), unobstructed, this, f);
                        add_pattern_point(new Vector2(-r, 0), unobstructed, this, f);
                        r -= step;
                    }

                    break;
                }

                case Pattern.CrossDist:
                {
                    double r = step;

                    add_pattern_point(Vector2.vector2_0, unobstructed, this, f);

                    for (int i = 0; i < d.get_radial_density(); i++)
                    {
                        add_pattern_point(new Vector2(0, r), unobstructed, this, f);
                        add_pattern_point(new Vector2(r, 0), unobstructed, this, f);
                        add_pattern_point(new Vector2(0, -r), unobstructed, this, f);
                        add_pattern_point(new Vector2(-r, 0), unobstructed, this, f);
                        r += step;
                    }

                    break;
                }

                case Pattern.RandomDist:
                {
                    double x, y;

                    for (x = -tr; x < tr; x += step)
                    {
                        double ybound = Math.Sqrt(MathUtils.square(tr) - MathUtils.square(x));

                        for (y = -ybound; y < ybound; y += step)
                        {
                            add_pattern_point(
                                new Vector2(x + (random.NextDouble() - .5) * step,
                                    y + (random.NextDouble() - .5) * step), unobstructed, this, f);
                        }
                    }

                    break;
                }

                case Pattern.HexaPolarDist:
                {
                    add_pattern_point(Vector2.vector2_0, unobstructed, this, f);

                    for (double r = tr; r > epsilon; r -= step)
                    {
                        double astep = (step / r) * (Math.PI / 3);

                        for (double a = 0; a < 2 * Math.PI - epsilon; a += astep)
                            add_pattern_point(new Vector2(Math.Sin(a) * r, Math.Cos(a) * r), unobstructed, this, f);
                    }

                    break;
                }

                case Pattern.SquareDist:
                {
                    add_pattern_point(Vector2.vector2_0, unobstructed, this, f);

                    double x, y;

                    for (x = tr; x > epsilon; x -= step)
                    {
                        double ybound = Math.Sqrt(MathUtils.square(tr) - MathUtils.square(x));

                        for (y = step; y < ybound; y += step)
                        {
                            add_pattern_point(new Vector2(x, y), unobstructed, this, f);
                            add_pattern_point(new Vector2(x, -y), unobstructed, this, f);
                            add_pattern_point(new Vector2(-x, y), unobstructed, this, f);
                            add_pattern_point(new Vector2(-x, -y), unobstructed, this, f);
                        }

                        add_pattern_point(new Vector2(x, 0), unobstructed, this, f);
                        add_pattern_point(new Vector2(-x, 0), unobstructed, this, f);
                    }

                    for (y = step; y < tr + epsilon; y += step)
                    {
                        add_pattern_point(new Vector2(0, y), unobstructed, this, f);
                        add_pattern_point(new Vector2(0, -y), unobstructed, this, f);
                    }

                    break;
                }

                case Pattern.DefaultDist:
                case Pattern.TriangularDist:
                {
                    const double sqrt_3_2 = 0.86602540378443864676;
                    double x, y;
                    int i = 1;

                    for (x = step * sqrt_3_2; x < tr + epsilon; x += step * sqrt_3_2)
                    {
                        for (y = step / (double) i; y < tr + epsilon; y += step)
                        {
                            double h = MathUtils.Hypot(x, y);

                            if (h > tr)
                                break;

                            add_pattern_point(new Vector2(x, y), unobstructed, this, f);
                            add_pattern_point(new Vector2(-x, y), unobstructed, this, f);
                            add_pattern_point(new Vector2(x, -y), unobstructed, this, f);
                            add_pattern_point(new Vector2(-x, -y), unobstructed, this, f);
                        }

                        i ^= 3;
                    }

                    for (y = step / 2.0; y < tr + epsilon; y += step)
                    {
                        add_pattern_point(new Vector2(0, y), unobstructed, this, f);
                        add_pattern_point(new Vector2(0, -y), unobstructed, this, f);
                    }

                    for (x = step * sqrt_3_2; x < tr + epsilon; x += step * sqrt_3_2 * 2.0)
                    {
                        add_pattern_point(new Vector2(x, 0), unobstructed, this, f);
                        add_pattern_point(new Vector2(-x, 0), unobstructed, this, f);
                    }

                    break;
                }

                default:
                    throw new InvalidOperationException("distribution pattern not supported for this shape");
            }
        }


        public double get_hole_radius(Vector2 dir)
        {
            return 0;
        }

        public abstract bool inside(Vector2 point);
        public abstract double max_radius();
        public abstract double min_radius();
        public abstract double get_outter_radius(Vector2 dir);
        public abstract Vector2Pair get_bounding_box();
        public abstract int get_contour_count();
        public abstract void get_contour(int contour, PatternConsumer f, double resolution);
        public abstract void get_triangles(ConsumerTriangle2 f, double resolution);
    }
}