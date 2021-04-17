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

namespace Redukti.Nfotopix
{
    public abstract class DiscreteSetBase : Set1d
    {
        class EntryS
        {
            public readonly double x, y, d;

            public EntryS(double x, double y, double d)
            {
                this.x = x;
                this.y = y;
                this.d = d;
            }
        }

        List<EntryS> _data = new List<EntryS>();

        /**
     * Insert data pair in data set. If a pair with the same x
     * value exists, it will be replaced by the new
     * value. Derivative value may be provided as well.
     */
        public void add_data(double x, double y, double d)
        {
            EntryS e = new EntryS(x, y, d);

            _version++;

            int di = get_interval(x);

            if (di > 0 && (_data[di - 1].x == x))
                _data.Insert(di - 1, e);
            else
                _data.Insert(di, e);
            invalidate();
        }


        protected abstract void invalidate();

        /**
     * Clear all data
     */
        public void clear()
        {
            _data.Clear();
            _version++;
            invalidate();
        }

        /**
     * Get stored derivative value at index x
     */
        public double get_d_value(int n)
        {
            return _data[n].d;
        }

        // inherited from Set1d
        override public int get_count()
        {
            return _data.Count;
        }

        override public double get_x_value(int n)
        {
            return _data[n].x;
        }

        override public double get_y_value(int n)
        {
            return _data[n].y;
        }

        override public Range get_x_range()
        {
            if (_data.Count == 0)
                throw new InvalidOperationException("_data set contains no _data");
            return new Range(_data[0].x, _data[_data.Count - 1].x);
        }

        /**
     * find lower bound index of interval containing value
     */
        public int get_interval(double x)
        {
            int min_idx = 0;
            int max_idx = _data.Count + 1;

            while (max_idx - min_idx > 1)
            {
                int p = (max_idx + min_idx) / 2;

                if (x >= _data[p - 1].x)
                    min_idx = p;
                else
                    max_idx = p;
            }

            return min_idx;
        }

        /**
     * find nearest value index
     */
        public int get_nearest(double x)
        {
            int min_idx = 0;
            int max_idx = _data.Count;

            while (max_idx - min_idx > 1)
            {
                int p = (max_idx + min_idx) / 2;

                if (x + x >= _data[p - 1].x + _data[p].x)
                    min_idx = p;
                else
                    max_idx = p;
            }

            return min_idx;
        }

        public double get_x_interval(int x)
        {
            return _data[x + 1].x - _data[x].x;
        }

        public double get_x_interval(int x1, int x2)
        {
            return _data[x2].x - _data[x1].x;
        }
    }
}