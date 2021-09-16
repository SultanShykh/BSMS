var CrudScript = {
    makeAjaxRequest: function (methodType,url,params,FormData) {
        return new Promise(function (resolve, reject) {
            if (methodType == "GET") {
                if (params == null) {
                    $.get(url).done(function (response) {
                        resolve(response)
                    }).fail(function (response) {
                        reject(response);
                    })
                } else {
                    $.get(url, params).done(function (response) {
                        resolve(response)
                    }).fail(function (response) {
                        reject(response);
                    })
                }
            } else {
                debugger
                if (params == null) {
                    $.post(url).done(function (response) {
                        resolve(response)
                    }).fail(function (response) {
                        reject(response);
                    });
                }
                else {
                    debugger
                    $.post(url, params).done(function (response) {
                        resolve(response)
                    }).fail(function (response) {
                        reject(response);
                    });
                }
            }
            })
            }
    ,
    JqueryDataTableForReport: function (elem, data, columns) {
        debugger
        $(elem).DataTable({
            data: data,
            "columns": columns,
            "columnDefs": [
                {
                    "targets": [0],
                    "visible": false,
                    "searchable": false,
                    "className": "hide_column"
                }
            ]
        });
    },

    JqueryDataTableForMultiRowSpan: function (elem, data, columns) {
        $(elem).DataTable({
            data: data,
            //"aaData": data,
            "columns": columns,
            "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false,
                className: "hide_column"
            },
            ],
            "order": [[1, "asc"]],
            "bPaginate": false, //
            "aaSorting": [],
            "bFilter": true, //
            "bInfo": false, //
            "fnDrawCallback": function () {

                var x = '';
                var y = '';
                var z = '';
                $(elem+' tbody tr').each(function () {
                    var txt = $("td:nth-last-child(5)", $(this)).text();
                    var txt1 = $("td:nth-last-child(4)", $(this)).text();
                    var txt2 = $("td:nth-last-child(3)", $(this)).text();
                    if (txt == x) {
                        $("td:first-child", $(this)).css('visibility', 'hidden');
                        $("td:last-child", $(this)).css('visibility', 'hidden');
                        $("td:nth-last-child(2)", $(this)).css('visibility', 'hidden')
                    }
                    else {
                        $("td:first-child", $(this)).css('visibility', 'visible');
                        x = txt;
                     
                    }
                    if (txt1 == y)
                    {
                        $("td:nth-last-child(4)", $(this)).css('visibility', 'hidden')
                    }
                    else {
                        $("td:nth-last-child(4)", $(this)).css('visibility', 'visible');
                     
                        y = txt1;
                     
                    }

                    if (txt2 == z)
                    {
                        $("td:nth-last-child(3)", $(this)).css('visibility', 'hidden')
                    }
                    else {
                        $("td:nth-last-child(3)", $(this)).css('visibility', 'visible');
                        z = txt2;
                    }
                   
                });
            } // end fnDrawC

        });
    },
    JqueryPFDataTable: function (elem, data, columns) {
        $(elem).DataTable({
            data: data,
            //"aaData": data,
            "columns": columns,
            "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false,
                className: "hide_column"
            },
            ],
            "bPaginate": false,
            "aaSorting": [],
            "bFilter": true, //
            "bInfo": false, //
        });
    },

    JqueryDataTable: function (elem, data, columns) {
        $(elem).DataTable({
            data: data,
            //"aaData": data,
            "columns": columns,
            "createdRow": function (row, data, index) {

                $('td', row).eq(0).prepend('<i class="fa fa-fw fa-link fa-lg Find"></i>');
              
            },
            "order": [[1, "des"]],
            "sPaginationType": "simple_numbers", //
            "aaSorting": [],
            "bFilter": true, //
            "bInfo": false, //
        });
    },


    JqueryDataTableServerSideProcessing: function (elem, data, columns) {
        
        $(elem).DataTable({
            data: data,
           //"aaData": data,
            "columns": columns,
            ////"columnDefs": [
            ////{
            ////    "targets": [0],
            ////    "visible": false,
            ////    "searchable": false,
            ////    className: "hide_column"
            ////},
            //],
            "processing":true,
            "serverSide":true,
            "sPaginationType": "simple_numbers", //
           
            "bFilter": true, //
            "bInfo": false, //
        });
    },
    Response:function (response) {
        var x = document.getElementById("snackbar");
        x.textContent = '' + response.Msg;

        if (response.MsgType == "1") {
            
            $('#snackbar').css("background-color", "mediumseagreen");
        }
        else {
            $('#snackbar').css("background-color", "salmon");
        }

        x.className = "show";
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
    }
}