using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace MovieLibrary.Common.Mapping
{
    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile mapper);
    }
}
