using System;

namespace SpeakEase.FastEndpoint.Attributes
{
    /// <summary>
    /// Route attribute for defining the route of an endpoint.
    /// </summary>
    /// <param name="route"></param>
    public class RouteAttribute(string route):Attribute
    {
        public string Route = route;
    }
}
