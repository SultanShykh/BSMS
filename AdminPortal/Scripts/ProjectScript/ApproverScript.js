var ApproverScript = {

        Columns:
            [
                { "data": "Id", "name": "Id" },
                { "data": "Name", "name": "Name" },
                { "data": "UserName", "name": "UserName" },
            { "data": "userType", "name": "userType" },
                {
                    "mData": null,
                    "bSortable": false,
                    "mRender": function (data, type, full) {
                        return '<button type="button" class="btn btn-danger delete" id="' + data.Id + '" value="Delete"><i class="fa fa-trash"></i></button> <button type="button" class="btn btn-danger edit" id="' + data.Id + '" value="Edit"><i class="fa fa-edit"></i></button>'

                    }
                }
            ],


        RenderDataTable: function () {
            CrudScript.makeAjaxRequest('GET', '/Configuration/GetApprovers').then(function (data) {

                var dt = $('.tblmanage').DataTable();
                dt.destroy();
                CrudScript.JqueryDataTableForReport('.tblmanage', data.data, ApproverScript.Columns);
            })


        },

        SaveApprover: function () {

            CrudScript.makeAjaxRequest('Post', '/Configuration/Create', $("form").serialize()).then(function (data) {
                $('#addRowModal').modal("hide");
                if (data.message == "Successfully Created!!!") {
                    var mainTable = $('#mainTable').DataTable();
                    mainTable.destroy();
                    ApproverScript.RenderDataTable();
                    ShowDivSuccess(data.message);
                }

                else {
                    ShowDivError(data.message);
                }

            })
        },

    FillModal: function (data) {
        debugger
            $('#addRowModal').modal("show");
            $('.fw-mediumbold').text('Update');
            $('#btnUpdate').css('display', 'block');
            $('#btnSubmit').css('display', 'none');
            $('#Id').val(data.Id);
        $('#userId').val(data.userId);
        var userType = data.userType == "Approver" ? 1 : 2;
        $("input[name=userType][value=" + data.userType + "]").prop('checked', true);
        },

        UpdateApprover: function () {
            CrudScript.makeAjaxRequest('Post', '/Configuration/UpdateApprover', $("form").serialize()).then(function (data) {
                $('#addRowModal').modal("hide");
                var mainTable = $('#mainTable').DataTable();
                mainTable.destroy();
                ApproverScript.RenderDataTable();
                ShowDivSuccess("Updated Successfully");
            })
        },

        DeleteApprover: function (data) {
            var url = '/Configuration/DeleteApprover';
            DeleteConfirm(url, data.Id, ApproverScript);
        }


  }