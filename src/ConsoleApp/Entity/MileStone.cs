namespace ConsoleApp.Entity
{
    using ConsoleApp.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public record MileStone(DateTime CreatedAt,
                            DateTime DueDate,
                            List<(DateTime Date, int Count)> IssuesCreateCounts,
                            List<(DateTime Date, int Count)> IssuesCloseCounts)
    {
        public static MileStone Create(IssuesMilestoneResponse res, DateTime today)
        {
            var createdAt = res.Group.Issues.Nodes.First().MileStone.CreatedAt.Date;
            var dueDate = res.Group.Issues.Nodes.First().MileStone.DueDate ?? today;
            var issuesCreateAt = res.Group.Issues.Nodes.Select(x => x.CreatedAt.Date)
                                                       .ToList();
            var issuesCloseAt = res.Group.Issues.Nodes.Where(x => x is not null)
                                                      .Select(x => x.ClosedAt?.Date)
                                                      .OfType<DateTime>()
                                                      .ToList();

            var duration = dueDate - createdAt;

            var issuesCreateCounts = from index in Enumerable.Range(0, duration.Days + 1)
                                     let day = createdAt.AddDays(index).Date
                                     where day.IsBusinessDay()
                                     let count = issuesCreateAt.Where(x => x < day.AddDays(1)).Count()
                                     select (day, count);

            var durationClose = today < dueDate
                              ? today - createdAt
                              : dueDate - createdAt;
              
            var issuesCloseCounts = from index in Enumerable.Range(0, durationClose.Days + 1)
                                    let day = createdAt.AddDays(index).Date
                                    where day.IsBusinessDay()
                                    let count = issuesCloseAt.Where(x => x < day.AddDays(1)).Count()
                                    select (day, count);

            return new MileStone(createdAt, dueDate, issuesCreateCounts.ToList(), issuesCloseCounts.ToList());
        }
    }
}
