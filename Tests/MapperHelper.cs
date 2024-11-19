using Application;
using AutoMapper;

namespace Tests;

public static class MapperHelper
{
    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
        return config.CreateMapper();
    }
}