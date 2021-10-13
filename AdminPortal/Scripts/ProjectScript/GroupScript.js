 var GroupScript = {
    Columns:
        [
            { "data": "id", "name": "id" },
            { "data": "Name", "name": "Name" },
            { "data": "isActive", "name": "isActive" },

            {
                "mData": null,
                "bSortable": false,
                "mRender": function (data, type, full) {
                    return '<button type="button" class="btn bg-gradient-dark edit mr-2" id="' + data.Id + '" value="Edit"><i class="fa fa-edit"></i></button><button type="button" class="btn btn-danger delete" id="' + data.Id + '" value="Delete"><i class="fa fa-trash"></i></button>'

                }
            }
        ],
    RenderDataTable: function () {
        CrudScript.makeAjaxRequest('GET', '/Group/GetGroups').then(function (data) {

            var dt = $('.tblmanage').DataTable();
            dt.destroy();
            CrudScript.JqueryDataTableForReport('.tblmanage', data.data, GroupScript.Columns);
        })


    },

    CheckGroup: function (name)
    {
        CrudScript.makeAjaxRequest('Post', '/Group/CheckGroupName', { groupName: name }).then(function (data) {

            if (data == false) {
                $('#message').css('display', 'block');

            }
            else {
                $('#message').css('display', 'none');
            }
        })
    },
    EditForm: function (groupId) {

        CrudScript.makeAjaxRequest('GET', '/Group/EditGroup', {Id:groupId}).then(function (data) {

            window.location = data;    
        })

    },
    SaveGroup: function (formData) {
        CrudScript.makeAjaxRequest('Post', '/Group/CreateGroup', $("form").serialize()).then(function (data) {
            debugger
            if (data.status == true) {
                ShowDivSuccess(data.message);
            }
            else {
                ShowDivError(data.message);
            }

        })
    },

    GetAllForms: function () {
        CrudScript.makeAjaxRequest('GET', '/Group/GetgroupPermissions').then(function (data) {
            var masterid = 0;
            var ul;
            var count = 0;
            var counter = 0;
            var formCounter = 1;
            var MenuName = '';
            var Id = 0;
            $.each(data, function (key, value) {
                var listElement = '';
               
                var flag = false;
               if (value.MenuName == null) {
                    listElement = '<li class="has"><input type="checkbox" name="' + value.FormMasterName + '" value="yes"> ' + value.FormMasterName + '  <ul>'
                        + ' </ul> </li>'

               }
               else if (value.MenuUrl == "#") {
                   counter++;
                   if (masterid != value.FormMasterId) {
                       listElement = '<li class="has"  "><input type="checkbox" name="' + value.FormMasterName + '" value="yes"> ' + value.FormMasterName + '  <ul>'
                           + '<li ><input type="checkbox" id="Upper' + counter + '  name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul id="Parent' + value.FormMasterId + '">'
                           + '</ul> </li> </ul> </li>'
                      
                   }
                   else {
                       listElement =  '<li ><input type="checkbox" id="Upper' + counter + '  name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul id="Parent' + value.FormMasterId + '">'
                           + '</ul> </li> </ul> </li>'
                   }
                  
                    Id = value.MenuId;
                }

                if (Id = value.MenuParentId) {
                    
                    listElement = '<li ><input type="checkbox"  name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowCreate" value="0"> Allow Create</li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowDelete" value="0"> Allow Delete </li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowEdit"  value="0"> Allow Edit </li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowView"  value="0"> Allow View </li>'
                        + '</ul> </li> </ul> </li>'
                    
                    $("#Parent" + value.FormMasterId).append(listElement);
                    MenuName = value.MenuName;
                    flag = true;
                }
                if (listElement == '' && masterid != value.FormMasterId) {
                    listElement = '<li class="has"  "><input type="checkbox" name="' + value.FormMasterName + '" value="yes"> ' + value.FormMasterName + '  <ul>'
                        + '<li class="has parentCheckBox"><input type="checkbox" name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowCreate" value="0"> Allow Create</li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowDelete" value="0"> Allow Delete</li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowEdit" value="0"> Allow Edit</li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowView" value="0"> Allow View</li>'
                        + '</ul> </li>'

                }
                else if (listElement == '') {
                    listElement = '<li class="has parentCheckBox"><input type="checkbox" name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowCreate" value="0"> Allow Create</li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowDelete" value="0"> Allow Delete</li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowEdit" value="0"> Allow Edit</li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowView" value="0"> Allow View</li>'
                        + '</ul> </li>'

                }

                

                if (masterid != value.FormMasterId) {
                    formCounter = 1;
                    count++;
                    
                    ul = document.createElement('ul');
                    ul.setAttribute("id", "Div" + count);
                    ul.setAttribute("class", "tree");
                    $("#Permission").append(ul);
                    $('#Div' + count).html(listElement);
                }
                else {
                    formCounter++;

                    if (flag == false && masterid == value.FormMasterId) {
                        if ($("#Parent" + value.FormMasterId).length > 0) {
                        
                            $("#Parent" + value.FormMasterId).parent().append(listElement);
                        }
                        else {
                            $("#Permission #Div" + count + ' ul:last').after(listElement);
                        }
                        
                    }
                  
                    else if (MenuName != value.MenuName){
                        $("#Permission #Div" + count + ' ul:last').after(listElement);
                       
                    }
                    
                }

                masterid = value.FormMasterId;
                $('#' + value.FormMasterName).text('(' + formCounter + ')');
            })

        });

    },

    FillFormssss: function (data) {
        var masterid = 0;
        var ul;
        var count = 0;
        var counter = 0;
        var formCounter = 1;
        var MenuName = '';
        var Id = 0;
        var checkbox = '';
        var indexer = 0;
        var ParentMenu = '';
        CrudScript.makeAjaxRequest('GET', '/Group/GetGroupsbyId').then(function (data) {
            var masterid = 0;
            var ul;
            var count = 0;
            var formCounter = 1;
            var listElement;
            var indexer = 0;
           
            $.each(data, function (key, value) {
                var checkbox = '';
                var header = false;
                if (value.MenuUrl == "#") {
                    counter++;
                    if (masterid != value.FormMasterId) {
                        listElement = '<li class="has"  "><input type="checkbox" name="' + value.FormMasterName + '" value="yes"> ' + value.FormMasterName + '  <ul>'
                            + '<li ><input type="checkbox" id="Upper' + counter + '"  name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul id="Parent' + value.FormMasterId + '">'
                            + '</ul> </li> </ul> </li>'

                    }
                    else {
                        if (value.AllowCreate == true || value.AllowEdit == true || value.AllowDelete == true || value.AllowView == true) {
                            listElement = '<li ><input type="checkbox" checked id="Upper' + counter + '"  name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul id="Parent' + value.FormMasterId + '">'
                                + '</ul> </li> </ul> </li>'
                        }
                        else {
                            listElement = '<li ><input type="checkbox" id="Upper' + counter + '"  name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul id="Parent' + value.FormMasterId + '">'
                                + '</ul> </li> </ul> </li>'
                        }
                        
                    }

                    Id = value.MenuId;


                    //listElement = '<li class="has"><input type="checkbox" checked name="' + value.FormMasterName + '" value="' + value.FormMasterId + '"> ' + value.FormMasterName + '  <ul>'
                    //    + '<li ><input type="checkbox" checked name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>'
                    header = true;
                }
                if (indexer == 0) {
                    $('#GroupName').val(value.Name);
                    $('#Status').val(value.Status);
                    $("#GroupId").val(value.id);
                    indexer++;
                }

                else {

                    if (header == false) {
                        
                        if (value.AllowCreate == true || value.AllowEdit == true || value.AllowDelete == true || value.AllowView == true) {
                            $(':checkbox[id=Upper' + counter + ']').prop('value', true);
                            checkbox = '<li class="has"><input type="checkbox" checked name="' + value.FormMasterName + '" value="' + value.FormMasterId + '"> ' + value.FormMasterName + '  <ul>'
                                + '<li ><input type="checkbox" checked name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>'
                        }
                        else {
                            checkbox = '<li class="has"><input type="checkbox" name="' + value.FormMasterName + '" value="' + value.FormMasterId + '"> ' + value.FormMasterName + '  <ul>'
                                + '<li ><input type="checkbox"  name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>'
                        }
                    }
                    if (value.Action != null && value.MenuUrl != "#") {
                        listElement = checkbox
                            + '<li><input type="checkbox" name="' + value.MenuName + 'AllowCreate" value="' + value.AllowCreate + '"> Allow Create</li>'
                            + '<li><input type="checkbox" name="' + value.MenuName + 'AllowDelete" value="' + value.AllowDelete + '"> Allow Delete </li>'
                            + '<li><input type="checkbox" name="' + value.MenuName + 'AllowEdit"  value="' + value.AllowEdit + '"> Allow Edit </li>'
                            + '<li><input type="checkbox" name="' + value.MenuName + 'AllowView"  value="' + value.AllowView + '"> Allow View </li>'
                            + '</ul> </li> </ul> </li>'
                    }
                    

                    if (masterid != value.FormMasterId) {
                        formCounter = 1;
                        count++;
                        ul = document.createElement('ul');
                        ul.setAttribute("id", "Div" + count);
                        ul.setAttribute("class", "tree");
                        $("#Permission").append(ul);
                        $('#Div' + count).html(listElement);
                    }
                    else {
                        var checkedCheckBox = '';
                        formCounter++;
                        if (value.Action != null && value.MenuUrl != "#") {
                            if (value.AllowCreate == true || value.AllowEdit == true || value.AllowDelete == true || value.AllowView == true) {
                                checkedCheckBox = '<li class="has parentCheckBox"><input type="checkbox" checked name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>';
                            }
                            else {
                                checkedCheckBox = '<li class="has parentCheckBox"><input type="checkbox" name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>';
                            }
                            listElement = checkedCheckBox
                                + '<li><input type="checkbox" name="' + value.MenuName + 'AllowCreate"  value="' + value.AllowCreate + '"> Allow Create</li>'
                                + '<li><input type="checkbox" name="' + value.MenuName + 'AllowDelete"  value="' + value.AllowDelete + '"> Allow Delete </li>'
                                + '<li><input type="checkbox" name="' + value.MenuName + 'AllowEdit"   value="' + value.AllowEdit + '"> Allow Edit </li>'
                                + '<li><input type="checkbox" name="' + value.MenuName + 'AllowView"   value="' + value.AllowView + '"> Allow View </li>'
                                + '</ul> </li>'
                        }
                        
                        $("#Permission #Div" + count + ' ul:last').after(listElement);
                    }

                   
                    $('#' + value.FormMasterName).text('(' + formCounter + ')');
                    indexer++;
                }
                masterid = value.FormMasterId;

            })
            $(":checkbox[value=true]").prop("value", "1");
            $(":checkbox[value=1]").prop("checked", "true");
            $(":checkbox[value=yes]").prop("checked", "true");


        });
       

    },
    FillForm: function ()
    {
        CrudScript.makeAjaxRequest('GET', '/Group/GetGroupsbyId').then(function (data) {
            var masterid = 0;
            var ul;
            var count = 0;
            var formCounter = 1;
            var listElement;
            var indexer = 0;
            $.each(data, function (key, value) {
                debugger
                if (value.MenuUrl == "#") {
                    return;
                }
                if (indexer == 0) {
                    $('#GroupName').val(value.Name);
                    $('#Status').val(value.Status);
                    $("#GroupId").val(value.id);
                    indexer++;
                }
                
                else {
                    var checkbox = '';

                    if (value.AllowCreate == true || value.AllowEdit == true || value.AllowDelete == true || value.AllowView == true) {
                        checkbox = '<li class="has"><input type="checkbox" checked="checked" name="' + value.FormMasterName + '" value="' + value.FormMasterId + '"> ' + value.FormMasterName + '  <ul>'
                            + '<li ><input type="checkbox" checked name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>'
                    }
                    else {
                        checkbox =  '<li class="has"><input type="checkbox" name="' + value.FormMasterName + '" value="' + value.FormMasterId + '"> ' + value.FormMasterName + '  <ul>'
                            + '<li ><input type="checkbox"  name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>'
                    }
                    listElement = checkbox
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowCreate" value="' + value.AllowCreate + '"> Allow Create</li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowDelete" value="' + value.AllowDelete + '"> Allow Delete </li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowEdit"  value="' + value.AllowEdit + '"> Allow Edit </li>'
                        + '<li><input type="checkbox" name="' + value.MenuName + 'AllowView"  value="' + value.AllowView + '"> Allow View </li>'
                        + '</ul> </li> </ul> </li>'

                    if (masterid != value.FormMasterId) {
                        formCounter = 1;
                        count++;
                        ul = document.createElement('ul');
                        ul.setAttribute("id", "Div" + count);
                        ul.setAttribute("class", "tree");
                        $("#Permission").append(ul);
                        $('#Div' + count).html(listElement);
                    }
                    else {
                        var checkedCheckBox = '';
                        formCounter++;
                        if (value.AllowCreate == true || value.AllowEdit == true || value.AllowDelete == true || value.AllowView == true) {
                            checkedCheckBox = '<li class="has parentCheckBox"><input type="checkbox" checked name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>';
                        }
                        else {
                            checkedCheckBox = '<li class="has parentCheckBox"><input type="checkbox" name="Menus-' + value.MenuName + '[' + value.MenuId + ']" value="' + value.MenuId + '"> ' + value.MenuName + ' <ul>';
                        }
                        listElement = checkedCheckBox
                            + '<li><input type="checkbox" name="' + value.MenuName + 'AllowCreate"  value="' + value.AllowCreate + '"> Allow Create</li>'
                            + '<li><input type="checkbox" name="' + value.MenuName + 'AllowDelete"  value="' + value.AllowDelete + '"> Allow Delete </li>'
                            + '<li><input type="checkbox" name="' + value.MenuName + 'AllowEdit"   value="' + value.AllowEdit + '"> Allow Edit </li>'
                            + '<li><input type="checkbox" name="' + value.MenuName + 'AllowView"   value="' + value.AllowView + '"> Allow View </li>'
                            + '</ul> </li>'
                        $("#Permission #Div" + count + ' ul:last').after(listElement);
                    }

                    masterid = value.FormMasterId;
                    $('#' + value.FormMasterName).text('(' + formCounter + ')');
                    indexer++;
                }
               
            })
            $(":checkbox[value=true]").prop("value", "1");
            $(":checkbox[value=1]").prop("checked", "true");
            $(":checkbox[value=yes]").prop("checked", "true");


        });
    },

    FillModal: function (data) {
        $('#add_modal').modal("show");
        var cList = $('ul#Menus');
        $.each(data, function (key, value) {
            var li = $('<li/>')
                .appendTo(cList);
            $('<input type="checkbox" name="' + value.MenuName + '[]" value="' + key.MenuName + '"><label>"' + value.MenuName + '"</label>').appendTo(li);

            //$('.fw-mediumbold').text('Update');
            //$('#btnUpdate').css('display', 'block');
            //$('#btnSubmit').css('display', 'none');
            //$('#menuId').val(data.Id);
            //$('#menuName').val(data.MenuName);
            //$('#menuURL').val(data.MenuUrl);
            //$('#controller').val(data.Controller);
            //$('#action').val(data.Action);
            //$('[name="isactive"]').val([data.IsActive]);
        })
    },

    UpdateGroup: function () {
        CrudScript.makeAjaxRequest('Post', '/Group/UpdateGroup', $("form").serialize()).then(function (data) {
            ShowDivSuccess("Updated Successfully");
        })
    },

    DeleteGroup: function (Id) {
        var url = '/Group/DeleteGroup';
        DeleteConfirm(url, Id, GroupScript);
    },

    GetSelectedUsers: function () {
        CrudScript.makeAjaxRequest('Post', '/Group/GetSelectedUsers').then(function (data) {
            $.each(data.userslist, function (key, value) {
                $('#Users').append($('<option>', {
                    value: value.Id,
                    text: value.fullname
                }));
            })

            if (data.SelectedUsers != "")
            {
                debugger
                var selectedOptions = data.SelectedUsers.split(",");
                for (var i in selectedOptions)
                {
                    var optionVal = selectedOptions[i];
                    $("#Users").find("option[value=" + optionVal + "]").prop("selected", "selected");
                }
               
            }
      
            $('#Users').selectpicker('refresh');
        })
    }
}