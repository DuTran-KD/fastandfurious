using eTUTOR.Controllers;
using eTUTOR.Tests.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using eTUTOR.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using System.Configuration;
using System;

namespace eTUTOR.Tests
{
    [TestClass]
    public class RegisterValidation
    {

        /// <summary>
        /// Purpose of TC:
        /// - Kiểm tra xem học sinh có thể tạo thành công tài khoản
        ///   hệ thống sẽ trang web thông báo tài khoản vừa tạo đang chờ duyệt
        /// </summary>
        [TestMethod]
        public void ValidateStudentRegisterAccount_WithValidModel_ExpectValidNavigation()
        {
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpPostedFileBase> moqPostedFile = new Mock<HttpPostedFileBase>();

            moqRequest.Setup(r => r.Files.Count).Returns(0);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            // Arrange
            var controller = new UserController();
            var student = new student
            {
                fullname = "Trần Phú Hòa",
                username = "Hòa Trần",
                phone = "090 123 7654",
                email = "hoatran130995@gmail.com",
                password = "12345678"
            };
            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, student);

            // Act
            var redirectRoute = controller.RegisterStudent(student) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(redirectRoute );
            Assert.AreEqual("ConfirmEmail", redirectRoute.RouteValues["action"]);
            Assert.AreEqual("User", redirectRoute.RouteValues["controller"]);
            Assert.AreEqual(0, validationResults.Count);
        }

        /// <summary>
        /// Purpose of TC:
        /// - Kiểm tra xem phụ huynh có thể tạo thành công tài khoản
        ///   hệ thống sẽ trang web thông báo tài khoản vừa tạo đang chờ duyệt
        /// </summary>
        [TestMethod]
        public void ValidateParentRegisterAccount_WithValidModel_ExpectValidNavigation()
        {
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpPostedFileBase> moqPostedFile = new Mock<HttpPostedFileBase>();

            moqRequest.Setup(r => r.Files.Count).Returns(0);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            // Arrange
            var controller = new UserController();
            var parent = new parent
            {
                fullname = "Trần Phú Hòa",
                username = "Hòa Trần",
                phone = "090 123 7654",
                email = "hoatran130995@gmail.com",
                password = "12345678"
            };
            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, parent);

            // Act
            var redirectRoute = controller.RegisterParent(parent) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(redirectRoute);
            Assert.AreEqual("ConfirmEmail", redirectRoute.RouteValues["action"]);
            Assert.AreEqual("User", redirectRoute.RouteValues["controller"]);
            Assert.AreEqual(0, validationResults.Count);
        }

        /// <summary>
        /// Purpose of TC:
        /// - Kiểm tra xem giản viên có thể tạo thành công tài khoản
        ///   hệ thống sẽ trang web thông báo tài khoản vừa tạo đang chờ duyệt
        /// </summary>
        [TestMethod]
        public void ValidateTuTorRegisterAccount_WithValidModel_ExpectValidNavigation()
        {
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpPostedFileBase> moqPostedFile = new Mock<HttpPostedFileBase>();

            moqRequest.Setup(r => r.Files.Count).Returns(0);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);
            

            // Arrange
            var controller = new UserController();
            var tutor = new tutor
            {
                fullname = "Trần Phú Hòa",
                username = "Hòa Trần",
                phone = "090 123 7654",
                email = "hoatran130995@gmail.com",
                password = "12345678"
            };
            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, tutor);

            // Act

            var redirectRoute = controller.RegisterTuTor(tutor) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(redirectRoute);
            Assert.AreEqual("ConfirmEmail", redirectRoute.RouteValues["action"]);
            Assert.AreEqual("User", redirectRoute.RouteValues["controller"]);
            Assert.AreEqual(0, validationResults.Count);
        }
    }
}

