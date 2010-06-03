(function($)
{
    $.fn.watermark = function(options)
    {
        var opt = $.extend({ focusCssClass: 'focusTextBox', idleCssClass: 'idleTextBox', validationCssClass: 'input-validation-error', title: null }, options);

        $(this).focus(
                        function()
                        {
                            $(this).removeClass(opt.idleCssClass)
                                   .removeClass(opt.validationCssClass)
                                   .addClass(opt.focusCssClass);

                            this.clearIfSame();
                        }
                     )
               .blur(
                        function()
                        {
                            this.addIfBlank();
                        }
                    )
               .each(
                        function()
                        {
                            var $this = this;
                            this.title = opt.title || $($this).attr('title');

                            $this.addIfBlank = function()
                            {
                                var self = $($this);

                                if (self.val() === '')
                                {
                                    self.val($this.title);
                                }

                                self.removeClass(opt.focusCssClass);

                                if (self.val() === $this.title)
                                {
                                    self.addClass(opt.idleCssClass);
                                }
                            }

                            $this.clearIfSame = function()
                            {
                                var self = $($this);

                                if (self.val() === $this.title)
                                {
                                    self.val('');
                                }
                            }

                            $this.addIfBlank();

                            $($this).parents('form:first').submit(
                                                                    function()
                                                                    {
                                                                        $this.clearIfSame();
                                                                    }
                                                                );
                        }
                );

        return $(this);
    }
})(jQuery);