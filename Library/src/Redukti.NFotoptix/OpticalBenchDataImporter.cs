/*
The software is ported from Goptical, hence is licensed under the GPL.
Copyright (c) 2021 Dibyendu Majumdar
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace Redukti.Nfotopix
{
    public class OpticalBenchDataImporter
    {
        static double parseDouble(string s)
        {
            if (s == null || String.IsNullOrEmpty(s))
            {
                return 0.0;
            }

            try
            {
                return Double.Parse(s);
            }
            catch (Exception)
            {
                return 0.0;
            }
        }

        class DescriptiveData
        {
            public string get_title()
            {
                return title_;
            }

            public void set_title(string title)
            {
                title_ = title;
            }

            string title_;
        }

        public class Variable
        {
            public Variable(string name)
            {
                this.name_ = name;
                this.values_ = new();
            }

            public string name()
            {
                return name_;
            }

            public void add_value(string value)
            {
                values_.Add(value);
            }

            public int num_scenarios()
            {
                return values_.Count;
            }

            public string get_value(int scenario)
            {
                return values_[scenario];
            }

            public double get_value_as_double(int scenario)
            {
                string s = get_value(scenario);
                try
                {
                    return parseDouble(s);
                }
                catch (Exception)
                {
                    return 0.0;
                }
            }

            string name_;
            List<string> values_;
        }

        public class AsphericalData
        {
            public AsphericalData(int surface_number)
            {
                this.surface_number_ = surface_number;
                this.data_ = new();
            }

            public void add_data(double d)
            {
                data_.Add(d);
            }

            public int data_points()
            {
                return data_.Count;
            }

            public double data(int i)
            {
                return i >= 0 && i < data_.Count ? data_[i] : 0.0;
            }

            //        void
//        dump (FILE *fp)
//        {
//            fprintf (fp, "Aspheric values[%d] = ", surface_number_);
//            for (int i = 0; i < data_points (); i++)
//            {
//                fprintf (fp, "%.12g ", data (i));
//            }
//            fputc ('\n', fp);
//        }
            public int get_surface_number()
            {
                return surface_number_;
            }

            int surface_number_;
            List<Double> data_;
        }

        public enum SurfaceType
        {
            surface,
            aperture_stop,
            field_stop
        }

        static string[] SurfaceTypeNames = {"S", "AS", "FS"};

        public class LensSurface
        {
            public LensSurface(int id)
            {
                id_ = id;
                surface_type_ = SurfaceType.surface;
                radius_ = 0;
                diameter_ = 0;
                refractive_index_ = 0;
                abbe_vd_ = 0;
                is_cover_glass_ = false;
            }

            public SurfaceType get_surface_type()
            {
                return surface_type_;
            }

            public void set_surface_type(SurfaceType surface_type)
            {
                surface_type_ = surface_type;
            }

            public double get_radius()
            {
                return radius_;
            }

            public void set_radius(double radius)
            {
                radius_ = radius;
            }

            public double get_thickness(int scenario)
            {
                if (scenario < thickness_by_scenario_.Count)
                    return thickness_by_scenario_[scenario];
                else
                {
                    // assert (1 == thickness_by_scenario_.size());
                    return thickness_by_scenario_[0];
                }
            }

            public void add_thickness(double thickness)
            {
                thickness_by_scenario_.Add(thickness);
            }

            public double get_diameter()
            {
                return diameter_;
            }

            public void set_diameter(double value)
            {
                diameter_ = value;
            }

            public double get_refractive_index()
            {
                return refractive_index_;
            }

            public void set_refractive_index(double refractive_index)
            {
                refractive_index_ = refractive_index;
            }

            public double get_abbe_vd()
            {
                return abbe_vd_;
            }

            public void set_abbe_vd(double abbe_vd)
            {
                abbe_vd_ = abbe_vd;
            }

            public AsphericalData get_aspherical_data()
            {
                return aspherical_data_;
            }

            public void set_aspherical_data(AsphericalData aspherical_data)
            {
                aspherical_data_ = aspherical_data;
            }

            public int get_id()
            {
                return id_;
            }

            public bool is_cover_glass()
            {
                return is_cover_glass_;
            }

            public void set_is_cover_glass(bool is_cover_glass)
            {
                is_cover_glass_ = is_cover_glass;
            }
//        void
//        dump (FILE *fp, unsigned scenario = 0)
//        {
//            fprintf (fp,
//                    "Surface[%d] = type=%s radius=%.12g thickness=%.12g diameter "
//                    "= %.12g nd = %.12g vd = %.12g\n",
//                    id_, SurfaceTypeNames[surface_type_], radius_,
//                    get_thickness (scenario), diameter_, refractive_index_, abbe_vd_);
//        }

            public void set_glass_name(string name)
            {
                glass_name_ = name;
            }

            public string get_glass_name()
            {
                return glass_name_;
            }

            int id_;
            SurfaceType surface_type_;
            double radius_;
            List<double> thickness_by_scenario_ = new();
            double diameter_;
            double refractive_index_;
            double abbe_vd_;
            bool is_cover_glass_;
            AsphericalData aspherical_data_;
            private string glass_name_ = null;
        }

        public class LensSpecifications
        {
            public string[] splitLine(string line)
            {
                List<string> words = new();
                while (line.Length > 0)
                {
                    int pos = line.IndexOf('\t');
                    if (pos < 0)
                    {
                        words.Add(line);
                        break;
                    }
                    else if (pos == 0)
                    {
                        words.Add("");
                        line = line.Substring(1);
                    }
                    else
                    {
                        words.Add(line.Substring(0, pos));
                        line = line.Substring(pos + 1);
                    }
                }

                return words.ToArray();
            }

            public bool parse_file(string file_name)
            {
                string[] lines = File.ReadAllLines(file_name);
                Section? current_section = null; // Current section
                int surface_id = 1; // We used to read the id from the OptBench data but
                // this doesn't always work

                foreach (string line in lines)
                {
                    string[] words = splitLine(line);
                    if (words.Length == 0)
                    {
                        continue;
                    }

                    if (words[0].StartsWith("#"))
                    {
                        // comment
                        continue;
                    }

                    if (words[0].StartsWith("["))
                    {
                        // section name
                        current_section = find_section(words[0]);
                        continue;
                    }

                    if (current_section == null)
                    {
                        continue;
                    }

                    switch (current_section)
                    {
                        case Section.DESCRIPTIVE_DATA:
                            if (words.Length >= 2 && words[0].Equals("title"))
                            {
                                descriptive_data_.set_title(words[1]);
                            }

                            break;
                        case Section.VARIABLE_DISTANCES:
                            if (words.Length >= 2)
                            {
                                Variable var = new Variable(words[0]);
                                for (int i = 1; i < words.Length; i++)
                                {
                                    var.add_value(words[i]);
                                }

                                variables_.Add(var);
                            }

                            break;
                        case Section.LENS_DATA:
                        {
                            if (words.Length < 2)
                                break;
                            int id = surface_id++;
                            LensSurface surface_data = new LensSurface(id);
                            SurfaceType type = SurfaceType.surface;
                            /* radius */
                            if (words[1].Equals("AS"))
                            {
                                type = SurfaceType.aperture_stop;
                                surface_data.set_radius(0.0);
                            }
                            else if (words[1].Equals("FS"))
                            {
                                type = SurfaceType.field_stop;
                                surface_data.set_radius(0.0);
                            }
                            else if (words[1].Equals("CG"))
                            {
                                surface_data.set_radius(0.0);
                                surface_data.set_is_cover_glass(true);
                            }
                            else
                            {
                                if (words[1].Equals("Infinity"))
                                    surface_data.set_radius(0.0);
                                else
                                    surface_data.set_radius(parseDouble(words[1]));
                            }

                            surface_data.set_surface_type(type);
                            /* thickness */
                            if (words.Length >= 3 && words[2].Length > 0)
                            {
                                parse_thickness(words[2], surface_data);
                            }

                            /* refractive index */
                            if (words.Length >= 4 && words[3].Length > 0)
                            {
                                surface_data.set_refractive_index(parseDouble(words[3]));
                            }

                            /* diameter */
                            if (words.Length >= 5 && words[4].Length > 0)
                            {
                                surface_data.set_diameter(parseDouble(words[4]));
                            }

                            /* abbe vd */
                            if (words.Length >= 6 && words[5].Length > 0)
                            {
                                surface_data.set_abbe_vd(parseDouble(words[5]));
                            }

                            if (words.Length >= 7 && words[6].Length > 0)
                            {
                                surface_data.set_glass_name(words[6]);
                            }

                            surfaces_.Add(surface_data);
                        }
                            break;
                        case Section.ASPHERICAL_DATA:
                        {
                            int id = Int32.Parse(words[0]);
                            AsphericalData aspherical_data = new AsphericalData(id);
                            for (int i = 1; i < words.Length; i++)
                            {
                                aspherical_data.add_data(parseDouble(words[i]));
                            }

                            aspherical_data_.Add(aspherical_data);
                            LensSurface surface_builder = find_surface(id);
                            if (surface_builder == null)
                            {
//                            fprintf (
//                                    stderr,
//                                    "Ignoring aspherical data as no surface numbered %d\n",
//                                    id);
                            }
                            else
                            {
                                surface_builder.set_aspherical_data(aspherical_data);
                            }
                        }
                            break;
                        default:
                            break;
                    }
                }

                return true;
            }

            public Variable find_variable(string name)
            {
                for (int i = 0; i < variables_.Count; i++)
                {
                    if (name.Equals(variables_[i].name()))
                    {
                        return variables_[i];
                    }
                }

                return null;
            }

            public LensSurface find_surface(int id)
            {
                for (int i = 0; i < surfaces_.Count; i++)
                {
                    if (surfaces_[i].get_id() == id)
                        return surfaces_[i];
                }

                return null;
            }

            //        void
//        dump (FILE *fp = stdout, unsigned scenario = 0)
//        {
//            for (int i = 0; i < surfaces_.size (); i++)
//            {
//                surfaces_.at (i)->dump (fp, scenario);
//                if (surfaces_.at (i)->get_aspherical_data ())
//                {
//                    surfaces_.at (i)->get_aspherical_data ()->dump (fp);
//                }
//            }
//        }
            public double get_image_height()
            {
                Variable var = find_variable("Image Height");
                if (var != null)
                    return var.get_value_as_double(0);
                return 43.2; // Assume 35mm
            }

            public void parse_thickness(string value,
                LensSurface surface_builder)
            {
                if (value.Length == 0)
                {
                    surface_builder.add_thickness(0.0);
                    return;
                }

                if (Char.IsLetter(value[0]))
                {
                    Variable var = find_variable(value);
                    if (var != null)
                    {
                        for (int i = 0; i < var.num_scenarios(); i++)
                        {
                            string s = var.get_value(i);
                            double d = parseDouble(s);
                            surface_builder.add_thickness(d);
                        }
                    }
                    else
                    {
                        //fprintf (stderr, "Variable %s was not found\n", value);
                        surface_builder.add_thickness(0.0);
                    }
                }
                else
                {
                    surface_builder.add_thickness(parseDouble(value));
                }
            }

            DescriptiveData get_descriptive_data()
            {
                return descriptive_data_;
            }

            List<Variable> get_variables()
            {
                return variables_;
            }

            public List<LensSurface> get_surfaces()
            {
                return surfaces_;
            }

            List<AsphericalData> get_aspherical_data()
            {
                return aspherical_data_;
            }

            DescriptiveData descriptive_data_ = new DescriptiveData();
            List<Variable> variables_ = new();
            List<LensSurface> surfaces_ = new();
            List<AsphericalData> aspherical_data_ = new();
        }

        enum Section
        {
            DESCRIPTIVE_DATA,
            CONSTANTS,
            VARIABLE_DISTANCES,
            LENS_DATA,
            ASPHERICAL_DATA
        }

        class SectionMapping
        {
            public string name;
            public Section section;

            public SectionMapping(string name, Section section)
            {
                this.name = name;
                this.section = section;
            }
        }

        static SectionMapping[] g_SectionMappings = new SectionMapping[]
        {
            new SectionMapping("[descriptive data]", Section.DESCRIPTIVE_DATA),
            new SectionMapping("[constants]", Section.CONSTANTS),
            new SectionMapping("[variable distances]", Section.VARIABLE_DISTANCES),
            new SectionMapping("[lens data]", Section.LENS_DATA),
            new SectionMapping("[aspherical data]", Section.ASPHERICAL_DATA)
        };

        static Section? find_section(string name)
        {
            Section? section = null;
            for (int i = 0; i < g_SectionMappings.Length; i++)
            {
                if (g_SectionMappings[i].name.Equals(name))
                {
                    section = g_SectionMappings[i].section;
                    break;
                }
            }

            return section;
        }

        private static double add_surface(Lens.Builder lens, LensSurface surface,
            int scenario)
        {
            double thickness = surface.get_thickness(scenario);
            double radius = surface.get_radius();
            double aperture_radius = surface.get_diameter() / 2.0;
            double refractive_index = surface.get_refractive_index();
            double abbe_vd = surface.get_abbe_vd();
            string glass_name = surface.get_glass_name();
            if (surface.get_surface_type() == SurfaceType.aperture_stop)
            {
                lens.add_stop(aperture_radius, thickness);
                return thickness;
            }

            AsphericalData aspherical_data = surface.get_aspherical_data();
            if (aspherical_data == null)
            {
                if (glass_name != null && GlassMap.glassByName(glass_name) != null)
                {
                    lens.add_surface(
                        radius, aperture_radius, thickness,
                        GlassMap.glassByName(glass_name));
                }
                else if (refractive_index != 0.0)
                {
                    if (abbe_vd == 0.0)
                    {
                        //fprintf (stderr, "Abbe vd not specified for surface %d\n",
                        //        surface.get_id ());
                        return -1.0;
                    }

                    lens.add_surface(
                        radius, aperture_radius, thickness,
                        new Abbe(Abbe.AbbeFormula.AbbeVd, refractive_index, abbe_vd, 0.0));
                }
                else
                {
                    lens.add_surface(radius, aperture_radius, thickness);
                }

                return thickness;
            }

            double k = aspherical_data.data(1) + 1.0;
            double a4 = aspherical_data.data(2);
            double a6 = aspherical_data.data(3);
            double a8 = aspherical_data.data(4);
            double a10 = aspherical_data.data(5);
            double a12 = aspherical_data.data(6);
            double a14 = aspherical_data.data(7);

            if (glass_name != null && GlassMap.glassByName(glass_name) != null)
            {
                lens.add_surface(
                    new Asphere(radius, k, a4, a6, a8, a10, a12,
                        a14),
                    new Disk(aperture_radius), thickness,
                    GlassMap.glassByName(glass_name));
            }
            else if (refractive_index > 0.0)
            {
                lens.add_surface(
                    new Asphere(radius, k, a4, a6, a8, a10, a12,
                        a14),
                    new Disk(aperture_radius), thickness,
                    new Abbe(Abbe.AbbeFormula.AbbeVd, refractive_index, abbe_vd, 0.0));
            }
            else
            {
                lens.add_surface(new Asphere(radius, k, a4, a6,
                        a8, a10, a12, a14),
                    new Disk(aperture_radius),
                    thickness, Air.air);
            }

            return thickness;
        }

        public static OpticalSystem.Builder buildSystem(LensSpecifications specs, int scenario)
        {
            OpticalSystem.Builder sys = new OpticalSystem.Builder();
            /* anchor lens */
            Lens.Builder lens = new Lens.Builder().position(Vector3Pair.position_000_001);

            double image_pos = 0.0;
            List<LensSurface> surfaces = specs.get_surfaces();
            for (int i = 0; i < surfaces.Count; i++)
            {
                double thickness = add_surface(lens, surfaces[i], scenario);
                image_pos += thickness;
            }

            // printf ("Image position is at %f\n", image_pos);
            sys.add(lens);

            Image.Builder image = new Image.Builder().position(
                    new Vector3Pair(new Vector3(0, 0, image_pos), Vector3.vector3_001))
                .curve(Flat.flat)
                .shape(new Rectangle(specs.get_image_height() * 2.0));
            sys.add(image);

            return sys;
        }

        public static double getAngleOfViewInRadians(LensSpecifications specs_, int scenario)
        {
            Variable view_angles = specs_.find_variable("Angle of View");
            return MathUtils.ToRadians(view_angles.get_value_as_double(scenario)
                                       / 2.0);
        }
    }
}