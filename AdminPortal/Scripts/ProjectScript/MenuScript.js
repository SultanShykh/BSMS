var MenuScript = {
    Columns:
        [
            { "data": "Id", "name": "Id" },
            { "data": "MenuName", "name": "MenuName" },
            { "data": "MenuUrl", "name": "MenuUrl" },
            { "data": "Controller", "name": "Controller" },
            { "data": "Action", "name": "Action" },
            { "data": "IsActive", "name": "IsActive" },
            {
                "mData": null,
                "bSortable": false,
                "mRender": function (data, type, full) {
                    return '<button type="button" class="btn bg-gradient-dark edit mr-2" id="' + data.Id + '" value="Edit"><i class="fa fa-edit"></i></button><button type="button" class="btn btn-danger delete" id="' + data.Id + '" value="Delete"><i class="fa fa-trash"></i></button>'
                }
            }
        ],

    RenderDataTable: function () {
        debugger
        CrudScript.makeAjaxRequest('GET', '/Menu/GetMenu').then(function (data) {
            debugger
            var dt = $('.tblmanage').DataTable();
            dt.destroy();
            CrudScript.JqueryDataTableForReport('.tblmanage', data.data, MenuScript.Columns);
        })
    },

    SaveMenu: function () {
        CrudScript.makeAjaxRequest('Post', '/Menu/CreateMenu', $("form").serialize()).then(function (data) {
            $('#addRowModal').modal("hide");
            if (data.status == true) {
                ShowDivSuccess(data.message);
            }
            else {
                ShowDivError(data.message);
            }
        })
    },

    FillModal: function (data) {
        $('#addRowModal').modal("show");
        $('.fw-mediumbold').text('Update');
        $('#btnUpdate').css('display', 'block');
        $('#btnSubmit').css('display', 'none');
        $('#Id').val(data.Id);
        $('#MenuName').val(data.MenuName);
        $('#MenuUrl').val(data.MenuUrl);
        $('#Controller').val(data.Controller);
        $('#Action').val(data.Action);
        $('[name="isactive"]').val([data.IsActive]);
    },

    UpdateMenu: function () {
        CrudScript.makeAjaxRequest('Post', '/Menu/UpdateMenu', $("form").serialize()).then(function (data) {
            $('#addRowModal').modal("hide");
            ShowDivSuccess("Updated Successfully");
        })
    },

    DeleteMenu: function (id) {
        var url = '/Menu/DeleteMenu';
        DeleteConfirm(url, id);
    }
}