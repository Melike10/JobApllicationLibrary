using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApllicationLibrary.Services
{
    public interface IIdentityValidator
    {
        public bool IsValid(string identyId);
        //public string Country { get; }
        public ICountryProvider CountryProvider { get; }

    }
    // hiyerarşik bir yapıda olsaydı verimiz 
    public interface ICountryData
    {
        public string Country { get; }
    }
    public interface ICountryProvider
    {
        ICountryData CountryData { get; }
    }
}
