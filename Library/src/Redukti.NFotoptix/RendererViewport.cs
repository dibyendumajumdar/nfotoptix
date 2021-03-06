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
    public abstract class RendererViewport : Renderer
    {
        /**
     * Current 2d viewport window
     */
        protected Vector2Pair _window2d_fit;

        /**
     * Current 2d viewport window (with margins)
     */
        protected Vector2Pair _window2d;

        /**
     * 2d device resolution
     */
        protected Vector2 _2d_output_res;

        protected enum margin_type_e
        {
            /**
         * _margin contains a size ratio
         */
            MarginRatio,

            /**
         * _margin contains the width in window size units
         */
            MarginLocal,

            /**
         * _margin contains the width in output size units
         */
            MarginOutput,
        }

        protected margin_type_e _margin_type;

        /**
     * Margin size or ratio
     */
        protected Vector2Pair _margin;

        /**
     * Current layout rows and columns counts
     */
        protected int _rows, _cols;

        /**
     * Current page id
     */
        protected int _pageid;

        /**
     * Current 2d page window
     */
        protected Vector2Pair _page;

        protected double _fov;

        protected RendererViewport()
        {
            _margin_type = margin_type_e.MarginRatio;
            _margin = new Vector2Pair(new Vector2(0.13, 0.13), new Vector2(0.13, 0.13));
            _rows = 1;
            _cols = 1;
            _pageid = 0;
            _fov = 45.0;
            _page = Vector2Pair.vector2_pair_00;
            _window2d = Vector2Pair.vector2_pair_00;
            _window2d_fit = Vector2Pair.vector2_pair_00;
            _2d_output_res = Vector2.vector2_0;
            //_precision (3), _format ()
        }

        void set_2d_size(double width, double height)
        {
            _2d_output_res = new Vector2(width, height);
        }

        public void set_window(Vector2 center, Vector2 size, bool keep_aspect)
        {
            Vector2 s = size;

            if (keep_aspect)
            {
                double out_ratio
                    = (_2d_output_res.x() / _cols) / (_2d_output_res.y() / _rows);
                if (Math.Abs(s.x() / s.y()) < out_ratio)
                    //s.x () = s.y () * out_ratio;
                    s = new Vector2(s.y() * out_ratio, s.y());
                else
                    //s.y () = s.x () / out_ratio;
                    s = new Vector2(s.x(), s.x() / out_ratio);
            }

            Vector2 sby2 = s.divide(2.0);
            //  (center - s / 2., center + s / 2.)
            _window2d_fit = new Vector2Pair(center.minus(sby2), center.plus(sby2));

            Vector2 ms0 = sby2;
            Vector2 ms1 = sby2;

            switch (_margin_type)
            {
                case margin_type_e.MarginLocal:
//                ms[0] = ms[0] + _margin[0];
//                ms[1] = ms[1] + _margin[1];
                    ms0 = ms0.plus(_margin.v0);
                    ms1 = ms1.plus(_margin.v1);
                    break;
                case margin_type_e.MarginRatio:
//                ms[0] = ms[0] + s.mul (_margin[0]);
//                ms[1] = ms[1] + s.mul (_margin[1]);
                    ms0 = ms0.plus(s.ebeTimes(_margin.v0));
                    ms1 = ms1.plus(s.ebeTimes(_margin.v1));
                    break;
                case margin_type_e.MarginOutput:
//                ms[0] = ms[0] / (math::vector2_1 - _margin[0] / _2d_output_res * 2);
//                ms[1] = ms[1] / (math::vector2_1 - _margin[1] / _2d_output_res * 2);
                    ms0 = ms0.ebeDivide(Vector2.vector2_1.minus(_margin.v0.ebeDivide(_2d_output_res.times(2.0))));
                    ms1 = ms1.ebeDivide(Vector2.vector2_1.minus(_margin.v1.ebeDivide(_2d_output_res.times(2.0))));
                    break;
            }

            //(center - ms[0], center + ms[1])
            _window2d = new Vector2Pair(center.minus(ms0), center.plus(ms1));

            update_2d_window();
            set_orthographic();
            set_page(_pageid);
        }

        public void set_window(Vector2 center, double radius,
            bool keep_aspect)
        {
            Vector2 size = new Vector2(radius, radius);
            set_window(center, size, keep_aspect);
        }

        protected void update_2d_window()
        {
        }

        /**
     * Set 3d projection to orthographic, called from @mref set_window.
     */
        public abstract void set_orthographic();

        /** Set 3d perspective projection mode. This function reset the
     viewport window to (-1,1). @see set_window @see set_fov */
        public abstract void set_perspective();

        public void set_page(int page)
        {
            if (page >= _cols * _rows)
                throw new InvalidOperationException("set_page: no such page number in current layout");

            _pageid = page;
            int row = page / _cols;
            int col = page % _cols;

            Vector2 size = new Vector2(_window2d.v1.x() - _window2d.v0.x(),
                _window2d.v1.y() - _window2d.v0.y());

            Vector2 a = new Vector2(_window2d.v0.x() - size.x() * col,
                _window2d.v0.y() - size.y() * (_rows - 1 - row));

            Vector2 b = new Vector2(a.x() + size.x() * _cols, a.y() + size.y() * _rows);

            _page = new Vector2Pair(a, b);
        }

        public virtual double x_scale(double x)
        {
            return ((x / (_page.v1.x() - _page.v0.x())) * _2d_output_res.x());
        }

        public virtual double y_scale(double y)
        {
            return ((y / (_page.v1.y() - _page.v0.y())) * _2d_output_res.y());
        }

        public double x_trans_pos(double x)
        {
            return x_scale(x - _page.v0.x());
        }

        public virtual double y_trans_pos(double y)
        {
            return y_scale(y - _page.v0.y());
        }

        public Vector2Pair get_window2d_fit()
        {
            return _window2d_fit;
        }

        public Vector2Pair get_window2d()
        {
            return _window2d;
        }

        public Vector2 get_2d_output_res()
        {
            return _2d_output_res;
        }

        void set_margin_output(double width, double height)
        {
            set_margin_output(width, height, width, height);
        }

        void set_margin(double width, double height)
        {
            set_margin(width, height, width, height);
        }

        void set_margin_ratio(double width, double height)
        {
            set_margin_ratio(width, height, width, height);
        }

        void set_margin(double left, double bottom, double right,
            double top)
        {
            _margin_type = margin_type_e.MarginLocal;
            _margin = new Vector2Pair(new Vector2(left, bottom), new Vector2(right, top));
            set_window(_window2d_fit, false);
        }

        void set_margin_ratio(double left, double bottom, double right,
            double top)
        {
            _margin_type = margin_type_e.MarginRatio;
            _margin = new Vector2Pair(new Vector2(left, bottom), new Vector2(right, top));
            set_window(_window2d_fit, false);
        }

        void set_margin_output(double left, double bottom, double right,
            double top)
        {
            _margin_type = margin_type_e.MarginOutput;
            _margin = new Vector2Pair(new Vector2(left, bottom), new Vector2(right, top));
            set_window(_window2d_fit, false);
        }

        public void set_window(Vector2Pair window, bool keep_aspect)
        {
            //(window[0] + window[1]) / 2
            Vector2 center = window.v0.plus(window.v1).divide(2.0);
            //(window[1].x () - window[0].x (),
            //window[1].y () - window[0].y ());
            Vector2 size = new Vector2(window.v1.x() - window.v0.x(),
                window.v1.y() - window.v0.y());
            set_window(center, size, keep_aspect);
        }

        void draw_frame_2d()
        {
            Vector2[] fr = new Vector2[4];

            fr[0] = _window2d_fit.v0;
            fr[1] = new Vector2(_window2d_fit.v0.x(), _window2d_fit.v1.y());
            fr[2] = _window2d_fit.v1;
            fr[3] = new Vector2(_window2d_fit.v1.x(), _window2d_fit.v0.y());

            draw_polygon(fr, get_style_color(Style.StyleForeground), false, true);
        }

        public void set_page_layout(int cols, int rows)
        {
            _cols = cols;
            _rows = rows;
            set_page(0);
        }

        public void set_feature_size(double v)
        {
            _feature_size = v;
        }

        public void set_camera_direction(Vector3 dir)
        {
            Transform3 t = get_camera_transform();
            t = t.set_direction(dir);
            set_camera_transform(t);
        }

        public void set_camera_position(Vector3 pos)
        {
            Transform3 t = get_camera_transform();
            t = t.set_translation(pos);
            set_camera_transform(t);
        }

        /** Get reference to 3d camera transform */
        public abstract Transform3 get_camera_transform();

        /** Get modifiable reference to 3d camera transform */
        public abstract void set_camera_transform(Transform3 t);
    }
}