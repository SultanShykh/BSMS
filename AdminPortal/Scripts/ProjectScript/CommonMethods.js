var objPageRoleAssign =
{
    AllowCreate: false,
    AllowEdit: false,
    AllowView: false,
    AllowDelete: false
};

function CallAsyncService(url, Param, funSucc, Type, funError) {
    debugger
    try {
        $.ajax({
            type: Type,
            url: url,
            data: Param,
            dataType: "json",
            async: true,
            beforeSend: function ()
            {
                ShowDivSuccess1("Processing! please wait");
            },
            success: function (data) {
                if (data.status == true)
                {
                    ShowDivSuccess(data.message);
                    location.reload();
                }
                else
                {
                    funError(data.message)
                }
            },
            error: function (xhr) {
                if (xhr.responseText) {
                    var err = xhr.responseText;
                    if (err) {
                        console.log(err);
                        funError(err);
                    }
                    else {
                        funError('Error');
                    }
                }
            }
        });
    }
    catch (Err) {
        ShowDivError(Err);
    }
    return false;
}

function CallAsyncService1(url, Param, funSucc, Type, funError) {
    try {
        $.ajax({
            type: Type,
            url: url,
            data: Param,
            async: true,
            beforeSend: function () {
                funSucc1("Processing! please wait");
            },
            success: function (data) {
                if (data.status == true) {
                    funSucc(data.message);
                }
                else {
                    funError(data.message)
                }
            },
            error: function (xhr) {
                if (xhr.responseText) {
                    var err = xhr.responseText;
                    if (err) {
                        console.log(err);
                        funError(err);
                    }
                    else {
                        funError('Error');
                    }
                }
            },
            cache: false,
            processData: true,
            contentType: false
        });
    }
    catch (Err) {
        ShowDivError(Err);
    }
    return false;
}

function DeleteConfirm(url,Id) {
    swal({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        buttons: {
            confirm: {
                text: 'Yes, delete it!',
                className: 'btn btn-success'
            },
            cancel: {
                visible: true,
                className: 'btn btn-danger'
            }
        }
    }).then((Delete) => {
        if (Delete) {
            debugger
            CrudScript.makeAjaxRequest('POST', url + "/"+Id).then(function (data) {
                if (data.status == true) {
                    location.reload();
                }
                else {
                    ShowDivError(data.message);
                }
            });
            
        } else {
            swal.close();
        }
    });
}

function ShowDivError(msg) {
    swal("Error!!", msg, {
        icon: "error",
        buttons: {
            confirm: {
                text: "OK",
                value: true,
                visible: true,
                className: 'btn btn-danger',
                closeModal: true
            }
        },
    });
}
function HideDivError() {
    $('#lblError').html('');
    $('#divError').hide();
}

function ShowDivSuccess(msg) {
    swal({
        text: msg,
        icon: "success",
        buttons: {
            confirm: {
                text: "OK",
                value: true,
                visible: true,
                className: "btn btn-success",
                closeModal: true
            }
        }
    }).then(() => {
        location.reload();
    });
}

function ShowDivSuccess1(msg) {
    swal({
        text: msg,
        icon: "/Media/loading_gif.gif"
    });
}

function HideDivSuccess() {
    $('#lblSuccess').html('');
    $('#divSuccess').hide();
}

function PageRoleAssign(AllowCreate, AllowEdit, AllowView, AllowDelete) {
    objPageRoleAssign.AllowCreate = AllowCreate;
    objPageRoleAssign.AllowEdit = AllowEdit;
    objPageRoleAssign.AllowView = AllowView;
    objPageRoleAssign.AllowDelete = AllowDelete;
}
