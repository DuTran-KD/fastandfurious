import*as $ from 'jquery';
$(document).ready(function () {


  $('.toggle_login').click(function(){
  
      if($(this).attr('data-click-state') == 1) {
        $(this).attr('data-click-state', 0)
        $(".content").html("Register your account");
        $(".title").html("Login");
        $("#cfg").html("Forgot your password?");
      } else {
        $(this).attr('data-click-state', 1)
        $(".content").html("Login your account");
        $(".title").html("Register");
        $("#cfg").html("");
      }
      $('.form').animate({
        height: "toggle",
        'padding-top': 'toggle',
        'padding-bottom': 'toggle',
        opacity: "toggle"
    }, "slow");
    
  })

  $('.toggle_register').click(function(){
  
  
        if($(this).attr('data-click-state') == 1) {
          $(this).attr('data-click-state', 0)
          $(".content_1").html("Login your account");
          $(".title_1").html("Register");
          $("#cfg_1").html("");
        } else {
          $(this).attr('data-click-state', 1)
          
          $(".content_1").html("Register your account");
          $(".title_1").html("Login");
          $("#cfg_1").html("Forgot your password?");
        }
        $('.form').animate({
          height: "toggle",
          'padding-top': 'toggle',
          'padding-bottom': 'toggle',
          opacity: "toggle"
      }, "slow");
  })

 
})
    
  


