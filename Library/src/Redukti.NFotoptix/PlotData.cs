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

namespace Redukti.Nfotopix
{
    public class PlotData
    {
        DataSet _set;
        Rgb _color;
        int _style;
        string _label;

        /**
     * Create a new data plot descriptor which describe the
     * specified dataset.
     */
        public PlotData(DataSet s)
        {
            _set = s;
            _color = Rgb.rgb_red;
            _style = ((int) PlotStyleMask.InterpolatePlot | (int) PlotStyleMask.PointPlot);
            _label = "";
        }

        /**
     * Get the described data set
     */
        public DataSet get_set()
        {
            return _set;
        }

        /**
     * Set data set plotting label
     */
        public void set_label(string title)
        {
            _label = title;
        }

        /**
     * Get data set plotting label
     */
        string get_label()
        {
            return _label;
        }

        /**
     * Set data set plotting color
     */
        public void set_color(Rgb color)
        {
            _color = color;
        }

        /**
     * Set data set plotting color
     */
        public Rgb get_color()
        {
            return _color;
        }

        /**
     * Enable a plotting style
     */
        void enable_style(PlotStyleMask style)
        {
            this._style |= (int) style;
        }

        /**
     * Disable a plotting style
     */
        public void disable_style(PlotStyleMask style)
        {
            this._style &= ~((int) style);
        }

        /**
     * Set the plotting style mask
     */
        public void set_style(int style)
        {
            this._style = style;
        }

        /**
     * Get the plotting style mask
     */
        public int get_style()
        {
            return _style;
        }
    }
}