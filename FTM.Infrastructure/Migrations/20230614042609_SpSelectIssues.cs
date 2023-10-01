using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTM.Infrastructure.Migrations
{
    public partial class SpSelectIssues : Migration
    {
        private const string Query = @"
create or replace function select_today_issues()
    returns table(UserId int, IssueId int)
    language plpgsql
as $$
declare
    
begin
    return query select distinct S.""UserId"" as UserId, I.""Id"" as IssueId from ""Settings"" S 
    left join ""Issues"" I on s.""UserId"" = I.""UserId"" 
    where s.""IsDailyScheduleEnabled"" = true
                                                                                                                                                                                   and S.""DailyScheduleHour"" = (SELECT EXTRACT(HOUR FROM NOW()))
    and I.""RemindTime""::date = current_date;
end$$;
";
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        { }
    }
}
