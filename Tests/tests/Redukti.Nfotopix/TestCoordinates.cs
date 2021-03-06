using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Redukti.Nfotopix
{
public class TestCoordinates {

    [Test]
    public void testBasics() {
        OpticalSystem.Builder sys = new OpticalSystem.Builder();

        Group.Builder g1 = new Group.Builder().position(Vector3Pair.position_000_001);
        Group.Builder g21 = new Group.Builder().position(Vector3Pair.position_000_001);
        Group.Builder g22 = new Group.Builder().position(Vector3Pair.position_000_001);

        Surface.Builder s211 = new Surface.Builder().position(Vector3Pair.position_000_001)
                .curve(Flat.flat)
                .shape(Infinite.infinite);
        Surface.Builder s221 = new Surface.Builder().position(Vector3Pair.position_000_001)
                .curve(Flat.flat)
                .shape(Infinite.infinite);

        g22.add(s221);
        g1.add(g21);
        g1.add(g22);
        g21.add(s211);
        sys.add(g1);

        {
            OpticalSystem system1 = sys.build();
            Element s211e = system1.getGroup(0).getGroup(0).getSurface(0);
            Assert.IsTrue(s211e.local_position().isEqual(Vector3.vector3_0, 1e-10));
            Assert.IsTrue(system1.getPosition(s211e).isEqual(Vector3.vector3_0, 1e-10));
        }

        g21.localPosition(new Vector3(1, 2, 3));

        {
            OpticalSystem system1 = sys.build();
            Element s211e = system1.getGroup(0).getGroup(0).getSurface(0);
            Assert.IsTrue(s211e.local_position().isEqual(Vector3.vector3_0, 1e-10));
            Assert.IsTrue(system1.getPosition(s211e).isEqual(new Vector3(1, 2, 3), 1e-10));
        }

        g1.localPosition(new Vector3(3, 2, 1));

        {
            OpticalSystem system1 = sys.build();
            Element s211e = system1.getGroup(0).getGroup(0).getSurface(0);
            Assert.IsTrue(system1.getPosition(s211e).isEqual(new Vector3(4, 4, 4), 1e-10));
        }

        s211.localPosition(new Vector3(7, 7, 7));

        {
            OpticalSystem system1 = sys.build();
            Element s211e = system1.getGroup(0).getGroup(0).getSurface(0);
            Assert.IsTrue(system1.getPosition(s211e).isEqual(new Vector3(11, 11, 11), 1e-10));
        }

        {
            OpticalSystem system1 = sys.build();
            system1 = sys.updatePosition(s211, new Vector3(9, 9, 9));
            Element s211e = system1.getGroup(0).getGroup(0).getSurface(0);
            Assert.IsTrue(system1.getPosition(s211e).isEqual(new Vector3(9, 9, 9), 1e-10));
            Assert.IsTrue(s211e.local_position().isEqual(new Vector3(5, 5, 5), 1e-10));
        }

        g21.rotate(0, 0, -90);
        {
            OpticalSystem system1 = sys.build();
            Element s211e = system1.getGroup(0).getGroup(0).getSurface(0);
            Assert.IsTrue(system1.getPosition(s211e).isEqual(new Vector3(-1, 9, 9), 1e-10));
        }

        {
            OpticalSystem system1 = sys.build();
            system1 = sys.updatePosition(s211, new Vector3(3, 4, 5));
            Element s211e = system1.getGroup(0).getGroup(0).getSurface(0);
            Assert.IsTrue(s211e.local_position().isEqual(new Vector3(0, 1, 1), 1e-10));
        }

        g1.rotate(0, 0, 90);
        {
            OpticalSystem system1 = sys.build();
            Element s211e = system1.getGroup(0).getGroup(0).getSurface(0);
            Assert.IsTrue(system1.getPosition(s211e).isEqual(new Vector3(5, 2, 5), 1e-10));
        }

    }

}

}