﻿using Microsoft.Extensions.DependencyInjection;

namespace Prueba.Application.Common.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
