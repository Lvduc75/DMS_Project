using Microsoft.EntityFrameworkCore.Migrations;

namespace DMS_FE.Migrations
{
    public partial class FixConfigPriceConstraint : Migration
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

            // Add new constraint with correct values
            migrationBuilder.Sql(@"
                ALTER TABLE [dbo].[ConfigPrice] 
                ADD CONSTRAINT [chk_price_type] 
                CHECK ([type] IN ('ROOM', 'ELECTRIC', 'WATER', 'INTERNET', 'CLEANING', 'OTHER'))
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
                CHECK ([type] IN ('room', 'electric', 'water', 'internet', 'cleaning', 'other'))
            ");
        }
    }
} 