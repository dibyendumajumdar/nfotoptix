using System;
using System.Collections.Generic;

namespace Redukti.Nfotopix
{
    public class GlassMap : Solid
    {
        Dictionary<double, double> indexMap = new();

        public GlassMap(string name, Dictionary<double, double> indices) : base(name)
        {
            this.indexMap = indices;
        }

        public GlassMap(string name, double d_index, double C, double F) : base(name)
        {
            indexMap[SpectralLine.d] = d_index;
            indexMap[SpectralLine.C] = C;
            indexMap[SpectralLine.F] = F;
        }

        public override bool is_opaque()
        {
            return false;
        }

        public override bool is_reflecting()
        {
            return false;
        }

        public override double get_refractive_index(double wavelen)
        {
            if (indexMap.TryGetValue(wavelen, out double index))
            {
                throw new InvalidOperationException(
                    "Do not know how to get the refractive index for wavelen " + wavelen);
            }

            return index;
        }

        public override string ToString()
        {
            return name + "{d=" + get_refractive_index(SpectralLine.d) + ",C=" + get_refractive_index(SpectralLine.C) +
                   ",F=" + get_refractive_index(SpectralLine.F) + '}';
        }

        public static GlassMap glassByName(String name)
        {
            if (glasses.TryGetValue(name, out GlassMap g))
            {
                return g;
            }

            return null;
        }

        static Dictionary<string, GlassMap> glasses = new();

        static GlassMap()
        {
            // Hikari
            glasses["J-FK5"] = new GlassMap("J-FK5", 1.48749, 1.485343, 1.492276);
            glasses["J-FK01A"] = new GlassMap("J-FK01A", 1.497, 1.495139, 1.501226);
            glasses["J-FKH1"] = new GlassMap("J-FKH1", 1.49782, 1.49598, 1.502009);
            glasses["J-FKH2"] = new GlassMap("J-FKH2", 1.456, 1.454469, 1.45946);
            glasses["J-PKH1"] = new GlassMap("J-PKH1", 1.5186, 1.516311, 1.523731);
            glasses["J-PSK02"] = new GlassMap("J-PSK02", 1.618, 1.615024, 1.624781);
            glasses["J-PSK03"] = new GlassMap("J-PSK03", 1.603, 1.600183, 1.609398);
            glasses["J-PSKH1"] = new GlassMap("J-PSKH1", 1.59319, 1.59054, 1.599276);
            glasses["J-PSKH4"] = new GlassMap("J-PSKH4", 1.59349, 1.590771, 1.599629);
            glasses["J-BK7A"] = new GlassMap("J-BK7A", 1.5168, 1.514324, 1.522382);
            glasses["J-BAK1"] = new GlassMap("J-BAK1", 1.5725, 1.569472, 1.579464);
            glasses["J-BAK2"] = new GlassMap("J-BAK2", 1.53996, 1.537199, 1.546271);
            glasses["J-BAK4"] = new GlassMap("J-BAK4", 1.56883, 1.565751, 1.575909);
            glasses["J-K3"] = new GlassMap("J-K3", 1.51823, 1.515551, 1.524362);
            glasses["J-K5"] = new GlassMap("J-K5", 1.52249, 1.519803, 1.528627);
            glasses["J-KZFH1"] = new GlassMap("J-KZFH1", 1.61266, 1.608532, 1.622313);
            glasses["J-KZFH4"] = new GlassMap("J-KZFH4", 1.552981, 1.549923, 1.559964);
            glasses["J-KZFH6"] = new GlassMap("J-KZFH6", 1.68376, 1.678397, 1.696564);
            glasses["J-KZFH7"] = new GlassMap("J-KZFH7", 1.73211, 1.727358, 1.74321);
            glasses["J-KF6"] = new GlassMap("J-KF6", 1.51742, 1.514429, 1.524341);
            glasses["J-BALF4"] = new GlassMap("J-BALF4", 1.57957, 1.576316, 1.5871);
            glasses["J-BAF3"] = new GlassMap("J-BAF3", 1.58267, 1.578929, 1.591464);
            glasses["J-BAF4"] = new GlassMap("J-BAF4", 1.60562, 1.601481, 1.615408);
            glasses["J-BAF8"] = new GlassMap("J-BAF8", 1.62374, 1.619775, 1.633044);
            glasses["J-BAF10"] = new GlassMap("J-BAF10", 1.67003, 1.665785, 1.679998);
            glasses["J-BAF11"] = new GlassMap("J-BAF11", 1.66672, 1.662593, 1.676388);
            glasses["J-BAF12"] = new GlassMap("J-BAF12", 1.6393, 1.635055, 1.649314);
            glasses["J-BASF2"] = new GlassMap("J-BASF2", 1.66446, 1.659032, 1.677556);
            glasses["J-BASF6"] = new GlassMap("J-BASF6", 1.66755, 1.662821, 1.678763);
            glasses["J-BASF7"] = new GlassMap("J-BASF7", 1.70154, 1.696483, 1.713586);
            glasses["J-BASF8"] = new GlassMap("J-BASF8", 1.72342, 1.717827, 1.736849);
            glasses["J-SK2"] = new GlassMap("J-SK2", 1.60738, 1.604139, 1.614843);
            glasses["J-SK4"] = new GlassMap("J-SK4", 1.61272, 1.609539, 1.620006);
            glasses["J-SK5"] = new GlassMap("J-SK5", 1.58913, 1.586191, 1.595814);
            glasses["J-SK10"] = new GlassMap("J-SK10", 1.6228, 1.619492, 1.630399);
            glasses["J-SK11"] = new GlassMap("J-SK11", 1.56384, 1.561006, 1.570294);
            glasses["J-SK12"] = new GlassMap("J-SK12", 1.58313, 1.580141, 1.589954);
            glasses["J-SK14"] = new GlassMap("J-SK14", 1.60311, 1.600078, 1.610015);
            glasses["J-SK15"] = new GlassMap("J-SK15", 1.62299, 1.619729, 1.630448);
            glasses["J-SK16"] = new GlassMap("J-SK16", 1.62041, 1.617264, 1.627562);
            glasses["J-SK18"] = new GlassMap("J-SK18", 1.63854, 1.63505, 1.646589);
            glasses["J-SSK1"] = new GlassMap("J-SSK1", 1.6172, 1.613738, 1.625175);
            glasses["J-SSK5"] = new GlassMap("J-SSK5", 1.65844, 1.654552, 1.667504);
            glasses["J-SSK8"] = new GlassMap("J-SSK8", 1.61772, 1.613998, 1.626399);
            glasses["J-LLF1"] = new GlassMap("J-LLF1", 1.54814, 1.54455, 1.556594);
            glasses["J-LLF2"] = new GlassMap("J-LLF2", 1.54072, 1.53728, 1.548793);
            glasses["J-LLF6"] = new GlassMap("J-LLF6", 1.53172, 1.528453, 1.539353);
            glasses["J-LF5"] = new GlassMap("J-LF5", 1.58144, 1.577238, 1.591428);
            glasses["J-LF6"] = new GlassMap("J-LF6", 1.56732, 1.563371, 1.576695);
            glasses["J-LF7"] = new GlassMap("J-LF7", 1.57501, 1.570908, 1.58476);
            glasses["J-F1"] = new GlassMap("J-F1", 1.62588, 1.620742, 1.638263);
            glasses["J-F2"] = new GlassMap("J-F2", 1.62004, 1.615037, 1.632073);
            glasses["J-F3"] = new GlassMap("J-F3", 1.61293, 1.608054, 1.624644);
            glasses["J-F5"] = new GlassMap("J-F5", 1.60342, 1.598747, 1.614615);
            glasses["J-F8"] = new GlassMap("J-F8", 1.59551, 1.591028, 1.606214);
            glasses["J-F16"] = new GlassMap("J-F16", 1.5927, 1.587788, 1.604592);
            glasses["J-SF1"] = new GlassMap("J-SF1", 1.71736, 1.710337, 1.734595);
            glasses["J-SF2"] = new GlassMap("J-SF2", 1.64769, 1.642082, 1.661287);
            glasses["J-SF4"] = new GlassMap("J-SF4", 1.7552, 1.747305, 1.774696);
            glasses["J-SF5"] = new GlassMap("J-SF5", 1.6727, 1.666619, 1.68752);
            glasses["J-SF6"] = new GlassMap("J-SF6", 1.80518, 1.796109, 1.827749);
            glasses["J-SF6HS"] = new GlassMap("J-SF6HS", 1.80518, 1.796109, 1.827749);
            glasses["J-SF7"] = new GlassMap("J-SF7", 1.6398, 1.634385, 1.652905);
            glasses["J-SF8"] = new GlassMap("J-SF8", 1.68893, 1.682509, 1.704616);
            glasses["J-SF10"] = new GlassMap("J-SF10", 1.72825, 1.720838, 1.7465);
            glasses["J-SF11"] = new GlassMap("J-SF11", 1.78472, 1.775941, 1.806548);
            glasses["J-SF13"] = new GlassMap("J-SF13", 1.74077, 1.733069, 1.759772);
            glasses["J-SF14"] = new GlassMap("J-SF14", 1.76182, 1.75358, 1.782237);
            glasses["J-SF15"] = new GlassMap("J-SF15", 1.69895, 1.692227, 1.715424);
            glasses["J-SF03"] = new GlassMap("J-SF03", 1.84666, 1.836505, 1.872084);
            glasses["J-SF03HS"] = new GlassMap("J-SF03HS", 1.84666, 1.836505, 1.872084);
            glasses["J-SFS3"] = new GlassMap("J-SFS3", 1.7847, 1.776116, 1.805989);
            glasses["J-SFH1"] = new GlassMap("J-SFH1", 1.80809, 1.797989, 1.833527);
            glasses["J-SFH2"] = new GlassMap("J-SFH2", 1.86074, 1.85012, 1.887417);
            glasses["J-LAK7"] = new GlassMap("J-LAK7", 1.6516, 1.648206, 1.659331);
            glasses["J-LAK7R"] = new GlassMap("J-LAK7R", 1.6516, 1.648206, 1.659322);
            glasses["J-LAK8"] = new GlassMap("J-LAK8", 1.713, 1.708982, 1.722196);
            glasses["J-LAK9"] = new GlassMap("J-LAK9", 1.691, 1.687171, 1.69975);
            glasses["J-LAK10"] = new GlassMap("J-LAK10", 1.71999, 1.715672, 1.729995);
            glasses["J-LAK12"] = new GlassMap("J-LAK12", 1.6779, 1.674187, 1.686435);
            glasses["J-LAK13"] = new GlassMap("J-LAK13", 1.6935, 1.689551, 1.702585);
            glasses["J-LAK14"] = new GlassMap("J-LAK14", 1.6968, 1.692974, 1.705525);
            glasses["J-LAK18"] = new GlassMap("J-LAK18", 1.72916, 1.725097, 1.738449);
            glasses["J-LAK01"] = new GlassMap("J-LAK01", 1.64, 1.636739, 1.647371);
            glasses["J-LAK02"] = new GlassMap("J-LAK02", 1.67, 1.66644, 1.678123);
            glasses["J-LAK04"] = new GlassMap("J-LAK04", 1.651, 1.647485, 1.659061);
            glasses["J-LAK06"] = new GlassMap("J-LAK06", 1.6779, 1.673877, 1.687256);
            glasses["J-LAK09"] = new GlassMap("J-LAK09", 1.734, 1.72968, 1.74393);
            glasses["J-LAK011"] = new GlassMap("J-LAK011", 1.741, 1.736741, 1.750784);
            glasses["J-LASKH2"] = new GlassMap("J-LASKH2", 1.755, 1.750628, 1.765054);
            glasses["J-LAF2"] = new GlassMap("J-LAF2", 1.744, 1.739042, 1.755647);
            glasses["J-LAF3"] = new GlassMap("J-LAF3", 1.717, 1.712517, 1.727462);
            glasses["J-LAF7"] = new GlassMap("J-LAF7", 1.7495, 1.743271, 1.764535);
            glasses["J-LAF01"] = new GlassMap("J-LAF01", 1.7, 1.695645, 1.710196);
            glasses["J-LAF02"] = new GlassMap("J-LAF02", 1.72, 1.715094, 1.731604);
            glasses["J-LAF04"] = new GlassMap("J-LAF04", 1.757, 1.752239, 1.768055);
            glasses["J-LAF05"] = new GlassMap("J-LAF05", 1.762, 1.756381, 1.775377);
            glasses["J-LAF09"] = new GlassMap("J-LAF09", 1.697, 1.692687, 1.707073);
            glasses["J-LAF010"] = new GlassMap("J-LAF010", 1.7432, 1.738649, 1.753737);
            glasses["J-LAF016"] = new GlassMap("J-LAF016", 1.801, 1.794267, 1.817203);
            glasses["J-LAF016HS"] = new GlassMap("J-LAF016HS", 1.801, 1.794267, 1.817203);
            glasses["J-LAFH3"] = new GlassMap("J-LAFH3", 1.79504, 1.787036, 1.814745);
            glasses["J-LAFH3HS"] = new GlassMap("J-LAFH3HS", 1.79504, 1.787036, 1.814745);
            glasses["J-LASF01"] = new GlassMap("J-LASF01", 1.7859, 1.780582, 1.798375);
            glasses["J-LASF02"] = new GlassMap("J-LASF02", 1.79952, 1.793865, 1.812862);
            glasses["J-LASF03"] = new GlassMap("J-LASF03", 1.8061, 1.800248, 1.819921);
            glasses["J-LASF05"] = new GlassMap("J-LASF05", 1.83481, 1.828989, 1.848524);
            glasses["J-LASF05HS"] = new GlassMap("J-LASF05HS", 1.83481, 1.828989, 1.848524);
            glasses["J-LASF08A"] = new GlassMap("J-LASF08A", 1.883, 1.876555, 1.898256);
            glasses["J-LASF09A"] = new GlassMap("J-LASF09A", 1.816, 1.810744, 1.828257);
            glasses["J-LASF010"] = new GlassMap("J-LASF010", 1.834, 1.827379, 1.849808);
            glasses["J-LASF013"] = new GlassMap("J-LASF013", 1.8044, 1.798372, 1.818682);
            glasses["J-LASF014"] = new GlassMap("J-LASF014", 1.788, 1.782997, 1.799638);
            glasses["J-LASF015"] = new GlassMap("J-LASF015", 1.804, 1.798824, 1.816078);
            glasses["J-LASF016"] = new GlassMap("J-LASF016", 1.7725, 1.767801, 1.78337);
            glasses["J-LASF017"] = new GlassMap("J-LASF017", 1.795, 1.789742, 1.807287);
            glasses["J-LASF021"] = new GlassMap("J-LASF021", 1.85026, 1.842602, 1.868883);
            glasses["J-LASF021HS"] = new GlassMap("J-LASF021HS", 1.85026, 1.842602, 1.868883);
            glasses["J-LASFH2"] = new GlassMap("J-LASFH2", 1.76684, 1.761914, 1.778307);
            glasses["J-LASFH6"] = new GlassMap("J-LASFH6", 1.8061, 1.799034, 1.823209);
            glasses["J-LASFH9A"] = new GlassMap("J-LASFH9A", 1.90265, 1.895235, 1.920469);
            glasses["J-LASFH13"] = new GlassMap("J-LASFH13", 1.90366, 1.895254, 1.924149);
            glasses["J-LASFH13HS"] = new GlassMap("J-LASFH13HS", 1.90366, 1.895254, 1.924149);
            glasses["J-LASFH15"] = new GlassMap("J-LASFH15", 1.95, 1.940626, 1.972976);
            glasses["J-LASFH16"] = new GlassMap("J-LASFH16", 2.001, 1.991039, 2.02541);
            glasses["J-LASFH17"] = new GlassMap("J-LASFH17", 2.00069, 1.989413, 2.028724);
            glasses["J-LASFH17HS"] = new GlassMap("J-LASFH17HS", 2.00069, 1.989413, 2.028724);
            glasses["J-LASFH21"] = new GlassMap("J-LASFH21", 1.95375, 1.945145, 1.974641);
            glasses["J-LASFH22"] = new GlassMap("J-LASFH22", 1.8485, 1.842718, 1.862094);
            glasses["J-LASFH23"] = new GlassMap("J-LASFH23", 1.85, 1.840948, 1.872398);
            glasses["J-LASFH24"] = new GlassMap("J-LASFH24", 1.902, 1.891774, 1.927478);
            glasses["J-LASFH24HS"] = new GlassMap("J-LASFH24HS", 1.902, 1.891774, 1.927478);
            glasses["J-KZFH9"] = new GlassMap("J-KZFH9", 1.738, 1.731309, 1.754185);
            glasses["CaF2"] = new GlassMap("CaF2", 1.43384, 1.43245, 1.437);
            glasses["J-SFH5"] = new GlassMap("J-SFH5", 1.755750, 1.747048, 1.777633);

            // Hoya
            glasses["FC5"] = new GlassMap("FC5", 1.48749, 1.48535, 1.49227);
            glasses["FCD1"] = new GlassMap("FCD1", 1.497, 1.49514, 1.50123);
            glasses["FCD1B"] = new GlassMap("FCD1B", 1.4971, 1.49524, 1.50134);
            glasses["FCD10A"] = new GlassMap("FCD10A", 1.4586, 1.45704, 1.46212);
            glasses["FCD100"] = new GlassMap("FCD100", 1.437, 1.43559, 1.44019);
            glasses["FCD515"] = new GlassMap("FCD515", 1.59282, 1.59021, 1.59884);
            glasses["FCD600"] = new GlassMap("FCD600", 1.5941, 1.59115, 1.60097);
            glasses["FCD705"] = new GlassMap("FCD705", 1.55032, 1.5481, 1.55539);
            glasses["PCD4"] = new GlassMap("PCD4", 1.618, 1.61503, 1.62478);
            glasses["PCD40"] = new GlassMap("PCD40", 1.61997, 1.61701, 1.62672);
            glasses["PCD51"] = new GlassMap("PCD51", 1.59349, 1.59078, 1.59964);
            glasses["BSC7"] = new GlassMap("BSC7", 1.5168, 1.51432, 1.52237);
            glasses["E-C3"] = new GlassMap("E-C3", 1.51823, 1.51556, 1.52435);
            glasses["BAC4"] = new GlassMap("BAC4", 1.56883, 1.56575, 1.5759);
            glasses["BACD5"] = new GlassMap("BACD5", 1.58913, 1.58619, 1.59581);
            glasses["BACD14"] = new GlassMap("BACD14", 1.60311, 1.60009, 1.61002);
            glasses["BACD15"] = new GlassMap("BACD15", 1.62299, 1.61973, 1.63045);
            glasses["BACD16"] = new GlassMap("BACD16", 1.62041, 1.61727, 1.62755);
            glasses["BACD18"] = new GlassMap("BACD18", 1.63854, 1.63505, 1.64657);
            glasses["BACED5"] = new GlassMap("BACED5", 1.65844, 1.65454, 1.66749);
            glasses["LAC8"] = new GlassMap("LAC8", 1.713, 1.70898, 1.7222);
            glasses["LAC14"] = new GlassMap("LAC14", 1.6968, 1.69297, 1.70553);
            glasses["TAC8"] = new GlassMap("TAC8", 1.72916, 1.7251, 1.73844);
            glasses["E-CF6"] = new GlassMap("E-CF6", 1.51742, 1.51444, 1.52436);
            glasses["E-FEL1"] = new GlassMap("E-FEL1", 1.54814, 1.54458, 1.55654);
            glasses["E-FEL2"] = new GlassMap("E-FEL2", 1.54072, 1.5373, 1.54876);
            glasses["E-FL5"] = new GlassMap("E-FL5", 1.58144, 1.57723, 1.59145);
            glasses["E-FL6"] = new GlassMap("E-FL6", 1.56732, 1.56339, 1.57663);
            glasses["E-F2"] = new GlassMap("E-F2", 1.62004, 1.61502, 1.6321);
            glasses["E-F5"] = new GlassMap("E-F5", 1.60342, 1.59874, 1.61462);
            glasses["E-FD1"] = new GlassMap("E-FD1", 1.71736, 1.71032, 1.73464);
            glasses["E-FD2"] = new GlassMap("E-FD2", 1.64769, 1.6421, 1.66124);
            glasses["E-FD4"] = new GlassMap("E-FD4", 1.7552, 1.74729, 1.77473);
            glasses["E-FD5"] = new GlassMap("E-FD5", 1.6727, 1.66661, 1.68752);
            glasses["E-FD8"] = new GlassMap("E-FD8", 1.68893, 1.68251, 1.70462);
            glasses["E-FD10"] = new GlassMap("E-FD10", 1.72825, 1.72082, 1.74654);
            glasses["E-FD13"] = new GlassMap("E-FD13", 1.74077, 1.73307, 1.75976);
            glasses["E-FD15"] = new GlassMap("E-FD15", 1.69895, 1.69221, 1.71547);
            glasses["FD60-W"] = new GlassMap("FD60-W", 1.80518, 1.79611, 1.82774);
            glasses["FD60"] = new GlassMap("FD60", 1.80518, 1.79611, 1.82774);
            glasses["FD110"] = new GlassMap("FD110", 1.78472, 1.77597, 1.80648);
            glasses["FD140"] = new GlassMap("FD140", 1.76182, 1.75359, 1.78222);
            glasses["FD225"] = new GlassMap("FD225", 1.80809, 1.79799, 1.83349);
            glasses["E-FDS1-W"] = new GlassMap("E-FDS1-W", 1.92286, 1.91038, 1.95457);
            glasses["E-FDS1"] = new GlassMap("E-FDS1", 1.92286, 1.91038, 1.95457);
            glasses["E-FDS2"] = new GlassMap("E-FDS2", 2.00272, 1.98813, 2.04003);
            glasses["E-FDS3"] = new GlassMap("E-FDS3", 2.1042, 2.08618, 2.15106);
            glasses["FDS16-W"] = new GlassMap("FDS16-W", 1.98612, 1.96949, 2.02931);
            glasses["FDS18-W"] = new GlassMap("FDS18-W", 1.94595, 1.93123, 1.98383);
            glasses["FDS18"] = new GlassMap("FDS18", 1.94595, 1.93123, 1.98383);
            glasses["FDS20-W "] = new GlassMap("FDS20-W ", 1.86966, 1.85742, 1.90086);
            glasses["FDS24"] = new GlassMap("FDS24", 1.92119, 1.9102, 1.94865);
            glasses["FDS90-SG"] = new GlassMap("FDS90-SG", 1.84666, 1.83649, 1.87209);
            glasses["FDS90"] = new GlassMap("FDS90", 1.84666, 1.83649, 1.87209);
            glasses["FDS90(P)"] = new GlassMap("FDS90(P)", 1.84666, 1.83653, 1.87199);
            glasses["FF5"] = new GlassMap("FF5", 1.5927, 1.58782, 1.60454);
            glasses["FF8"] = new GlassMap("FF8", 1.75211, 1.74352, 1.77355);
            glasses["BAFD7"] = new GlassMap("BAFD7", 1.70154, 1.69651, 1.71356);
            glasses["BAFD8"] = new GlassMap("BAFD8", 1.72342, 1.71781, 1.73685);
            glasses["LAF2"] = new GlassMap("LAF2", 1.744, 1.73906, 1.75563);
            glasses["LAF3"] = new GlassMap("LAF3", 1.717, 1.71251, 1.72745);
            glasses["NBF1"] = new GlassMap("NBF1", 1.7433, 1.73874, 1.75384);
            glasses["NBFD3"] = new GlassMap("NBFD3", 1.8045, 1.79849, 1.81879);
            glasses["NBFD10"] = new GlassMap("NBFD10", 1.834, 1.82742, 1.84975);
            glasses["NBFD11"] = new GlassMap("NBFD11", 1.7859, 1.78053, 1.79842);
            glasses["NBFD13"] = new GlassMap("NBFD13", 1.8061, 1.80022, 1.82001);
            glasses["NBFD15-W"] = new GlassMap("NBFD15-W", 1.8061, 1.79902, 1.82325);
            glasses["NBFD15"] = new GlassMap("NBFD15", 1.8061, 1.79902, 1.82325);
            glasses["NBFD30"] = new GlassMap("NBFD30", 1.85883, 1.85052, 1.87915);
            glasses["TAF1"] = new GlassMap("TAF1", 1.7725, 1.7678, 1.78336);
            glasses["TAF3D"] = new GlassMap("TAF3D", 1.8042, 1.799, 1.8163);
            glasses["TAF3"] = new GlassMap("TAF3", 1.8042, 1.799, 1.8163);
            glasses["TAFD5G"] = new GlassMap("TAFD5G", 1.83481, 1.82898, 1.84852);
            glasses["TAFD5F"] = new GlassMap("TAFD5F", 1.83481, 1.82898, 1.84852);
            glasses["TAFD25"] = new GlassMap("TAFD25", 1.90366, 1.89526, 1.92412);
            glasses["TAFD30"] = new GlassMap("TAFD30", 1.883, 1.87657, 1.89821);
            glasses["TAFD32"] = new GlassMap("TAFD32", 1.8707, 1.86436, 1.88573);
            glasses["TAFD33"] = new GlassMap("TAFD33", 1.881, 1.8745, 1.89644);
            glasses["TAFD35"] = new GlassMap("TAFD35", 1.91082, 1.90323, 1.92907);
            glasses["TAFD37A"] = new GlassMap("TAFD37A", 1.90043, 1.89333, 1.91742);
            glasses["TAFD37"] = new GlassMap("TAFD37", 1.90043, 1.89333, 1.91742);
            glasses["TAFD40-W"] = new GlassMap("TAFD40-W", 2.00069, 1.98941, 2.02872);
            glasses["TAFD40"] = new GlassMap("TAFD40", 2.00069, 1.98941, 2.02872);
            glasses["TAFD45"] = new GlassMap("TAFD45", 1.95375, 1.94513, 1.97465);
            glasses["TAFD55"] = new GlassMap("TAFD55", 2.001, 1.99105, 2.0254);
            glasses["TAFD65"] = new GlassMap("TAFD65", 2.0509, 2.03965, 2.07865);
            glasses["FCD10"] = new GlassMap("FCD10", 1.4565, 1.45495, 1.46001);
            glasses["FCD505"] = new GlassMap("FCD505", 1.59282, 1.59021, 1.59884);
            glasses["LBC3N"] = new GlassMap("LBC3N", 1.60625, 1.60336, 1.61288);
            glasses["BACD2"] = new GlassMap("BACD2", 1.60738, 1.60414, 1.61485);
            glasses["BACD4"] = new GlassMap("BACD4", 1.61272, 1.60954, 1.62);
            glasses["BACD11"] = new GlassMap("BACD11", 1.56384, 1.56101, 1.57028);
            glasses["LAC7"] = new GlassMap("LAC7", 1.6516, 1.64821, 1.65936);
            glasses["LAC9"] = new GlassMap("LAC9", 1.691, 1.68715, 1.69978);
            glasses["LAC10"] = new GlassMap("LAC10", 1.72, 1.71568, 1.72998);
            glasses["LAC12"] = new GlassMap("LAC12", 1.6779, 1.6742, 1.68641);
            glasses["LAC13"] = new GlassMap("LAC13", 1.6935, 1.68955, 1.70256);
            glasses["LACL60"] = new GlassMap("LACL60", 1.64, 1.63674, 1.64737);
            glasses["E-FEL6"] = new GlassMap("E-FEL6", 1.53172, 1.52847, 1.53935);
            glasses["E-F1"] = new GlassMap("E-F1", 1.62588, 1.62074, 1.63825);
            glasses["E-F3"] = new GlassMap("E-F3", 1.61293, 1.60805, 1.62463);
            glasses["E-F8"] = new GlassMap("E-F8", 1.59551, 1.59103, 1.60621);
            glasses["E-FD7"] = new GlassMap("E-FD7", 1.6398, 1.63439, 1.6529);
            glasses["TAC2"] = new GlassMap("TAC2", 1.741, 1.73672, 1.75081);
            glasses["TAC4"] = new GlassMap("TAC4", 1.734, 1.72965, 1.74403);
            glasses["TAC6"] = new GlassMap("TAC6", 1.755, 1.75063, 1.76506);
            glasses["BAF10"] = new GlassMap("BAF10", 1.67003, 1.66579, 1.67999);
            glasses["BAF11"] = new GlassMap("BAF11", 1.66672, 1.66262, 1.67642);
            glasses["E-ADF10"] = new GlassMap("E-ADF10", 1.6131, 1.60895, 1.62277);
            glasses["E-ADF50"] = new GlassMap("E-ADF50", 1.65412, 1.64921, 1.66572);
            glasses["E-BACD10"] = new GlassMap("E-BACD10", 1.6228, 1.61949, 1.63043);
            glasses["E-BACED20"] = new GlassMap("E-BACED20", 1.6485, 1.64482, 1.65705);
            glasses["E-BAF8"] = new GlassMap("E-BAF8", 1.62374, 1.61978, 1.63304);
            glasses["E-LAF7"] = new GlassMap("E-LAF7", 1.7495, 1.74325, 1.76464);
            glasses["NBFD12"] = new GlassMap("NBFD12", 1.7995, 1.79388, 1.81276);
            glasses["TAF2"] = new GlassMap("TAF2", 1.7945, 1.78925, 1.80675);
            glasses["TAF4"] = new GlassMap("TAF4", 1.788, 1.783, 1.79959);
            glasses["TAF5"] = new GlassMap("TAF5", 1.816, 1.81074, 1.82827);
            glasses["M-FCD1"] = new GlassMap("M-FCD1", 1.4971, 1.49524, 1.50134);
            glasses["MP-FCD1-M20"] = new GlassMap("MP-FCD1-M20", 1.4969, 1.49504, 1.50114);
            glasses["MC-FCD1-M20"] = new GlassMap("MC-FCD1-M20", 1.4969, 1.49504, 1.50114);
            glasses["M-FCD500"] = new GlassMap("M-FCD500", 1.55332, 1.55097, 1.55869);
            glasses["MP-FCD500-20"] = new GlassMap("MP-FCD500-20", 1.55352, 1.55117, 1.55889);
            glasses["MC-FCD500-20"] = new GlassMap("MC-FCD500-20", 1.55352, 1.55117, 1.55889);
            glasses["M-PCD4"] = new GlassMap("M-PCD4", 1.61881, 1.61586, 1.62555);
            glasses["MP-PCD4-40"] = new GlassMap("MP-PCD4-40", 1.61921, 1.61626, 1.62595);
            glasses["MC-PCD4-40"] = new GlassMap("MC-PCD4-40", 1.61921, 1.61626, 1.62595);
            glasses["M-PCD51"] = new GlassMap("M-PCD51", 1.59201, 1.58931, 1.59814);
            glasses["MP-PCD51-70"] = new GlassMap("MP-PCD51-70", 1.59271, 1.59, 1.59885);
            glasses["MC-PCD51-70"] = new GlassMap("MC-PCD51-70", 1.59271, 1.59, 1.59885);
            glasses["M-BACD5N"] = new GlassMap("M-BACD5N", 1.58913, 1.58618, 1.5958);
            glasses["MP-BACD5N"] = new GlassMap("MP-BACD5N", 1.58913, 1.58618, 1.5958);
            glasses["MC-BACD5N"] = new GlassMap("MC-BACD5N", 1.58913, 1.58618, 1.5958);
            glasses["M-BACD12"] = new GlassMap("M-BACD12", 1.58313, 1.58014, 1.58995);
            glasses["MP-BACD12"] = new GlassMap("MP-BACD12", 1.58313, 1.58014, 1.58995);
            glasses["MC-BACD12"] = new GlassMap("MC-BACD12", 1.58313, 1.58014, 1.58995);
            glasses["M-BACD15"] = new GlassMap("M-BACD15", 1.62263, 1.61935, 1.63005);
            glasses["MP-BACD15"] = new GlassMap("MP-BACD15", 1.62263, 1.61935, 1.63005);
            glasses["M-LAC130"] = new GlassMap("M-LAC130", 1.6935, 1.68955, 1.70258);
            glasses["MP-LAC130"] = new GlassMap("MP-LAC130", 1.6935, 1.68955, 1.70258);
            glasses["MC-LAC130"] = new GlassMap("MC-LAC130", 1.6935, 1.68955, 1.70258);
            glasses["M-LAC14"] = new GlassMap("M-LAC14", 1.6968, 1.69297, 1.70553);
            glasses["MP-LAC14-80"] = new GlassMap("MP-LAC14-80", 1.6976, 1.69377, 1.70634);
            glasses["M-TAC60"] = new GlassMap("M-TAC60", 1.75501, 1.75055, 1.76531);
            glasses["MP-TAC60-90"] = new GlassMap("MP-TAC60-90", 1.75591, 1.75145, 1.76622);
            glasses["M-TAC80"] = new GlassMap("M-TAC80", 1.72903, 1.72494, 1.73843);
            glasses["MP-TAC80-60"] = new GlassMap("MP-TAC80-60", 1.72963, 1.72554, 1.73903);
            glasses["M-FD80"] = new GlassMap("M-FD80", 1.68893, 1.68252, 1.70463);
            glasses["MP-FD80"] = new GlassMap("MP-FD80", 1.68893, 1.68252, 1.70463);
            glasses["M-FDS2"] = new GlassMap("M-FDS2", 2.00178, 1.98721, 2.03905);
            glasses["MP-FDS2"] = new GlassMap("MP-FDS2", 2.00178, 1.98721, 2.03905);
            glasses["MC-FDS2"] = new GlassMap("MC-FDS2", 2.00178, 1.98721, 2.03905);
            glasses["M-FDS910"] = new GlassMap("M-FDS910", 1.82115, 1.8114, 1.84553);
            glasses["MP-FDS910-50"] = new GlassMap("MP-FDS910-50", 1.82165, 1.8119, 1.84607);
            glasses["MC-FDS910-50"] = new GlassMap("MC-FDS910-50", 1.82165, 1.8119, 1.84607);
            glasses["M-NBFD10"] = new GlassMap("M-NBFD10", 1.83441, 1.82781, 1.85019);
            glasses["MP-NBFD10-20"] = new GlassMap("MP-NBFD10-20", 1.83461, 1.82802, 1.8504);
            glasses["M-NBFD130"] = new GlassMap("M-NBFD130", 1.8061, 1.80022, 1.82002);
            glasses["MP-NBFD130"] = new GlassMap("MP-NBFD130", 1.8061, 1.80022, 1.82002);
            glasses["MC-NBFD135"] = new GlassMap("MC-NBFD135", 1.80834, 1.80247, 1.82223);
            glasses["M-TAF31"] = new GlassMap("M-TAF31", 1.80139, 1.7961, 1.81373);
            glasses["MP-TAF31-15"] = new GlassMap("MP-TAF31-15", 1.80154, 1.79625, 1.81388);
            glasses["MC-TAF31-15"] = new GlassMap("MC-TAF31-15", 1.80154, 1.79625, 1.81388);
            glasses["M-TAF101"] = new GlassMap("M-TAF101", 1.76802, 1.76331, 1.77891);
            glasses["MP-TAF101-100"] = new GlassMap("MP-TAF101-100", 1.76902, 1.76431, 1.77991);
            glasses["MC-TAF101-100"] = new GlassMap("MC-TAF101-100", 1.76902, 1.76431, 1.77991);
            glasses["M-TAF105"] = new GlassMap("M-TAF105", 1.7725, 1.76779, 1.7834);
            glasses["MP-TAF105"] = new GlassMap("MP-TAF105", 1.7725, 1.76779, 1.7834);
            glasses["MC-TAF105"] = new GlassMap("MC-TAF105", 1.7725, 1.76779, 1.7834);
            glasses["M-TAF401"] = new GlassMap("M-TAF401", 1.77377, 1.76884, 1.78524);
            glasses["MP-TAF401"] = new GlassMap("MP-TAF401", 1.77377, 1.76884, 1.78524);
            glasses["MC-TAF401"] = new GlassMap("MC-TAF401", 1.77377, 1.76884, 1.78524);
            glasses["M-TAFD51"] = new GlassMap("M-TAFD51", 1.8208, 1.81507, 1.83429);
            glasses["MP-TAFD51-50"] = new GlassMap("MP-TAFD51-50", 1.8213, 1.81557, 1.83479);
            glasses["MC-TAFD51-50"] = new GlassMap("MC-TAFD51-50", 1.8213, 1.81557, 1.83479);
            glasses["M-TAFD305"] = new GlassMap("M-TAFD305", 1.85135, 1.84505, 1.86628);
            glasses["MP-TAFD305"] = new GlassMap("MP-TAFD305", 1.85135, 1.84505, 1.86628);
            glasses["MC-TAFD305"] = new GlassMap("MC-TAFD305", 1.85135, 1.84505, 1.86628);
            glasses["M-TAFD307"] = new GlassMap("M-TAFD307", 1.88202, 1.87504, 1.89873);
            glasses["MP-TAFD307"] = new GlassMap("MP-TAFD307", 1.88202, 1.87504, 1.89873);
            glasses["MC-TAFD307"] = new GlassMap("MC-TAFD307", 1.88202, 1.87504, 1.89873);
            glasses["M-TAFD405"] = new GlassMap("M-TAFD405", 1.9515, 1.94223, 1.97413);
            glasses["MP-TAFD405"] = new GlassMap("MP-TAFD405", 1.9515, 1.94223, 1.97413);
            glasses["M-LAC8"] = new GlassMap("M-LAC8", 1.713, 1.70899, 1.72221);
            glasses["MP-LAC8-30"] = new GlassMap("MP-LAC8-30", 1.7133, 1.70929, 1.72251);
            glasses["M-FDS1"] = new GlassMap("M-FDS1", 1.92286, 1.91037, 1.95456);
            glasses["MP-FDS1"] = new GlassMap("MP-FDS1", 1.92286, 1.91037, 1.95456);
            glasses["M-LAF81"] = new GlassMap("M-LAF81", 1.73077, 1.72541, 1.74345);
            glasses["MP-LAF81"] = new GlassMap("MP-LAF81", 1.73077, 1.72541, 1.74345);
            glasses["M-NBF1"] = new GlassMap("M-NBF1", 1.7433, 1.73876, 1.75383);
            glasses["MP-NBF1"] = new GlassMap("MP-NBF1", 1.7433, 1.73876, 1.75383);
            glasses["MC-NBF1"] = new GlassMap("MC-NBF1", 1.7433, 1.73876, 1.75383);
            glasses["M-TAF1"] = new GlassMap("M-TAF1", 1.7725, 1.76781, 1.78342);
            glasses["MC-TAF1"] = new GlassMap("MC-TAF1", 1.7725, 1.76781, 1.78342);

            // Schott
            glasses["F2"] = new GlassMap("F2", 1.62004, 1.61503, 1.63208);
            glasses["F2HT"] = new GlassMap("F2HT", 1.62004, 1.61503, 1.63208);
            glasses["F5"] = new GlassMap("F5", 1.60342, 1.59875, 1.61461);
            glasses["FK5HTi"] = new GlassMap("FK5HTi", 1.48748, 1.48534, 1.49225);
            glasses["K10"] = new GlassMap("K10", 1.50137, 1.49867, 1.50756);
            glasses["K7"] = new GlassMap("K7", 1.51112, 1.50854, 1.517);
            glasses["LAFN7"] = new GlassMap("LAFN7", 1.7495, 1.74319, 1.76464);
            glasses["LASF35"] = new GlassMap("LASF35", 2.02204, 2.01185, 2.04702);
            glasses["LF5"] = new GlassMap("LF5", 1.58144, 1.57723, 1.59146);
            glasses["LF5HTi"] = new GlassMap("LF5HTi", 1.58144, 1.57724, 1.59145);
            glasses["LLF1"] = new GlassMap("LLF1", 1.54814, 1.54457, 1.55655);
            glasses["LLF1HTi"] = new GlassMap("LLF1HTi", 1.54815, 1.54459, 1.55653);
            glasses["N-BAF10"] = new GlassMap("N-BAF10", 1.67003, 1.66578, 1.68);
            glasses["N-BAF4"] = new GlassMap("N-BAF4", 1.60568, 1.60157, 1.61542);
            glasses["N-BAF51"] = new GlassMap("N-BAF51", 1.65224, 1.64792, 1.66243);
            glasses["N-BAF52"] = new GlassMap("N-BAF52", 1.60863, 1.60473, 1.61779);
            glasses["N-BAK1"] = new GlassMap("N-BAK1", 1.5725, 1.56949, 1.57943);
            glasses["N-BAK2"] = new GlassMap("N-BAK2", 1.53996, 1.53721, 1.54625);
            glasses["N-BAK4"] = new GlassMap("N-BAK4", 1.56883, 1.56575, 1.57591);
            glasses["N-BAK4HT"] = new GlassMap("N-BAK4HT", 1.56883, 1.56575, 1.57591);
            glasses["N-BALF4"] = new GlassMap("N-BALF4", 1.57956, 1.57631, 1.58707);
            glasses["N-BALF5"] = new GlassMap("N-BALF5", 1.54739, 1.5443, 1.55451);
            glasses["N-BASF2"] = new GlassMap("N-BASF2", 1.66446, 1.65905, 1.67751);
            glasses["N-BASF64"] = new GlassMap("N-BASF64", 1.704, 1.69872, 1.71659);
            glasses["N-BK10"] = new GlassMap("N-BK10", 1.49782, 1.49552, 1.50296);
            glasses["N-BK7"] = new GlassMap("N-BK7", 1.5168, 1.51432, 1.52238);
            glasses["N-BK7HT"] = new GlassMap("N-BK7HT", 1.5168, 1.51432, 1.52238);
            glasses["N-BK7HTi"] = new GlassMap("N-BK7HTi", 1.5168, 1.51432, 1.52238);
            glasses["N-F2"] = new GlassMap("N-F2", 1.62005, 1.61506, 1.63208);
            glasses["N-FK5"] = new GlassMap("N-FK5", 1.48749, 1.48535, 1.49227);
            glasses["N-FK51A"] = new GlassMap("N-FK51A", 1.48656, 1.4848, 1.49056);
            glasses["N-FK58"] = new GlassMap("N-FK58", 1.456, 1.45446, 1.45948);
            glasses["N-K5"] = new GlassMap("N-K5", 1.52249, 1.51982, 1.5286);
            glasses["N-KF9"] = new GlassMap("N-KF9", 1.52346, 1.5204, 1.53056);
            glasses["N-KZFS11"] = new GlassMap("N-KZFS11", 1.63775, 1.63324, 1.64828);
            glasses["N-KZFS2"] = new GlassMap("N-KZFS2", 1.55836, 1.55519, 1.56553);
            glasses["N-KZFS4"] = new GlassMap("N-KZFS4", 1.61336, 1.60922, 1.623);
            glasses["N-KZFS4HT"] = new GlassMap("N-KZFS4HT", 1.61336, 1.60922, 1.623);
            glasses["N-KZFS5"] = new GlassMap("N-KZFS5", 1.65412, 1.64922, 1.6657);
            glasses["N-KZFS8"] = new GlassMap("N-KZFS8", 1.72047, 1.71437, 1.73513);
            glasses["N-LAF2"] = new GlassMap("N-LAF2", 1.74397, 1.73903, 1.75562);
            glasses["N-LAF21"] = new GlassMap("N-LAF21", 1.788, 1.78301, 1.7996);
            glasses["N-LAF33"] = new GlassMap("N-LAF33", 1.78582, 1.78049, 1.79833);
            glasses["N-LAF34"] = new GlassMap("N-LAF34", 1.7725, 1.7678, 1.78337);
            glasses["N-LAF35"] = new GlassMap("N-LAF35", 1.7433, 1.73876, 1.75381);
            glasses["N-LAF7"] = new GlassMap("N-LAF7", 1.7495, 1.7432, 1.76472);
            glasses["N-LAK10"] = new GlassMap("N-LAK10", 1.72003, 1.71572, 1.72995);
            glasses["N-LAK12"] = new GlassMap("N-LAK12", 1.6779, 1.67419, 1.68647);
            glasses["N-LAK14"] = new GlassMap("N-LAK14", 1.6968, 1.69297, 1.70554);
            glasses["N-LAK21"] = new GlassMap("N-LAK21", 1.64049, 1.63724, 1.6479);
            glasses["N-LAK22"] = new GlassMap("N-LAK22", 1.65113, 1.6476, 1.65925);
            glasses["N-LAK33B"] = new GlassMap("N-LAK33B", 1.755, 1.75062, 1.76506);
            glasses["N-LAK34"] = new GlassMap("N-LAK34", 1.72916, 1.72509, 1.73847);
            glasses["N-LAK7"] = new GlassMap("N-LAK7", 1.6516, 1.64821, 1.65934);
            glasses["N-LAK8"] = new GlassMap("N-LAK8", 1.713, 1.70897, 1.72222);
            glasses["N-LAK9"] = new GlassMap("N-LAK9", 1.691, 1.68716, 1.69979);
            glasses["N-LASF31A"] = new GlassMap("N-LASF31A", 1.883, 1.87656, 1.89822);
            glasses["N-LASF40"] = new GlassMap("N-LASF40", 1.83404, 1.82745, 1.84981);
            glasses["N-LASF41"] = new GlassMap("N-LASF41", 1.83501, 1.82923, 1.84859);
            glasses["N-LASF43"] = new GlassMap("N-LASF43", 1.8061, 1.8002, 1.82005);
            glasses["N-LASF44"] = new GlassMap("N-LASF44", 1.8042, 1.79901, 1.8163);
            glasses["N-LASF45"] = new GlassMap("N-LASF45", 1.80107, 1.79436, 1.81726);
            glasses["N-LASF45HT"] = new GlassMap("N-LASF45HT", 1.80107, 1.79436, 1.81726);
            glasses["N-LASF46A"] = new GlassMap("N-LASF46A", 1.90366, 1.89526, 1.92411);
            glasses["N-LASF46B"] = new GlassMap("N-LASF46B", 1.90366, 1.89526, 1.92411);
            glasses["N-LASF9"] = new GlassMap("N-LASF9", 1.85025, 1.84255, 1.86898);
            glasses["N-LASF9HT"] = new GlassMap("N-LASF9HT", 1.85025, 1.84255, 1.86898);
            glasses["N-PK51"] = new GlassMap("N-PK51", 1.52855, 1.52646, 1.53333);
            glasses["N-PK52A"] = new GlassMap("N-PK52A", 1.497, 1.49514, 1.50123);
            glasses["N-PSK3"] = new GlassMap("N-PSK3", 1.55232, 1.54965, 1.55835);
            glasses["N-PSK53A"] = new GlassMap("N-PSK53A", 1.618, 1.61503, 1.62478);
            glasses["N-SF1"] = new GlassMap("N-SF1", 1.71736, 1.71035, 1.73457);
            glasses["N-SF10"] = new GlassMap("N-SF10", 1.72828, 1.72091, 1.74643);
            glasses["N-SF11"] = new GlassMap("N-SF11", 1.78472, 1.77596, 1.80651);
            glasses["N-SF14"] = new GlassMap("N-SF14", 1.76182, 1.75356, 1.78228);
            glasses["N-SF15"] = new GlassMap("N-SF15", 1.69892, 1.69222, 1.71536);
            glasses["N-SF2"] = new GlassMap("N-SF2", 1.64769, 1.6421, 1.66125);
            glasses["N-SF4"] = new GlassMap("N-SF4", 1.75513, 1.74719, 1.77477);
            glasses["N-SF5"] = new GlassMap("N-SF5", 1.67271, 1.66664, 1.6875);
            glasses["N-SF57"] = new GlassMap("N-SF57", 1.84666, 1.8365, 1.8721);
            glasses["N-SF57HT"] = new GlassMap("N-SF57HT", 1.84666, 1.8365, 1.8721);
            glasses["N-SF57HTultra"] = new GlassMap("N-SF57HTultra", 1.84666, 1.8365, 1.8721);
            glasses["N-SF6"] = new GlassMap("N-SF6", 1.80518, 1.79608, 1.82783);
            glasses["N-SF66"] = new GlassMap("N-SF66", 1.92286, 1.91039, 1.95459);
            glasses["N-SF6HT"] = new GlassMap("N-SF6HT", 1.80518, 1.79608, 1.82783);
            glasses["N-SF6HTultra"] = new GlassMap("N-SF6HTultra", 1.80518, 1.79608, 1.82783);
            glasses["N-SF8"] = new GlassMap("N-SF8", 1.68894, 1.68254, 1.70455);
            glasses["N-SK11"] = new GlassMap("N-SK11", 1.56384, 1.56101, 1.57028);
            glasses["N-SK14"] = new GlassMap("N-SK14", 1.60311, 1.60008, 1.61003);
            glasses["N-SK16"] = new GlassMap("N-SK16", 1.62041, 1.61727, 1.62756);
            glasses["N-SK2"] = new GlassMap("N-SK2", 1.60738, 1.60414, 1.61486);
            glasses["N-SK2HT"] = new GlassMap("N-SK2HT", 1.60738, 1.60414, 1.61486);
            glasses["N-SK4"] = new GlassMap("N-SK4", 1.61272, 1.60954, 1.61999);
            glasses["N-SK5"] = new GlassMap("N-SK5", 1.58913, 1.58619, 1.59581);
            glasses["N-SSK2"] = new GlassMap("N-SSK2", 1.62229, 1.61877, 1.63045);
            glasses["N-SSK5"] = new GlassMap("N-SSK5", 1.65844, 1.65455, 1.66749);
            glasses["N-SSK8"] = new GlassMap("N-SSK8", 1.61773, 1.61401, 1.62641);
            glasses["N-ZK7"] = new GlassMap("N-ZK7", 1.50847, 1.50592, 1.51423);
            glasses["N-ZK7A"] = new GlassMap("N-ZK7A", 1.50805, 1.5055, 1.51382);
            glasses["P-BK7"] = new GlassMap("P-BK7", 1.5164, 1.51392, 1.52198);
            glasses["P-LAF37"] = new GlassMap("P-LAF37", 1.7555, 1.75054, 1.76708);
            glasses["P-LAK35"] = new GlassMap("P-LAK35", 1.6935, 1.68955, 1.70259);
            glasses["P-LASF47"] = new GlassMap("P-LASF47", 1.8061, 1.80023, 1.81994);
            glasses["P-LASF50"] = new GlassMap("P-LASF50", 1.8086, 1.80266, 1.82264);
            glasses["P-LASF51"] = new GlassMap("P-LASF51", 1.81, 1.80411, 1.8239);
            glasses["P-SF68"] = new GlassMap("P-SF68", 2.0052, 1.99171, 2.03958);
            glasses["P-SF69"] = new GlassMap("P-SF69", 1.7225, 1.71535, 1.74007);
            glasses["P-SF8"] = new GlassMap("P-SF8", 1.68893, 1.68252, 1.70457);
            glasses["P-SK57"] = new GlassMap("P-SK57", 1.587, 1.58399, 1.59384);
            glasses["P-SK57Q1"] = new GlassMap("P-SK57Q1", 1.586, 1.58299, 1.59284);
            glasses["P-SK58A"] = new GlassMap("P-SK58A", 1.58913, 1.58618, 1.59581);
            glasses["P-SK60"] = new GlassMap("P-SK60", 1.61035, 1.60714, 1.61768);
            glasses["SF1"] = new GlassMap("SF1", 1.71736, 1.71031, 1.73462);
            glasses["SF10"] = new GlassMap("SF10", 1.72825, 1.72085, 1.74648);
            glasses["SF11"] = new GlassMap("SF11", 1.78472, 1.77599, 1.80645);
            glasses["SF2"] = new GlassMap("SF2", 1.64769, 1.6421, 1.66123);
            glasses["SF4"] = new GlassMap("SF4", 1.7552, 1.7473, 1.77468);
            glasses["SF5"] = new GlassMap("SF5", 1.6727, 1.66661, 1.6875);
            glasses["SF56A"] = new GlassMap("SF56A", 1.7847, 1.77605, 1.80615);
            glasses["SF57"] = new GlassMap("SF57", 1.84666, 1.8365, 1.87204);
            glasses["SF57HTultra"] = new GlassMap("SF57HTultra", 1.84666, 1.8365, 1.87204);
            glasses["SF6"] = new GlassMap("SF6", 1.80518, 1.79609, 1.82775);
            glasses["SF6HT"] = new GlassMap("SF6HT", 1.80518, 1.79609, 1.82775);
        }
    }
}