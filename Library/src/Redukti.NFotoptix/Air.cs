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

    /**
     * Air optical material model
     * This class models optical properties of air. Refractive index
     * of air depends on temperature and pressure.
     * <p>
     * This class provides different formulas:
     * <ol>
     * <li>AirBirch94 : described in Birch, Metrologia, 1994, 31, 315.</li>
     * <li>AirKohlrausch68 : described in F. Kohlrausch, Praktische Physik, 1968, 1, 408}</li>
     * </ol>
     * <p>
     * Global variables air and std_air are available with
     * default parameters and Birch model.
     */
    public class Air : MaterialBase
    {

        /**
         * Standard air pressure is 101325 @em Pa
         */
        static readonly double std_pressure = 101325.0;
        public static readonly Air std_air = new Air(AirFormula.AirBirch94Formula);
        public static readonly Air air = new Air(AirFormula.AirBirch94Formula);

        public readonly double _pressure;
        public readonly AirFormula airFormula;

        public Air(AirFormula formula, double pressure) : base("air", 20.0)
        {
            this.airFormula = formula;
            this._pressure = pressure;
        }

        public Air(AirFormula formula, double pressure, double temp) : base("air", temp)
        {
            this.airFormula = formula;
            this._pressure = pressure;
        }

        public Air(AirFormula formula) : this(formula, std_pressure)
        {
        }



        public override bool is_opaque()
        {
            return false;
        }


        public override bool is_reflecting()
        {
            return false;
        }


        public override double get_internal_transmittance(double wavelen, double thickness)
        {
            // FIXME find a formula
            return 1.0;
        }


        public override double get_refractive_index(double wavelen)
        {
            switch (airFormula)
            {
                case AirFormula.AirBirch94Formula:
                    {
                        // Birch, Metrologia, 1994, 31, 315

                        // _temperature in celsius
                        // _pressure in pascal

                        double s2 = MathUtils.square(1 / (wavelen / 1000.0));

                        double ref1
                                = /*1.0 +*/ 1e-8
                                * (+8342.54 + 2406147.0 / (130.0 - s2) + 15998.0 / (38.9 - s2));

                        return 1.0
                                + (ref1 /*- 1.0*/)
                                * (_pressure
                                * (1.0
                                + _pressure * (60.1 - 0.972 * _temperature)
                                * 1e-10))
                                / (96095.43 * (1.0 + 0.003661 * _temperature));
                    }

                case AirFormula.AirKohlrausch68Formula:
                    {
                        // F. Kohlrausch, Praktische Physik, 1968, 1, 408

                        double w2 = MathUtils.square(wavelen / 1000.0);

                        double nref = 1.0
                                + (+6432.8 + (2949810.0 * w2) / (146.0 * w2 - 1.0)
                                + (25540.0 * w2) / (41.0 * w2 - 1.0))
                                * 1e-8;

                        return 1.0
                                + (((nref - 1.0) * (_pressure / std_pressure))
                                / (1.0 + (_temperature - 15.0) * 0.0034785));
                    }
            }
            throw new InvalidOperationException();
        }

        public override double get_extinction_coef(double wavelen)
        {
            // FIXME find a formula
            return 0.0;
        }

    }

}