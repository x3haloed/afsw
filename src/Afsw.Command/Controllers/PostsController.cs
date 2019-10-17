using Afsw.Command.IdentityProvider;
using Afsw.Command.Services;
using Afsw.Types;
using Hangfire;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Afsw.Command.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        public PostsController(
            ILogger<PostsController> logger,
            LiteDatabase database,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _database = database;
            _userManager = userManager;
        }

        private readonly ILogger<PostsController> _logger;
        private readonly LiteDatabase _database;
        private readonly UserManager<ApplicationUser> _userManager;

        // POST: Posts
        [HttpPost("")]
        [Consumes(MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]

        // ignore authentication until its fixed
        [AllowAnonymous]

        public async Task<ActionResult> PostAsync()
        {
            string markdown;

            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                markdown = await reader.ReadToEndAsync();
            }

            // Get customer collection
            var posts = _database.GetCollection<Post>("posts");

            // Create your new customer instance
            var post = new Post
            {
                Name = "Post Name",
                Content = markdown,
                //AuthorId = (await _userManager.GetUserAsync(User)).Id,
                //AuthorName = User.Identity.Name,
                AuthorId = new Guid(),
                AuthorName = "chad",
            };

            // Insert new customer document(Id will be auto - incremented)
            Guid newPostId = posts.Insert(post);

            _logger.LogInformation("new post added.");

            string postCompileJobId = BackgroundJob.Enqueue<HangfireTasks>(hf => hf.CompilePost(newPostId));
            BackgroundJob.ContinueJobWith<HangfireTasks>(postCompileJobId, hf => hf.CompileArchive());

            return Accepted("http://localhost/path", post);
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
