namespace Shrinkr.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    [CLSCompliant(false)]
    public static class HtmlHelperExtensions
    {
        public static string CustomValidationMessage(this HtmlHelper instance, string modelName)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNull(modelName, "modelName");

            TagBuilder builder = new TagBuilder("span");
            builder.MergeAttribute("class", HtmlHelper.ValidationMessageCssClassName);

            if (instance.ViewData.ModelState.ContainsKey(modelName))
            {
                ModelState modelState = instance.ViewData.ModelState[modelName];
                ModelErrorCollection modelErrors = (modelState == null) ? null : modelState.Errors;
                ModelError modelError = ((modelErrors == null) || (modelErrors.Count == 0)) ? null : modelErrors[0];

                if (modelError != null)
                {
                    string validationMessage = GetValidationMessage(modelError);

                    TagBuilder iconBuilder = new TagBuilder("img");

                    iconBuilder.MergeAttribute("src", instance.ViewContext.RequestContext.UrlHelper().InputValidationErrorIcon());
                    iconBuilder.MergeAttribute("alt", string.Empty);
                    iconBuilder.MergeAttribute("title", validationMessage);

                    builder.InnerHtml = iconBuilder.ToString(TagRenderMode.SelfClosing);
                }
            }

            return builder.ToString();
        }

        public static string CustomValidationSummary(this HtmlHelper instance, string message, object htmlAttributes)
        {
            Check.Argument.IsNotNull(instance, "instance");

            string innerHtml = string.Empty;

            if (!instance.ViewData.ModelState.IsValid)
            {
                TagBuilder spanTag = new TagBuilder("span");
                spanTag.MergeAttribute("class", HtmlHelper.ValidationSummaryCssClassName);
                spanTag.SetInnerText(message);
                string messageSpan = spanTag + Environment.NewLine;

                StringBuilder htmlSummary = new StringBuilder();
                TagBuilder unorderedList = new TagBuilder("ul");
                unorderedList.MergeAttribute("class", HtmlHelper.ValidationSummaryCssClassName);

                foreach (KeyValuePair<string, ModelState> pair in instance.ViewData.ModelState)
                {
                    foreach (ModelError modelError in pair.Value.Errors)
                    {
                        string errorText = GetValidationMessage(modelError);

                        TagBuilder label = new TagBuilder("label");
                        label.MergeAttribute("for", pair.Key);
                        label.SetInnerText(errorText);

                        TagBuilder listItem = new TagBuilder("li") { InnerHtml = label.ToString() };

                        htmlSummary.AppendLine(listItem.ToString());
                    }
                }

                unorderedList.InnerHtml = htmlSummary.ToString();

                innerHtml = messageSpan + unorderedList;
            }

            TagBuilder boxBuilder = new TagBuilder("div");
            boxBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            boxBuilder.AddCssClass("validationBox");
            boxBuilder.InnerHtml = innerHtml;

            if (instance.ViewData.ModelState.IsValid)
            {
                string style = boxBuilder.Attributes.ContainsKey("style") ? 
                               boxBuilder.Attributes["style"] :
                               string.Empty;

                style += (!string.IsNullOrWhiteSpace(style) ? ";" : string.Empty) + "display:none";

                boxBuilder.Attributes["style"] = style;
            }

            return boxBuilder.ToString();
        }

        public static ListHtmlHelper List(this HtmlHelper htmlHelper)
        {
            return new ListHtmlHelper(htmlHelper);
        }

        public static string DropDownList<TEnum>(this HtmlHelper instance, string name, TEnum selectedValue) where TEnum : IComparable, IFormattable, IConvertible
        {
            Type enumType = typeof(TEnum);

            string[] names = Enum.GetNames(enumType);
            Array values = Enum.GetValues(enumType);

            List<SelectListItem> list = new List<SelectListItem>();

            for (int i = 0; i < names.Length; i++)
            {
                string text = names[i];
                object value = values.GetValue(i);

                list.Add(new SelectListItem { Text = text, Value = value.ToString(), Selected = value.Equals(selectedValue) });
            }

            return instance.DropDownList(name, list.OrderBy(i => i.Text), (string)null).ToString();
        }

        private static string GetValidationMessage(ModelError modelError)
        {
            string validationMessage = !string.IsNullOrEmpty(modelError.ErrorMessage) ?
                                        modelError.ErrorMessage :
                                        ((modelError.Exception != null) ? modelError.Exception.Message : "Error");

            return validationMessage;
        }
    }
}