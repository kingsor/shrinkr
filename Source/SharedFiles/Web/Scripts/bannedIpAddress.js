var bannedIpAddress =
{
    init: function (validationErrorIcon, deleteUrl) {

        $.validator.addMethod(
                                'validIpAddress',
                                function (value, element) {
                                    return this.optional(element) || /^((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){3}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})$/.test(value);
                                },
                                'Ip address is not valid.'
                            );

        var rules = { ipAddress: { required: true, validIpAddress: true} };
        var messages = { ipAddress: { required: 'Ip address cannot be blank.'} };

        var newItem = function (model) {
            return '<tr>' +
                        '<td scope=\"col\">' +
                            model.iPAddress +
                         '</td>' +
                         '<td scope=\"col\">' +
                            '<form method=\"post\" class=\"delete\" action=\"' + deleteUrl + '\">' +
                                '<div>' +
                                    ' <input type=\"hidden\" id=\"id-' + model.id + '\" name=\"id\" value=\"' + model.id + '\"/>' +
                                    ' <input type=\"submit\" class=\"smallButton\" value=\"remove\"/>' +
                                '</div>' +
                            '</form>' +
                        '</td>' +
                    '</tr>';
        };

        new administrativeItem(validationErrorIcon, rules, messages, newItem);
    }
}