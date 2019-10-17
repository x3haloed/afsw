using System;

namespace Afsw.Types
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
