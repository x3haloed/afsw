using System.Dynamic;
using System.IO;

namespace Afsw.Command.Services
{
    public class HangfireTasks
    {
        ViewRenderService _viewRender;
        public HangfireTasks(ViewRenderService viewRender)
        {
            _viewRender = viewRender;
        }

        public void CompilePosts()
        {
            //ViewModel is of type dynamic - just for testing
            dynamic x = new ExpandoObject();
            x.Test = "Yes";
            string viewWithViewModel = _viewRender.Render("Templates/Post.cshtml", x);

            //output the post html
            File.WriteAllText(@"C:\Code\Afsw\src\client-site\postname.html", viewWithViewModel);
        }
    }
}
