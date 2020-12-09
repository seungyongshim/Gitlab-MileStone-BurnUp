using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    public record IssuesMilestoneResponse
    {
        public GroupContent Group { get; init; }

        public record GroupContent
        {
            public string Name { get; init; }
            public string WebUrl { get; init; }
            public IssueContent Issues { get; init; }

            public record IssueContent
            {
                public List<NodeContent> Nodes { get; init; }
                public record NodeContent
                {
                    public string Iid { get; init; }
                    public string Title { get; init; }
                    public DateTime CreatedAt { get; init; }
                    public DateTime? ClosedAt { get; init; }
                    public MileStoneContent MileStone { get; init; }
                    public record MileStoneContent
                    {
                        public string Id { get; init; }
                        public DateTime CreatedAt { get; init; }
                        public DateTime? StartDate { get; init; }
                        public DateTime? DueDate { get; init; }
                    }
                }
            }
        }
    }
}
