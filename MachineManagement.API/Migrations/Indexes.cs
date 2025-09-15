using FluentMigrator;

namespace MachineManagement.API.Migrations;

[Migration(003)]
public class Indexes : Migration
{
    public override void Up()
    {
        Create.Index("ix_malfunctions_priority_start").
            OnTable("malfunctions")
            .OnColumn("priority").Ascending()
            .OnColumn("start_time").Descending();
        
        Create.Index("ix_malfunctions_machine_start")
            .OnTable("malfunctions")
            .OnColumn("machine_id").Ascending()
            .OnColumn("start_time").Descending();
    }

    public override void Down()
    {
        Delete.Index("ix_malfunctions_machine_start").OnTable("malfunctions");
        Delete.Index("ix_malfunctions_priority_start").OnTable("malfunctions");
    }
}