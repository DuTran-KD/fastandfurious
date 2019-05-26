using System;
using Moq;
using System.Web;
using System.Web.Mvc;
using eTUTOR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eTUTOR.Controllers;
using eTUTOR.Models;
using System.Web.Routing;
using System.Web.SessionState;
using eTUTOR.Tests.Support;
using eTUTOR.Filter;

namespace eTUTOR.Tests
{
    [TestClass]
    public class UnitTest_Login
    {
        /*
            UNIT TEST FOR TUTOR LOGIN FUNCTION
        */
        //Test case : tutor login with valid email and password
        [TestMethod]
        public void TutorLogin_WithValidInput()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();

            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);           
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var tutorAccount = new tutor
            {
                email = "tieumynvx@gmail.com",
                password = "12345678",
            };
            
            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, tutorAccount);

            //Action
            var redirectRoute = controller.Login(tutorAccount.email, tutorAccount.password, null) as JsonResult;  
            //Assert
            Assert.IsNotNull(redirectRoute.Data);
        }
        //Test case : tutor login with invalid email
        [TestMethod]
        public void TutorLogin_WithInvalidUserName()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var tutorAccount = new tutor
            {
                email = "invalidusername@gmail.com",
                password = "12345678",
            };
              var temp = ( new { error = "No Access", message = "username hoặc email không tồn tại" , status =0});
            
            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, tutorAccount);

            //Action
            var redirectRoute = controller.Login(tutorAccount.email, tutorAccount.password, null) as JsonResult;
            var data = redirectRoute.Data;
            //Assert
            Assert.IsNotNull(redirectRoute);
            Assert.AreEqual(temp.ToString(), data.ToString());
            
        }
        //Test case : tutor login with Valid email and Invalid password
        [TestMethod]
        public void TutorLogin_WithValidUsernameAndInvalidPassword()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var tutorAccount = new tutor
            {
                email = "tieumynvx@gmail.com",
                password = "invalidPassword",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, tutorAccount);

            //Action
            var redirectRoute = controller.Login(tutorAccount.email, tutorAccount.password, null) as JsonResult;
            var temp = (new { error = "No Access", message = "Mật khẩu sai rồi !", status = 0 });
            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
        }
        //Test case : tutor login with account Status is 2 (Account not activated yet)
        [TestMethod]
        public void TutorLogin_WithAccountStatusIs2()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var tutorAccount = new tutor
            {
                email = "nguyenthequang@gmail.com",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, tutorAccount);
            var temp = (new { error = "No Access", message = "Tài khoản của bạn chưa được kích hoạt", status = 0 });

            //Action
            var redirectRoute = controller.Login(tutorAccount.email, tutorAccount.password, null) as JsonResult;

            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
        }
        //Test case : tutor login with account status is 3 (Account blocked)
        [TestMethod]
        public void TutorLogin_WithAccountStatusIs3()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);



            var controller = new UserController();
            var tutorAccount = new tutor
            {
                email = "tieumynvx123@gmail.com",
                password = "12345678",
            };
            
            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, tutorAccount);
            var temp = (new { error = "Block", message = "Tài khoản của bạn đã bị khóa , vui lòng liên hệ ban quản trị hệ thống", status = 3 });

            //Action
            var redirectRoute = controller.Login(tutorAccount.email, tutorAccount.password, null) as JsonResult;

            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
        }
        /*
            UNIT TEST FOR STUDENT LOGIN FUNCTION
        */

        //Test case : student login with valid username and valid password
        [TestMethod]
        public void StudentLogin_WithValidInput()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var studentAccount = new tutor
            {
                username = "vyvy",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, studentAccount);

            //Action
            var redirectRoute = controller.Login(studentAccount.username, studentAccount.password, null) as JsonResult;

            //Assert
            Assert.IsNotNull(redirectRoute.Data);
        }
        //Test case : student login with invalid username
        [TestMethod]
        public void StdudentLogin_WithInvalidUsername()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var studentAccount = new tutor
            {
                username = "invalidStudentusername@gmail.com",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, studentAccount);

            //Action
            var temp = (new { error = "No Access", message = "username hoặc email không tồn tại", status = 0 });
            var redirectRoute = controller.Login(studentAccount.username, studentAccount.password, null) as JsonResult;

            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
        }
        //Test case : student login with valid username and invalid password
        [TestMethod]
        public void StudentLogin_WithValidUsernameAndInvalidPassword()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var studentAccount = new tutor
            {
                username = "vyvy",
                password = "InvalidPassword",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, studentAccount);

            //Action
            var redirectRoute = controller.Login(studentAccount.username, studentAccount.password, null) as JsonResult;
            var temp = (new { error = "No Access", message = "Mật khẩu sai rồi !", status = 0 });

            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
            
        }
        //Test case : student login with account status is 2 (Account not activated yet)
        [TestMethod]
        public void StudentLogin_WithAccountStatusIs2()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var studentAccount = new tutor
            {
                username = "LevanD",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, studentAccount);
            var temp = (new { error = "No Access", message = "Tài khoản của bạn chưa được kích hoạt", status = 2 });

            //Action
            var redirectRoute = controller.Login(studentAccount.username, studentAccount.password, null) as JsonResult;

            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
        }
        //Test case : student login with account status is 3 (Account blocked)
        [TestMethod]
        public void StudentLogin_WithAccountStatusIs3()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var studentAccount = new tutor
            {
                username = "thanh",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, studentAccount);
            var temp = (new { error = "Block", message = "Tài khoản của bạn đã bị khóa , vui lòng liên hệ ban quản trị hệ thống", status = 3 });

            //Action
            var redirectRoute = controller.Login(studentAccount.username, studentAccount.password, null) as JsonResult;

            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
        }
        /*
           UNIT TEST FOR PARENT LOGIN FUNCTION
        */

        //Test case : parent login with valid input
        [TestMethod]
        public void ParentLogin_WithValidInput()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var parentAccount = new tutor
            {
                email = "parent@gmail.com",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, parentAccount);

            //Action
            var redirectRoute = controller.Login(parentAccount.email, parentAccount.password, null) as JsonResult;

            //Assert
            Assert.IsNotNull(redirectRoute.Data);            
        }
        //Test case : parent login with invalid email
        [TestMethod]
        public void ParentLogin_WithInvalidEmail()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var parentAccount = new tutor
            {
                email = "InvalidParentAccount@gmail.com",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, parentAccount);

            //Action
            var redirectRoute = controller.Login(parentAccount.email, parentAccount.password, null) as JsonResult;
            var temp = (new { error = "No Access", message = "username hoặc email không tồn tại", status = 0 });
            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
            
        }
        //Test case : parent login with valid email and invalid password
        [TestMethod]
        public void ParentLogin_WithValidEmailAndInvanlidPassword()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var parentAccount = new tutor
            {
                email = "parent@gmail.com",
                password = "invalidPassword",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, parentAccount);
            var temp = (new { error = "No Access", message = "Mật khẩu sai rồi !", status = 0 });

            //Action
            var redirectRoute = controller.Login(parentAccount.email, parentAccount.password, null) as JsonResult;

            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
        }
        //Test case : parent login with account status is 2 (Account not activated yet)
        [TestMethod]
        public void ParentLogin_WithAccountStatusIs2()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var parentAccount = new tutor
            {
                email = "abc@gmail.com",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, parentAccount);
            var temp = (new { error = "No Access", message = "Tài khoản của bạn chưa được kích hoạt", status = 2 });

            //Action
            var redirectRoute = controller.Login(parentAccount.email, parentAccount.password, null) as JsonResult;

            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
        }
        //Test case : parent login with account status is 3 (Account blocked)
        [TestMethod]
        public void ParentLogin_WithAccountStatusIs3()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var parentAccount = new tutor
            {
                email = "parent345@gmail.com",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, parentAccount);
            var temp = (new { error = "Block", message = "Tài khoản của bạn đã bị khóa , vui lòng liên hệ ban quản trị hệ thống", status = 3 });

            //Action
            var redirectRoute = controller.Login(parentAccount.email, parentAccount.password, null) as JsonResult;

            //Assert
            Assert.IsNotNull(redirectRoute.Data);
            Assert.AreEqual(temp.ToString(), redirectRoute.Data.ToString());
        }
    }
}
