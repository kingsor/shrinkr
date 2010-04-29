var shortUrl =
{
    init: function (spamText, safeText, domain, ipAddress, blockDomainUrl, blockIpAddressUrl) {

        var spamOptions = {
            dataType: 'json',
            beforeSubmit: function () {
                $('form#spam input.smallButton').attr('disabled', true).val('Wait...');
                $('form#safe input.smallButton').attr('disabled', true);
            },
            success: function (result) {
                $('#status').text(spamText).toggleClass('warningText');
                $('form#spam input.smallButton').attr('disabled', false).val('Mark as Spam');
                $('form#safe input.smallButton').attr('disabled', false);
                $('form#spam').hide();
                $('form#safe').css('display', 'inline');
            }
        };

        var safeOptions = {
            dataType: 'json',
            beforeSubmit: function () {
                $('form#safe input.smallButton').attr('disabled', true).val('Wait...');
                $('form#spam input.smallButton').attr('disabled', true);
            },
            success: function (result) {
                $('#status').text(safeText).toggleClass('warningText');
                $('form#safe input.smallButton').attr('disabled', false).val('Mark as Safe');
                $('form#spam input.smallButton').attr('disabled', false);
                $('form#safe').hide();
                $('form#spam').css('display', 'inline');
            }
        };

        $('#spam').ajaxForm(spamOptions);
        $('#safe').ajaxForm(safeOptions);

        var blockDomain = $('a#blockDomain');

        blockDomain.click(function () {
            $.ajax(
                    {
                        url: blockDomainUrl,
                        type: 'POST',
                        dataType: 'json',
                        data: 'name=' + encodeURIComponent(domain),
                        beforeSend: function () {
                            blockDomain.attr('disabled', 'true').text('Wait...');
                        },
                        success: function (result) {
                            blockDomain.hide();
                        }
                    }
                );
        });

        var blockIp = $('a#blockIp');

        blockIp.click(function () {
            $.ajax(
                {
                    url: blockIpAddressUrl,
                    type: 'POST',
                    dataType: 'json',
                    data: 'ipAddress=' + encodeURIComponent(ipAddress),
                    beforeSend: function () {
                        blockIp.attr('disabled', 'true').text('Wait...');
                    },
                    success: function (result) {
                        blockIp.hide();
                    }
                }
            );
        });
    }
}