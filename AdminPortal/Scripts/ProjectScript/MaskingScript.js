var MaskingScript =
{
  SaveMasking: function () {
        CrudScript.makeAjaxRequest('Post', '/Message/QuickSMS', $("#form").serialize()).then(function (data) {
            if (data.response == true)
            {
                ShowDivSuccess(data.response);

            }
            else {
                ShowDivError(data.response);
            }
        })
    },

}