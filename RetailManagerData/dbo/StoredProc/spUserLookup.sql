CREATE PROCEDURE [dbo].[spUserLookup]
	@id NVARCHAR(128)
AS
begin
set nocount on;
	SELECT FirstName,LastName,Email,CreateDate
	FROM [dbo].[User] Where Id = @id
end
