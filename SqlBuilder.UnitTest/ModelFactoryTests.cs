using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class ModelFactoryTests
  {
    public ModelFactoryTests()
    {
      // in case there are any registrations
      ModelFactory.Clear();
    }

    [TestMethod]
    public void CreateViewModel_creates_object()
    {
      ModelFactory<TestDataModel, TestViewModel> modelFactory = new ModelFactory<TestDataModel, TestViewModel>();

      modelFactory.CreateViewModel().MustNotBeNull();
    }

    [TestMethod]
    public void FindDataModelMember_after_registration_finds_member()
    {
      ModelFactory.RegisterType<TestMappedDataModel>();

      ModelFactory<TestMappedDataModel, TestViewModel> modelFactory = new ModelFactory<TestMappedDataModel, TestViewModel>();

      modelFactory.FindDataModelMember("Foo").MustEqual("Foo");
    }

    private class TestMappedDataModel : DataModel<TestMappedDataModel, TestViewModel>
    {
      public string Foo { get; set; }
    }

    private class TestDataModel : DataModel
    {
      public string Foo { get; set; }
    }

    private class TestViewModel
    {
      public string Foo { get; set; }
    }
  }
}