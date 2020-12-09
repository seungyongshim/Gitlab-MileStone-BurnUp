namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Flurl.Http;
    using GitLabApiClient;
    using GitLabApiClient.Models.Milestones.Requests;
    using GitLabApiClient.Models.Projects.Requests;
    using GitLabApiClient.Models.Uploads.Requests;
    using GitLabApiClient.Models.Uploads.Responses;

    public class Uploader
    {
        static string token = "qkXEyz8KqYfdGfHFsDzJ";

        public static async Task File (string path, string MilestoneName)
        {
            var client = new GitLabClient("http://wish.mirero.co.kr", token);

            using var stream = new FileStream(path: path, FileMode.Open);
            
                var retUpload = await client.Uploads.UploadFile(54, new CreateUploadRequest
                    (stream, "burnup.png"));
            

            var ret = await client.Projects.GetMilestonesAsync(54, x => x.State = GitLabApiClient.Models.Milestones.Responses.MilestoneState.All);

            var milestone = ret.Where(x => x.Title == MilestoneName).First();

            await client.Projects.UpdateMilestoneAsync(54, milestone.Id, new UpdateProjectMilestoneRequest()
            {
                Description = retUpload.Markdown
            });
        }
    }
}
