using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaptainsLogNamespace
{
    public class TacAtomicClockSettings
    {
        public double earthSecondsPerKerbinDay { get; private set; }
        public double kerbinSolarDaysPerYear { get; private set; }

        public double initialOffsetInEarthSeconds { get; set; }
        public double kerbinSecondsPerMinute { get; set; }
        public double kerbinMinutesPerHour { get; set; }
        public double kerbinHoursPerDay { get; set; }
        public double kerbinDaysPerMonth { get; set; }
        public double kerbinDaysPerYear { get; set; }
        public double kerbinMonthsPerYear { get; set; }

        public bool debug { get; set; }

        public TacAtomicClockSettings()
        {
            // Retrieve the values straight from the game. All values will be in Earth seconds.
            CelestialBody kerbin = FlightGlobals.Bodies[1];
            CelestialBody mun = FlightGlobals.Bodies[2];
            CelestialBody minmus = FlightGlobals.Bodies[3];
            double kerbinSiderealRotationPeriod = kerbin.rotationPeriod; // ~ 21,599.912
            double kerbinOrbitalPeriod = kerbin.orbit.period; // ~ 9,203,544.618
            double munOrbitalPeriod = mun.orbit.period; // ~ 138,984.377
            double minmusOrbitalPeriod = minmus.orbit.period; // ~ 1,077,310.521

            // Length of the Kerbin Solar Day. Two ways to calculate according to
            // http://en.wikipedia.org/wiki/Sidereal_time#Sidereal_time_and_solar_time
            // and http://en.wikipedia.org/wiki/Sidereal_time#Sidereal_days_compared_to_solar_days_on_other_planets
            // Using method 2:
            double kerbinSolarDay = kerbinSiderealRotationPeriod / (1 - kerbinSiderealRotationPeriod / kerbinOrbitalPeriod);
            // Squad changed the sidereal day so that the solar day is now 6 hours (21,600.0 seconds)
            // The sidereal day should be ~21,549.425 seconds. Shorter by ~50.575 seconds.
            // Except that it looks like something is wrong? The sidereal day is 5:59:59 (21,599.912 s)
            // which would make the solar day 21650.724 s.

            Log.Info("Kerbin Sidereal Rotation Period = " + kerbinSiderealRotationPeriod
                + "\nKerbin Solar Day = " + kerbinSolarDay
                + "\nKerbin Orbital Period = " + kerbinOrbitalPeriod
                + "\nMun Orbital Period = " + munOrbitalPeriod
                + "\nMinmus Orbital Period = " + minmusOrbitalPeriod);

            earthSecondsPerKerbinDay = kerbinSolarDay;
            kerbinSolarDaysPerYear = kerbinOrbitalPeriod / kerbinSolarDay; // ~ 426.090 days

            // The Kerbal Space Center is not at 0 Longitude but the game starts at about noon there,
            // so we need to adjust the start time so that time=0 means noon at KSC.
            // The KSC is at 74:34:31 W according to the wiki, which
            double kscLongitudeDegrees = 74 + (34.0 / 60.0) + (31.0 / 3600.0); // ~ 74.575 degrees
            initialOffsetInEarthSeconds = kerbinSiderealRotationPeriod * (1.0 - kscLongitudeDegrees / 180.0);

            // Mun synodic period
            // http://en.wikipedia.org/wiki/Synodic_period#Synodic_period
            double munSynodicPeriod = 1 / (1 / munOrbitalPeriod - 1 / kerbinOrbitalPeriod);
            // approx 141,114.997 seconds (or 6.533 Kerbin Solar Days)

            // Minmus synodic period
            double minmusSynodicPeriod = 1 / (1 / minmusOrbitalPeriod - 1 / kerbinOrbitalPeriod);
            // approx 1,220,132.261 seconds (or 56.488 Kerbin Solar Days)

            // Mun's synodic period is too short to define a month. There would be 65.220 months
            // per year. Using Minmus' synodic period gives some interesting numbers, month =
            // 56.488 days & year = 7.543 months. But I do not like it. Too few months per year.
            // We can use 4x Mun's synodic period, which makes month = 26.132 days and year =
            // 16.305 months.
            // Kerbal's have four fingers on each hand (including the thumb), so why not?
            double kerbinMonth = munSynodicPeriod * 4.0;

            kerbinSecondsPerMinute = 24.0;
            kerbinMinutesPerHour = 24.0;
            kerbinHoursPerDay = 12.0;
            kerbinDaysPerMonth = kerbinMonth / kerbinSolarDay; // ~ 26.132 solar days
            kerbinMonthsPerYear = kerbinOrbitalPeriod / kerbinMonth; // ~ 16.305 months
            kerbinDaysPerYear = kerbinSolarDaysPerYear;
        }
    }
}
