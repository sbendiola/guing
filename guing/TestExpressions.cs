using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NUnit.Framework;
using Spring.Core;
using Spring.Util;

namespace guing
{
    [TestFixture]
    public class TestExpressions
    {
        [Test]
        public void ShouldSetPropertyToExpectedValue()
        {
            var afoo = new Binder()
                        .Bind<Foo>("foo")
                        .Property(x => x.Bar, "elmi")
                        .Bind();
            Assert.AreEqual("elmi", afoo.Bar);
        }

        [Test]
        public void ShouldFailIfPropertyNotWriteable()
        {
            try
            {
                new Binder().Bind<Foo>("foo")
                    .Property(x => x.NotWriteableBar, "elmi")
                    .Bind();
                Assert.Fail("expected an exception");
            }
            catch (NotWritablePropertyException expected)
            {
                Assert.That(expected.Message.Contains("NotWriteableBar"));
                Assert.That(expected.Message.Contains("Foo"));
            }

        }

        [Test]
        public void ShouldSetValueWithConstantThroughMethod()
        {
            var afoo = new Binder()
                        .Bind<Foo>("foo")
                        .Property(x => x.Booze(400))
                        .Bind();
            Assert.AreEqual(400, afoo.Booze());
			
			
        }
		
		[Test]
		public void ShouldReturnDefaultValueForPrimitive() {
			var anint = new Binder().Bind<int>("123");
			var result = anint.Bind();
			Assert.AreEqual(0, result);
		}
		
		[Test]
		public void ShouldSetPrimitiveValue() {
			var anint = new Binder().Bind<int>("123");
			var result = anint.Target(22).Bind();
			Assert.AreEqual(22, result);
		}

//        [Test]
//        public void ShouldSetPropertyToExpectedValue()
//        {
//            var binder = new Binder();
//            var binding = binder.Bind<Foo>("foo");
//            var anint = 200;
//            var foo = binding
//                .Property(x => x.Bar, "elmi")
//                //                .Property(x => x.Baz, "andy")
//                //                .Property(x => x.Booze(400))
//                //                .Property(x => x.Booze(anint))
//                .Bind();
//
//            //.OnStart(x => x.Start);
//
//            Assert.AreEqual("elmi", foo.Bar);
//        }
              
        public class Binder
        {
            public Binding<T> Bind<T>(string name) 
            {
                return new Binding<T>();
            }
			
			public Binding<T> Bind<T>(T name) 
            {
                return new Binding<T>();
            }
        }

        public class Foo
        {
            private int booze;
            public string NotWriteableBar { get; private set; }
            public string Bar { get; set; }
            public string Baz { get; set; }    
            public void Booze(int intvale)
            {
                booze = intvale;
            }

            public int Booze()
            {
                return booze;
            }
        }
    }

    
}
