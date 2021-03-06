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
 * Dielectric optical material model base class.
 *
 * @module {Core}       get_refractive_index function when no temperature model
 * is in use.
 * <p>
 * Temperature coefficients can be defined to take current
 * material temperature into account when computing absolute
 * efractive index:
 *
 * <ol>
 * <li> The @ref set_temperature_dndt function enables use of
 * refractive index temperature deviation factor.</li>
 *
 * <li>The @ref set_temperature_schott function enables Schott
 * temperature model as described in Schott TIE-19:
 * Temperature Coefficient of the Refractive Index technical
 * information document. It uses the following formula:
 * @math $ n_t = \frac{{n}^{2}-1.0}{2\times n} \times \Delta t
 * \times \left( \frac{\Delta t\times
 * E_1+E_0}{{\lambda}^{2}-{\lambda_{tk}}^{2}} + D_2\times {\Delta
 * t}^{2}+D_1\times \Delta t+D_0 \right) $
 * <p>
 * with @math $ \Delta t = t - t_{ref} $
 * and @math $\lambda$ the micrometer wavelength.
 *
 * </li>
 * </ol>
 */
public abstract class Dielectric : Solid {

    /**
     * normalized 1mm thickness transmittance data
     */
    DiscreteSet _transmittance;

    /**
     * refractive index thermal data
     */
    enum thermal_model_e {
        ThermalNone,
        ThermalSchott,
        ThermalDnDt
    }

    thermal_model_e _temp_model;
    double _temp_d0, _temp_d1, _temp_d2;
    double _temp_e0, _temp_e1;
    double _temp_wl_tk;

    /**
     * wavelen data validity range (nm)
     */
    double _low_wavelen;
    double _high_wavelen;

    /**
     * medium used during refractive index measurement
     */
    Medium _measurement_medium;

    double _last_wavelen;
    double _last_get_refractive_index;

    /**
     * Get material relative refractive index in measurment medium
     * at specified wavelen in @em nm.
     */
    public abstract double get_measurement_index(double wavelen);

    public Dielectric() : this("dielectric") {
    }

    protected Dielectric(string name): base(name) {
        _transmittance = new DiscreteSet();
        _temp_model = thermal_model_e.ThermalNone;
        _low_wavelen = 350.0;
        _high_wavelen = 750.0;
        _measurement_medium = Air.std_air;
        _last_wavelen = 0;
        _transmittance.setInterpolation(Interpolation.Cubic);
    }

    public override bool is_opaque() {
        return false;
    }

    public override bool is_reflecting() {
        return false;
    }

    public override double get_internal_transmittance(double wavelen, double thickness) {
        double t = _transmittance.interpolate(wavelen);

        return Math.Pow(t, thickness);
    }

    /**
     * Add transmittance data, wavelen in nm
     */
    public void set_internal_transmittance(double wavelen, double thickness,
                                           double transmittance) {
        _transmittance.add_data(wavelen, Math.Pow(transmittance, 1.0 / thickness), 0.0);
    }

    public void clear_internal_transmittance() {
        _transmittance.clear();
    }

    public void set_temperature_schott(double d0, double d1, double d2, double e0,
                                       double e1, double wl_tk) {
        _temp_model = thermal_model_e.ThermalSchott;
        _temp_d0 = d0;
        _temp_d1 = d1;
        _temp_d2 = d2;
        _temp_e0 = e0;
        _temp_e1 = e1;
        _temp_wl_tk = wl_tk;
    }

    public void set_temperature_dndt(double dndt) {
        _temp_model = thermal_model_e.ThermalDnDt;
        _temp_d0 = dndt;
    }

    public void disable_temperature_coeff() {
        _temp_model = thermal_model_e.ThermalNone;
    }

    public void set_measurement_medium(Medium medium) {
        _measurement_medium = medium;
    }

    public void set_wavelen_range(double low, double high) {
        _low_wavelen = low;
        _high_wavelen = high;
    }

    /**
     * Get internal tranmittance dataset object.
     *
     * @see .clear_internal_transmittance()
     */
    public DiscreteSet get_transmittance_dataset() {
        return _transmittance;
    }


    public override double get_internal_transmittance(double wavelen) {
        try {
            return _transmittance.interpolate(wavelen);
        } catch (Exception) {
            return 1.0;
        }
    }

    public double get_schott_temp(double wavelen, double n) {
        // SCHOTT TIE-19: Temperature Coefficient of the Refractive Index

        double dt = _temperature - _measurement_medium.get_temperature();
        double wl = wavelen / 1000.0;
        double wl_tk = _temp_wl_tk;

        return (n * n - 1.0) / (2 * n) * dt
                * (_temp_d0 + _temp_d1 * dt + _temp_d2 * dt * dt
                + (_temp_e0 + _temp_e1 * dt) / (wl * wl - wl_tk * wl_tk));
    }

    public override double get_refractive_index(double wavelen) {
        if (wavelen == _last_wavelen) {
            return _last_get_refractive_index;
        }

        double a = _measurement_medium.get_refractive_index(wavelen);
        double m = get_measurement_index(wavelen);

        // get absolute refractive index
        double n = m * a;

        // apply temperature coefficients
        switch (_temp_model) {
            case thermal_model_e.ThermalSchott:
                n = n + get_schott_temp(wavelen, m);
                break;

            case thermal_model_e.ThermalDnDt: {
                double dt = _temperature - _measurement_medium.get_temperature();
                n = n + dt * _temp_d0;
                break;
            }

            case thermal_model_e.ThermalNone:
                break;
        }

        _last_wavelen = wavelen;
        _last_get_refractive_index = n;

        return n;
    }

    public double get_principal_dispersion() {
        return get_measurement_index(SpectralLine.F)
                - get_measurement_index(SpectralLine.C);
    }

    public double get_abbe_vd() {
        return (get_measurement_index(SpectralLine.d) - 1.0)
                / (get_measurement_index(SpectralLine.F)
                - get_measurement_index(SpectralLine.C));
    }

    public double get_abbe_ve() {
        return (get_measurement_index(SpectralLine.e) - 1.0)
                / (get_measurement_index(SpectralLine.F_)
                - get_measurement_index(SpectralLine.C_));
    }

    public double get_partial_dispersion(double wavelen1, double wavelen2) {
        return (get_measurement_index(wavelen1) - get_measurement_index(wavelen2))
                / get_principal_dispersion();
    }

}

}