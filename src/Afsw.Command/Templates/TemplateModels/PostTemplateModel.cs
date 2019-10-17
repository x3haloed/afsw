using System;

namespace Afsw.Command.Templates.TemplateModels
{
    public class PostTemplateModel
    {
        public DateTime PublishedOn { get; set; }
        public string Title { get; set; }
        public string ContentHtml { get; set; }
        public string AuthorName { get; set; }
    }
}