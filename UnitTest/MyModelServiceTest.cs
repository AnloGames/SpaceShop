using LogicService.Dto;
using LogicService.IAdapter;
using LogicService.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class MyModelServiceTest
    {
        readonly Mock<IMyModelAdapter> mockMyModelAdapter = new Mock<IMyModelAdapter>();
        MyModelService myModelService;

        [SetUp]
        public void Setup()
        {
            mockMyModelAdapter.Setup(mma => mma.FirstOrDefaultById(It.IsAny<int>(), It.IsAny<bool>())).Returns((int id, bool _) =>
            {
                if (id == 1 || id == 2)
                {
                    return new MyModelDto() { Id = id };
                }
                return null;    
            });
            myModelService = new MyModelService(mockMyModelAdapter.Object);
        }

        [Test]
        public void CreateMyModel()
        {
            var res1 = myModelService.CreateMyModel(false, new MyModelDto());
            Assert.IsNull(res1);
            var res2 = myModelService.CreateMyModel(true, new MyModelDto());
            Assert.IsNotNull(res2);
        }
        [Test]
        public void RemoveMyModel()
        {
            int id = 1;
            var res1 = myModelService.RemoveMyModel(id);
            var expectedRes = new MyModelDto() { Id=id};
            Assert.That(res1.Id, Is.EqualTo(expectedRes.Id));
            id = 23;
            var res2 = myModelService.RemoveMyModel(id);
            Assert.IsNull(res2);
        }
    }
}
