using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;
using Shouldly;
using NUnit.Framework;

namespace FubuMVC.Tests.Resources.Conneg
{
    [TestFixture]
    public class ConnegGraphTester
    {
        private readonly ConnegGraph graph = ConnegGraph.Build(new BehaviorGraph { ApplicationAssembly = Assembly.GetExecutingAssembly() }).Result();

        [Test]
        public void build_conneg_graph_for_the_app_domain()
        {;


            graph.Readers.ShouldContain(typeof(Input1Reader1));
            graph.Readers.ShouldContain(typeof(Input1Reader2));
            graph.Readers.ShouldContain(typeof(Input2Reader));
        }

        [Test]
        public void find_readers_for_input_type()
        {
            graph.ReaderTypesFor(typeof (Input2)).Single()
                .ShouldBe(typeof (Input2Reader));
        }

        
    }

    public class Input1{}
    public class Input2{}

    public class Resource1{}
    public class Resource2{}


    public class Input1Reader1 : IReader<Input1>
    {
        public IEnumerable<string> Mimetypes { get; private set; }
        public Task<Input1> Read(string mimeType, IFubuRequestContext context)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Input1Reader2 : Input1Reader1{}


    public class Input2Reader : IReader<Input2>
    {
        public IEnumerable<string> Mimetypes { get; private set; }
        public Task<Input2> Read(string mimeType, IFubuRequestContext context)
        {
            throw new System.NotImplementedException();
        }
    }

}