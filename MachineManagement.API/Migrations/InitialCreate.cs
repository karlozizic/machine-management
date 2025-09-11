using FluentMigrator;

namespace MachineManagement.API.Migrations;

//generally it's better to version migrations with date and time, because of team conflicts
[Migration(001)]
public class InitialCreate : Migration
{
    public override void Up()
    {
        Create.Table("machines")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(255).NotNullable().Unique()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updated_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
        
        Create.Table("malfunctions")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(255).NotNullable()
            .WithColumn("machine_id").AsInt32().NotNullable()
            .WithColumn("priority").AsInt32().NotNullable().WithDefaultValue(1) // 1=Low, 2=Medium, 3=High
            .WithColumn("start_time").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("end_time").AsDateTime().Nullable()
            .WithColumn("description").AsString().NotNullable()
            .WithColumn("is_resolved").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updated_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);

        Create.ForeignKey("FK_malfunctions_machines")
            .FromTable("malfunctions").ForeignColumn("machine_id")
            .ToTable("machines").PrimaryColumn("id")
            .OnDelete(System.Data.Rule.Cascade);
    }
    
    public override void Down()
    {
        Delete.Table("malfunctions");
        Delete.Table("machines");
    }
}