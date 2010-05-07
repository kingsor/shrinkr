namespace Shrinkr.Web
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    using Extensions;

    public class ApiResult : ActionResult
    {
        public ApiResult(ApiResponseFormat responseFormat)
        {
            ResponseFormat = responseFormat;
        }

        public ApiResponseFormat ResponseFormat
        {
            get;
            private set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Check.Argument.IsNotNull(context, "context");

            ViewDataDictionary viewData = context.Controller.ViewData;
            CreateUrlViewModel viewModel = viewData.Model as CreateUrlViewModel;
            CreateResult model = null;

            if (viewModel != null)
            {
                model = new CreateResult
                            {
                                ShortUrl = viewModel.VisitUrl,
                                PreviewUrl = viewModel.PreviewUrl,
                                Alias = viewModel.Alias,
                                LongUrl = viewModel.Url,
                                Title = viewModel.Title
                            };
            }

            IList<Error> errors = null;

            if (model == null)
            {
                errors = viewData.ModelState.IsValid ?
                         null :
                         viewData.ModelState.Select(ms => new Error { Parameter = ms.Key, Messages = new MessageCollection(ms.Value.Errors.Select(error => (error.Exception == null) ? error.ErrorMessage : error.Exception.Message).Where(error => !string.IsNullOrWhiteSpace(error)).ToList()) })
                                            .Where(ms => ms.Messages.Any()) // No need to include items that does not have any errors
                                            .ToList();
            }

            string contentType;
            string content;

            switch (ResponseFormat)
            {
                case ApiResponseFormat.Json:
                    {
                        contentType = "application/json";
                        content = JsonSerialize(model, errors);
                        break;
                    }

                case ApiResponseFormat.Xml:
                    {
                        contentType = "application/xml";
                        content = XmlSerialize(model, errors);

                        break;
                    }

                default:
                    {
                        contentType = "text/plain";
                        content = TextSerialize(model, errors);

                        break;
                    }
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.Clear();
            response.ContentType = contentType;
            response.Write(content);
        }

        private static string JsonSerialize(CreateResult model, IList<Error> errors)
        {
            string content = null;

            if (model != null)
            {
                content = Serialize(new DataContractJsonSerializer(typeof(CreateResult)), model);
            }
            else if (errors != null)
            {
                content = Serialize(new DataContractJsonSerializer(typeof(IList<Error>)), errors);
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                content = "{\"errors\":[{\"error\":{\"key\":\"\",\"messages\":[\"" + TextMessages.SomethingUnholyGoingOn + "\"]}}]}";
            }

            return content;
        }

        private static string XmlSerialize(CreateResult model, IList<Error> errors)
        {
            string content = null;

            if (model != null)
            {
                content = Serialize(new DataContractSerializer(typeof(CreateResult)), model);
            }
            else if (errors != null)
            {
                content = Serialize(new DataContractSerializer(typeof(IList<Error>), "errors", string.Empty), errors);
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                content = "<errors><error><key/><messages><message>{0}</message></messages></error></errors>".FormatWith(TextMessages.SomethingUnholyGoingOn);
            }

            return content;
        }

        private static string TextSerialize(CreateResult model, IEnumerable<Error> errors)
        {
            string content = null;

            if (model != null)
            {
                content = model.ShortUrl;
            }
            else if (errors != null)
            {
                StringBuilder errorBuilder = new StringBuilder();

                foreach (Error error in errors)
                {
                    foreach (string message in error.Messages)
                    {
                        errorBuilder.AppendLine("{0}: {1}".FormatWith(error.Parameter, message));
                    }
                }

                content = errorBuilder.ToString();
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                content = TextMessages.SomethingUnholyGoingOn;
            }

            return content;
        }

        private static string Serialize(XmlObjectSerializer serializer, object target)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, target);

                ms.Seek(0, SeekOrigin.Begin);

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        [DataContract(Name = "create", Namespace = "")]
        private sealed class CreateResult
        {
            [DataMember(Name = "shortUrl", Order = 0)]
            public string ShortUrl
            {
                get;
                set;
            }

            [DataMember(Name = "previewUrl", Order = 1)]
            public string PreviewUrl
            {
                get;
                set;
            }

            [DataMember(Name = "alias", Order = 2)]
            public string Alias
            {
                get;
                set;
            }

            [DataMember(Name = "longUrl", Order = 3)]
            public string LongUrl
            {
                get;
                set;
            }

            [DataMember(Name = "title", Order = 4)]
            public string Title
            {
                get;
                set;
            }
        }

        [DataContract(Name = "error", Namespace = "")]
        private sealed class Error
        {
            [DataMember(Name = "parameter", Order = 0)]
            public string Parameter { get; set; }

            [DataMember(Name = "messages", Order = 1)]
            public MessageCollection Messages { get; set; }
        }

        [CollectionDataContract(Name = "messages", ItemName = "message", Namespace = "")]
        private sealed class MessageCollection : Collection<string>
        {
            public MessageCollection()
            {
            }

            public MessageCollection(IList<string> messages) : base(messages)
            {
            }
        }
    }
}