using System;

namespace Marketplace.Modules.Listings.Application.Services;

public interface ISlugService
{
    string Generate(string value);
}
