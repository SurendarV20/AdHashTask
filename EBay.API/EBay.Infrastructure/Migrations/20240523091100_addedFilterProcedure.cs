using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBay.Infrastructure.Migrations
{
    public partial class addedFilterProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
CREATE TYPE VehicleDataListRequestType AS TABLE
(
    Make NVARCHAR(MAX),
    Model NVARCHAR(MAX),
    Year INT
);
GO
CREATE PROCEDURE GetVehicleDetailList
    @VehicleDataListRequest VehicleDataListRequestType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        vd.VehicleDetailId,
        vd.Make,
        vd.Model,
        vd.Year,
        vd.Status,
        vd.Trim,
        vd.Engine,
        ISNULL(vd.Notes,'') AS Notes
    FROM
        VehicleDetails vd
    INNER JOIN
        @VehicleDataListRequest req
    ON
        vd.Make = req.Make AND
        vd.Model = req.Model AND
        vd.Year = req.Year;
END;


");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP PROCEDURE GetVehicleDetailList;
GO
DROP TYPE VehicleDataListRequestType;
GO

");
        }
    }
}
