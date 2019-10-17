using Afsw.Command.Templates.TemplateModels;
using Afsw.Types;
using LiteDB;
using System;
using System.Dynamic;
using System.IO;

namespace Afsw.Command.Services
{
    public class HangfireTasks
    {
        public HangfireTasks(ViewRenderService viewRender, LiteDatabase database)
        {
            _viewRender = viewRender;
            _database = database;
        }

        private readonly ViewRenderService _viewRender;
        private readonly LiteDatabase _database;

        public void CompilePost(Guid postId)
        {
            var post = _database.GetCollection<Post>("posts").FindById(postId);

            if (post == null)
            {
                throw new InvalidOperationException("Cannot find post with ID:" + postId);
            }

            var templateModel = new PostTemplateModel
            {
                AuthorName = post.AuthorName,
                Name = post.Name,
                Content = post.Content,
            };

            string viewWithViewModel = _viewRender.RenderAsync("Templates/Post.cshtml", templateModel).GetAwaiter().GetResult();

            //output the post html
            File.WriteAllText(@"C:\Code\Afsw\src\client-site\postname.html", viewWithViewModel);
        }

        public void CompileArchive()
        {
            //var post = _database.GetCollection<Post>("posts").FindById(postId);

            //if (post == null)
            //{
            //    throw new InvalidOperationException("Cannot find post with ID:" + postId);
            //}

            //var templateModel = new PostTemplateModel
            //{
            //    AuthorName = post.AuthorName,
            //    Name = post.Name,
            //    Content = post.Content,
            //};

            //string viewWithViewModel = _viewRender.RenderAsync("Templates/Post.cshtml", templateModel).GetAwaiter().GetResult();

            ////output the post html
            //File.WriteAllText(@"C:\Code\Afsw\src\client-site\postname.html", viewWithViewModel);
        }
    }
}
