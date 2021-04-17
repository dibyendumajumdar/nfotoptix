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
    public abstract class Medium
    {
        public readonly string name;
        public double _temperature; // celcius

        public Medium(string name, double temp)
        {
            this.name = name;
            this._temperature = temp;
        }

        public Medium(string name) : this(name, 20.0)
        {
        }

        /** Return true if material must be considered opaque for ray
         tracing */
        public abstract bool is_opaque();

        /** Return true if material may reflect most of the light and
         must be considered as a mirror when ignoring ray intensity
         computation during ray tracing. */
        public abstract bool is_reflecting();

        /** Get material internal transmittance for thickness in
         mm. Subclasses _must_ provide this function or the
         get_extinction_coef() function. */
        public virtual double get_internal_transmittance(double wavelen,
            double thickness)
        {
            // compute internal transmittance from extinction coefficient
            // Beer-Lambert law

            // FIXME simplify and check
            double tr
                = Math.Exp(-(4 * Math.PI * get_extinction_coef(wavelen) * 0.001 /* 1 mm */)
                           / (wavelen * 1e-9f));

            return Math.Pow(tr, thickness);
        }

        /** Get material internal transmittance for 1mm thickness. */
        public virtual double get_internal_transmittance(double wavelen)
        {
            // compute internal transmittance from extinction coefficient
            return get_internal_transmittance(wavelen, 1.0);
        }

        /** Get material absolute refractive index at specified wavelen in @em nm. */
        public abstract double get_refractive_index(double wavelen);

        /** Get material relative refractive index in given medium at specified
         * wavelen in @em nm. */
        public virtual double get_refractive_index(double wavelen, Medium env)
        {
            return get_refractive_index(wavelen) / env.get_refractive_index(wavelen);
        }

        /** Get extinction coefficient. Subclasses _must_ provide this
         function or the get_internal_transmittance() function. */
        public virtual double get_extinction_coef(double wavelen)
        {
            // Beer-Lambert law
            // FIXME check this formula
            return -(Math.Log(get_internal_transmittance(wavelen, 1.0)) * (wavelen * 1e-9f))
                   / (4 * Math.PI * 0.001 /* 1 mm */);
        }


        /** Get reflectance at normal incidence */
        public virtual double get_normal_reflectance(Medium from,
            double wavelen)
        {
            // default reflectance at normal incidence, valid for metal and dielectric
            // material
            // McGraw Hill, Handbook of optics, vol1, 1995, 5-10 (47)

            double n0 = from.get_refractive_index(wavelen);
            double k12 = MathUtils.square(get_extinction_coef(wavelen));
            double n1 = get_refractive_index(wavelen);
            double res = (MathUtils.square(n0 - n1) + k12) / (MathUtils.square(n0 + n1) + k12);

            return res;
        }

        /** Get transmittance at normal incidence */
        public virtual double get_normal_transmittance(Medium from,
            double wavelen)
        {
            // default transmittance at normal incidence, valid for non absorbing material
            // McGraw Hill, Handbook of optics, vol1, 1995, 5-8 (23)

            double n0 = from.get_refractive_index(wavelen);
            double n1 = get_refractive_index(wavelen);

            return (4.0 * n0 * n1) / MathUtils.square(n0 + n1);
        }

        /** Get material color and alpha */
        public virtual Rgb get_color()
        {
            // FIXME color depends on material properties
            return new Rgb(1, 1, 1, 1);
        }

        public virtual double get_temperature()
        {
            return _temperature;
        }

        public void set_temperature(double temp)
        {
            _temperature = temp;
        }
    }
}