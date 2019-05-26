$(document).ready(function () {

    $("#signupForm").validate({
      rules: {
      
        username: {
          required: true,
          minlength: 2
        },

        password: {
          required: true,
          minlength: 6
        },
         confirm_password: {
          required: true,
          minlength: 6,
          equalTo: "#password"
        },
        email: {
          required: true,
          email: true
        },
        phone: {
          required: true,
          number: true
        },
        agree: "required"
      },
      messages: {
        
        username: {
          required: "! Please enter a username",
          minlength: "! Your username must consist of at least 2 characters"
        },
        password: {
          required: "! Please provide a password",
          minlength: "! Your password must be at least 6 characters long"
        },
        confirm_password: {
          required: "! Please provide a password",
          minlength: "! Your password must be at least 6 characters long",
          equalTo: "! Please enter the same password as above"
        },
        email: "! Please enter a valid email address",
        agree: "! Please accept our policy",
        phone: "! Please enter your phone number"
      }
    });
});