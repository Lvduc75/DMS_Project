using Microsoft.EntityFrameworkCore.Migrations;

namespace DMS_FE.Migrations
{
    public partial class EnsureConfigPriceConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing constraint if exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'chk_price_type')
                BEGIN
                    ALTER TABLE [dbo].[ConfigPrice] DROP CONSTRAINT [chk_price_type]
                END
            ");

            // Update data to match new schema
            migrationBuilder.Sql(@"
                UPDATE [dbo].[ConfigPrice]
                SET type = 'electricity'
                WHERE type IN ('ELECTRIC', 'electric', 'electricity')
            ");

            migrationBuilder.Sql(@"
                UPDATE [dbo].[ConfigPrice]
                SET type = 'water'
                WHERE type IN ('WATER', 'water')
            ");

            migrationBuilder.Sql(@"
                UPDATE [dbo].[ConfigPrice]
                SET type = 'room'
                WHERE type IN ('ROOM', 'room')
            ");

            // Add new constraint with correct values
            migrationBuilder.Sql(@"
                ALTER TABLE [dbo].[ConfigPrice] 
                ADD CONSTRAINT [chk_price_type] 
                CHECK ([type] IN ('electricity', 'water', 'room'))
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the new constraint
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'chk_price_type')
                BEGIN
                    ALTER TABLE [dbo].[ConfigPrice] DROP CONSTRAINT [chk_price_type]
                END
            ");

            // Restore original constraint (if needed)
            migrationBuilder.Sql(@"
                ALTER TABLE [dbo].[ConfigPrice] 
                ADD CONSTRAINT [chk_price_type] 
                CHECK ([type] IN ('ROOM', 'ELECTRIC', 'WATER', 'INTERNET', 'CLEANING', 'OTHER'))
            ");
        }
    }
} 