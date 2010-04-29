var user =
{
    init: function () {

        var unlockOptions = {
            dataType: 'json',
            beforeSubmit: function () {
                $('form#unlock input.smallButton').attr('disabled', true).val('Wait...');
                $('form#lock input.smallButton').attr('disabled', true);
            },
            success: function (result) {
                $('form#unlock input.smallButton').attr('disabled', false).val('Unlock');
                $('form#lock input.smallButton').attr('disabled', false);
                $('form#unlock').hide();
                $('form#lock').show();
            }
        };

        var lockOptions = {
            dataType: 'json',
            beforeSubmit: function () {
                $('form#lock input.smallButton').attr('disabled', true).val('Wait...');
                $('form#unlock input.smallButton').attr('disabled', true);
            },
            success: function (result) {
                $('form#lock input.smallButton').attr('disabled', false).val('Lock');
                $('form#unlock input.smallButton').attr('disabled', false);
                $('form#lock').hide();
                $('form#unlock').show();
            }
        };

        var updateRoleOptions = {
            dataType: 'json',
            beforeSubmit: function () {
                $('form#updateRole input.smallButton').attr('disabled', true).val('Wait...');
                $('form#updateRole select').attr('disabled', true);
            },
            success: function (result) {
                $('form#updateRole input.smallButton').attr('disabled', false).val('Update');
                $('form#updateRole select').attr('disabled', false);
            }
        };

        var updateApiAccessOptions = {
            dataType: 'json',
            beforeSubmit: function () {
                $('form#updateApiAccess input.smallButton').attr('disabled', true).val('Wait...');
                $('form#updateApiAccess select').attr('disabled', true);
            },
            success: function (result) {
                $('form#updateApiAccess input.smallButton').attr('disabled', false).val('Update');
                $('form#updateApiAccess select').attr('disabled', false);
            }
        };

        $('#unlock').ajaxForm(unlockOptions);
        $('#lock').ajaxForm(lockOptions);
        $('#updateRole').ajaxForm(updateRoleOptions);
        $('#updateApiAccess').ajaxForm(updateApiAccessOptions);
    }
}