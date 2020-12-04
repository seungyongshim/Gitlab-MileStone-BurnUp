namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Query
    {
        public static string IssuesMilestone(string milestoneTitle = "ADC 4.0 | Model Recipe C# 기능 1차 개발") =>
            @$"
{{
  group(fullPath: ""mirero"")
  {{
        name
        webUrl
        issues (includeSubgroups:true, milestoneTitle: ""{milestoneTitle}"") {{
            nodes{{
                iid
                title
                createdAt
                closedAt
                milestone {{
                  id
                  createdAt
                  dueDate
                }}
            }}
            pageInfo {{
                endCursor
                hasNextPage
            }}
        }}
    }}
}}
";
    }
}
