using Afsw.Command.IdentityProvider;
using Afsw.Command.Models;
using Afsw.Command.Services;
using Afsw.Types;
using Hangfire;
using LiteDB;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
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

        private static readonly Regex _slugRegex = new Regex("[^a-zA-Z0-9]");

        private readonly ILogger<PostsController> _logger;
        private readonly LiteDatabase _database;
        private readonly UserManager<ApplicationUser> _userManager;

        // POST: Posts
        [HttpPost("")]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [AllowAnonymous]
        public async Task<ActionResult> PostAsync([FromForm]PostModel postModel)
        {
            var user = await _userManager.FindByNameAsync(postModel.Username);

            if (user == null)
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            if (! await _userManager.CheckPasswordAsync(user, postModel.Password))
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            // Get customer collection
            var posts = _database.GetCollection<Post>("posts");

            // Create your new customer instance
            var post = new Post
            {
                Title = postModel.Title,
                Slug = _slugRegex.Replace(postModel.Title, "-"),
                PublishedOn = DateTime.UtcNow,
                ContentMarkdown = postModel.Content,
                AuthorId = user.Id,
                AuthorName = user.Name,
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
