using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Xml;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Endpoints;
using FubuMVC.Core.Packaging;
using FubuMVC.Katana;
using FubuMVC.StructureMap;
using FubuTestingSupport;
using NUnit.Framework;
using StructureMap;

namespace FubuMVC.Validation.IntegrationTesting
{
    public static class ServerHarness
    {
        private static EmbeddedFubuMvcServer _server;

        public static void Start()
        {
            var rootDirectory = GetRootDirectory();
            new FileSystem().DeleteDirectory(rootDirectory.AppendPath(FubuMvcPackageFacility.FubuContentFolder));

            var port = PortFinder.FindPort(5501);

            _server = FubuApplication.For<HarnessRegistry>()
                                     .StructureMap(new Container())
                                     .RunEmbedded(rootDirectory, port);
        }

        public static string GetRootDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory.ParentDirectory().ParentDirectory();
        }

        public static string Root
        {
            get
            {
                return _server.BaseAddress;
            }
        }

        public static EndpointDriver Endpoints
        {
            get
            {
                return _server.Endpoints;
            }
        }

        public static void Shutdown()
        {
            _server.SafeDispose();
        }
    }

    public class HarnessRegistry : FubuRegistry
    {
    }

    public static class HttpResponseExtensions
    {
        public static HttpResponse StatusCodeShouldBe(this HttpResponse response, HttpStatusCode code)
        {
            if (response.StatusCode != code)
            {
                Debug.WriteLine(response.ToString());
            }

            response.StatusCode.ShouldEqual(code);

            return response;
        }

        public static IEnumerable<string> ScriptNames(this HttpResponse response)
        {
            var document = response.ReadAsXml();
            var tags = document.DocumentElement.SelectNodes("//script");

            foreach (XmlElement tag in tags)
            {
                var name = tag.GetAttribute("src");
                yield return name.Substring(name.IndexOf('_'));
            }
        }
    }
}