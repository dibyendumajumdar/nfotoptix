using System;
using System.Text;

namespace Redukti.Nfotopix
{
    class Args
    {
        public int scenario = 0;
        public String filename = null;
        public String outputType = "layout";
        public bool skewRays = false;
        public bool dumpSystem = false;

        public static Args parseArguments(String[] args)
        {
            Args arguments = new Args();
            for (int i = 0; i < args.Length; i++)
            {
                string arg1 = args[i];
                string arg2 = i + 1 < args.Length ? args[i + 1] : null;
                if (arg1.Equals("--specfile"))
                {
                    arguments.filename = arg2;
                    i++;
                }
                else if (arg1.Equals("--scenario"))
                {
                    arguments.scenario = Int32.Parse(arg2);
                    i++;
                }
                else if (arg1.Equals("--output"))
                {
                    arguments.outputType = arg2;
                    i++;
                }
                else if (arg1.Equals("--skew"))
                {
                    arguments.skewRays = true;
                }
                else if (arg1.Equals("--dump-system"))
                {
                    arguments.dumpSystem = true;
                }
            }
            return arguments;
        }
    }

    public class LensTool
    {
        static void Main(string[] args)
        {
            Args arguments = Args.parseArguments(args);
            if (arguments.filename == null)
            {
                Console.WriteLine("Usage: --specfile inputfile [--scenario num] [--skew] [--output layout|spot] [--dump-system]");
                Environment.Exit(1);
            }
            OpticalBenchDataImporter.LensSpecifications specs = new OpticalBenchDataImporter.LensSpecifications();
            specs.parse_file(arguments.filename);
            OpticalSystem.Builder systemBuilder = OpticalBenchDataImporter.buildSystem(specs, arguments.scenario);
            double angleOfView = OpticalBenchDataImporter.getAngleOfViewInRadians(specs, arguments.scenario);
            Vector3 direction = Vector3.vector3_001;
            if (arguments.skewRays)
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

            OpticalSystem system = systemBuilder.build();
            if (arguments.dumpSystem)
            {
                Console.WriteLine(system);
            }
            if (arguments.outputType.Equals("layout"))
            {
                RendererSvg renderer = new RendererSvg(800, 400);
                // draw 2d system layout
                SystemLayout2D systemLayout2D = new SystemLayout2D();
                systemLayout2D.layout2d(renderer, system);

                RayTraceParameters parameters = new RayTraceParameters(system);

                RayTracer rayTracer = new RayTracer();
                parameters.set_default_distribution(
                    new Distribution(Pattern.MeridionalDist, 10, 0.999));
                // TODO set save generated state on point source
                if (arguments.dumpSystem)
                {
                    Console.WriteLine(parameters.sequenceToString(new StringBuilder()).ToString());
                }
                RayTraceResults result = rayTracer.trace(system, parameters);
                RayTraceRenderer.draw_2d(renderer, result, false, null);
                Console.WriteLine(renderer.write(new StringBuilder()).ToString());
            }
            if (arguments.outputType.Equals("spot"))
            {
                RendererSvg renderer = new RendererSvg(300, 300, Rgb.rgb_black);
                renderer = new RendererSvg(300, 300, Rgb.rgb_black);
                AnalysisSpot spot = new AnalysisSpot(system);
                spot.draw_diagram(renderer, true);
                Console.WriteLine(renderer.write(new StringBuilder()).ToString());
            }
        }
    }
}