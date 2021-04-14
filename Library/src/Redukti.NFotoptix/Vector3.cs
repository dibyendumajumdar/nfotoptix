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


/**
 * Vector with 3 components named x,y,z.
 * Note that in the optical system the lens axis is z.
 */
public class Vector3 {

    private static readonly int N = 3;

    public static readonly Vector3 vector3_0 = new Vector3(0.0, 0.0, 0.0);
    public static readonly Vector3 vector3_1 = new Vector3(1.0, 1.0, 1.0);

    public static readonly Vector3 vector3_001 = new Vector3(0.0, 0.0, 1.0);
    public static readonly Vector3 vector3_010 = new Vector3(0.0, 1.0, 0.0);
    public static readonly Vector3 vector3_100 = new Vector3(1.0, 0.0, 0.0);

    readonly double[] values;

    public Vector3(double x, double y, double z) {
        if (Double.IsNaN(x) || Double.IsNaN(y) || Double.IsNaN(z)) {
            throw new InvalidOperationException("NaN");
        }
        this.values = new double[N];
        this.values[0] = x;
        this.values[1] = y;
        this.values[2] = z;
    }
    public Vector3(double v): this(v, v, v) {
    }

    private Vector3(double[] values) {
        this.values = values;
    }

    public double x() {
        return this.values[0];
    }

    public double y() {
        return this.values[1];
    }

    public double z() {
        return this.values[2];
    }

    public Vector3 x(double v) {
        return new Vector3(v, y(), z());
    }
    public Vector3 y(double v) {
        return new Vector3(x(), v, z());
    }
    public Vector3 z(double v) {
        return new Vector3(x(), y(), v);
    }

    public double dot(Vector3 v)
    {
        double r = 0;
        for (int i = 0; i < N; i++)
            r += values[i] * v.values[i];
        return r;
    }

    /**
     * The cross product a × b is defined as a vector c that is
     * perpendicular (orthogonal) to both a and b, with a direction given by the right-hand rule
     * and a magnitude equal to the area of the parallelogram that the vectors span.
     *
     * https://en.wikipedia.org/wiki/Cross_product
     */
    public Vector3 cross(Vector3 b) {
        return new Vector3(y() * b.z() - z() * b.y(),
                z() * b.x() - x() * b.z(),
                x() * b.y() - y() * b.x());
    }

    public Vector3 plus(Vector3 v)
    {
        double[] r = new double[N];
        for (int i = 0; i < N; i++)
            r[i] = values[i] + v.values[i];
        return new Vector3(r);
    }

    public Vector3 minus(Vector3 v)
    {
        double[] r = new double[N];
        for (int i = 0; i < N; i++)
            r[i] = values[i] - v.values[i];
        return new Vector3(r);
    }

    public Vector3 negate()
    {
        double[] r = new double[N];
        for ( int i = 0; i < N; i++)
            r[i] = -values[i];
        return new Vector3(r);
    }

    public Vector2 project_xy() {
        return new Vector2(x (), y());
    }
    public Vector2 project_zy() {
        return new Vector2(z (), y());
    }

    public double len ()
    {
        double r = 0;
        for (int i = 0; i < N; i++)
            r += MathUtils.square (values[i]);
        return Math.Sqrt (r);
    }

    public Vector3 times(double scale) {
        double[] r = new double[N];
        for (int i = 0; i < N; i++)
            r[i] = values[i] * scale;
        return new Vector3(r);
    }

    public Vector3 divide(double scale) {
        double[] r = new double[N];
        for (int i = 0; i < N; i++)
            r[i] = values[i] / scale;
        return new Vector3(r);
    }

    public Vector3 normalize() {
        return this.divide(len());
    }

    public double v(int i) {
        return this.values[i];
    }
    public Vector3 v(int i, double d) {
        double[] val = new double[this.values.Length];
        Array.Copy(val, values, values.Length);
        val[i] = d;
        return new Vector3(val);
    }

    
    override public string ToString() {
        return "[" + x() + ',' + y() + ',' + z() + ']';
    }

    public bool isEqual(Vector3 other, double tolerance) {
        return Math.Abs(this.x() - other.x()) < tolerance &&
                Math.Abs(this.y() - other.y()) < tolerance &&
                Math.Abs(this.z() - other.z()) < tolerance;
    }
}

}