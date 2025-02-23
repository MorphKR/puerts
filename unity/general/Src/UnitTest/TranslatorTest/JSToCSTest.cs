﻿/*
* Tencent is pleased to support the open source community by making Puerts available.
* Copyright (C) 2020 THL A29 Limited, a Tencent company.  All rights reserved.
* Puerts is licensed under the BSD 3-Clause License, except for the third-party components listed in the file 'LICENSE' which may be subject to their corresponding license terms. 
* This file is subject to the terms and conditions defined in file 'LICENSE', which is part of this source code package.
*/

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Puerts.UnitTest.TranslatorTest
{
    public class JSToCSTest
    {
        [Test]
        public void BaseTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());

            int ret = jsEnv.Eval<int>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                obj.Id(100);
            ");

            jsEnv.Dispose();

            Assert.AreEqual(100, ret);
        }


        [Test]
        public void NestTypeTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());

            int ret = jsEnv.Eval<int>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass.Inner();
                obj.Add(obj.A, 1);
            ");

            jsEnv.Dispose();

            Assert.AreEqual(101, ret);
        }

        [Test]
        public void NullString()
        {
            var jsEnv = new JsEnv(new TxtLoader());

            bool ret = jsEnv.Eval<bool>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                obj.IsStringNull(null);
            ");

            jsEnv.Dispose();

            Assert.True(ret);
        }

        [Test]
        public void DoubleInheritStaticMethod()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            bool res = jsEnv.Eval<bool>(@"
                CS.Puerts.UnitTest.ParentParent.doSth();
                CS.Puerts.UnitTest.SonClass.doSth();
                true
            ");
            Assert.AreEqual(true, res);
            jsEnv.Dispose();
        }
        [Test]
        public void Array()
        {
            var jsEnv = new JsEnv(new TxtLoader());

            var ret = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.ArrayTest();
                let sum = 0;
                for (var i = 0; i < 10; i++) {
                    for (var j = 0; j < obj['a' + i].Length; j++) {
                        sum += Number(obj['a' + i].get_Item(j));
                    }
                }
                for (var i = 0; i < obj.astr.Length; i++) {
                    sum += obj.astr.get_Item(i);
                }
                for (var i = 0; i < obj.ab.Length; i++) {
                    sum += obj.ab.get_Item(i);
                }
                let sum2 = 0;
                for (var i = 0; i < 10; i++) {
                    for (var j = 0; j < obj['a' + i].Length; j++) {
                        obj['a' + i].set_Item(j, obj['a' + i].get_Item(j) + obj['a' + i].get_Item(j));
                    }
                }
                for (var i = 0; i < 10; i++) {
                    for (var j = 0; j < obj['a' + i].Length; j++) {
                        sum2 += Number(obj['a' + i].get_Item(j));
                    }
                }
                //CS.System.Console.WriteLine('sum = ' + sum2 );
                sum + sum2;
            ");

            jsEnv.Dispose();

            Assert.AreEqual("240hellojohntruefalsetruefalse480", ret);
        }

        [Test]
        public void Long()
        {
            Assert.Catch(() =>
            {
                var jsEnv1 = new JsEnv(new TxtLoader());
                jsEnv1.Eval(@"
                    let obj = new CS.Puerts.UnitTest.DerivedClass();
                    obj.Long(1);
                ");
            });

            var jsEnv = new JsEnv(new TxtLoader());
            var ret = jsEnv.Eval<long>(@"
                    let obj = new CS.Puerts.UnitTest.DerivedClass();
                    obj.Long(1n);
                ");
            Assert.AreEqual((long)1, ret);

            Assert.Catch(() =>
            {
                var jsEnv2 = new JsEnv(new TxtLoader());
                jsEnv2.Eval<long>("1");
            });

            jsEnv.Dispose();
        }

        [Test]
        public void NestTypeStaticMethodTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());

            int ret = jsEnv.Eval<int>(@"
                let res = puerts.$ref(0);
                CS.Puerts.UnitTest.DerivedClass.Inner.Sub(10,5,res);
                puerts.$unref (res);
            ");

            jsEnv.Dispose();

            Assert.AreEqual(5, ret);
        }

        [Test]
        public void VirtualTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());

            string ret = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let ret = obj.TestVirt(3,'gyx');
                ret;
            ");

            jsEnv.Dispose();

            Assert.AreEqual("gyx30 10", ret);
        }

        [Test]
        public void CallBaseVirt()
        {
            var jsEnv = new JsEnv(new TxtLoader());

            string ret = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let ret = obj.TestBaseVirt();
                ret;
            ");

            jsEnv.Dispose();

            Assert.AreEqual("base print fixed-base-static-field ", ret);
        }

        [Test]
        public void InterfaceTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            string ret = jsEnv.Eval<string>(@"
                let iSubObj = new CS.Puerts.UnitTest.ISubA();
                let iSubObj1 = puerts.$ref(new CS.Puerts.UnitTest.ISubA());
                let iSubObj2 = puerts.$ref(new CS.Puerts.UnitTest.ISubA());
                let deriveObj = new CS.Puerts.UnitTest.DerivedClass();
                deriveObj.OutRefFunc(iSubObj,iSubObj1,iSubObj2);
                let res = iSubObj.TestDerivedObj(deriveObj,3,'gyx') + iSubObj.TestArr(iSubObj.a8) + iSubObj.running + puerts.$unref(iSubObj1).cmpTarget;
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(ret, "gyx30 10789true100");
        }

        [Test]
        public void AbstractRefParamTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            string name = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.C();
                let name = puerts.$ref ('gyx');
                let res = puerts.$ref ('');
                obj.TestRef(name,res);
                puerts.$unref (name) + puerts.$unref(res);
            ");
            jsEnv.Dispose();
            Assert.AreEqual(name, "annagyx23");

        }

        [Test]
        public void StringOutParamIsNullTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            string res = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let res =  puerts.$ref ('');
                obj.OutString(res);
                res.value;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, null);

        }

        [Test]
        public void StructTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            string res = jsEnv.Eval<string>(@"
                let s = new CS.Puerts.UnitTest.S(22,'haha');
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let res = s.TestParamObj(obj);
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "haha220 111");
        }

        [Test]
        public void ParamStructTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var age = jsEnv.Eval<int>(@"
                let s = new CS.Puerts.UnitTest.S(22,'gyx');
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let res = obj.PrintStruct(s);
                s.Age;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(age, 22);
        }

        [Test]
        public void NoParamConstructorStructTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            string res = jsEnv.Eval<string>(@"
                let s = new CS.Puerts.UnitTest.S();
                s
            ");
            Assert.AreEqual(res, "Puerts.UnitTest.S");
            jsEnv.Dispose();
        }

        [Test]
        public void ParamStructRefTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var age = jsEnv.Eval<int>(@"
                let s = puerts.$ref(new CS.Puerts.UnitTest.S(22,'gyx'));
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                obj.PrintStructRef(s);
                puerts.$unref(s).Age;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(age, 20);
        }

        [Test]
        public void OverloadTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            string res = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let i = '1';
                let j = '2';
                let res = obj.Adds(i,j);
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "12");
        }

        [Test]
        public void EventTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.EventTest();
                let dele = new CS.Puerts.UnitTest.MyCallBack((str) => { return str; });
                obj.myCallBack = CS.System.Delegate.Combine(obj.myCallBack, dele);
                obj.add_myEvent(dele);
                CS.Puerts.UnitTest.EventTest.add_myStaticEvent(dele);
                let res = obj.Trigger();
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "start  delegate  event  static-event  end");
        }

        [Test]
        public void TryCatchFinallyTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            string res = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let t = puerts.$ref(false);
                let c = puerts.$ref(false);
                let f = puerts.$ref(false);
                let e = puerts.$ref(false);
                let res = obj.TryCatchFinally(true, t, c, f, e);
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "cfe");
        }

        [Test]
        public void CatchByNextLevelTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            string res = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let f1 = puerts.$ref(false);
                let f2 = puerts.$ref(false);
                let f3 = puerts.$ref(false);
                let res = obj.CatchByNextLevel(f1,f2, f3);
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "try-try-finally-catch-finally");
        }
        
        [Test]
        public void DefaultParamTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            string res = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let res = obj.TestDefaultParam();
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "1str");
        }


        [Test]
        public void ParamBaseClassTest()
        {

            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let iobj = new CS.Puerts.UnitTest.ISubA();
                let res;
                res = iobj.TestBaseObj(obj,1,'gyx');
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "gyx10 10");
        }

        [Test]
        public void ParamIntArrayTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            int res = jsEnv.Eval<int>(@"
                let obj = new CS.Puerts.UnitTest.ISubA();
                let arrayInt = CS.System.Array.CreateInstance(puerts.$typeof(CS.System.Int32), 3);
                arrayInt.set_Item(0, 111);
                arrayInt.set_Item(1, 222);
                arrayInt.set_Item(2, 333);
                let res = obj.TestArrInt(arrayInt);
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, 666);
        }

        [Test]
        public void UndefinedParamIntTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<int>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let j;
                let res ;
                try { res = obj.TestInt(j);} catch(e){ res = -1;}
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, -1);
        }
        [Test]
        public void NullParamIntTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<int>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let res;
                try {res = obj.TestInt(null);} catch(e){ res = -1;}
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, -1);
        }

        [Test]
        public void UndefinedParamStringTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let s;
                let res;
                try { res = obj.TestString(s);} catch(e){ res = 'null';}
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "gyx");
        }

        [Test]
        public void NullParamStringTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let res;
                try { res = obj.TestString(null);} catch(e){ res = 'null';}
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "gyx");
        }

        [Test]
        public void UndefinedParamStringLenTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<int>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let s;
                let res;
                try { res = obj.TestStringLen(s);} catch(e){ res = -1;}
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, -1);
        }


        [Test]
        public void UndefinedParamDateTimeTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<string>(@"
                let obj = new CS.Puerts.UnitTest.DerivedClass();
                let t;
                let res;
                try { res = obj.TestTime(t);} catch(e){ res = 'null';}
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "null");
        }

        [Test]
        public void NullParamDateTimeTest()
        {
            Assert.Catch(() =>
            {
                var jsEnv = new JsEnv(new TxtLoader());
                jsEnv.Eval(@"
                    let obj = new CS.Puerts.UnitTest.DerivedClass();
                    let res;
                    res = obj.TestTime(null);"
                );
                jsEnv.Dispose();
            });
        }

        [Test]
        public void UndefinedParamArrayBufferTest()
        {
            Assert.Catch(() =>
            {
                var jsEnv = new JsEnv(new TxtLoader());
                jsEnv.Eval(@"
                    let obj = new CS.Puerts.UnitTest.DerivedClass();
                    let res;
                    res = obj.TestArrayBuffer(res);"
                );
                jsEnv.Dispose();
            });
        }


        [Test]
        public void NullParamArrayBufferTest()
        {
            Assert.Catch(() =>
            {
                var jsEnv = new JsEnv(new TxtLoader());
                jsEnv.Eval(@"
                    let obj = new CS.Puerts.UnitTest.DerivedClass();
                    let res;
                    res = obj.TestArrayBuffer(null);"
                );
                jsEnv.Dispose();
            });
        }
        [Test]
        public void MathTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<int>(@"
                let res1 = Math.abs(-10);
                let res2 = Math.sqrt(4)
                res1 + res2;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, 12);
        }

        [Test]
        public void ReadonlyStaticFieldTest()
        {
            // readonly static的字段无法在js中修改，并且读取它时不会产生跨语言
            var jsEnv = new JsEnv(new TxtLoader());
            Assert.True(Puerts.UnitTest.ReadonlyStaticTest.ReadonlyStaticField == 1);
            Assert.True(Puerts.UnitTest.ReadonlyStaticTest.StaticField == 3);
            var ret = jsEnv.Eval<bool>(@"
                CS.Puerts.UnitTest.ReadonlyStaticTest.ReadonlyStaticField = 2;
                CS.Puerts.UnitTest.ReadonlyStaticTest.StaticField = 4;
                typeof Object.getOwnPropertyDescriptor(CS.Puerts.UnitTest.ReadonlyStaticTest, 'ReadonlyStaticField').get == 'undefined' &&
                !typeof Object.getOwnPropertyDescriptor(CS.Puerts.UnitTest.ReadonlyStaticTest, 'ReadonlyStaticField').configurable
                typeof Object.getOwnPropertyDescriptor(CS.Puerts.UnitTest.ReadonlyStaticTest, 'StaticField').configurable
            ");
            Assert.True(Puerts.UnitTest.ReadonlyStaticTest.ReadonlyStaticField == 1);
            Assert.True(Puerts.UnitTest.ReadonlyStaticTest.StaticField == 4);
            Assert.True(ret);
            jsEnv.Dispose();
        }

        [Test]
        public void OperatorAddTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<int>(@"
                let obj1 = new CS.Puerts.UnitTest.BaseClass();
                let obj2 = new CS.Puerts.UnitTest.BaseClass();
                obj1.baseIntField = 11;
                obj2.baseIntField = 22;
                let obj3 = CS.Puerts.UnitTest.BaseClass.op_Addition(obj1, obj2);
                obj3.baseIntField;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, 33);
        }

        [Test]
        public void OperatorGreaterThanOrEqualTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<string>(@"
                let obj1 = new CS.Puerts.UnitTest.BaseClass();
                let obj2 = new CS.Puerts.UnitTest.BaseClass();
                obj1.baseIntField = 11;
                obj2.baseIntField = 22;
                let flag = CS.Puerts.UnitTest.BaseClass.op_GreaterThanOrEqual(obj1, obj2);
                let res = flag + '-';
                flag = CS.Puerts.UnitTest.BaseClass.op_LessThanOrEqual(obj1, obj2);
                res = res + flag;
                res;
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, "false-true");
        }

        [Test]
        public void ThisArrayTest()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            var res = jsEnv.Eval<int>(@"
                let obj1 = new CS.Puerts.UnitTest.BaseClass();
                obj1.set_Item(0,111);
                obj1.set_Item(1,222);
                obj1.get_Item(1);
            ");
            jsEnv.Dispose();
            Assert.AreEqual(res, 222);
        }
        
        [Test]
        public void Reentrant()
        {
            var jsEnv = new JsEnv(new TxtLoader());
            jsEnv.Eval(@"
                let obj = new CS.Puerts.UnitTest.Reentrant();
                function dosomething(){}
                obj.Callback = () => {
                    obj.Call(false);
                    dosomething();// 注释这行，或者Call没返回值就没事
                }
                obj.Call(true);
                
            ");
            jsEnv.Dispose();
        }
    }
}