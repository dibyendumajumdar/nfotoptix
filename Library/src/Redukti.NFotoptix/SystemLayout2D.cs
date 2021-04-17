using System;
using System.Collections.Generic;

namespace Redukti.Nfotopix
{
    public class SystemLayout2D
    {
        public void layout2d(RendererSvg r, OpticalSystem system)
        {
            draw_2d_fit(r, system);
            draw_2d(r, system);
        }

        void draw_2d_fit(RendererViewport r, OpticalSystem system, bool keep_aspect)
        {
            Vector3Pair b = system.get_bounding_box();

            r.set_window(Vector2Pair.from(b, 2, 1), keep_aspect);
            r.set_camera_direction(Vector3.vector3_100);
            r.set_camera_position(Vector3.vector3_0);

            r.set_feature_size(b.v1.y() - b.v0.y() / 20.0);
        }

        void draw_2d_fit(RendererSvg r, OpticalSystem system)
        {
            draw_2d_fit(r, system, true);
        }

        void draw_2d(RendererSvg r, OpticalSystem system)
        {
            // optical axis
            Vector3Pair b = system.get_bounding_box();
            r.draw_segment(new Vector2Pair(new Vector2(b.v0.z(), 0.0), new Vector2(b.v1.z(), 0.0)), Rgb.rgb_gray);

            foreach (Element e in system.elements())
            {
                draw_element_2d(r, e, null);
            }
        }

        void draw_element_2d(Renderer r, Element self, Element ref_)
        {
            // Order is important here as Lens extends Group
            // and Stop extends Surface
            switch (self)
            {
                case Lens lens:
                {
                    r.group_begin("element");
                    draw_2d_e(r, lens, ref_);
                    r.group_end();
                    break;
                }
                case Group group:
                {
                    r.group_begin("element");
                    draw_2d_e(r, group, ref_);
                    r.group_end();
                    break;
                }
                case Stop stop:
                {
                    r.group_begin("element");
                    draw_2d_e(r, stop, ref_);
                    r.group_end();
                    break;
                }
                case Surface surface:
                {
                    r.group_begin("element");
                    draw_2d_e(r, surface, ref_);
                    r.group_end();
                    break;
                }
            }
        }

        void draw_2d_e(Renderer r, Group g, Element ref_)
        {
            foreach (Element e in g.elements())
            {
                draw_element_2d(r, e, ref_);
            }
        }

        void draw_2d_e(Renderer r, Lens lens, Element ref_)
        {
            bool grp = false;

            if (lens.stop() != null)
                draw_2d_e(r, lens.stop(), ref_);

            if (lens.elements().Count == 0)
                return;

            List<OpticalSurface> surfaces = lens.surfaces();
            OpticalSurface first = surfaces[0];
            if (first.get_material(1) != first.get_material(0))
            {
                if (!grp)
                {
                    r.group_begin("");
                    grp = true;
                }

                draw_2d_e(r, first, ref_);
            }

            for (int i = 0; i < surfaces.Count - 1; i++)
            {
                OpticalSurface left = surfaces[i];
                OpticalSurface right = surfaces[i + 1];

                if (left.get_material(1) == null || !(left.get_material(1) is Solid))
                {
                    if (grp)
                    {
                        r.group_end();
                        grp = false;
                    }
                }
                else
                {
                    // draw outter edges
                    double left_top_edge
                        = left.get_shape().get_outter_radius(Vector2.vector2_01);
                    double left_bot_edge
                        = -left.get_shape().get_outter_radius(Vector2.vector2_01.negate());
                    double right_top_edge
                        = right.get_shape().get_outter_radius(Vector2.vector2_01);
                    double right_bot_edge
                        = -right.get_shape().get_outter_radius(Vector2.vector2_01.negate());

                    draw_2d_edge(r, left, left_top_edge, right, right_top_edge,
                        Lens.LensEdge.StraightEdge, ref_);
                    draw_2d_edge(r, left, left_bot_edge, right, right_bot_edge,
                        Lens.LensEdge.StraightEdge, ref_);

                    // draw hole edges if not coincident
                    double left_top_hole
                        = left.get_shape().get_hole_radius(Vector2.vector2_01);
                    double left_bot_hole
                        = -left.get_shape().get_hole_radius(Vector2.vector2_01.negate());
                    double right_top_hole
                        = right.get_shape().get_hole_radius(Vector2.vector2_01);
                    double right_bot_hole
                        = -right.get_shape().get_hole_radius(Vector2.vector2_01.negate());

                    if (Math.Abs(left_bot_hole - left_top_hole) > 1e-6
                        || Math.Abs(right_bot_hole - right_top_hole) > 1e-6)
                    {
                        draw_2d_edge(r, left, left_top_hole, right, right_top_hole,
                            Lens.LensEdge.SlopeEdge, ref_);
                        draw_2d_edge(r, left, left_bot_hole, right, right_bot_hole,
                            Lens.LensEdge.SlopeEdge, ref_);
                    }
                }

                if (right.get_material(1) != right.get_material(0))
                {
                    if (!grp)
                    {
                        r.group_begin("");
                        grp = true;
                    }

                    draw_2d_e(r, right, ref_);
                }
            }

            if (grp)
            {
                r.group_end();
            }
        }

        void draw_2d_edge(Renderer r, Surface left, double l_y,
            Surface right, double r_y, Lens.LensEdge type,
            Element ref_)
        {
            Vector3 l3 = new Vector3(0.0, l_y,
                left.get_curve().sagitta(new Vector2(0.0, l_y)));
            Vector2 l2 = left.get_transform_to(ref_).transform(l3).project_zy();
            Vector3 r3 = new Vector3(0.0, r_y, right.get_curve().sagitta(new Vector2(0.0, r_y)));
            Vector2 r2 = right.get_transform_to(ref_).transform(r3).project_zy();

            switch (type)
            {
                case Lens.LensEdge.StraightEdge:
                {
                    if (Math.Abs(l2.y() - r2.y()) > 1e-6)
                    {
                        double m;

                        if (Math.Abs(l2.y()) > Math.Abs(r2.y()))
                        {
                            m = l2.y();
                            r.draw_segment(new Vector2Pair(new Vector2(r2.x(), m), new Vector2(r2.x(), r2.y())),
                                r.get_style_color(left.get_style()));
                        }
                        else
                        {
                            m = r2.y();
                            r.draw_segment(new Vector2Pair(new Vector2(l2.x(), m), new Vector2(l2.x(), l2.y())),
                                r.get_style_color(left.get_style()));
                        }

                        r.draw_segment(new Vector2Pair(new Vector2(l2.x(), m), new Vector2(r2.x(), m)),
                            r.get_style_color(left.get_style()));

                        break;
                    }

                    goto case Lens.LensEdge.SlopeEdge;
                }

                case Lens.LensEdge.SlopeEdge:
                    r.draw_segment(l2, r2, r.get_style_color(left.get_style()));
                    break;
            }
        }

        void draw_2d_e(Renderer r, Stop stop, Element ref_)
        {
            Vector3 mr = new Vector3(0, stop.get_external_radius(), 0);
            Vector3 top = new Vector3(0, stop.get_shape().get_outter_radius(Vector2.vector2_01), 0);
            Vector3 bot = new Vector3(0, -stop.get_shape().get_outter_radius(Vector2.vector2_01.negate()),
                0);

            Transform3 t = stop.get_transform_to(ref_);
            Rgb color = r.get_style_color(stop.get_style());
            r.group_begin("");
            r.draw_segment(t.transform(top), t.transform(mr), color);
            r.draw_segment(t.transform(bot), t.transform(mr.negate()), color);
            r.group_end();
        }

        void draw_2d_e(Renderer r, Surface surface, Element ref_)
        {
            double top_edge = surface.get_shape().get_outter_radius(Vector2.vector2_01);
            double top_hole = surface.get_shape().get_hole_radius(Vector2.vector2_01);

            double bot_edge = -surface.get_shape().get_outter_radius(Vector2.vector2_01.negate());
            double bot_hole = -surface.get_shape().get_hole_radius(Vector2.vector2_01.negate());

            int res = Math.Max(
                100,
                Math.Min(4, (int) (Math.Abs(top_edge - bot_edge) / r.get_feature_size())));

            Rgb color = r.get_style_color(surface.get_style());

            if (Math.Abs(bot_hole - top_hole) > 1e-6)
            {
                Vector2[] p = new Vector2[res / 2];

                get_2d_points(surface, p, top_edge, top_hole, ref_);
                r.draw_polygon(p, color, false, false);
                get_2d_points(surface, p, bot_hole, bot_edge, ref_);
                r.draw_polygon(p, color, false, false);
            }
            else
            {
                Vector2[] p = new Vector2[res];

                get_2d_points(surface, p, top_edge, bot_edge, ref_);
                r.draw_polygon(p, color, false, false);
            }
        }

        void get_2d_points(Surface surface, Vector2[] array, double start,
            double end, Element ref_)
        {
            int count = array.Length;

            double y1 = start;
            double step = (end - start) / (count - 1);
            int i;

            Transform3 t = surface.get_transform_to(ref_);

            for (i = 0; i < (int) count; i++)
            {
                Vector3 v = new Vector3(0.0, y1, 0.0);
                v = v.z(surface.get_curve().sagitta(v.project_xy()));

                array[i] = t.transform(v).project_zy();
                y1 += step;
            }
        }
    }
}