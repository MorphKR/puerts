using NUnit.Framework;

namespace Puerts.UnitTest
{
    [TestFixture]
    public class OptionalParametersTest
    {
        [Test]
        public void WarpTest1()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test(1,3);
           ");
            Assert.AreEqual(132, ret);
            jsEnv.Dispose();
        }
        [Test]
        public void WarpTest2()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test('1',3);
           ");
            Assert.AreEqual(32, ret);
            jsEnv.Dispose();
        }
        [Test]
        public void WarpTest3()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test('1');
           ");
            Assert.AreEqual(12, ret);
            jsEnv.Dispose();
        }
        [Test]
        public void WarpTest4()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test(6,6,6);
           ");
            Assert.AreEqual(666, ret);
            jsEnv.Dispose();
        }
        [Test]
        public void WarpTest5()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test2('1',100);
           ");
            Assert.AreEqual(100, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void WarpTest6()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test2('1');
           ");
            Assert.AreEqual(0, ret);
            jsEnv.Dispose();
        }
        [Test]
        public void WarpTest7()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);

            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test5('1', 1, false,false,false);
           ");
            Assert.AreEqual(-1, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void WarpTest8()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test5('1', 1, false);
           ");
            Assert.AreEqual(-1, ret);
            jsEnv.Dispose();
        }

        //行为不一致
        /*[Test]
        public void WarpTest9()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
                const CS = require('csharp');
                let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
                let ret = 0;                
                try{temp.Test3('1');}catch(e){ret = 1;}
                ret;
            ");
            Assert.AreEqual(1, ret);
            jsEnv.Dispose();
        }*/

        [Test]
        public void WarpTest10()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               let ret = 0;                
               try{temp.Test3('1',1);}catch(e){ret = 1;}
               ret;
           ");
            Assert.AreEqual(0, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void WarpTest11()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               let ret = 0;                
               try{temp.Test4('1');}catch(e){ ret = 1; }
               ret;
           ");
            Assert.AreEqual(1, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void WarpTest12()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               let ret = 0;                
               try{temp.Test4('1',1);}catch(e){ret = 1;}
               ret;
           ");
            Assert.AreEqual(0, ret);
            jsEnv.Dispose();
        }
        [Test]
        public void WarpTest14()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            PuertsStaticWrap.AutoStaticCodeRegister.Register(jsEnv);
            string ret = jsEnv.Eval<string>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.TestFilter('world');
           ");
            Assert.AreEqual("world hello", ret);
            jsEnv.Dispose();
        }

        [Test]
        public void ReflectTest1()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test(1,3);
           ");
            Assert.AreEqual(132, ret);
            jsEnv.Dispose();
        }
        [Test]
        public void ReflectTest2()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test('1',3);
           ");
            Assert.AreEqual(32, ret);
            jsEnv.Dispose();
        }
        [Test]
        public void ReflectTest3()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test('1');
           ");
            Assert.AreEqual(12, ret);
            jsEnv.Dispose();
        }
        [Test]
        public void ReflectTest4()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test(6,6,6);
           ");
            Assert.AreEqual(666, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void ReflectTest5()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test2('1',100);
           ");
            Assert.AreEqual(100, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void ReflectTest6()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test2('1');
           ");
            Assert.AreEqual(0, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void ReflectTest7()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test5('1', 1, false,false,false);
           ");
            Assert.AreEqual(-1, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void ReflectTest8()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.Test5('1', 1, false);
           ");
            Assert.AreEqual(-1, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void ReflectTest9()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               let ret = 0;                
               try{temp.Test3('1');}catch(e){ret = 1;}
               ret;
           ");
            Assert.AreEqual(1, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void ReflectTest10()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               let ret = 0;                
               try{temp.Test3('1',1);}catch(e){ret = 1;}
               ret;
           ");
            Assert.AreEqual(0, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void ReflectTest11()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               let ret = 0;                
               try{temp.Test4('1');}catch(e){ if (e.message.indexOf('invalid') != -1) ret = 1; }
               ret;
           ");
            Assert.AreEqual(1, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void ReflectTest12()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               let ret = 0;                
               try{temp.Test4('1',1);}catch(e){ret = 1;}
               ret;
           ");
            Assert.AreEqual(0, ret);
            jsEnv.Dispose();
        }

        [Test]
        public void ReflectTest13()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int ret = jsEnv.Eval<int>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();            
               let ret = temp.Test6(1);
               ret;
           ");
            Assert.AreEqual(2, ret);
            jsEnv.Dispose();
        }
        [Test]
        public void ReflectTest14()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            string ret = jsEnv.Eval<string>(@"
               let temp = new CS.Puerts.UnitTest.OptionalParametersClass();
               temp.TestFilter('world');
           ");
            Assert.AreEqual("world hello", ret);
            jsEnv.Dispose();
        }
    }
}