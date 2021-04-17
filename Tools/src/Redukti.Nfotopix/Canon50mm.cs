using System;
using System.Text;

namespace Redukti.Nfotopix
{
    public class Program
    {
        static void Main(string[] args)
        {
        OpticalBenchDataImporter.LensSpecifications specs = new OpticalBenchDataImporter.LensSpecifications();
        specs.parse_file("C:\\work\\github\\goptical\\data\\canon-rf-50mmf1.2\\canon-rf-50mmf1.2.txt");
        OpticalSystem.Builder systemBuilder = OpticalBenchDataImporter.buildSystem(specs, 0);
        double angleOfView = OpticalBenchDataImporter.getAngleOfViewInRadians (specs, 0);
        Vector3 direction = Vector3.vector3_001;
        bool skew = true;
        if (skew)
        {
            // Construct unit vector at an angle
            //      double z1 = cos (angleOfView);
            //      double y1 = sin (angleOfView);
            //      unit_vector = math::Vector3 (0, y1, z1);

            Matrix3 r = Matrix3.get_rotation_matrix(0, angleOfView);
            direction = r.times(direction);
        }
        PointSource.Builder ps = new PointSource.Builder(PointSource.SourceInfinityMode.SourceAtInfinity, direction)
                .add_spectral_line(SpectralLine.d)
                .add_spectral_line(SpectralLine.C)
                .add_spectral_line(SpectralLine.F);
        systemBuilder.add(ps);

        RendererSvg renderer = new RendererSvg( 800, 400);
        OpticalSystem system = systemBuilder.build();
        Console.WriteLine(system);
        // draw 2d system layout
//        system.draw_2d_fit(renderer);
//        system.draw_2d(renderer);
        SystemLayout2D systemLayout2D = new SystemLayout2D();
        systemLayout2D.layout2d(renderer, system);

        RayTraceParameters parameters = new RayTraceParameters(system);

        RayTracer rayTracer = new RayTracer();
        parameters.set_default_distribution (
                new Distribution(Pattern.MeridionalDist, 10, 0.999));
        // TODO set save generated state on point source
        Console.WriteLine(parameters.sequenceToString(new StringBuilder()).ToString());

        RayTraceResults result = rayTracer.trace(system, parameters);
        RayTraceRenderer.draw_2d(renderer, result, false, null);

//        renderer =  new RendererSvg (300, 300, Rgb.rgb_black);
//        AnalysisSpot spot = new AnalysisSpot(system);
//        spot.draw_diagram(renderer, true);
        Console.WriteLine(renderer.write(new StringBuilder()).ToString());

        }
    }
}