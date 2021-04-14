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

namespace Redukti.Nfotopix {


public class PlotAxes {
    /**
     * Specify axes
     */
    public enum AxisMask {
        X = 1,
        Y = 2,
        Z = 4,
        XY = 3,
        YZ = 6,
        XZ = 5,
        XYZ = 7;
    }

    enum step_mode_e {
        step_interval,
        step_count,
        step_base
    }

    class Axis {
        public Axis() {
            _axis = true;
            _tics = true;
            _values = true;
            _step_mode = step_mode_e.step_base;
            _count = 5;
            _step_base = 10.0;
            _si_prefix = false;
            _pow10_scale = true;
            _pow10 = 0;
            _unit = "";
            _label = "";
            _range = new Range(0, 0);
        }

        public bool _axis;
        public bool _tics;
        public bool _values;

        public step_mode_e _step_mode;
        public int _count;
        public double _step_base;
        public bool _si_prefix;
        public bool _pow10_scale;
        public int _pow10;

        public string _unit;
        public string _label;
        public Range _range;
    }

    Axis[] _axes = new Axis[]{new Axis(), new Axis(), new Axis()};
    bool _grid;
    bool _frame;
    Vector3 _pos;
    Vector3 _origin;

    public PlotAxes() {
        _grid = false;
        _frame = true;
        _pos = Vector3.vector3_0;
        _origin = Vector3.vector3_0;
    }

    static readonly int[] _axes_bits = new int[] {(int)AxisMask.X, (int)AxisMask.Y, (int)AxisMask.Z};

    /**
     * This sets distance between axis tics to specified value.
     * see set_tics_count, set_tics_base
     */
    void set_tics_step(double step, AxisMask a) {
        for (int i = 0; i < _axes_bits.Length; i++) {
            if (((int)a & _axes_bits[i]) != 0) {
                _axes[i]._step_base = step;
                _axes[i]._step_mode = step_mode_e.step_interval;
            }
        }
    }

    void set_tics_step(double step) {
        set_tics_step(step, AxisMask.XYZ);
    }

    /**
     * @This sets tics count. @see {set_tics_step, set_tics_base}
     */
    public void set_tics_count(int count, AxisMask a) {
        for (int i = 0; i < _axes_bits.Length; i++) {
            if (((int)a & _axes_bits[i]) != 0) {
                _axes[i]._count = count;
                _axes[i]._step_mode = step_mode_e.step_count;
            }
        }
    }

    void set_tics_count(int count) {
        set_tics_count(count, AxisMask.XYZ);
    }

    /**
     * @This sets distance between axis tics to best fit power of
     * specified base divided by sufficient factor of 2 and 5 to
     * have at least @tt min_count tics. @see {set_tics_step,
     * set_tics_count}
     */
    void set_tics_base(int min_count, double base_, AxisMask a) {
        for (int i = 0; i < _axes_bits.Length; i++) {
            if (((int)a & _axes_bits[i]) != 0) {
                _axes[i]._count = min_count;
                _axes[i]._step_base = base_;
                _axes[i]._step_mode = step_mode_e.step_base;
            }
        }
    }

    void set_tics_base() {
        set_tics_base(5, 10.0, AxisMask.XYZ);
    }

    /**
     * This sets axis tics values origin.
     */
    public void set_origin(Vector3 origin) {
        _origin = origin;
    }

    /**
     * This returns axes tics values origin.
     */
    Vector3 get_origin() {
        return _origin;
    }

    /**
     * This returns axis position
     */
    public void set_position(Vector3 position) {
        _pos = position;
    }

    /**
     * This returns axis position
     */
    Vector3 get_position() {
        return _pos;
    }

    /**
     * This sets grid visibility. Grid points use tic
     * step.
     */
    void set_show_grid(bool show) {
        _grid = show;
    }

    /**
     * see set_show_grid
     */
    bool get_show_grid() {
        return _grid;
    }

    /**
     * @This sets frame visibility.
     */
    void set_show_frame(bool show) {
        _frame = show;
    }

    /**
     * see set_show_frame
     */
    bool get_show_frame() {
        return _frame;
    }

    /**
     * @This sets axes visibility.
     */
    public void set_show_axes(bool show, AxisMask a) {
        for (int i = 0; i < _axes_bits.Length; i++) {
            if (((int)a & _axes_bits[i]) != 0) {
                _axes[i]._axis = show;
            }
        }
    }

    /**
     * see set_show_axes
     */
    bool get_show_axes(int axis) {
        return _axes[axis]._axis;
    }

    /**
     * This sets tics visibility. Tics are located on axes and
     * frame. see {set_show_axes, set_show_frame}
     */
    void set_show_tics(bool show, AxisMask a) {
        for (int i = 0; i < _axes_bits.Length; i++) {
            if (((int)a & _axes_bits[i]) != 0) {
                _axes[i]._tics = show;
                _axes[i]._axis |= show;
            }
        }
    }

    /**
     * see set_show_tics
     */
    bool get_show_tics(int axis) {
        return _axes[axis]._tics;
    }

    /**
     * @This sets tics value visibility. When frame is visible,
     * tics value is located on frame tics instead of axes tics.
     * @see {set_show_axes, set_show_frame}
     */
    void set_show_values(bool show, AxisMask a) {
        for (int i = 0; i < _axes_bits.Length; i++) {
            if (((int)a & _axes_bits[i]) != 0) {
                _axes[i]._values = show;
                _axes[i]._tics |= show;
                _axes[i]._axis |= show;
            }
        }
    }

    /**
     * see set_show_values
     */
    bool get_show_values(int axis) {
        return _axes[axis]._values;
    }

    /**
     * This set axis label
     */
    public void set_label(string label, AxisMask a) {
        for (int i = 0; i < _axes_bits.Length; i++) {
            if (((int)a & _axes_bits[i]) != 0) {
                _axes[i]._label = label;
            }
        }
    }

    /**
     * This sets axis unit.
     * <p>
     * When @tt pow10_scale is set, value will be scaled to shorten
     * their length and appropriate power of 10 factor will be
     * displayed in axis label.
     * <p>
     * If @tt si_prefix is set, SI letter decimal prefix is used
     * and the @tt pow10 parameter can be used to scale base unit
     * by power of ten (useful when input data use scaled SI base unit).
     */
    public void set_unit(string unit, bool pow10_scale,
                  bool si_prefix, int pow10, AxisMask a) {
        for (int i = 0; i < _axes_bits.Length; i++) {
            if (((int)a & _axes_bits[i]) != 0) {
                _axes[i]._si_prefix = si_prefix;
                _axes[i]._unit = unit;
                _axes[i]._pow10_scale = pow10_scale;
                _axes[i]._pow10 = pow10;
            }
        }
    }

    /**
     * Get axis label
     */
    string get_label(int axis) {
        return _axes[axis]._label;
    }

    /**
     * Set value range for given axis. Default range is [0,0] which
     * means automatic range.
     */
    void set_range(Range r, AxisMask a) {
        for (int i = 0; i < _axes_bits.Length; i++) {
            if (((int)a & _axes_bits[i]) != 0) {
                _axes[i]._range = r;
            }
        }
    }

    /**
     * get distance between axis tics
     */
    double get_tics_step(int index, Range r) {
        Axis a = _axes[index];
        double d = r.second - r.first;

        switch (a._step_mode) {
            case step_mode_e.step_interval:
                return d > 0 ? a._step_base : -a._step_base;

            case step_mode_e.step_count:
                return d / (double) a._count;

            case step_mode_e.step_base: {
                if (d == 0.0)
                    return 1;

                double da = Math.Abs(d);
                double p = Math.Floor(Math.Log(da) / Math.Log(a._step_base));
                double n = Math.Pow(a._step_base, p);
                int f = 1;

                while ((int) (da / n * f) < a._count) {
                    if ((int) (da / n * f * 2) >= a._count) {
                        f *= 2;
                        break;
                    } else if ((int) (da / n * f * 5) >= a._count) {
                        f *= 5;
                        break;
                    } else {
                        f *= 10;
                    }
                }

                n /= f;

                return d > 0 ? n : -n;
            }

            default:
                throw new InvalidOperationException();
        }
    }

}

}