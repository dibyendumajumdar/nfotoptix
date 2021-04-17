using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Redukti.Nfotopix
{
public class TestShapes {

    class shape_test_s {
        public string name;
        public Shape s;

        public shape_test_s(string name, Shape s) {
            this.name = name;
            this.s = s;
        }
    }

    shape_test_s[] st = {
            new shape_test_s("disk", new Disk(30.0)),
            new shape_test_s("rectangle", new Rectangle(70.0, 40.0)),
            new shape_test_s("ellipse1", new Ellipse(20, 35)),
            new shape_test_s("ellipse2", new Ellipse(35, 20)),
    };

    [Test]
    public void testShapes() {
        for (int i = 0; i < st.Length; i++) {
            shape_test_s s = st[i];

            string fname = String.Format("test_shape_{0}.svg", s.name);

            RendererSvg rsvg = new RendererSvg(800, 600, Rgb.rgb_black);
            RendererViewport r = rsvg;

            r.set_window(Vector2.vector2_0, 70.0, true);

            {
                ConsumerTriangle2 d = (Triangle2 t) => {
                    r.draw_triangle(t, true, new Rgb(.2, .2, .2, 1.0));
                    r.draw_triangle(t, false, Rgb.rgb_gray);
                };
                s.s.get_triangles(d, 10.0);
            }

            for (int c = 0; c < s.s.get_contour_count(); c++) {

                List<Vector2> poly = new ();
                PatternConsumer d = (Vector2 v) => {
                    poly.Add(v);
                };
                s.s.get_contour(c, d, 10.0);
                r.draw_polygon(poly.ToArray(), Rgb.rgb_yellow, false, true);
            }

            for (double a = 0; a < 2.0 * Math.PI - 1e-8; a += 2.0 * Math.PI / 50.0) {
                Vector2 d = new Vector2(Math.Cos(a), Math.Sin(a));

                double ro = s.s.get_outter_radius(d);
                r.draw_point(d.times(ro), Rgb.rgb_magenta, Renderer.PointStyle.PointStyleCross);
                double rh = s.s.get_hole_radius(d);
                r.draw_point(d.times(rh), Rgb.rgb_cyan, Renderer.PointStyle.PointStyleCross);
            }

            r.draw_circle(Vector2.vector2_0, s.s.max_radius(), Rgb.rgb_red, false);
            r.draw_circle(Vector2.vector2_0, s.s.min_radius(), Rgb.rgb_blue, false);

            r.draw_box(s.s.get_bounding_box(), Rgb.rgb_cyan);

            {
                PatternConsumer d = (Vector2 v) => {
                    r.draw_point(v, Rgb.rgb_green, Renderer.PointStyle.PointStyleDot);
                };
                Distribution dist = new Distribution();
                ((ShapeBase) s.s).get_base_pattern(d, dist, false);
            }

            Console.WriteLine(rsvg.write(new StringBuilder()).ToString());
        }

    }

}

}