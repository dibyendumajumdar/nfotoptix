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
using System.Text;

namespace Redukti.Nfotopix {


/**
 * SVG file rendering driver
 * <p>
 * This class implements a SVG graphic output driver.
 */
public class RendererSvg : Renderer2d {

    StringBuilder _out = new StringBuilder();
    //readonly DecimalFormat _decimal_format;

    string format(double value) {
        //return string.format("%.3f", value);
        return _decimal_format.format(value);
    }

    /**
     * Create a new svg renderer with given resolution. The
     *
     * write function must be used to write svg to output
     * stream.
     */
    public RendererSvg(double width, double height,
                Rgb bg) {
        // _decimal_format = MathUtils.decimal_format();
        _2d_output_res = new Vector2(width, height);
        _styles_color[(int)Style.StyleBackground] = bg;
        _styles_color[(int)Style.StyleForeground] = bg.negate();

        clear();
    }

    public RendererSvg(double width, double height): this(width, height, Rgb.rgb_white) {
    }

    /**
     * Create a new svg renderer with given resolution. The
     *
     * write function must be used to write svg to output
     * stream.
     */
    public RendererSvg(): this(800, 600, Rgb.rgb_white) {
    }

    void svg_begin_rect(double x1, double y1, double x2, double y2,
                        bool terminate) {
        _out.Append("<rect ")
                .Append("x=\"").Append(format(x1)).Append("\" ")
                .Append("y=\"").Append(format(y1)).Append("\" ")
                .Append("width=\"").Append(format(x2 - x1)).Append("\" ")
                .Append("height=\"").Append(format(y2 - y1)).Append("\" ");

        if (terminate)
            _out.Append(" />").Append('\n');
    }

    void svg_begin_line(double x1, double y1, double x2, double y2,
                        bool terminate) {
        _out.Append("<line ")
                .Append("x1=\"").Append(format(x1)).Append("\" ")
                .Append("y1=\"").Append(format(y1)).Append("\" ")
                .Append("x2=\"").Append(format(x2)).Append("\" ")
                .Append("y2=\"").Append(format(y2)).Append("\" ");

        if (terminate)
            _out.Append(" />").Append('\n');
    }

    void svg_begin_ellipse(double x, double y, double rx, double ry,
                           bool terminate) {
        _out.Append("<ellipse ")
                .Append("cx=\"").Append(format(x)).Append("\" ")
                .Append("cy=\"").Append(format(y)).Append("\" ")
                .Append("rx=\"").Append(format(rx)).Append("\" ")
                .Append("ry=\"").Append(format(ry)).Append("\" ");

        if (terminate)
            _out.Append(" />").Append('\n');
    }

    StringBuilder write_srgb(Rgb rgb) {
        _out.Append(string.format("#%02x%02x%02x", (int) (rgb.r * 255.0),
                (int) (rgb.g * 255.0), (int) (rgb.b * 255.0)));
        return _out;
    }

    void svg_add_fill(Rgb rgb) {
        _out.Append(" fill=\"");
        write_srgb(rgb);
        _out.Append("\"");
    }

    void svg_end() {
        _out.Append(" />").Append('\n');
    }

    public void clear() {
        _out.Clear();

        // background
        svg_begin_rect(0.0, 0.0, _2d_output_res.x(), _2d_output_res.y(), false);
        svg_add_fill(get_style_color(Style.StyleBackground));
        svg_end();

        _out.Append("<defs>").Append('\n');

        // dot shaped point
        _out.Append("<g id=\"")
                .Append("dot")
                .Append("\">").Append('\n');
        svg_begin_line(1, 1, 0, 0, true);
        _out.Append("</g>").Append('\n');

        // cross shaped point
        _out.Append("<g id=\"")
                .Append("cross")
                .Append("\">").Append('\n');
        svg_begin_line(-3, 0, 3, 0, true);
        svg_begin_line(0, -3, 0, 3, true);
        _out.Append("</g>").Append('\n');

        // square shaped point
        _out.Append("<g id=\"")
                .Append("square")
                .Append("\">").Append('\n');
        svg_begin_line(-3, -3, -3, 3, true);
        svg_begin_line(-3, 3, 3, 3, true);
        svg_begin_line(3, 3, 3, -3, true);
        svg_begin_line(3, -3, -3, -3, true);
        _out.Append("</g>").Append('\n');

        // round shaped point
        _out.Append("<g id=\"")
                .Append("round")
                .Append("\">").Append('\n');
        svg_begin_ellipse(0, 0, 3, 3, false);
        _out.Append(" fill=\"none\" />");
        _out.Append("</g>").Append('\n');

        // triangle shaped point
        _out.Append("<g id=\"")
                .Append("triangle")
                .Append("\">").Append('\n');
        svg_begin_line(0, -3, -3, 3, true);
        svg_begin_line(-3, 3, 3, 3, true);
        svg_begin_line(0, -3, +3, +3, true);
        _out.Append("</g>").Append('\n');

        _out.Append("</defs>").Append('\n');
    }

    
    public override void group_begin(string name) {
        _out.Append("<g>");
        if (name.Length == 0)
            _out.Append("<title>").Append(name).Append("</title>");
        _out.Append('\n');
    }

    
    public override void group_end() {
        _out.Append("</g>").Append('\n');
    }

    void svg_begin_use(string id, double x, double y,
                       bool terminate) {
        _out.Append("<use ")
                .Append("x=\"").Append(format(x)).Append("\" ")
                .Append("y=\"").Append(format(y)).Append("\" ")
                .Append("xlink:href=\"#").Append(id).Append("\" ");

        if (terminate)
            _out.Append(" />").Append('\n');
    }

    void svg_add_stroke(Rgb rgb) {
        _out.Append(" stroke=\"");
        write_srgb(rgb);
        _out.Append("\"");
    }

    void svg_add_id(string id) {
        _out.Append(" fill=\"").Append(id).Append("\"");
    }

    static string[] ids = {"dot", "cross", "round", "square", "triangle"};

    
    public override void draw_point(Vector2 p, Rgb rgb, PointStyle s) {
        if ((int)s >= ids.Length)
            s = PointStyle.PointStyleCross;

        Vector2 v2d = trans_pos(p);

        svg_begin_use(ids[(int)s], v2d.x(), v2d.y(), false);
        svg_add_stroke(rgb);
        svg_end();
    }

    
    public override void draw_segment(Vector2Pair l, Rgb rgb) {
        Vector2 v2da = trans_pos(l.v0);
        Vector2 v2db = trans_pos(l.v1);

        svg_begin_line(v2da.x(), v2da.y(), v2db.x(), v2db.y(), false);
        svg_add_stroke(rgb);
        svg_end();
    }

    
    public override void draw_circle(Vector2 c, double r, Rgb rgb, bool filled) {
        Vector2 v2d = trans_pos(c);

        svg_begin_ellipse(v2d.x(), v2d.y(), x_scale(r), y_scale(r), false);
        svg_add_stroke(rgb);
        if (filled)
            svg_add_fill(rgb);
        else
            _out.Append(" fill=\"none\"");
        svg_end();
    }

    
    public override void draw_text(Vector2 v, Vector2 dir,
                          string str, int a, int size,
                          Rgb rgb) {
        int margin = size / 2;
        Vector2 v2d = trans_pos(v);
        double x = v2d.x();
        double y = v2d.y();
        double yo = y, xo = x;

        _out.Append("<text style=\"font-size:").Append(size).Append(";");

        if ((a & (int)TextAlignMask.TextAlignLeft) != 0) {
            //_out << "text-align:left;text-anchor:start;";
            x += margin;
        } else if ((a & (int)TextAlignMask.TextAlignRight) != 0) {
            _out.Append("text-align:right;text-anchor:end;");
            x -= margin;
        } else
            _out.Append("text-align:center;text-anchor:middle;");

        if ((a & (int)TextAlignMask.TextAlignTop) != 0)
            y += size + margin;
        else if ((a & (int)TextAlignMask.TextAlignBottom) != 0)
            y -= margin;
        else
            y += size / 2.0;

        _out.Append("\" x=\"").Append(format(x)).Append("\" y=\"").Append(format(y)).Append("\"");

        double ra = Math.toDegrees(Math.Atan2(-dir.y(), dir.x()));
        if (ra != 0)
            _out.Append(" transform=\"rotate(").Append(format(ra)).Append(",").Append(format(xo)).Append(",").Append(format(yo)).Append(")\"");

        svg_add_fill(rgb);

        _out.Append(">").Append(str).Append("</text>").Append('\n');
    }

    
    public override void draw_polygon(Vector2[] array,
                             Rgb rgb, bool filled, bool closed) {
        if (array.Length < 3)
            return;

        closed = closed || filled;

        if (closed) {
            _out.Append("<polygon");

            if (filled)
                svg_add_fill(rgb);
            else {
                _out.Append(" fill=\"none\"");
                svg_add_stroke(rgb);
            }
        } else {
            _out.Append("<polyline fill=\"none\"");

            svg_add_stroke(rgb);
        }

        _out.Append(" points=\"");

        for (int i = 0; i < array.Length; i++) {
            Vector2 v2d = trans_pos(array[i]);

            _out.Append(format(v2d.x())).Append(",").Append(format(v2d.y())).Append(" ");
        }

        _out.Append("\" />").Append('\n');
    }


    Vector2 trans_pos(Vector2 v) {
        return new Vector2(x_trans_pos(v.x()), y_trans_pos(v.y()));
    }

    double y_trans_pos(double y) {
        return (((y - _page.v1.y()) / (_page.v0.y() - _page.v1.y()))
                * _2d_output_res.y());
    }

    public StringBuilder write(StringBuilder s) {
        s.Append("<?xml version=\"1.0\" standalone=\"no\"?>").Append('\n');

        s.Append("<svg width=\"").Append(format(_2d_output_res.x())).Append("px\" height=\"")
                .Append(format(_2d_output_res.y())).Append("px\" ")
                .Append("version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" ")
                .Append("xmlns:xlink=\"http://www.w3.org/1999/xlink\">")
                .Append('\n');

        // content
        s.Append(_out);

        s.Append("</svg>").Append('\n');
        return s;
    }

}

}