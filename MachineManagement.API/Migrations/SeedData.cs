using FluentMigrator;

namespace MachineManagement.API.Migrations;

[Migration(002)]
public class SeedData : Migration
{
    public override void Up()
    {
        Insert.IntoTable("machines")
            .Row(new { name = "CNC Machine 1" })
            .Row(new { name = "CNC Machine 2" })
            .Row(new { name = "CNC Machine 3" });

        Insert.IntoTable("malfunctions")
            .Row(new
            {
                name = "Overheating",
                machine_id = 1,
                priority = 3,
                description = "Temperature 130C exceeds safe limits.",
                is_resolved = true,
                start_time = DateTime.Now.AddHours(-1)
            })
            .Row(new
            {
                name = "Overheating",
                machine_id = 2,
                priority = 3,
                description = "Temperature 120C exceeds safe limits.",
                is_resolved = false,
                start_time = DateTime.Now.AddMinutes(-30)
            })
            .Row(new
            {
                name = "Minor Overheating",
                machine_id = 3,
                priority = 2,
                description = "Temperature 90C exceeds recommended limits.",
                is_resolved = false,
                start_time = DateTime.Now.AddHours(-2)
            });
    }

    public override void Down()
    {
        Delete.FromTable("malfunctions").AllRows();
        Delete.FromTable("machines").AllRows();
    }
}