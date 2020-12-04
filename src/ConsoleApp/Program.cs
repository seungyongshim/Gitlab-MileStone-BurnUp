using System;
using System.Text.Json;
using System.Linq;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using System.Threading.Tasks;
using ConsoleApp.Entity;
using System.Drawing;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var GraphQLClient = new GraphQLHttpClient(@"http://wish.mirero.co.kr" + @"/api/graphql", new SystemTextJsonSerializer());

            var issuesMilestoneResponse = await GraphQLClient.SendQueryAsync<IssuesMilestoneResponse>(new GraphQL.GraphQLRequest
            {
                Query = Query.IssuesMilestone("ADC 4.0 | Model Recipe C# 기능 2차 개발")
            });

            //var mileStone = MileStone.Create(issuesMilestoneResponse.Data, DateTime.Parse("2020-11-30"));
            var mileStone = MileStone.Create(issuesMilestoneResponse.Data, DateTime.Today);

            var plt = new ScottPlot.Plot(800, 300);

            plt.PlotScatter(mileStone.IssuesCreateCounts.Select(x => x.Date.ToOADate()).ToArray(),
                            mileStone.IssuesCreateCounts.Select(y => Convert.ToDouble(y.Count)).ToArray(),
                            color: Color.Red,
                            label: "총 액션아이템");
            plt.PlotScatter(mileStone.IssuesCloseCounts.Select(x => x.Date.ToOADate()).ToArray(),
                            mileStone.IssuesCloseCounts.Select(y => Convert.ToDouble(y.Count)).ToArray(),
                            color: Color.Blue,
                            label: "해결된 액션아이템");

            plt.Grid(xSpacing: 1, xSpacingDateTimeUnit: ScottPlot.Config.DateTimeUnit.Day);
            
            plt.Legend();
            plt.Ticks(dateTimeX: true);
            plt.SetCulture(shortDatePattern: "M\\/dd");
            plt.SaveFig("quickstart.png");

            
        }
    }
}
