using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Infrastructure.Domain.Test.Specification
{
    using Framework.Infrastructure.Domain.Specification;

    [TestClass]
    public class SpecificationTest
    {
        [TestMethod]
        public void Sepcification_OfType_Test()
        {
            //初始化
            var baseSpec = new DirectSpecification<BaseEntity>(be => be.Id == 1);

            //操作
            var inheritSpec = baseSpec.OfType<InheritEntity>();

            //验证
            Assert.IsNotNull(inheritSpec);
            Assert.IsTrue(inheritSpec.SatisfiedBy().Compile()(new InheritEntity() { Id = 1 }));
        }

        [TestMethod]
        public void Sepcification_OfType_And_Composite_Test()
        {
            //初始化
            var inheritSpec = new DirectSpecification<InheritEntity>(be => be.SampleProperty == "Test");
            var baseSpec = new DirectSpecification<BaseEntity>(be => be.Id == 1).OfType<InheritEntity>();

            //操作
            var result = inheritSpec & baseSpec.OfType<InheritEntity>();

            //验证
            Assert.IsNotNull(inheritSpec);
            Assert.IsTrue(inheritSpec.SatisfiedBy().Compile()(
                new InheritEntity() { Id = 1, SampleProperty = "Test" }));
        }
    }
}
