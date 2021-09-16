
var PermissionScript = {
  
    GetPermissions: function () {

        let Permissions = localStorage.getItem("Permissions");
        //PermissionScript.getCookie('Permissions') //document.cookie.split(";").reduce((ac, cv, i) => Object.assign(ac, { [cv.split('=')[0]]: cv.split('=')[1] }), {});
        if (Permissions == "[]") {
            $('.nav').remove();
            return;
        }
        var json = $.parseJSON(Permissions);
        var FormName = $('.page-title').text();
        
        $.each(json, function (key, value) {

            if (value.AllowView == false || value.AllowView == null) {
                if (value.Controller != null) {
                  
                  
                    $('.' + value.Controller).remove();
                    $('.' + value.Action).remove();
                  

                }
                
                console.log(value.Controller);
            }

            if (value.MenuName == FormName && value.AllowCreate == false) {

                $('.create').remove();
                }
           
            
            if (value.MenuName == FormName && value.AllowEdit == false) {
                $('.edit').remove();
            }
            if (value.MenuName == FormName && value.AllowDelete == false) {
                $('.delete').remove();
            }

            
        })
        
        if ($('#Contract-nav ul li').length < 1) {
          
            $('#ContractManagement').remove();
        }
        if ($('#Invoice-nav ul li').length < 1) {
            
            $('#Invoice').remove();
        }
        if ($('#detail-nav ul li').length < 1) {
            $('#details').remove();
        }
        if ($('#Master-nav ul li').length < 1) {
            
            $('#Masters').remove();
        }
        if ($('#access-app-nav ul li').length < 1) {

            $('#Access').remove();
        }

        if ($('#report-app-nav ul li').length < 1) {

            $('#reports').remove();
        }
        
    },

    getCookie: function (name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
            }

}

