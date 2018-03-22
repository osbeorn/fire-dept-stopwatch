using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireDeptStopwatch.Classes
{
    public class Country
    {
        public CountryCode Code { get; set; }
        public string Name { get; set; }

        public Country(CountryCode code, string name)
        {
            Code = code;
            Name = name;
        }

        public static List<Country> GetList()
        {
            return new List<Country>()
            {
                new Country(CountryCode.AT, "Avstrija"),
                new Country(CountryCode.AT_2, "Avstrija (Bischofstetten)"),
                new Country(CountryCode.HR, "Hrvaška"),
                new Country(CountryCode.SI, "Slovenija")
            };
        }
    }

    public enum CountryCode
    {
        AT, // Austria
        AT_2, // Austria Bischofstetten
        HR, // Croatia
        SI // Slovenia
    }
}
